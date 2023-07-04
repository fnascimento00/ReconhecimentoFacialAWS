using ReconhecimentoFacialAWS.Domains.Commands;
using ReconhecimentoFacialAWS.Repositories;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReconhecimentoFacialAWS.Domains.Receivers;

public interface IAddPhotoUserREC
{
    string Validate(AddPhotoUserCOM command);
    string Execute(AddPhotoUserCOM command);
}

public class AddPhotoUserREC : IAddPhotoUserREC
{
    private readonly IUserRepository _userRepository;

    public AddPhotoUserREC(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public string Validate(AddPhotoUserCOM command)
    {
        if (command == null)
        {
            return "O comando não foi carregado com as informações necessárias para adicionar a foto!";
        }

        var _user = _userRepository.GetUser(command.Login);

        if (_user == null)
        {
            return "Usuário não encontrado!";
        }

        if (string.IsNullOrWhiteSpace(command.Login))
        {
            return "Informe o Login!";
        }

        if (string.IsNullOrWhiteSpace(command.Photo))
        {
            return "Informe a Foto!";
        }

        return "";
    }

    public string Execute(AddPhotoUserCOM command)
    {
        var _users = _userRepository.GetAllUsers().ToList();
        var _user = _userRepository.GetUser(command.Login);

        _user.Photo = command.Photo;
        _users.Where(x => x.Login == x.Login).ToList().ForEach(user => { user = _user; });

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

        return "Foto adicionada com sucesso!";
    }
}
