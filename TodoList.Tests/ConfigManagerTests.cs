using TodoList.Models;
using System.Text.Json;

namespace TodoList.Tests;

public class ConfigManagerTests
{
    private readonly string _testConfigPath = "test_todo_config.json";

    [Fact]
    public void SaveTodos_ShouldWriteTodosToFile()
    {
        // Arrange
        var todos = new List<Todo>
        {
            Todo.Create("Test Todo 1", "Description 1"),
            Todo.Create("Test Todo 2", "Description 2")
        };

        // Act
        ConfigManager.SaveTodos(todos, _testConfigPath);

        // Assert
        var fileContent = File.ReadAllText(_testConfigPath);
        var savedTodos = JsonSerializer.Deserialize<List<Todo>>(fileContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(savedTodos);
        Assert.Equal(todos.Count, savedTodos.Count);

        // Clean up
        File.Delete(_testConfigPath);
    }

    [Fact]
    public void GetTodosFromSaved_ShouldReturnTodos_WhenFileExists()
    {
        // Arrange
        var todos = new List<Todo>
        {
            Todo.Create("Test Todo 1", "Description 1"),
            Todo.Create("Test Todo 2", "Description 2")
        };
        ConfigManager.SaveTodos(todos, _testConfigPath);

        // Act
        var loadedTodos = ConfigManager.GetTodosFromSaved(_testConfigPath);

        // Assert
        Assert.NotNull(loadedTodos);
        Assert.Equal(todos.Count, loadedTodos.Count);

        // Clean up
        File.Delete(_testConfigPath);
    }

    [Fact]
    public void GetTodosFromSaved_ShouldReturnDefaultConfig_WhenFileDoesNotExist()
    {
        // Arrange
        if (File.Exists(_testConfigPath))
        {
            File.Delete(_testConfigPath);
        }

        // Act
        var loadedTodos = ConfigManager.GetTodosFromSaved(_testConfigPath);

        // Assert
        Assert.NotNull(loadedTodos);
        Assert.Single(loadedTodos);
        Assert.Equal("New todo", loadedTodos[0].Title);
    }

    [Fact]
    public void GetTodosFromSaved_ShouldReturnDefaultConfig_WhenFileIsEmpty()
    {
        // Arrange
        File.WriteAllText(_testConfigPath, string.Empty);

        // Act
        var loadedTodos = ConfigManager.GetTodosFromSaved(_testConfigPath);

        // Assert
        Assert.NotNull(loadedTodos);
        Assert.Single(loadedTodos);
        Assert.Equal("New todo", loadedTodos[0].Title);

        // Clean up
        File.Delete(_testConfigPath);
    }
}
