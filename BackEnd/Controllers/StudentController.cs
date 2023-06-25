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

public class StudentController : UserController
{
    [HttpPost("v1/student")]
    public async Task<IActionResult> PostAsync(
        [FromBody] RegisterViewModel model,
        [FromServices] DataContext context,
        [FromServices] EmailService emailService)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));
        }

        var student = new Student()
        {
            Name = model.Name,
            Email = model.Email
        };

        var password = PasswordGenerator.Generate(25);
        student.PasswordHash = PasswordHasher.Hash(password);
        
        try
        {
            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();

            emailService.Send(
                student.Name, 
                student.Email, 
                "Bem vindo ao teste", 
                $"Sua senha é {password}");

            return Ok(new ResultViewModel<dynamic>(new
            {
                user = student.Email, password
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