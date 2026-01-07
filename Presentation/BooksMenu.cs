using Student_Library_Assistant.Application.Services;
using Student_Library_Assistant.Domain.Entities;
using Student_Library_Assistant.Infrastructure.Repositories;

namespace Student_Library_Assistant.Presentation;
using Spectre.Console;

public class BooksMenu
{
    public static void ShowMenu(BookService bookService)
    {
        bool running = true;

        while (running)
        {
            AnsiConsole.Clear();

            // Title
            var title = new FigletText("Books")
                .Centered()
                .Color(Color.Cyan);
            AnsiConsole.Write(title);
            AnsiConsole.WriteLine();

            var books = bookService.GetAllBooks();

            // Books table
            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("Index");
            table.AddColumn("Title");
            table.AddColumn("Author");
            table.AddColumn("Year");
            table.AddColumn("Available");

            for (int i = 0; i < books.Count; i++)
            {
                var b = books[i];
                table.AddRow(
                    i.ToString(),
                    b.Title,
                    b.Author,
                    b.Year.ToString(),
                    b.IsAvailable ? "[green]Yes[/]" : "[red]No[/]"
                );
            }

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();

            // Bottom menu
            var action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[cyan]Choose action[/]")
                    .AddChoices(
                        "Add book",
                        "Remove book",
                        "Back"
                    )
            );

            switch (action)
            {
                case "Add book":
                    AddBook(bookService);
                    break;

                case "Remove book":
                    RemoveBook(bookService);
                    break;

                case "Back":
                    running = false;
                    break;
            }
        }
    }

    static void AddBook(BookService service)
    {
        string title = AnsiConsole.Ask<string>("Enter book [green]title[/]:");
        string author = AnsiConsole.Ask<string>("Enter book [green]author[/]:");
        int year = AnsiConsole.Ask<int>("Enter book [green]year[/]:");

        int nextId = 1; // default if no books exist
        
        var books = service.GetAllBooks();
        if (books.Any()) // if books exist
            nextId = books.Max(b => b.Id) + 1;
        
        
        var newBook = new Book
        {
            Id = nextId,
            Title = title,
            Author = author,
            Year = year
            // IsAvailable is automatically set by BookService
        };

        service.AddBook(newBook);
        AnsiConsole.MarkupLine("[green]Book added successfully![/]");
        Pause();
    }

    static void RemoveBook(BookService service)
    {
        var books = service.GetAllBooks();
        if (!books.Any())
        {
            AnsiConsole.MarkupLine("[red]No books available.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<Book>()
                .Title("[red]Select book to remove[/]")
                .UseConverter(b => $"{b.Title} ({b.Author})")
                .AddChoices(books)
        );

        try
        {
            if (AnsiConsole.Confirm($"Delete [red]{selected.Title}[/]?"))
            {
                service.DeleteBook(selected.Id);
                AnsiConsole.MarkupLine("[green]Book removed.[/]");
            }
        }
        catch (Exception ex)
        {
            // Handles the case where the book is loaned
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }

        Pause();
    }

    static void Pause()
    {
        AnsiConsole.MarkupLine("\n[grey]Press any key to continue...[/]");
        Console.ReadKey(true);
    }
}