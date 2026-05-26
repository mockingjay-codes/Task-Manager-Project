namespace TaskManager.Api.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } 
    public TaskStatus Status { get; set; } = TaskStatus.Pending;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow; 
    public DateTime? DueDate { get; set; }
}