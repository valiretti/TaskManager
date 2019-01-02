using Microsoft.AspNetCore.Mvc;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Common.Models;

namespace TrainingTask.Web.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
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

            var result = _projectService.Add(project);
            project.Id = result;

            return Ok(project);
        }

        [HttpPut, Route("")]
        public IActionResult Edit([FromBody] CreateProject project)
        {
            if (project == null)
            {
                ModelState.AddModelError("", "No data for employee");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_projectService.Get(project.Id) == null)
            {
                ModelState.AddModelError("", "Not found.");
                return NotFound(ModelState);
            }

            _projectService.Update(project);
            return NoContent();
        }

        [HttpDelete, Route("{id:int:min(1)}")]
        public IActionResult Delete(int id)
        {
            var task = _projectService.Get(id);
            if (task == null)
            {
                return NotFound();
            }

            _projectService.Delete(id);
            return NoContent();
        }
    }
}