
using TodoList;
using TodoList.Models;

namespace TodoList.Tests;

public class TodoServiceTests
{
    [Fact]
    public void AddTodo_ShouldAddTodoToList()
    {
        // Arrange
        var todoService = new TodoService();
        var initialCount = todoService.GetTodos().Count;

        // Act
        todoService.AddTodo("Test Todo", "Test Description");

        // Assert
        var todos = todoService.GetTodos();
        Assert.Equal(initialCount + 1, todos.Count);
        Assert.Equal("Test Todo", todos.Last().Title);
    }

    [Fact]
    public void MarkTodoAsCompleted_ShouldMarkTodoAsCompleted()
    {
        // Arrange
        var todoService = new TodoService();
        todoService.AddTodo("Test Todo", "Test Description");
        var todo = todoService.GetTodos().Last();

        // Act
        todoService.MarkTodoAsCompleted(todo);

        // Assert
        Assert.True(todo.IsCompleted);
        Assert.NotNull(todo.CompletedAt);
    }

    [Fact]
    public void DeleteTodo_ShouldRemoveTodoFromList()
    {
        // Arrange
        var todoService = new TodoService();
        todoService.AddTodo("Test Todo", "Test Description");
        var todo = todoService.GetTodos().Last();
        var initialCount = todoService.GetTodos().Count;

        // Act
        todoService.DeleteTodo(todo);

        // Assert
        var todos = todoService.GetTodos();
        Assert.Equal(initialCount - 1, todos.Count);
        Assert.DoesNotContain(todo, todos);
    }
}
