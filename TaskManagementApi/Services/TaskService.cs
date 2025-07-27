using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace TaskManagementApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskManagementContext _context;
        
        public TaskService(TaskManagementContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(int projectId)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(int userId)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .Where(t => t.AssignedUserId == userId)
                .ToListAsync();
        }
        
        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
        
        public async Task<TaskItem> CreateTaskAsync(TaskItem task)
        {
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;
            
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            
            return task;
        }
        
        public async Task<TaskItem?> UpdateTaskAsync(int id, TaskItem task)
        {
            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return null;
            }
            
            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Priority = task.Priority;
            existingTask.Status = task.Status;
            existingTask.DueDate = task.DueDate;
            existingTask.AssignedUserId = task.AssignedUserId;
            existingTask.EstimatedHours = task.EstimatedHours;
            existingTask.ActualHours = task.ActualHours;
            existingTask.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            
            return existingTask;
        }
        
        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return false;
            }
            
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            
            return true;
        }
        
        public async Task<bool> TaskExistsAsync(int id)
        {
            return await _context.Tasks.AnyAsync(t => t.Id == id);
        }
        
        public async Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(Models.TaskStatus status)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .Where(t => t.Status == status)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<TaskItem>> GetTasksByPriorityAsync(Models.TaskPriority priority)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .Where(t => t.Priority == priority)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<TaskItem>> GetOverdueTasksAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .Where(t => t.DueDate.HasValue && t.DueDate.Value.Date < today && t.Status != Models.TaskStatus.Done)
                .ToListAsync();
        }
    }
}