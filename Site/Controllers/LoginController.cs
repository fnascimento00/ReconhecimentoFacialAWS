using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ReconhecimentoFacialAWS.Domains.Receivers;
using ReconhecimentoFacialAWS.Mappers;
using ReconhecimentoFacialAWS.ViewModels;
using System.Security.Claims;

namespace ReconhecimentoFacialAWS.Controllers;

public class LoginController : Controller
{
    private readonly ILoginUserREC _LoginUser;
    private readonly IAddUserREC _addUser;

    public LoginController(ILoginUserREC loginUser,
                           IAddUserREC addUser)
    {
        _LoginUser = loginUser;
        _addUser = addUser;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Connect(LoginVM vm)
    {
        if (ModelState.IsValid)
        {
            var _command = Mapper.MapToCommand(vm);
            var _validate = _LoginUser.Validate(_command);

            if (string.IsNullOrWhiteSpace(_validate))
            {
                await Authenticate(vm);

                return Json(new
                {
                    valid = true
                });
            }
            else
            {
                return Json(new
                {
                    valid = false,
                    message = _validate
                });
            }
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

            if (string.IsNullOrWhiteSpace(_validate)) 
            { 
                var _execute = _addUser.Execute(_command);

                return Json(new
                {
                    valid = true,
                    message = _execute
                });
            }
            else
            {
                return Json(new
                {
                    valid = false,
                    message = _validate
                });
            }
        }

        return Json(new
        {
            valid = false,
            message = "Dados Inválidos!"
        });
    }
}
