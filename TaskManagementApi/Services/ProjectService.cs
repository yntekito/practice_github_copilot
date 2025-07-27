using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace TaskManagementApi.Services
{
    public class ProjectService : IProjectService
    {
        private readonly TaskManagementContext _context;
        
        public ProjectService(TaskManagementContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Tasks)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId)
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Tasks)
                .Where(p => p.OwnerId == userId)
                .ToListAsync();
        }
        
        /// <summary>
        /// 指定されたIDに基づいてプロジェクトを非同期で取得します。
        /// プロジェクトの所有者および関連するタスクとその担当ユーザーも含めて取得します。
        /// </summary>
        /// <param name="id">取得するプロジェクトのID。</param>
        /// <returns>
        /// 指定されたIDのプロジェクトを含む <see cref="Project"/> オブジェクト。
        /// プロジェクトが存在しない場合は <c>null</c> を返します。
        /// </returns>
        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Tasks)
                .ThenInclude(t => t.AssignedUser)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        
        public async Task<Project> CreateProjectAsync(Project project)
        {
            project.CreatedAt = DateTime.UtcNow;
            project.UpdatedAt = DateTime.UtcNow;
            
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            
            return project;
        }
        
        public async Task<Project?> UpdateProjectAsync(int id, Project project)
        {
            var existingProject = await _context.Projects.FindAsync(id);
            if (existingProject == null)
            {
                return null;
            }
            
            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.StartDate = project.StartDate;
            existingProject.EndDate = project.EndDate;
            existingProject.Status = project.Status;
            existingProject.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            
            return existingProject;
        }
        
        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return false;
            }
            
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            
            return true;
        }
        
        public async Task<bool> ProjectExistsAsync(int id)
        {
            return await _context.Projects.AnyAsync(p => p.Id == id);
        }
        
        public async Task<IEnumerable<Project>> GetProjectsByStatusAsync(ProjectStatus status)
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Tasks)
                .Where(p => p.Status == status)
                .ToListAsync();
        }
    }
}