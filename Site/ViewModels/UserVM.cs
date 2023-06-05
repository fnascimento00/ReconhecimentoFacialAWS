using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ReconhecimentoFacialAWS.ViewModels;

public class UserVM
{
    [Required(ErrorMessage = "Informe o Login!")]
    [Display(Name = "Login")]
    public string Login { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Informe a Senha!")]
    [Display(Name = "Senha")]
    public string Password { get; set; }

    [DisplayName("Foto")]
    [Required(ErrorMessage = "Informe a Foto!")]
    public IFormFile Photo { get; set; }
}
