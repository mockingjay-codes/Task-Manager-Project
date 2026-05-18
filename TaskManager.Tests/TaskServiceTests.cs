using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.Services;

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
        Assert.Equal("Pending", task.Status);
        Assert.Equal(1, await db.Tasks.CountAsync());
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

        var result = await service.UpdateStatusAsync(created.Id, "InProgress");

        Assert.NotNull(result);
        Assert.Equal("InProgress", result.Status);
    }

    [Fact]
    public async Task UpdateStatusAsync_ReturnsNull_WhenNotFound()
    {
        using var db = CreateDb();
        var service = new TaskService(db);

        var result = await service.UpdateStatusAsync(999, "Completed");

        Assert.Null(result);
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