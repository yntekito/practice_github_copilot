using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly ILogger<ProjectsController> _logger;
        
        public ProjectsController(IProjectService projectService, IUserService userService, ILogger<ProjectsController> logger)
        {
            _projectService = projectService;
            _userService = userService;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            try
            {
                var projects = await _projectService.GetAllProjectsAsync();
                return Ok(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting projects");
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            try
            {
                var project = await _projectService.GetProjectByIdAsync(id);
                if (project == null)
                {
                    return NotFound($"Project with ID {id} not found");
                }
                
                return Ok(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting project {ProjectId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjectsByUser(int userId)
        {
            try
            {
                if (!await _userService.UserExistsAsync(userId))
                {
                    return NotFound($"User with ID {userId} not found");
                }
                
                var projects = await _projectService.GetProjectsByUserIdAsync(userId);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting projects for user {UserId}", userId);
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpGet("by-status/{status}")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjectsByStatus(ProjectStatus status)
        {
            try
            {
                var projects = await _projectService.GetProjectsByStatusAsync(status);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting projects by status {Status}", status);
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(Project project)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                if (!await _userService.UserExistsAsync(project.OwnerId))
                {
                    return BadRequest($"User with ID {project.OwnerId} not found");
                }
                
                var createdProject = await _projectService.CreateProjectAsync(project);
                return CreatedAtAction(nameof(GetProject), new { id = createdProject.Id }, createdProject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating project");
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<Project>> UpdateProject(int id, Project project)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                var updatedProject = await _projectService.UpdateProjectAsync(id, project);
                if (updatedProject == null)
                {
                    return NotFound($"Project with ID {id} not found");
                }
                
                return Ok(updatedProject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating project {ProjectId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                var result = await _projectService.DeleteProjectAsync(id);
                if (!result)
                {
                    return NotFound($"Project with ID {id} not found");
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting project {ProjectId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}