namespace Student_Library_Assistant;

public class LoanService
{
    private const int MaxLoansPerStudent = 5;
    
    private readonly List<Loan> _loans;
    private readonly List<Book> _books;
    private readonly List<Student> _students;

    public LoanService(List<Loan> loans, List<Book> books, List<Student> students)
    {
        _loans = loans;
        _books = books;
        _students = students;
    }
    
    public void LoanBook(int studentId, int bookId)
    {
        var book = _books.FirstOrDefault(b => b.Id == bookId);
        if (book == null)
            throw new Exception("The book doesn't exist.");

        if (!book.IsAvailable)
            throw new Exception("The book is already loaned.");
        
        var student = _students.FirstOrDefault(s => s.Id == studentId);
        if (student == null)
            throw new Exception("The student doesn't exist.");
        
        if(student.BorrowedBooks >= MaxLoansPerStudent)
            throw new Exception("The student has exceeded the maximum concurrent loans allowed.");

        book.IsAvailable = false;
        student.BorrowedBooks++;

        _loans.Add(new Loan
        {
            StudentId = studentId,
            BookId = bookId,
            LoanDate = DateTime.UtcNow
        });
    }

    public void ReturnBook(int studentId, int bookId)
    {
        var loan = _loans.FirstOrDefault(l => l.BookId == bookId);
        if (loan == null)
            throw new Exception("The book has already been returned.");

        var book = _books.First(b => b.Id == bookId);
        
        book.IsAvailable = true;
        
        var student = _students.FirstOrDefault(s => s.Id == studentId);
        student.BorrowedBooks--;
        
        _loans.Remove(loan);
    }
}