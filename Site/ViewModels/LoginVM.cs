using System.ComponentModel.DataAnnotations;

namespace ReconhecimentoFacialAWS.ViewModels;

public class LoginVM
{
    [Required(ErrorMessage = "Informe o Login!")]
    [Display(Name = "Login")]
    public string Login { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Informe a Senha!")]
    [Display(Name = "Senha")]
    public string Password { get; set; }
}
