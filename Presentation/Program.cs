using static System.Console;
using Student_Library_Assistant;
using System;
using System.Collections.Generic;
using Student_Library_Assistant.Domain.Entities;
using Student_Library_Assistant.Infrastructure.Repositories;
using Spectre.Console;
using Student_Library_Assistant.Application.Services;
using Student_Library_Assistant.Presentation;

class Program
{
    static void Main()
    {
        bool running = true;
        Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight+15);
        
        var studentRepo = new StudentRepository();
        var bookRepo = new BookRepository();
        var loansRepo = new LoanRepository();
        
        var bookService = new BookService(bookRepo, loansRepo);
        var studentService = new StudentService(studentRepo, loansRepo);
        var loanService = new LoanService(loansRepo,  bookRepo, studentRepo);

        while (running)
        {
           AnsiConsole.Clear();
            
            var title = new FigletText("Student & Library Assistant")
                .Centered()
                .Color(Color.Cyan);
            
            var panel = new Panel(title)
            {
                Border = BoxBorder.Double,
                BorderStyle = new Style(Color.White),
                Padding = new Padding(1, 1, 1, 1),
                Width = 250,
                Height = 15
            };
  
            AnsiConsole.Write(panel);
            
            Style highlight = new Style(decoration: Decoration.Bold | Decoration.Underline);

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[cyan]Select an item[/]")
                    .AddChoices("Books", "Students", "Loans", "Exit")
                    .HighlightStyle(highlight)
            );

            switch (choice)
            {
                case "Books":
                    BooksMenu.ShowMenu(bookService);
                    break;
                case "Students":
                    StudentsMenu.ShowMenu(studentService);
                    break;
                case "Loans":
                    LoansMenu.ShowMenu(loanService, studentService, bookService);
                    break;
                case "Exit":
                    running = false;
                    break;
            }
        }
    }

    static void ShowBooksMenu()
    {
        AnsiConsole.MarkupLine("[green]Books menu[/]");
        Console.ReadKey();
    }

    static void ShowLoansMenu()
    {
        AnsiConsole.MarkupLine("[green]Loans menu[/]");
        Console.ReadKey();
    }

}


// Book book1 = new Book
// {
//     Id = 1,
//     Title = "Think and grow rich", 
//     Year = 1937,
//     Author = "Napoleon Hill",
//     IsAvailable = true
// };
//         
// bookRepo.Add(book1);
// Book book1 = new Book
//     {
//         Id = 1,
//         Title = "Think and grow rich", 
//         Year = 1937,
//         Author = "Napoleon Hill",
//         IsAvailable = true
//     };
//
// Console.WriteLine(book1.Title);
// Console.WriteLine(book1.Year);
// Console.WriteLine(book1.Author);
//
// List<Student> students = new List<Student>();
//
// Student student1 = new Student
// {
//     Id = 1,
//     Name = "Georgi",
//     Year = 2,
//     FacultyNumber = "4302"
// };
//
// students.Add(student1);
//
// var studentRepo = new StudentRepository();
//
// studentRepo.Add(student1);
//
//
// Student student2 = new Student
// {
//     Id = 2,
//     Name = "Hristo",
//     Year = 3,
//     FacultyNumber = "4205"
// };
//
// students.Add(student2);
//
// foreach (var student in studentRepo.GetAll())
// {
//     Console.WriteLine(student.Name);
// }
//
// var form = new Form(54, new ThinBoxStyle());
// var numclass = book1;
// {
//
// };
// form.Write(numclass);
//
// Console.ReadLine();