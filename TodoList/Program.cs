using Spectre.Console;
using TodoList;
using TodoList.Models;

const string AddOption = "Add";
const string MarkAsCompletedOption = "Mark as completed";
const string SetDueDateOption = "Set Due Date";
const string DeleteOption = "Delete";
const string ExitOption = "Exit";

Console.Title = "Todo List Application";

try
{
    Console.SetWindowSize(150, 30);
}
catch (IOException) { }

var todoService = new TodoService();

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
            .AddChoices(
            [
                AddOption, MarkAsCompletedOption, SetDueDateOption, DeleteOption, ExitOption
            ]));

    switch (choice)
    {
        case AddOption:
            AddTodo();
            break;
        case MarkAsCompletedOption:
            MarkTodoAsCompleted();
            break;
        case SetDueDateOption:
            SetDueDate();
            break;
        case DeleteOption:
            DeleteTodo();
            break;
        case ExitOption:
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
    var dueAtInput = AnsiConsole.Ask<string>("What is the due date of the new todo? (optional, format: yyyy-MM-dd HH:mm) or leave empty");

    DateTime? dueAt = null;
    if (!string.IsNullOrWhiteSpace(dueAtInput) && DateTime.TryParse(dueAtInput, out var parsedDueAt))
    {
        dueAt = parsedDueAt;
    }
    else if (!string.IsNullOrWhiteSpace(dueAtInput))
    {
        AnsiConsole.MarkupLine("[red]Invalid date format. Due date was not set.[/]");
        AnsiConsole.Confirm("Press any key to continue...");
    }

    try
    {
        todoService.AddTodo(title, description, dueAt);
    }
    catch (ArgumentException ex)
    {
        AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
        AnsiConsole.Confirm("Press any key to continue...");
    }
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
        try
        {
            todoService.SetDueDate(todoToUpdate, dueAt);
        }
        catch (ArgumentException ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
            AnsiConsole.Confirm("Press any key to continue...");
        }
    }
    else
    {
        AnsiConsole.MarkupLine("[red]Invalid date format.[/]");
        AnsiConsole.Confirm("Press any key to continue...");
    }
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