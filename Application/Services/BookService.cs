using Student_Library_Assistant.Domain.Entities;
using Student_Library_Assistant.Infrastructure.Repositories;

namespace Student_Library_Assistant.Application.Services;

public class BookService
{
    private readonly BaseRepository<Book> _bookRepo;
    private readonly LoanRepository _loanRepo;

    public BookService(BaseRepository<Book> bookRepo, LoanRepository loanRepo)
    {
        _bookRepo = bookRepo;
        _loanRepo = loanRepo;
    }

    public void AddBook(Book book)
    {
        book.IsAvailable = true;
        _bookRepo.Add(book);
    }

    public void UpdateBook(Book book)
    {
        var existing = _bookRepo.GetAll().FirstOrDefault(b => b.Id == book.Id);
        if (existing == null)
            throw new Exception("Book not found.");

        if (book.IsAvailable != existing.IsAvailable)
            existing.IsAvailable = book.IsAvailable;

        existing.Title = book.Title;
        existing.Author = book.Author;
        existing.Year = book.Year;

        _bookRepo.Update(b => b.Id == book.Id, existing);
    }

    public void DeleteBook(int bookId)
    {
        // Check if book is currently loaned
        var loan = _loanRepo.GetAll().FirstOrDefault(l => l.BookId == bookId);
        if (loan != null)
            throw new Exception("Cannot delete book. It is currently loaned to a student.");

        _bookRepo.Delete(b => b.Id == bookId);
    }

    public List<Book> GetAllBooks()
    {
        return _bookRepo.GetAll().ToList();
    }
}