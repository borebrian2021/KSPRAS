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

namespace KSPRAS.Controllers;

public class PesaPal : Controller
{
    public ApplicationDBContext DBContext;
    public KeysSecret keysecrets = new KeysSecret("GUlNXa5GEYSX95mdmnr/Rdfd9SqEbtln", "lgUPZRN1tnFDLrJF4Ekl0MLIO6M=");

    public PesaPal(ApplicationDBContext DBcontext)
    {
        DBContext = DBcontext;

    }




    public string Authenticate()
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
            // Deserialize JSON response into a dynamic object
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(result);

            // Extract the token property
            string token = jsonResponse?.token;
            return JustPay(token);

        }

         
    }
[HttpGet]
    public void  InsertIPN(string OrderTrackingId,string OrderNotificationType,string OrderMerchantReference)
    {
        IPNResponses x = new IPNResponses();
        x.OrderTrackingId =OrderTrackingId;
        x.OrderNotificationType  = OrderNotificationType;
        x.OrderMerchantReference = OrderMerchantReference;
        DBContext.Add(x);
        DBContext.SaveChangesAsync();

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
        request.Headers.Add("Authorization","Bearer " +authToken);

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

    public string JustPay(string token)
    {
        string data = JsonConvert.SerializeObject(keysecrets);
        var url = "https://pay.pesapal.com/v3/api/Transactions/SubmitOrderRequest";
        var paymentRequest = new
        {
            id = "4424445",
            currency = "KES",
            amount = 1,
            description = "Registration for the Pabs Conference",
            callback_url = "https://www.myapplication.com/response-page",
            notification_id = "4835c08f-4383-4908-9980-dc12a88afc8c",
            billing_address = new
            {
                email_address = "bkimutai2021@gmail.com",
                phone_number = "",
                country_code = "KE",
                first_name = "Brian",
                middle_name = "Kimutai",
                last_name = "Koskei",
                line_1 = "",
                line_2 = "",
                city = "",
                state = "",
                postal_code = "",
                zip_code = ""
            }
        };

        // Convert to JSON string
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
            return result;

        }


    }
}
