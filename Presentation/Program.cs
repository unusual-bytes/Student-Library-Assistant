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
    static async Task Main()
    {
        bool running = true;
        Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight + 10);

        var studentRepo = new StudentRepository();
        var bookRepo = new BookRepository();
        var loansRepo = new LoanRepository();

        var bookService = new BookService(bookRepo, loansRepo);
        var studentService = new StudentService(studentRepo, loansRepo);
        var loanService = new LoanService(loansRepo, bookRepo, studentRepo);

        await ShowStartupAnimation();

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

    static async Task ShowStartupAnimation()
    {
        string fullText = "Student & Library Assistant";
        string current = "";

        await AnsiConsole.Live(new FigletText("") { Color = Color.Cyan })
            .StartAsync(async ctx =>
            {
                foreach (char c in fullText)
                {
                    current += c;
                    ctx.UpdateTarget(new FigletText(current) { Color = Color.Cyan });
                    await Task.Delay(80); // animate one char at a time
                }

                await Task.Delay(500); // pause on full text
            });
    }
    
    public static void TaskComplete()
    {
        AnsiConsole.MarkupLine("\n[grey]Press any key to continue...[/]");
        Console.ReadKey(true);
    }
}