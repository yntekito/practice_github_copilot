using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Models
{
    public class Project
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        public ProjectStatus Status { get; set; } = ProjectStatus.Planning;
        
        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
    
    public enum ProjectStatus
    {
        Planning,
        InProgress,
        OnHold,
        Completed,
        Cancelled
    }
}