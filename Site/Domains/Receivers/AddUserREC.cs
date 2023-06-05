using ReconhecimentoFacialAWS.Domains.Commands;
using ReconhecimentoFacialAWS.Models;
using ReconhecimentoFacialAWS.Repositories;
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

    public AddUserREC(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public string Validate(AddUserCOM command)
    {
        if (command == null) 
        {
            return "Os valores para adicionar o usuário não foram informados!";
        }

        if (string.IsNullOrWhiteSpace(command.Login))
        {
            return "Informe o Login!";
        }

        if (string.IsNullOrWhiteSpace(command.Password))
        {
            return "Informe a Senha!";
        }

        if (command.Photo == null)
        {
            return "Informe a Foto!";
        }

        if (command.Photo.ContentType.ToLower() != "image/jpg" &&
            command.Photo.ContentType.ToLower() != "image/jpeg" &&
            command.Photo.ContentType.ToLower() != "image/png")
        {
            return "A foto deve possuir uma das seguintes extensões: jpg, jpeg ou png!";
        }

        return "";
    }

    public string Execute(AddUserCOM command) 
    {
        var _users = _userRepository.GetAllUsers().ToList();
        var _user = _userRepository.GetUser(command.Login);

        using var _memoryStream = new MemoryStream();
        command.Photo.CopyTo(_memoryStream);
        var _bytes = _memoryStream.ToArray();
        var _base64 = "data:" + command.Photo.ContentType + ";base64," + Convert.ToBase64String(_bytes);

        if (_user == null)
        {
            _users.Add(new()
            {
                Login = command.Login,
                Password = command.Password,
                Photo = _base64
            });
        }
        else
        {
            _user.Login = command.Login;
            _user.Password = command.Password;
            _user.Photo = _base64;
            _users.Where(x => x.Login == x.Login).ToList().ForEach(user => { user = _user; });
        }

        var _options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
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
