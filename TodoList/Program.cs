using Spectre.Console;
using TodoList;
using TodoList.Models;

using static TodoList.Resources.Literals;

Console.Title = ConsoleTitle;
Console.SetWindowSize(150, 30);

var todoService = new TodoService();

AppDomain.CurrentDomain.UnhandledException += (o, e) =>
{
    if (e.ExceptionObject is Exception exception)
    {
        AnsiConsole.WriteException(exception, ExceptionFormats.ShortenEverything | ExceptionFormats.ShowLinks);
        todoService.SaveChanges();
        AnsiConsole.Confirm("[red]Press any key to exit...[/]");
        Environment.Exit(1);
    }
};

while (true)
{
    AnsiConsole.Clear();
    var todos = todoService.GetTodos();
    DisplayTodos(todos);

    var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[red]What do you want to do?[/]")
            .PageSize(10)
            .HighlightStyle(new(foreground: Color.Yellow, decoration: Decoration.Bold))
            .AddChoices([AddOption, MarkAsCompletedOption, SetDueDateOption, DeleteOption, ExitOption]));

    switch (choice)
    {
        case var opt when opt == AddOption:
            AddTodo();
            break;
        case var opt when opt == MarkAsCompletedOption:
            MarkTodoAsCompleted();
            break;
        case var opt when opt == SetDueDateOption:
            SetDueDate();
            break;
        case var opt when opt == DeleteOption:
            DeleteTodo();
            break;
        case var opt when opt == ExitOption:
            todoService.SaveChanges();
            return;
    }
}


void DisplayTodos(List<Todo> todos)
{
    var table = new Table();
    table.AddColumn("Id");
    table.AddColumn("Title");
    table.AddColumn("Description");
    table.AddColumn("Completed");
    table.AddColumn("Created At");
    table.AddColumn("Completed At");
    table.AddColumn("Due At");
    table.AddColumn("Is Overdue");
    table.Border = TableBorder.Rounded;

    foreach (var todo in todos)
    {
        table.AddRow(
            todo.Id.ToString(),
            todo.Title,
            todo.Description,
            todo.IsCompleted ? "[green]Yes[/]" : "[red]No[/]",
            todo.CreatedAt.ToString(),
            todo.CompletedAt?.ToString() ?? string.Empty,
            todo.DueAt?.ToString() ?? string.Empty,
            todo.IsOverdue() ? "[red]Yes[/]" : "No"
        );
    }
    AnsiConsole.Write(table);
}

void AddTodo()
{
    var title = AnsiConsole.Ask<string>("What is the title of the new todo?");
    var description = AnsiConsole.Ask<string>("What is the description of the new todo?");
    var dueAtInput = AnsiConsole.Ask<string>("What is the due date of the new todo? (optional, format: yyyy-MM-dd HH:mm) or input something shit:");

    DateTime? dueAt = null;

    if (DateTime.TryParse(dueAtInput, out var parsedDueAt))
        dueAt = parsedDueAt != DateTime.MinValue ? parsedDueAt : null;
    
    todoService.AddTodo(title, description, dueAt);
}

void SetDueDate()
{
    var todos = todoService.GetTodos();
    var todoToUpdate = AnsiConsole.Prompt(
        new SelectionPrompt<Todo>()
            .Title("Which todo do you want to set the due date for?")
            .PageSize(10)
            .UseConverter(todo => $"{todo.Id}: {todo.Title}")
            .AddChoices(todos));

    var dueAtInput = AnsiConsole.Ask<string>("What is the new due date? (format: yyyy-MM-dd HH:mm)");

    if (DateTime.TryParse(dueAtInput, out var dueAt))
    {
        todoService.SetDueDate(todoToUpdate, dueAt);
        return;
    }

    AnsiConsole.MarkupLine("[red]Invalid date format.[/]");
    AnsiConsole.Confirm("Press any key to continue...");
}

void MarkTodoAsCompleted()
{
    var todos = todoService.GetTodos();
    var todoToMark = AnsiConsole.Prompt(
        new SelectionPrompt<Todo>()
            .Title("Which todo do you want to mark as completed?")
            .PageSize(10)
            .UseConverter(todo => $"{todo.Id}: {todo.Title}")
            .AddChoices(todos.Where(t => !t.IsCompleted)));

    todoService.MarkTodoAsCompleted(todoToMark);
}

void DeleteTodo()
{
    var todos = todoService.GetTodos();
    var todoToDelete = AnsiConsole.Prompt(
        new SelectionPrompt<Todo>()
            .Title("Which todo do you want to delete?")
            .PageSize(10)
            .UseConverter(todo => $"{todo.Id}: {todo.Title}")
            .AddChoices(todos));

    todoService.DeleteTodo(todoToDelete);
}