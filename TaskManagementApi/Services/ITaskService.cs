using TaskManagementApi.Models;

namespace TaskManagementApi.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(int projectId);
        Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(int userId);
        Task<TaskItem?> GetTaskByIdAsync(int id);
        Task<TaskItem> CreateTaskAsync(TaskItem task);
        Task<TaskItem?> UpdateTaskAsync(int id, TaskItem task);
        Task<bool> DeleteTaskAsync(int id);
        Task<bool> TaskExistsAsync(int id);
        Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(Models.TaskStatus status);
        Task<IEnumerable<TaskItem>> GetTasksByPriorityAsync(Models.TaskPriority priority);
        Task<IEnumerable<TaskItem>> GetOverdueTasksAsync();
    }
}