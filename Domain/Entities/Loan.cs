namespace Student_Library_Assistant.Domain.Entities;

public class Loan
{
    public int BookId { get; set; }
    public int StudentId { get; set; }
    public DateTime LoanDate {get; set; }
    
}