using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.Models;
using TaskManager.Api.Services;
using TaskStatus = TaskManager.Api.Models.TaskStatus;

namespace TaskManager.Tests;

public class TaskServiceTests
{
    private static AppDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmpty_WhenNoTasks()
    {
        using var db = CreateDb();
        var service = new TaskService(db);

        var result = await service.GetAllAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task CreateAsync_AddsTask_WithPendingStatus()
    {
        using var db = CreateDb();
        var service = new TaskService(db);

        var task = await service.CreateAsync("Test task", "desc", DateTime.UtcNow.AddDays(1));

        Assert.Equal("Test task", task.Title);
        Assert.Equal(TaskStatus.Pending, task.Status);
    }

    [Fact]
    public async Task CreateAsync_PersistsToDatabase()
    {
        using var db = CreateDb();
        var service = new TaskService(db);

        await service.CreateAsync("Test task", "desc", DateTime.UtcNow.AddDays(1));

        Assert.Equal(1, await db.Tasks.CountAsync());
    }

    [Fact]
    public async Task CreateAsync_Throws_WhenTitleIsEmpty()
    {
        using var db = CreateDb();
        var service = new TaskService(db);

        await Assert.ThrowsAsync<ArgumentException>(
            () => service.CreateAsync("", null, null));
    }

    [Fact]
    public async Task CreateAsync_Throws_WhenTitleIsWhitespace()
    {
        using var db = CreateDb();
        var service = new TaskService(db);

        await Assert.ThrowsAsync<ArgumentException>(
            () => service.CreateAsync("   ", null, null));
    }

    [Fact]
    public async Task CreateAsync_Throws_WhenDueDateIsNull()
    {
        using var db = CreateDb();
        var service = new TaskService(db);

        await Assert.ThrowsAsync<ArgumentException>(
            () => service.CreateAsync("Title", null, null));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        using var db = CreateDb();
        var service = new TaskService(db);

        var result = await service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsTask_WhenFound()
    {
        using var db = CreateDb();
        var service = new TaskService(db);
        var created = await service.CreateAsync("Find me", null, DateTime.UtcNow.AddDays(1));

        var result = await service.GetByIdAsync(created.Id);

        Assert.NotNull(result);
        Assert.Equal("Find me", result.Title);
    }

    [Fact]
    public async Task UpdateStatusAsync_UpdatesStatus_WhenFound()
    {
        using var db = CreateDb();
        var service = new TaskService(db);
        var created = await service.CreateAsync("Update me", null, DateTime.UtcNow.AddDays(1));

        var result = await service.UpdateStatusAsync(created.Id, TaskStatus.InProgress);

        Assert.NotNull(result);
        Assert.Equal(TaskStatus.InProgress, result.Status);
    }

    [Fact]
    public async Task UpdateStatusAsync_ReturnsNull_WhenNotFound()
    {
        using var db = CreateDb();
        var service = new TaskService(db);

        var result = await service.UpdateStatusAsync(999, TaskStatus.Completed);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesTask_WhenFound()
    {
        using var db = CreateDb();
        var service = new TaskService(db);
        var created = await service.CreateAsync("Old Title", "Old Desc", DateTime.UtcNow.AddDays(1));

        var result = await service.UpdateAsync(created.Id, "New Title", "New Desc", DateTime.UtcNow.AddDays(5), TaskStatus.Cancelled);

        Assert.NotNull(result);
        Assert.Equal("New Title", result.Title);
        Assert.Equal("New Desc", result.Description);
        Assert.Equal(TaskStatus.Cancelled, result.Status);
        Assert.Equal(DateTime.UtcNow.AddDays(5).Date, result.DueDate!.Value.Date);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenNotFound()
    {
        using var db = CreateDb();
        var service = new TaskService(db);

        var result = await service.UpdateAsync(999, "Title", "Desc", DateTime.UtcNow, TaskStatus.Pending);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenDueDateIsNull()
    {
        using var db = CreateDb();
        var service = new TaskService(db);
        var created = await service.CreateAsync("Title", null, DateTime.UtcNow.AddDays(1));

        await Assert.ThrowsAsync<ArgumentException>(
            () => service.UpdateAsync(created.Id, "Title", null, null, TaskStatus.Pending));
    }

    [Fact]
    public async Task DeleteAsync_RemovesTask_WhenFound()
    {
        using var db = CreateDb();
        var service = new TaskService(db);
        var created = await service.CreateAsync("Delete me", null, DateTime.UtcNow.AddDays(1));

        var result = await service.DeleteAsync(created.Id);

        Assert.True(result);
        Assert.Equal(0, await db.Tasks.CountAsync());
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
    {
        using var db = CreateDb();
        var service = new TaskService(db);

        var result = await service.DeleteAsync(999);

        Assert.False(result);
    }
}
