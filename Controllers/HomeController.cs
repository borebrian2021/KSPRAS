using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KSPRAS.Models;
using Microsoft.EntityFrameworkCore;
using BurnSociety.Application;
using Pesapal.APIHelper;

namespace KSPRAS.Controllers;

public class HomeController : Controller
{
    public ApplicationDBContext DBContext;
 
 
    public HomeController(ApplicationDBContext DBcontext)
    {
        DBContext = DBcontext;
  
    }


    [HttpPost]

    public async Task<IActionResult> Upload(AbstractSubmissionModel model)
    {
       
            try
            {
                // Generate a unique ID for the submission
                model.ID = Guid.NewGuid().ToString();

                // Set default values for additional fields
                model.isPaid = false;
                model.ammount = 0;
                model.reffCode = GenerateReferenceCode(); // Implement this method to generate a reference code

                // Add the model to the database
                DBContext.AbstractSubmissionModel.Add(model);
                await DBContext.SaveChangesAsync();

                // Return a success response
                return Json(new { success = true, message = "Abstract submitted successfully!" });
            }
            catch (Exception ex)
            {
                // Log the error and return an error response
                return Json(new { success = false, message = "An error occurred while submitting the abstract." });
            }
        
    }
       
       
        
    

    // Helper method to generate a reference code
    private string GenerateReferenceCode()
    {
        return "REF-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
    }
 
    public IActionResult UploadAbstract()
    {
        return View();
    }
    public string  JustPay()
    {
        Uri pesapalPostUri = new Uri("https://demo.pesapal.com/API/PostPesapalDirectOrderV4"); /*change to      

      https://www.pesapal.com/API/PostPesapalDirectOrderV4 when you are ready to go live!*/
     
   Uri pesapalCallBackUri = new Uri("https://localhost:7209/home/uploadabstract");
       


   IBuilder builder = new APIPostParametersBuilderV2()
         .ConsumerKey("qkio1BGGYAXTu2JOfm7XSXNruoZsrqEW")
         .ConsumerSecret("osGQ364R49cXKeOYSpaOnT++rHs=")
           .OAuthVersion(EOAuthVersion.VERSION1)
           .SignatureMethod(ESignatureMethod.HMACSHA1)
           .SimplePostHttpMethod(EHttpMethod.GET)
           .SimplePostBaseUri(pesapalPostUri)
           .OAuthCallBackUri(pesapalCallBackUri);
 
   APIHelper<IBuilder> helper = new APIHelper<IBuilder>(builder);

   var lineItems = new List<LineItem> { };

        var lineItem =

            new LineItem

            {

                Particulars = "",

                UniqueId = "",

                Quantity = 5,

                UnitCost =6


             };

   lineItem.SubTotal = (lineItem.Quantity * lineItem.UnitCost);
   lineItems.Add(lineItem);

        // Compose the order

        PesapalDirectOrderInfo webOrder = new PesapalDirectOrderInfo()

        {

            Amount = (lineItems.Sum(x => x.SubTotal)).ToString(),

            Description = "PAYMENT OF THE CONFERENCE REGISTRATION FEE",

            Type = "MERCHANT",

            Reference = "JGSJHSJVJH",

            Email = "bkimutai2021@gmail.com",

            FirstName = "Brian",

            LastName = "Kimutai",

            PhoneNumber = "0712035642",

            LineItems = lineItems

        };
             
 
   return helper.PostGetPesapalDirectOrderUrl(webOrder);


        return "";

    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
