using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KSPRAS.Models;
using Microsoft.EntityFrameworkCore;
using BurnSociety.Application;
using Pesapal.APIHelper;
using System.Net;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace KSPRAS.Controllers;

public class PesaPal : Controller
{
    public ApplicationDBContext DBContext;
    public KeysSecret keysecrets = new KeysSecret("GUlNXa5GEYSX95mdmnr/Rdfd9SqEbtln", "lgUPZRN1tnFDLrJF4Ekl0MLIO6M=");
    public float ammountToPay =0;

    public PesaPal(ApplicationDBContext DBcontext)
    {
        DBContext = DBcontext;

    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] Registrations registrations)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        Random random = new Random();
        string RandomRef = new string(Enumerable.Repeat(chars, 12)
                                   .Select(s => s[random.Next(s.Length)]).ToArray());

        try
        {
            // Set default values
            registrations.isPaid = false;
            registrations.ammount = GetConferenceFee(registrations.PaymentCategory);
            registrations.reffCode = RandomRef;
            ammountToPay = GetConferenceFee(registrations.PaymentCategory);

            // Save to database
            DBContext.Registrations.Add(registrations);
            await DBContext.SaveChangesAsync();

            // Authenticate and Process Payment (returns only redirect_url)
            var redirectUrl = await Authenticate(RandomRef, registrations);

            return Json(new
            {
                success = true,
                message = "Abstract submitted successfully!",
                data = redirectUrl
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                success = false,
                message = "An error occurred while submitting the abstract.",
                data = "N/A"
            });
        }
    }

    public static float GetConferenceFee(string category)
    {
        // Get today's date in real-time
        DateTime today = DateTime.Today;

        // Define pricing periods
        DateTime superEarlyBirdEnd = new DateTime(2025, 4, 30);
        DateTime earlyBirdEnd = new DateTime(2025, 7, 31);
        DateTime onSiteStart = new DateTime(2025, 8, 1);

        // Define category-based pricing
        var pricing = new System.Collections.Generic.Dictionary<string, float[]>
        {
            { "Residents", new float[] { 21, 23, 26 } },
            { "Consultants", new float[] { 27, 30, 33 } },
            { "Nurses, Allied Professionals, Medical Officers", new float[] { 18, 20, 22 } },
            { "Medical Students", new float[] { 10, 10, 15 } },
            { "East African Delegates", new float[] { 36, 40, 45 } },
            { "International Delegates", new float[] { 3, 4, 4 } } // Prices in USD
        };

        if (!pricing.ContainsKey(category))
        {
            throw new ArgumentException("Invalid category selected.");
        }

        // Determine the correct pricing bracket based on today's date
        float baseFee;
        if (today <= superEarlyBirdEnd)
        {
            baseFee = pricing[category][0]; // Super Early Bird Price
        }
        else if (today <= earlyBirdEnd)
        {
            baseFee = pricing[category][1]; // Early Bird Price
        }
        else
        {
            baseFee = pricing[category][2]; // On-Site Price
        }

        // Apply 10% discount if the amount paid is equal to or exceeds the discounted price
        string discountedPrice = ((90f/100f)* baseFee).ToString(); // 10% discount applied

        return float.Parse(discountedPrice);
    }

    public async Task<string> Authenticate(string Refference, Registrations abstractModel)
    {
        string data = JsonConvert.SerializeObject(keysecrets);
        var url = "https://pay.pesapal.com/v3/api/Auth/RequestToken";

        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Headers.Add("Accept", "application/json");

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(data);
        }

        var response = (HttpWebResponse)request.GetResponse();
        using (var streamReader = new StreamReader(response.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(result);

            // Extract the token
            string token = jsonResponse?.token;

            // Get redirect_url from JustPay
            return await JustPay(token, Refference, abstractModel);
        }
    }
    public string GetToken()
    {
        string data = JsonConvert.SerializeObject(keysecrets);
        var url = "https://pay.pesapal.com/v3/api/Auth/RequestToken";

        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Headers.Add("Accept", "application/json");

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(data);
        }

        var response = (HttpWebResponse)request.GetResponse();
        using (var streamReader = new StreamReader(response.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(result);

            // Extract the token
            string token = jsonResponse?.token;

            // Get redirect_url from JustPay
            return  token;
        }
    }


    [HttpGet]
    public async Task<string> InsertIPN(string OrderTrackingId, string OrderNotificationType, string OrderMerchantReference)
    {
        IPNResponses x = new IPNResponses();
        var findRef = DBContext.Registrations.FirstOrDefault(x => x.reffCode == OrderMerchantReference);
        string data = JsonConvert.SerializeObject(keysecrets);
        var url = "https://pay.pesapal.com/v3/api/Transactions/GetTransactionStatus?orderTrackingId="+OrderTrackingId;

        string token = GetToken();
        // To convert to JSON string (if needed)
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.ContentType = "application/json";
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Authorization", "Bearer " + token);

        var response = (HttpWebResponse)request.GetResponse();
        using (var streamReader = new StreamReader(response.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
         
            var jsonObject = JsonConvert.DeserializeObject<JObject>(result);
            PaymentResponse paymentResponse = new PaymentResponse

            {
                payment_method = jsonObject["payment_method"]?.ToString(),
                amount = jsonObject["amount"]?.ToObject<double>() ?? 0,
                created_date = jsonObject["created_date"]?.ToObject<DateTime>() ?? DateTime.UtcNow,
                confirmation_code = jsonObject["confirmation_code"]?.ToString(),
                order_tracking_id = jsonObject["order_tracking_id"]?.ToString(),
                payment_status_description = jsonObject["payment_status_description"]?.ToString(),
                description = jsonObject["description"]?.ToString(),
                message = jsonObject["message"]?.ToString(),
                payment_account = jsonObject["payment_account"]?.ToString(),
                call_back_url = jsonObject["call_back_url"]?.ToString(),
                status_code = jsonObject["status_code"]?.ToObject<int>() ?? 0,
                merchant_reference = jsonObject["merchant_reference"]?.ToString(),
                account_number = jsonObject["account_number"]?.ToString(),
                payment_status_code = jsonObject["payment_status_code"]?.ToString(),
                currency = jsonObject["currency"]?.ToString(),
                status = jsonObject["status"]?.ToString(),
                error_type = jsonObject["error"]?["error_type"]?.ToString(),
                code = jsonObject["error"]?["code"]?.ToString(),
             
            };
            DBContext.PaymentResponse.Add(paymentResponse);
            await DBContext.SaveChangesAsync();

            x.OrderTrackingId = OrderTrackingId;
            x.OrderNotificationType = OrderNotificationType;
            x.OrderMerchantReference = OrderMerchantReference;
            DBContext.Add(x);
            await DBContext.SaveChangesAsync();
        }


           
        return "Payment success";
    }



    

    public string RegisterIPN()
    {
        string data = JsonConvert.SerializeObject(keysecrets);
        var url = "https://cybqa.pesapal.com/pesapalv3/api/URLSetup/RegisterIPN";
        var jsonObject = new
        {
            url = "https://81a8-102-217-67-229.ngrok-free.app",
            ipn_notification_type = "GET"
        };

        // To convert to JSON string (if needed)
        string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject);
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Headers.Add("Accept", "application/json");
        string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3VzZXJkYXRhIjoiZWQ2MTkwMGYtZGNiMy00NjM2LWIxNGUtY2U1MGQwYzk2M2I1IiwidWlkIjoicWtpbzFCR0dZQVhUdTJKT2ZtN1hTWE5ydW9ac3JxRVciLCJuYmYiOjE3NDEzNDE2MzgsImV4cCI6MTc0MTM0NTIzOCwiaWF0IjoxNzQxMzQxNjM4LCJpc3MiOiJodHRwOi8vY3licWEucGVzYXBhbC5jb20vIiwiYXVkIjoiaHR0cDovL2N5YnFhLnBlc2FwYWwuY29tLyJ9.eJzwc22YbPXSW9zEAlgcX74HwP3AOJRbJwF8lVrHrFo";
        request.Headers.Add("Authorization", "Bearer " + authToken);

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(jsonObject);
        }

        var response = (HttpWebResponse)request.GetResponse();
        using (var streamReader = new StreamReader(response.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            return result;

        }
    }


    [HttpGet]
    public async Task<string> JustPay(string token, string refference, Registrations registrations)
    {
        var url = "https://pay.pesapal.com/v3/api/Transactions/SubmitOrderRequest";

        var paymentRequest = new
        {
            id = refference,
            currency = "KES",
            amount = ammountToPay,
            description = "Registration for the Pabs Conference",
            callback_url = "https://www.myapplication.com/response-page",
            notification_id = "4835c08f-4383-4908-9980-dc12a88afc8c",
            billing_address = new
            {
                email_address = registrations.EmailAddress,
                phone_number = registrations.TelephoneNumber,
                country_code = registrations.Currency,
                first_name = registrations.FName,
                middle_name = "",
                last_name = registrations.SName,
                line_1 = "",
                line_2 = "",
                city = "",
                state = "",
                postal_code = "",
                zip_code = ""
            }
        };

        string jsonString = JsonConvert.SerializeObject(paymentRequest);
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Authorization", "Bearer " + token);

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(jsonString);
        }

        var response = (HttpWebResponse)request.GetResponse();
        using (var streamReader = new StreamReader(response.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();

            // Deserialize to extract only redirect_url
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(result);
            string redirectUrl = jsonResponse?.redirect_url ?? "N/A";

            return redirectUrl;
        }
    }

}



