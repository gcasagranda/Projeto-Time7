namespace BackEnd.Models;

public class Professor : User
{
    public Guid UserId { get; set; }

    public User User;
}