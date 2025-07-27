using System.ComponentModel.DataAnnotations;
using TaskManagementApi.Models;

namespace TaskManagementApi.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(2000)]
        public string? Description { get; set; }
        
        public Models.TaskPriority Priority { get; set; } = Models.TaskPriority.Medium;
        public Models.TaskStatus Status { get; set; } = Models.TaskStatus.Todo;
        
        public DateTime? DueDate { get; set; }
        
        [Required]
        public int ProjectId { get; set; }
        
        public int? AssignedUserId { get; set; }
        
        [Range(0, int.MaxValue)]
        public int EstimatedHours { get; set; }
    }
    
    public class UpdateTaskDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(2000)]
        public string? Description { get; set; }
        
        public Models.TaskPriority Priority { get; set; }
        public Models.TaskStatus Status { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        public int? AssignedUserId { get; set; }
        
        [Range(0, int.MaxValue)]
        public int EstimatedHours { get; set; }
        
        [Range(0, int.MaxValue)]
        public int? ActualHours { get; set; }
    }
}