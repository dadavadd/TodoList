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
        todo.UpdateContent(newTitle);

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
        todo.UpdateContent(null, newDescription);

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

    [Fact]
    public void SetDueDate_ShouldSetDueDate()
    {
        // Arrange
        var todo = Todo.Create("Test Todo", "Test Description");
        var dueDate = DateTime.UtcNow.AddDays(5);

        // Act
        todo.SetDueDate(dueDate);

        // Assert
        Assert.Equal(dueDate, todo.DueAt);
    }

    [Fact]
    public void ClearDueDate_ShouldClearDueDate()
    {
        // Arrange
        var todo = Todo.Create("Test Todo", "Test Description");
        var dueDate = DateTime.UtcNow.AddDays(5);
        todo.SetDueDate(dueDate);

        // Act
        todo.ClearDueDate();

        // Assert
        Assert.Null(todo.DueAt);
    }

    [Fact]
    public void IsOverdue_ShouldReturnTrueIfOverdue()
    {   
        // Arrange
        var todo = Todo.Create("Test Todo", "Test Description");
        var dueDate = DateTime.UtcNow.AddDays(-1);
        todo.SetDueDate(dueDate);

        // Act
        var isOverdue = todo.IsOverdue();

        // Assert
        Assert.True(isOverdue);
    }
}
