using System;
using Microsoft.AspNetCore.Mvc;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Common.Exceptions;
using TrainingTask.Common.Models;

namespace TrainingTask.Web.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ITaskService _taskService;

        public ProjectController(IProjectService projectService, ITaskService taskService)
        {
            _projectService = projectService;
            _taskService = taskService;
        }

        [HttpGet, Route("")]
        public IActionResult GetAll()
        {
            return Ok(_projectService.GetAll());
        }

        [HttpGet, Route("{id:int:min(1)}")]
        public IActionResult GetById(int id)
        {
            var task = _projectService.Get(id);
            if (task == null)
                return NotFound();
            return new ObjectResult(task);
        }

        [HttpPost, Route("")]
        public IActionResult Add([FromBody] CreateProject project)
        {
            if (project == null)
            {
                ModelState.AddModelError("", "No data for project");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var insertedId = _projectService.Add(project);
                project.Id = insertedId;

                return Ok(project);
            }
            catch (UniquenessViolationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (ForeignKeyViolationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return BadRequest(ModelState);
        }

        [HttpPut, Route("")]
        public IActionResult Edit([FromBody] CreateProject project)
        {
            if (project == null)
            {
                ModelState.AddModelError("", "No data for project");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_projectService.Get(project.Id) == null)
            {
                return NotFound(ModelState);
            }

            try
            {
                _projectService.Update(project);
                return NoContent();
            }
            catch (UniquenessViolationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (ForeignKeyViolationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete, Route("{id:int:min(1)}")]
        public IActionResult Delete(int id)
        {
            var project = _projectService.Get(id);
            if (project == null)
            {
                return NotFound();
            }

            _projectService.Delete(id);
            return NoContent();
        }

        [HttpGet, Route("{id:int:min(1)}/tasks")]
        public IActionResult GetByProjectId(int id)
        {
            var tasks = _taskService.GetByProjectId(id);
            if (tasks == null)
                return NotFound();
            return new ObjectResult(tasks);
        }
    }
}