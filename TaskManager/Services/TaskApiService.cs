using System.Net.Http.Json;
using TaskManager.Models;

namespace TaskManager.Services;

public class TaskApiService(IHttpClientFactory httpClientFactory)
{
    private HttpClient HttpClient => httpClientFactory.CreateClient("TaskAPI");

    public async Task<List<TaskItem>> GetAllTasksAsync() =>
        await HttpClient.GetFromJsonAsync<List<TaskItem>>("api/tasks") ?? new List<TaskItem>();

    public async Task<TaskItem?> GetTaskByIdAsync(int id) =>
        await HttpClient.GetFromJsonAsync<TaskItem>($"api/tasks/{id}");

    public async Task<TaskItem?> CreateTaskAsync(string title, string? description, DateTime? dueDate)
    {
        var response = await HttpClient.PostAsJsonAsync("api/tasks", new
        {
            Title = title,
            Description = description,
            DueDate = dueDate
        });
        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<TaskItem>();
        return null;
    }

    public async Task<TaskItem?> UpdateTaskStatusAsync(int id, string newStatus)
    {
        var response = await HttpClient.PatchAsJsonAsync($"api/tasks/{id}/status", new
        {
            NewStatus = newStatus
        });
        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<TaskItem>();
        return null;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var response = await HttpClient.DeleteAsync($"api/tasks/{id}");
        return response.IsSuccessStatusCode;
    }
}