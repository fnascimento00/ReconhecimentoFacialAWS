namespace ReconhecimentoFacialAWS.Domains.Commands;

public class AddUserCOM
{
    public string Login { get; set; }
    public string Password { get; set; }
    public IFormFile Photo { get; set; }
}
