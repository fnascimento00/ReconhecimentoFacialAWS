using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ReconhecimentoFacialAWS.Domains.Receivers;
using ReconhecimentoFacialAWS.Mappers;
using ReconhecimentoFacialAWS.Repositories;
using ReconhecimentoFacialAWS.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ReconhecimentoFacialAWS.Controllers;

public class LoginController : Controller
{
    private readonly ILoginUserREC _LoginUser;
    private readonly IAddUserREC _addUser;
    private readonly IUserRepository _userRepository;

    public LoginController(ILoginUserREC loginUser,
                           IAddUserREC addUser,
                           IUserRepository userRepository)
    {
        _LoginUser = loginUser;
        _addUser = addUser;
        _userRepository = userRepository;
    }

    public IActionResult Index()
    {
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            var _claimUser = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User");
            var _photoUser = _userRepository.GetPhoto(_claimUser.Value);

            if (string.IsNullOrWhiteSpace(_photoUser))
            {
                return RedirectToAction("Index", "CapturePhoto");
            }
            else
            {
                return RedirectToAction("Index", "Recognition");
            }
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Connect(LoginVM vm)
    {
        if (ModelState.IsValid)
        {
            var _command = Mapper.MapToCommand(vm);
            var _validate = _LoginUser.Validate(_command);

            if (!string.IsNullOrWhiteSpace(_validate))
            {
                return Json(new
                {
                    valid = false,
                    message = _validate
                });
            }

            await Authenticate(vm);
            var _user = _userRepository.GetUser(vm.Login);

            return Json(new
            {
                valid = true,
                redirectTo = _user.Photo == null ? "CapturePhoto" : "Recognition"
            });
        }

        return Json(new
        {
            valid = false,
            message = "Dados Inválidos!"
        });
    }

    protected async Task Authenticate(LoginVM vm)
    {
        ClaimsIdentity _identityLogin = new("login");
        _identityLogin.AddClaim(new Claim("User", vm.Login));
        ClaimsPrincipal _claimPrincipal = new(new[] { _identityLogin });

        AuthenticationProperties _properties = new()
        {
            IsPersistent = false,
            ExpiresUtc = DateTimeOffset.Now.AddSeconds(172800),
            RedirectUri = Url.Action("Index", "Login")
        };

        await HttpContext.SignInAsync(_claimPrincipal, _properties);
    }

    [HttpPost]
    public IActionResult NewUser(UserVM vm)
    {
        if (ModelState.IsValid)
        {
            var _command = Mapper.MapToCommand(vm);
            var _validate = _addUser.Validate(_command);

            if (!string.IsNullOrWhiteSpace(_validate)) 
            {
                return Json(new
                {
                    valid = false,
                    message = _validate
                });
            }

            var _execute = _addUser.Execute(_command);

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

    [HttpPost]
    public async Task<JsonResult> Disconnect()
    {
        if (!HttpContext.User.Identity.IsAuthenticated)
        {
            return Json(new
            {
                valid = true
            });
        }

        await HttpContext.SignOutAsync();

        return Json(new
        {
            valid = true
        });
    }
}
