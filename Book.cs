using System.Reflection;

namespace Student_Library_Assistant;

public class Book
{
    private string title;
    private int year;
    private string author;

    public int BookId { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public string Author { get; set; }
    
    public Book(int id, string bookTitle, int bookYear, string bookAuthor)
    {
        BookId = id;
        Title = bookTitle;
        Year = bookYear;
        Author = bookAuthor;
    }
    
}

// When deleting a book:
// check if the book is currently loaned to a student, if it is, book can't be deleted