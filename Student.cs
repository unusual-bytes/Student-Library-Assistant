namespace Student_Library_Assistant;

public class Student
{
    public int BookId
    { get; set; }

    public string Name
    { get; set; }

    public int Year
    { get; set; }
    
    public string FacultyNumber
    { get; set; }
    
    
    public Student(int id, string studentName, int studentYear, string facultyNumber)
    {
        BookId = id;
        Name = studentName;
        Year = studentYear;
        FacultyNumber = facultyNumber;
    }
}

// When deleting a student:
// check if student has current loans, if they do, student can't be deleted