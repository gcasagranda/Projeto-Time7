namespace BackEnd.Models;

public class Student : User
{
    
    public Guid UserId { get; set; }

    public User User;
    
}