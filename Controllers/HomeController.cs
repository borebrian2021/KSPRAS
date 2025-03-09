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

    // Helper method to generate a reference code
    private string GenerateReferenceCode()
    {
        return "REF-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
    }

    public IActionResult Registrations()
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
