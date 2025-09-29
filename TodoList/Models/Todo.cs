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
    public DateTime? DueAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public static Todo Create(string title, string description, DateTime? dueAt = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title, nameof(title));

        return new()
        {
            Title = title,
            Description = description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            DueAt = dueAt
        };
    }

    public void UpdateContent(string? newTitle = null, string? newDescription = null)
    {
        if (!string.IsNullOrWhiteSpace(newTitle))
            Title = newTitle;
        if (!string.IsNullOrWhiteSpace(newDescription))
            Description = newDescription;
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
    }

    public void SetDueDate(DateTime dueDate)
    {
        if (dueDate < CreatedAt)
            throw new ArgumentException("Due date cannot be earlier than the creation date.", nameof(dueDate));

        DueAt = dueDate;
    }

    public void ClearDueDate() => DueAt = null;
    public bool IsOverdue() => DueAt.HasValue && !IsCompleted && DateTime.UtcNow > DueAt.Value;

    public void MarkAsPending()
    {
        IsCompleted = false;
        CompletedAt = null;
    }

    public override string ToString() => $"{Id}: {Title} (Completed: {IsCompleted})";
}
