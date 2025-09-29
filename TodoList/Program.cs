using Spectre.Console;
using TodoList;
using TodoList.Models;

const string AddOption = "Add";
const string MarkAsCompletedOption = "Mark as completed";
const string DeleteOption = "Delete";
const string ExitOption = "Exit";

Console.Title = "Todo List Application";

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
            .HighlightStyle(new Style(foreground: Color.Yellow, decoration: Decoration.Bold))
            .AddChoices(
            [
                AddOption, MarkAsCompletedOption, DeleteOption, ExitOption
            ]));

    switch (choice)
    {
        case AddOption:
            AddTodo();
            break;
        case MarkAsCompletedOption:
            MarkTodoAsCompleted();
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

    foreach (var todo in todos)
    {
        table.AddRow(
            todo.Id.ToString(),
            todo.Title,
            todo.Description,
            todo.IsCompleted ? "[green]Yes[/]" : "[red]No[/]",
            todo.CreatedAt.ToString(),
            todo.CompletedAt?.ToString() ?? string.Empty
        );
    }

    AnsiConsole.Write(table);
}

void AddTodo()
{
    var title = AnsiConsole.Ask<string>("What is the title of the new todo?");
    var description = AnsiConsole.Ask<string>("What is the description of the new todo?");
    todoService.AddTodo(title, description);
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