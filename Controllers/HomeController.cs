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
