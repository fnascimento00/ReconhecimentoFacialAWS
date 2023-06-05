using ReconhecimentoFacialAWS.Domains.Commands;
using ReconhecimentoFacialAWS.Repositories;

namespace ReconhecimentoFacialAWS.Domains.Receivers;

public interface ILoginUserREC
{
    string Validate(LoginUserCOM command);
}

public class LoginUserREC : ILoginUserREC
{
    private readonly IUserRepository _userRepository;

    public LoginUserREC(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public string Validate(LoginUserCOM command)
    {
        if (string.IsNullOrEmpty(command.Login) ||
            string.IsNullOrEmpty(command.Password))
        {
            return "Os valores para realizar o login não foram informados!";
        }

        var _user = _userRepository.GetUser(command.Login, command.Password);

        if (_user == null)
        {
            return "Dados Inválidos!";
        }

        return "";
    }
}
