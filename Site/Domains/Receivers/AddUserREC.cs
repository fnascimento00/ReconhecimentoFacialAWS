using ReconhecimentoFacialAWS.Domains.Commands;
using ReconhecimentoFacialAWS.Repositories;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReconhecimentoFacialAWS.Domains.Receivers;

public interface IAddUserREC
{
    string Validate(AddUserCOM command);
    string Execute(AddUserCOM command);
}

public class AddUserREC : IAddUserREC
{
    private readonly IUserRepository _userRepository;

    public JavaScriptEncoder UnsafeRelaxedJsonEscaping { get; private set; }

    public AddUserREC(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public string Validate(AddUserCOM command)
    {
        if (command == null) 
        {
            return "O comando não foi carregado com as informações necessárias para adicionar o usuário!";
        }

        if (string.IsNullOrWhiteSpace(command.Login))
        {
            return "Informe o Login!";
        }

        if (string.IsNullOrWhiteSpace(command.Password))
        {
            return "Informe a Senha!";
        }

        return "";
    }

    public string Execute(AddUserCOM command) 
    {
        var _users = _userRepository.GetAllUsers().ToList();
        var _user = _userRepository.GetUser(command.Login);

        if (_user == null)
        {
            _users.Add(new()
            {
                Login = command.Login,
                Password = command.Password
            });
        }
        else
        {
            _user.Login = command.Login;
            _user.Password = command.Password;
            _users.Where(x => x.Login == x.Login).ToList().ForEach(user => { user = _user; });
        }

        var _options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = UnsafeRelaxedJsonEscaping
        };

        var _usersTable = new UsersTable()
        {
            Users = _users
        };

        var _json = JsonSerializer.Serialize(_usersTable, _options);
        File.WriteAllText("users.json", _json);

        return "Usuário adicionado com sucesso!";
    }
}
