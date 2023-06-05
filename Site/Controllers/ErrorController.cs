using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReconhecimentoFacialAWS.ViewModels;
using System.Net;

namespace ReconhecimentoFacialAWS.Controllers;

public class ErrorController : Controller
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index()
    {
        IExceptionHandlerPathFeature _exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        ErrorVM _erro;

        if (_exceptionHandlerPathFeature != null)
        {
            _erro = new ErrorVM
            {
                StatusCode = (HttpStatusCode)HttpContext.Response.StatusCode,
                Acao = _exceptionHandlerPathFeature.Path,
                Origem = _exceptionHandlerPathFeature.Error.Source,
                Mensagem = _exceptionHandlerPathFeature.Error.Message,
                RastreamentoPilha = _exceptionHandlerPathFeature.Error.StackTrace
            };
        }
        else
        {
            _erro = new ErrorVM
            {
                StatusCode = (HttpStatusCode)HttpContext.Response.StatusCode,
                Acao = HttpContext.Request.Path,
                Origem = HttpContext.Request.PathBase.Value,
                Mensagem = "Erro ao obter a exceção.",
                RastreamentoPilha = ""
            };
        }

        return View(_erro);
    }
}
