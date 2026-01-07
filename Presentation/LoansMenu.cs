using Spectre.Console;
using Student_Library_Assistant.Application.Services;
using Student_Library_Assistant.Domain.Entities;
using Student_Library_Assistant.Infrastructure.Repositories;

namespace Student_Library_Assistant.Presentation;

public class LoansMenu
{
    public static void ShowMenu(
        LoanService loanService,
        StudentService studentService,
        BookService bookService)
    {
        bool running = true;

        while (running)
        {
            AnsiConsole.Clear();

            // Title
            var title = new FigletText("Loans")
                .Centered()
                .Color(Color.Cyan);
            AnsiConsole.Write(title);
            AnsiConsole.WriteLine();

            // Current loans table
            var loans = GetLoansWithDetails(loanService, studentService, bookService);

            if (loans.Any())
            {
                var table = new Table().Border(TableBorder.Rounded);
                table.AddColumn("Index");
                table.AddColumn("Student");
                table.AddColumn("Book");
                table.AddColumn("Loan Date");

                for (int i = 0; i < loans.Count; i++)
                {
                    var l = loans[i];
                    table.AddRow(
                        i.ToString(),
                        $"{l.StudentName} ({l.FacultyNumber})",
                        $"{l.BookTitle} ({l.BookAuthor})",
                        l.LoanDate.ToString("yyyy-MM-dd HH:mm")
                    );
                }

                AnsiConsole.Write(table);
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]No active loans.[/]");
            }

            AnsiConsole.WriteLine();

            // Bottom menu
            var action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[cyan]Choose action[/]")
                    .AddChoices(
                        "Loan a book",
                        "Return a book",
                        "Back"
                    )
            );

            switch (action)
            {
                case "Loan a book":
                    LoanBook(loanService, studentService, bookService);
                    break;

                case "Return a book":
                    ReturnBook(loanService, studentService, bookService);
                    break;

                case "Back":
                    running = false;
                    break;
            }
        }
    }

    // Helper: combine loan info with student/book details
    private static List<(string StudentName, string FacultyNumber, string BookTitle, string BookAuthor, DateTime LoanDate)>
        GetLoansWithDetails(LoanService loanService, StudentService studentService, BookService bookService)
    {
        var result = new List<(string, string, string, string, DateTime)>();

        foreach (var loan in loanService.GetAllLoans())
        {
            var student = studentService.GetAllStudents().FirstOrDefault(s => s.Id == loan.StudentId);
            var book = bookService.GetAllBooks().FirstOrDefault(b => b.Id == loan.BookId);

            if (student != null && book != null)
            {
                result.Add((student.Name, student.FacultyNumber, book.Title, book.Author, loan.LoanDate));
            }
        }

        return result;
    }

    private static void LoanBook(LoanService loanService, StudentService studentService, BookService bookService)
    {
        var students = studentService.GetAllStudents();
        if (!students.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No students available.[/]");
            Pause();
            return;
        }

        var student = AnsiConsole.Prompt(
            new SelectionPrompt<Student>()
                .Title("[cyan]Select a student[/]")
                .UseConverter(s => $"{s.Name} ({s.FacultyNumber})")
                .AddChoices(students)
        );

        var availableBooks = bookService.GetAllBooks().Where(b => b.IsAvailable).ToList();
        if (!availableBooks.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No available books to loan.[/]");
            Pause();
            return;
        }

        var book = AnsiConsole.Prompt(
            new SelectionPrompt<Book>()
                .Title("[cyan]Select a book to loan[/]")
                .UseConverter(b => $"{b.Title} ({b.Author})")
                .AddChoices(availableBooks)
        );

        try
        {
            loanService.LoanBook(student.Id, book.Id);
            AnsiConsole.MarkupLine("[green]Book loaned successfully![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }

        Pause();
    }

    private static void ReturnBook(LoanService loanService, StudentService studentService, BookService bookService)
    {
        var studentsWithLoans = studentService.GetAllStudents().Where(s => s.BorrowedBooks > 0).ToList();
        if (!studentsWithLoans.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No students have borrowed books.[/]");
            Pause();
            return;
        }

        var student = AnsiConsole.Prompt(
            new SelectionPrompt<Student>()
                .Title("[cyan]Select a student[/]")
                .UseConverter(s => $"{s.Name} ({s.FacultyNumber})")
                .AddChoices(studentsWithLoans)
        );

        var loanedBooks = bookService.GetAllBooks().Where(b => !b.IsAvailable).ToList();
        if (!loanedBooks.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No books to return.[/]");
            Pause();
            return;
        }

        var book = AnsiConsole.Prompt(
            new SelectionPrompt<Book>()
                .Title("[cyan]Select book to return[/]")
                .UseConverter(b => $"{b.Title} ({b.Author})")
                .AddChoices(loanedBooks)
        );

        try
        {
            loanService.ReturnBook(student.Id, book.Id);
            AnsiConsole.MarkupLine("[green]Book returned successfully![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }

        Pause();
    }

    private static void Pause()
    {
        AnsiConsole.MarkupLine("\n[grey]Press any key to continue...[/]");
        Console.ReadKey(true);
    }
}