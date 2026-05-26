using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TaskManager.Models;
using TaskStatus = TaskManager.Models.TaskStatus;

namespace TaskManager.Services;

public class TaskApiService(IHttpClientFactory httpClientFactory)
{
    private HttpClient HttpClient => httpClientFactory.CreateClient("TaskAPI");

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    public async Task<List<TaskItem>> GetAllTasksAsync()
    {
        var response = await HttpClient.GetAsync("api/tasks");
        if (!response.IsSuccessStatusCode) return new List<TaskItem>();
        return await response.Content.ReadFromJsonAsync<List<TaskItem>>(JsonOptions) ?? new List<TaskItem>();
    }

    public async Task<TaskItem?> GetTaskByIdAsync(int id)
    {
        var response = await HttpClient.GetAsync($"api/tasks/{id}");
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<TaskItem>(JsonOptions);
    }

    public async Task<TaskItem?> CreateTaskAsync(string title, string? description, DateTime? dueDate)
    {
        var response = await HttpClient.PostAsJsonAsync("api/tasks", new
        {
            Title = title,
            Description = description,
            DueDate = dueDate
        });
        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<TaskItem>(JsonOptions);
        return null;
    }

    public async Task<TaskItem?> UpdateTaskStatusAsync(int id, TaskStatus newStatus)
    {
        var response = await HttpClient.PatchAsJsonAsync($"api/tasks/{id}/status", new
        {
            NewStatus = newStatus
        }, JsonOptions);
        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<TaskItem>(JsonOptions);
        return null;
    }

    public async Task<TaskItem?> UpdateTaskAsync(int id, string title, string? description, DateTime? dueDate, TaskStatus status)
    {
        var response = await HttpClient.PutAsJsonAsync($"api/tasks/{id}", new
        {
            Title = title,
            Description = description,
            DueDate = dueDate,
            Status = status
        }, JsonOptions);
        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<TaskItem>(JsonOptions);
        return null;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var response = await HttpClient.DeleteAsync($"api/tasks/{id}");
        return response.IsSuccessStatusCode;
    }
}