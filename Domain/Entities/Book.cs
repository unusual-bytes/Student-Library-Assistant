namespace Student_Library_Assistant.Domain.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public string Author { get; set; }
    
    public bool IsAvailable { get; set; }
}

// When deleting a book:
// check if the book is currently loaned to a student, if it is, book can't be deleted