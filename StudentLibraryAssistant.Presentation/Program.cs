using Student_Library_Assistant;
using System;
using System.Collections.Generic;

Book book1 = new Book
    {
        Id = 1,
        Title = "Think and grow rich", 
        Year = 1937,
        Author = "Napoleon Hill",
        IsAvailable = true
    };

Console.WriteLine(book1.Title);
Console.WriteLine(book1.Year);
Console.WriteLine(book1.Author);

List<Student> students = new List<Student>();

Student student1 = new Student
{
    Id = 1,
    Name = "Georgi",
    Year = 2,
    FacultyNumber = "4302"
};

students.Add(student1);

Student student2 = new Student
{
    Id = 2,
    Name = "Hristo",
    Year = 3,
    FacultyNumber = "4205"
};

students.Add(student2);

Console.WriteLine(students.Count);
Console.WriteLine(students[0].Name);

students.Remove(student1);

Console.WriteLine(students.Count);
Console.WriteLine(students[0].Name);