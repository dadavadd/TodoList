
using TodoList.Models;

namespace TodoList.Tests;

public class TodoTests
{
    [Fact]
    public void Create_ShouldCreateTodo()
    {
        // Arrange
        var title = "Test Todo";
        var description = "Test Description";

        // Act
        var todo = Todo.Create(title, description);

        // Assert
        Assert.Equal(title, todo.Title);
        Assert.Equal(description, todo.Description);
        Assert.False(todo.IsCompleted);
        Assert.Null(todo.CompletedAt);
    }

    [Fact]
    public void UpdateTitle_ShouldUpdateTitle()
    {
        // Arrange
        var todo = Todo.Create("Initial Title", "Initial Description");
        var newTitle = "Updated Title";

        // Act
        todo.UpdateTitle(newTitle);

        // Assert
        Assert.Equal(newTitle, todo.Title);
    }

    [Fact]
    public void UpdateDescription_ShouldUpdateDescription()
    {
        // Arrange
        var todo = Todo.Create("Initial Title", "Initial Description");
        var newDescription = "Updated Description";

        // Act
        todo.UpdateDescription(newDescription);

        // Assert
        Assert.Equal(newDescription, todo.Description);
    }

    [Fact]
    public void MarkAsCompleted_ShouldMarkTodoAsCompleted()
    {
        // Arrange
        var todo = Todo.Create("Test Todo", "Test Description");

        // Act
        todo.MarkAsCompleted();

        // Assert
        Assert.True(todo.IsCompleted);
        Assert.NotNull(todo.CompletedAt);
    }

    [Fact]
    public void MarkAsPending_ShouldMarkTodoAsPending()
    {
        // Arrange
        var todo = Todo.Create("Test Todo", "Test Description");
        todo.MarkAsCompleted();

        // Act
        todo.MarkAsPending();

        // Assert
        Assert.False(todo.IsCompleted);
        Assert.Null(todo.CompletedAt);
    }
}
