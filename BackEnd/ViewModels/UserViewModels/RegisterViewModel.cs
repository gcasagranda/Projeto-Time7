using System.ComponentModel.DataAnnotations;

namespace BackEnd.ViewModels.UserViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Nome obrigatorio")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Email obrigatorio")]
    [EmailAddress(ErrorMessage = "Email invalido")]
    public string Email { get; set; }
}