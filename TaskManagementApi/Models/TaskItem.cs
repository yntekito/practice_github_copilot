using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(2000)]
        public string? Description { get; set; }
        
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public TaskStatus Status { get; set; } = TaskStatus.Todo;
        
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        
        public int? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }
        
        public int EstimatedHours { get; set; }
        public int? ActualHours { get; set; }
    }
    
    public enum TaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }
    
    public enum TaskStatus
    {
        Todo,
        InProgress,
        Review,
        Done,
        Cancelled
    }
}