using ReconhecimentoFacialAWS.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReconhecimentoFacialAWS.Repositories;

public interface IUserRepository 
{
    User GetUser(string login);
    User GetUser(string login, string password);
    IEnumerable<User> GetAllUsers();
    string GetPhoto(string login);
}

public class UserRepository : IUserRepository
{
    private UsersTable _usersTable;

    public static UserRepository Create()
    {
        var _instance = new UserRepository();
        _instance.Initialize();
        return _instance;
    }

    private void Initialize()
    {
        string _json = File.ReadAllText("users.json");

        var _options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        _usersTable = JsonSerializer.Deserialize<UsersTable>(_json, _options);
    }

    public User GetUser(string login)
    {
        return _usersTable.Users.FirstOrDefault(x => x.Login == login);
    }

    public User GetUser(string login, string password)
    {
        return _usersTable.Users.FirstOrDefault(x => x.Login == login && x.Password == password);
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _usersTable.Users;
    }

    public string GetPhoto(string login)
    {
        var _user = _usersTable.Users.FirstOrDefault(x => x.Login == login);

        if (_user == null) return "";

        return _user.Photo;
    }
}
