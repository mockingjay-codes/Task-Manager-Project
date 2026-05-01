using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Services;
using TaskManager.Api.Models;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TasksController(TaskService taskService) : ControllerBase
{
    // GET /api/tasks
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await taskService.GetAllAsync()); 

    // GET /api/tasks/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        TaskItem? task = await taskService.GetByIdAsync(id); 
        
        if (task is null) return NotFound();
        return Ok(task);
    }

    // POST /api/tasks
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title)) return BadRequest("Title is required");

        TaskItem? task = await taskService.CreateAsync(request.Title, request.Description, request.DueDate);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);

    }

    // PATCH /api/tasks/{id}/status
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NewStatus)) return BadRequest("Status is required");

        TaskItem? updatedTask = await taskService.UpdateStatusAsync(id, request.NewStatus);
    
        if (updatedTask is null) return NotFound();
        return Ok(updatedTask);

    }

    // DELETE /api/tasks/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await taskService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}

// Define the expected JSON shape for a task creation
public record CreateTaskRequest(string Title, string? Description, DateTime DueDate);
public record UpdateStatusRequest(string NewStatus);

