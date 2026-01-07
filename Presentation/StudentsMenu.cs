using Spectre.Console;
using Student_Library_Assistant.Application.Services;
using Student_Library_Assistant.Domain.Entities;
using Student_Library_Assistant.Infrastructure.Repositories;

namespace Student_Library_Assistant.Presentation;

public class StudentsMenu
{
    public static void ShowMenu(StudentService studentService)
    {
        bool running = true;

        while (running)
        {
            AnsiConsole.Clear();

            // Title
            var title = new FigletText("Students")
                .Centered()
                .Color(Color.Cyan);
            AnsiConsole.Write(title);
            AnsiConsole.WriteLine();

            var students = studentService.GetAllStudents();

            // Students table
            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("Index");
            table.AddColumn("Name");
            table.AddColumn("Year");
            table.AddColumn("Faculty Number");
            table.AddColumn("Borrowed Books");

            for (int i = 0; i < students.Count; i++)
            {
                var s = students[i];
                table.AddRow(
                    i.ToString(),
                    s.Name,
                    s.Year.ToString(),
                    s.FacultyNumber,
                    s.BorrowedBooks.ToString()
                );
            }

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();

            // Bottom menu
            var action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[cyan]Choose action[/]")
                    .AddChoices(
                        "Add student",
                        "Remove student",
                        "Back"
                    )
            );

            switch (action)
            {
                case "Add student":
                    AddStudent(studentService);
                    break;

                case "Remove student":
                    RemoveStudent(studentService);
                    break;

                case "Back":
                    running = false;
                    break;
            }
        }
    }

    static void AddStudent(StudentService service)
    {
        string name = AnsiConsole.Ask<string>("Enter student [green]name[/]:");
        int year = AnsiConsole.Ask<int>("Enter student [green]year[/]:");
        string faculty = AnsiConsole.Ask<string>("Enter [green]faculty number[/]:");

        int nextId = 1; // default if no students exist
        
        var students = service.GetAllStudents();
        if (students.Any()) // if students exist
            nextId = students.Max(b => b.Id) + 1;

        
        var newStudent = new Student
        {
            Id = nextId,
            Name = name,
            Year = year,
            FacultyNumber = faculty
            // BorrowedBooks is automatically set by StudentService
        };

        service.AddStudent(newStudent);
        AnsiConsole.MarkupLine("[green]Student added successfully![/]");
        Program.TaskComplete();
    }

    static void RemoveStudent(StudentService service)
    {
        var students = service.GetAllStudents();
        if (!students.Any())
        {
            AnsiConsole.MarkupLine("[red]No students available.[/]");
            Program.TaskComplete();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<Student>()
                .Title("[red]Select student to remove[/]")
                .UseConverter(s => $"{s.Name} ({s.FacultyNumber})")
                .AddChoices(students)
        );

        try
        {
            if (AnsiConsole.Confirm($"Delete [red]{selected.Name}[/]?"))
            {
                service.DeleteStudent(selected.Id);
                AnsiConsole.MarkupLine("[green]Student removed successfully![/]");
            }
        }
        catch (Exception ex)
        {
            // Handles the case where the student has borrowed books
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }

        Program.TaskComplete();
    }
}
