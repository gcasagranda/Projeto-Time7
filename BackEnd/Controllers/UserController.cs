using BackEnd.Data;
using BackEnd.Extensions;
using BackEnd.Models;
using BackEnd.Services;
using BackEnd.ViewModels;
using BackEnd.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace BackEnd.Controllers;

public class UserController : Controller
{
    [HttpPost("v1/login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginViewModel model,
        [FromServices] DataContext context)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));
        }

        var user = await context
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("Usuario ou senha invalido"));
        
        if(!PasswordHasher.Verify(user.PasswordHash, model.Password))
            return StatusCode(401, new ResultViewModel<string>("Usuario ou senha invalido"));

        try
        {
            return Ok(new ResultViewModel<String>(user.Name, null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Server error"));
        }
    }
    
    [HttpPost("v1/user")]
    public async Task<IActionResult> PostAsync(
        [FromBody] RegisterViewModel model,
        [FromServices] DataContext context,
        [FromServices] EmailService emailService)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));
        }

        var user = new User()
        {
            Name = model.Name,
            Email = model.Email
        };

        var password = PasswordGenerator.Generate(25);
        user.PasswordHash = PasswordHasher.Hash(password);
        
        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            emailService.Send(
                user.Name, 
                user.Email, 
                "Bem vindo ao teste", 
                $"Sua senha é {password}");

            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email, password
            }));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("Usuario ja cadastrado"));
        }
        
        catch 
        {
            return StatusCode(500, new ResultViewModel<string>("Server Error"));
        }
    }
}