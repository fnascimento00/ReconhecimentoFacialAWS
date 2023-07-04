using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReconhecimentoFacialAWS.Domains.Receivers;
using ReconhecimentoFacialAWS.Extensions;
using ReconhecimentoFacialAWS.Helpers;
using ReconhecimentoFacialAWS.Mappers;
using ReconhecimentoFacialAWS.Repositories;

namespace ReconhecimentoFacialAWS.Controllers;

[Authorize]
public class CapturePhotoController : ControllerBaseExtension
{
    private readonly IAddPhotoUserREC _addPhotoUser;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CapturePhotoController(IAddPhotoUserREC addPhotoUser,
                                  IHttpContextAccessor httpContextAccessor)
    {
        _addPhotoUser = addPhotoUser;
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Capture(string photoCapture)
    {
        if (ModelState.IsValid)
        {
            var _claimUser = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User");
            var _command = Mapper.MapToCommand(_claimUser.Value, photoCapture);
            var _validate = _addPhotoUser.Validate(_command);

            if (!string.IsNullOrWhiteSpace(_validate))
            {
                return Json(new
                {
                    valid = false,
                    message = _validate
                });
            }

            var _execute = _addPhotoUser.Execute(_command);

            return Json(new
            {
                valid = true,
                message = _execute
            });
        }

        return Json(new
        {
            valid = false,
            message = "Dados Inválidos!"
        });
    }
}
