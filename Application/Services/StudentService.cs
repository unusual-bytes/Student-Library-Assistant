using Student_Library_Assistant.Domain.Entities;
using Student_Library_Assistant.Infrastructure.Repositories;

namespace Student_Library_Assistant.Application.Services;

public class StudentService
{
    private readonly BaseRepository<Student> _studentRepo;
    private readonly LoanRepository _loanRepo;

    public StudentService(BaseRepository<Student> studentRepo, LoanRepository loanRepo)
    {
        _studentRepo = studentRepo;
        _loanRepo = loanRepo;
    }

    public void AddStudent(Student student)
    {
        student.BorrowedBooks = 0; // default
        _studentRepo.Add(student);
    }

    public void UpdateStudent(Student student)
    {
        var existing = _studentRepo.GetAll().FirstOrDefault(s => s.Id == student.Id);
        if (existing == null)
            throw new Exception("Student not found.");

        existing.Name = student.Name;
        existing.Year = student.Year;
        existing.FacultyNumber = student.FacultyNumber;

        _studentRepo.Update(s => s.Id == student.Id, existing);
    }

    public void DeleteStudent(int studentId)
    {
        // Check if student has current loans
        var loan = _loanRepo.GetAll().FirstOrDefault(l => l.StudentId == studentId);
        if (loan != null)
            throw new Exception("Cannot delete student. They currently have borrowed books.");

        _studentRepo.Delete(s => s.Id == studentId);
    }

    public List<Student> GetAllStudents()
    {
        return _studentRepo.GetAll().ToList();
    }
}