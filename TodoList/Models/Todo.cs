using System.Text.Json.Serialization;

namespace TodoList.Models;

public class Todo
{
    [JsonConstructor]
    private Todo() { }

    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    public static Todo Create(string title, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title, nameof(title));

        return new()
        {
            Title = title,
            Description = description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateTitle(string newTitle)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newTitle, nameof(newTitle));
        Title = newTitle;
    }

    public void UpdateDescription(string newDescription)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newDescription, nameof(newDescription));
        Description = newDescription;
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
    }

    public void MarkAsPending()
    {
        IsCompleted = false;
        CompletedAt = null;
    }

    public override string ToString() => $"{Id}: {Title} (Completed: {IsCompleted})";
}
