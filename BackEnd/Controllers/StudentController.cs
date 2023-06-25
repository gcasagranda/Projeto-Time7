using BackEnd.Data;
using BackEnd.Extensions;
using BackEnd.Models;
using BackEnd.Services;
using BackEnd.ViewModels;
using BackEnd.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers;

public class StudentController : Controller
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
            UserId = model.UserId,
            User = model.User
        };

        try
        {
            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();
            
            return Ok(new ResultViewModel<dynamic>(new
            {
                student = student.UserId, User
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