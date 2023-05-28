using System.ComponentModel.DataAnnotations;

namespace BackEnd.ViewModels.UserViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Necessario Email")]
    [EmailAddress(ErrorMessage = "Email invalido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Senha obrigatoria")]
    public string Password { get; set; }
    
}