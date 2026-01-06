namespace Student_Library_Assistant;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    public string FacultyNumber { get; set; }
    public int BorrowedBooks {get; set; }
}

// When deleting a student:
// check if student has current loans, if they do, student can't be deleted