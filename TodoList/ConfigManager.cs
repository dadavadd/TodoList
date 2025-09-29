using System.Text.Json;
using TodoList.Models;

namespace TodoList;

public class ConfigManager
{
    private readonly static JsonSerializerOptions s_jsonSerializerOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public static List<Todo> GetTodosFromSaved(string configPath = "todoConfig.json")
    {
        if (!File.Exists(configPath))
            return GetDefaultConfig();

        var fileContent = File.ReadAllText(configPath);

        if (!string.IsNullOrWhiteSpace(fileContent))
            return JsonSerializer.Deserialize<List<Todo>>(fileContent, s_jsonSerializerOptions) ?? GetDefaultConfig();

        return GetDefaultConfig();
    }

    public static void SaveTodos(List<Todo> todos, string configPath = "todoConfig.json")
    {
        var jsonString = JsonSerializer.Serialize(todos, s_jsonSerializerOptions);
        File.WriteAllText(configPath, jsonString);
    }

    private static List<Todo> GetDefaultConfig()
        => [Todo.Create("New todo", "Todo description")];
}