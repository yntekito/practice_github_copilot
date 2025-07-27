using TaskManagementApi.Models;

namespace TaskManagementApi.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId);
        Task<Project?> GetProjectByIdAsync(int id);
        Task<Project> CreateProjectAsync(Project project);
        Task<Project?> UpdateProjectAsync(int id, Project project);
        Task<bool> DeleteProjectAsync(int id);
        Task<bool> ProjectExistsAsync(int id);
        Task<IEnumerable<Project>> GetProjectsByStatusAsync(ProjectStatus status);
    }
}