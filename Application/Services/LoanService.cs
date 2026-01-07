using Student_Library_Assistant.Domain.Entities;
using Student_Library_Assistant.Infrastructure.Repositories;

namespace Student_Library_Assistant.Application.Services;

public class LoanService
{
    private const int MaxLoansPerStudent = 5;

    private readonly LoanRepository _loanRepo;
    private readonly BaseRepository<Book> _bookRepo;
    private readonly BaseRepository<Student> _studentRepo;

    public LoanService(
        LoanRepository loanRepo, 
        BaseRepository<Book> bookRepo, 
        BaseRepository<Student> studentRepo)
    {
        _loanRepo = loanRepo;
        _bookRepo = bookRepo;
        _studentRepo = studentRepo;
    }

    public void LoanBook(int studentId, int bookId)
    {
        var book = _bookRepo.GetAll().FirstOrDefault(b => b.Id == bookId);
        if (book == null)
            throw new Exception("The book doesn't exist.");

        if (!book.IsAvailable)
            throw new Exception("The book is already loaned.");

        var student = _studentRepo.GetAll().FirstOrDefault(s => s.Id == studentId);
        if (student == null)
            throw new Exception("The student doesn't exist.");

        if (student.BorrowedBooks >= MaxLoansPerStudent)
            throw new Exception("The student has exceeded the maximum concurrent loans allowed.");

        book.IsAvailable = false;
        student.BorrowedBooks++;

        _bookRepo.Update(b => b.Id == book.Id, book);
        _studentRepo.Update(s => s.Id == student.Id, student);

        // Add loan
        _loanRepo.Add(new Loan
        {
            StudentId = studentId,
            BookId = bookId,
            LoanDate = DateTime.UtcNow
        });
    }

    public void ReturnBook(int studentId, int bookId)
    {
        var loan = _loanRepo.GetAll()
            .FirstOrDefault(l => l.BookId == bookId && l.StudentId == studentId);

        if (loan == null)
            throw new Exception("This student has not borrowed this book.");

        var book = _bookRepo.GetAll().FirstOrDefault(b => b.Id == bookId);
        if (book == null)
            throw new Exception("The book doesn't exist.");

        var student = _studentRepo.GetAll().FirstOrDefault(s => s.Id == studentId);
        if (student == null)
            throw new Exception("The student doesn't exist.");

        book.IsAvailable = true;
        student.BorrowedBooks--;

        _bookRepo.Update(b => b.Id == book.Id, book);
        _studentRepo.Update(s => s.Id == student.Id, student);

        // Remove loan
        _loanRepo.Delete(l => l.BookId == bookId && l.StudentId == studentId);
    }

    public List<Loan> GetAllLoans()
    {
        return _loanRepo.GetAll().ToList();
    }
}
