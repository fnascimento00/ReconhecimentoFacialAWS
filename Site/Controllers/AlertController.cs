using Microsoft.AspNetCore.Mvc;
using ReconhecimentoFacialAWS.ViewModels;

namespace ReconhecimentoFacialAWS.Controllers;

public class AlertController : Controller
{
    public IActionResult LoadModal(string message)
    {
        return PartialView("_Modal", new AlertVM { Message = message });
    }
}
