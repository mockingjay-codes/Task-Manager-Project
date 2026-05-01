using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.Models;

namespace TaskManager.Api.Services;

// Handle all database operations for tasks

public class TaskService(AppDbContext db)
{
    // Get all tasks, sort by due date
    public async Task<List<TaskItem>> GetAllAsync() =>
        await db.Tasks.OrderBy(t => t.DueDate).ToListAsync();

    // Get a single task by ID
    public async Task<TaskItem?> GetByIdAsync(int id) =>
        await db.Tasks.FindAsync(id);

    // Create new task
    // Default status to pending
    public async Task<TaskItem> CreateAsync(string title, string? description, DateTime? dueDate)
    {
        TaskItem task = new TaskItem
        {
            Title = title,
            Description = description,
            DueDate = dueDate,
            Status = "Pending",
            CreatedOn = DateTime.UtcNow
        };
        db.Tasks.Add(task);
        await db.SaveChangesAsync();
        return task;
    }

    // Update status of existing task
    // Return null if task not found
    public async Task<TaskItem?> UpdateStatusAsync(int id, string newStatus)
    {
        TaskItem? task = await db.Tasks.FindAsync(id);
        if (task is null) return null;
        task.Status = newStatus;
        await db.SaveChangesAsync();
        return task;
    }

    // Delete task
    // Return true if deleted, false if not found
    public async Task<bool> DeleteAsync(int id)
    {
        TaskItem? task = await db.Tasks.FindAsync(id);
        if (task is null) return false;
        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return true;
    }

}
