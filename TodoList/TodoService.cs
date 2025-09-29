using TodoList.Models;

namespace TodoList;

public class TodoService
{
    private readonly List<Todo> _todos;
    private int _nextId;

    public TodoService()
    {
        _todos = ConfigManager.GetTodosFromSaved();
        _nextId = _todos.Count > 0 ? _todos.Max(t => t.Id) + 1 : 1;
    }

    public List<Todo> GetTodos() 
        => _todos;

    public void AddTodo(string title, string description, DateTime? dueAt = null)
    {
        var newTodo = Todo.Create(title, description, dueAt);
        newTodo.Id = _nextId++;
        _todos.Add(newTodo);
    }

    public void MarkTodoAsCompleted(Todo todoToMark)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == todoToMark.Id);

        if (todo != null)
            todo.MarkAsCompleted();
    }

    public void SetDueDate(Todo todoToUpdate, DateTime dueDate)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == todoToUpdate.Id);

        if (todo != null)
            todo.SetDueDate(dueDate);
    }

    public void DeleteTodo(Todo todoToDelete) 
        => _todos.Remove(todoToDelete);

    public void SaveChanges() 
        => ConfigManager.SaveTodos(_todos);
}
