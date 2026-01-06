namespace Student_Library_Assistant;

public class Loan
{
    public int LoanId
    { get; set; }
    
    public int BookId
    { get; set; }
    
    public int StudentId
    { get; set; }
    
    
    public Loan(int id, int bookId, int studentId)
    {
        LoanId = id;
        BookId = bookId;
        StudentId = studentId;
    }
}


// When making a loan:
// check if student exists
// check if desired book exists
// check if student has available loans to make (max loans per student)
// check if the book has already been loaned
