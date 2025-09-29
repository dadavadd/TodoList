
using TodoList.Models;

namespace TodoList;

public class TodoService
{
    private List<Todo> _todos;
    private int _nextId;

    public TodoService()
    {
        _todos = ConfigManager.GetTodosFromSaved();
        _nextId = _todos.Count > 0 ? _todos.Max(t => t.Id) + 1 : 1;
    }

    public List<Todo> GetTodos()
    {
        return _todos;
    }

    public void AddTodo(string title, string description)
    {
        var newTodo = Todo.Create(title, description);
        newTodo.Id = _nextId++;
        _todos.Add(newTodo);
    }

    public void MarkTodoAsCompleted(Todo todoToMark)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == todoToMark.Id);
        if (todo != null)
        {
            todo.MarkAsCompleted();
        }
    }

    public void DeleteTodo(Todo todoToDelete)
    {
        _todos.Remove(todoToDelete);
    }

    public void SaveChanges()
    {
        ConfigManager.SaveTodos(_todos);
    }
}
