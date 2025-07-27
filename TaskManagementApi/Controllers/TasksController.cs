using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly ILogger<TasksController> _logger;
        
        public TasksController(
            ITaskService taskService, 
            IProjectService projectService, 
            IUserService userService,
            ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _projectService = projectService;
            _userService = userService;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting tasks");
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound($"Task with ID {id} not found");
                }
                
                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting task {TaskId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpGet("by-project/{projectId}")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasksByProject(int projectId)
        {
            try
            {
                if (!await _projectService.ProjectExistsAsync(projectId))
                {
                    return NotFound($"Project with ID {projectId} not found");
                }
                
                var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting tasks for project {ProjectId}", projectId);
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasksByUser(int userId)
        {
            try
            {
                if (!await _userService.UserExistsAsync(userId))
                {
                    return NotFound($"User with ID {userId} not found");
                }
                
                var tasks = await _taskService.GetTasksByUserIdAsync(userId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting tasks for user {UserId}", userId);
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpGet("by-status/{status}")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasksByStatus(Models.TaskStatus status)
        {
            try
            {
                var tasks = await _taskService.GetTasksByStatusAsync(status);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting tasks by status {Status}", status);
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpGet("by-priority/{priority}")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasksByPriority(Models.TaskPriority priority)
        {
            try
            {
                var tasks = await _taskService.GetTasksByPriorityAsync(priority);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting tasks by priority {Priority}", priority);
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetOverdueTasks()
        {
            try
            {
                var tasks = await _taskService.GetOverdueTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting overdue tasks");
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                if (!await _projectService.ProjectExistsAsync(task.ProjectId))
                {
                    return BadRequest($"Project with ID {task.ProjectId} not found");
                }
                
                if (task.AssignedUserId.HasValue && !await _userService.UserExistsAsync(task.AssignedUserId.Value))
                {
                    return BadRequest($"User with ID {task.AssignedUserId} not found");
                }
                
                var createdTask = await _taskService.CreateTaskAsync(task);
                return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating task");
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<TaskItem>> UpdateTask(int id, TaskItem task)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                if (task.AssignedUserId.HasValue && !await _userService.UserExistsAsync(task.AssignedUserId.Value))
                {
                    return BadRequest($"User with ID {task.AssignedUserId} not found");
                }
                
                var updatedTask = await _taskService.UpdateTaskAsync(id, task);
                if (updatedTask == null)
                {
                    return NotFound($"Task with ID {id} not found");
                }
                
                return Ok(updatedTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating task {TaskId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var result = await _taskService.DeleteTaskAsync(id);
                if (!result)
                {
                    return NotFound($"Task with ID {id} not found");
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting task {TaskId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}