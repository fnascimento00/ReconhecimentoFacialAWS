using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReconhecimentoFacialAWS.Extensions;
using ReconhecimentoFacialAWS.Helpers;
using ReconhecimentoFacialAWS.Mappers;
using ReconhecimentoFacialAWS.Repositories;

namespace ReconhecimentoFacialAWS.Controllers;

[Authorize]
public class RecognitionController : ControllerBaseExtension
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICameraService _cameraService;

    public RecognitionController(IUserRepository userRepository,
                                 IHttpContextAccessor httpContextAccessor, 
                                 ICameraService cameraService)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _cameraService = cameraService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Capture(string photoCapture)
    {
        var _claimUser = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User");
        var _photoUser = _userRepository.GetPhoto(_claimUser.Value);
        var _compareFaces = await _cameraService.CompareWithRekognition(photoCapture, _photoUser);
        var _compareFacesVM = Mapper.MapToView(_compareFaces);
        var _partial = await RenderViewAsync(this, "_CompareFaces", _compareFacesVM, true);

        return Json(new
        {
            html = _partial
        });
    }
}
