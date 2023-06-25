using System.ComponentModel.DataAnnotations;
using BackEnd.Models;

namespace BackEnd.ViewModels.UserViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Nome obrigatorio")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Email obrigatorio")]
    [EmailAddress(ErrorMessage = "Email invalido")]
    public string Email { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
}