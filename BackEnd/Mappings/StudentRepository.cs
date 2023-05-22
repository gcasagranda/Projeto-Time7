using BackEnd.Models;

namespace BackEnd.Mappings;

public class StudentRepository
{
    private List<Student> _students;

    public StudentRepository()
    {
        _students = new List<Student>();
    }

    public void Create(Student student)
    {
        student.Id = Guid.NewGuid();
        student.CreatedAt = DateTime.Now;
        student.UpdatedAt = DateTime.Now;
        student.Deleted = false;
        _students.Add(student);
    }

    public Student Read(Guid id)
    {
        return _students.Find(student => student.Id == id && !student.Deleted);
    }

    public void Update(Student updatedStudent)
    {
        Student existingStudent = _students.Find(student => student.Id == updatedStudent.Id && !student.Deleted);
        if (existingStudent != null)
        {
            existingStudent.Name = updatedStudent.Name;
            existingStudent.Email = updatedStudent.Email;
            existingStudent.PasswordHash = updatedStudent.PasswordHash;
            existingStudent.UpdatedAt = DateTime.Now;
        }
    }

    public void Delete(Guid id)
    {
        Student student = _students.Find(s => s.Id == id && !s.Deleted);
        if (student != null)
        {
            student.Deleted = true;
            student.UpdatedAt = DateTime.Now;
        }
    }
}