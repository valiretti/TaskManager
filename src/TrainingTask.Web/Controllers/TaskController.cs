using Microsoft.AspNetCore.Mvc;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Common.Models;

namespace TrainingTask.Web.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet, Route("")]
        public IActionResult GetAll()
        {
            return Ok(_taskService.GetAll());
        }

        [HttpGet, Route("{id:int:min(1)}")]
        public IActionResult GetById(int id)
        {
            var task = _taskService.GetViewModel(id);
            if (task == null)
                return NotFound();
            return new ObjectResult(task);
        }

        [HttpGet, Route("byProject/{id:int:min(1)}")]
        public IActionResult GetByProjectId(int id)
        {
            var tasks = _taskService.GetByProjectId(id);
            if (tasks == null)
                return NotFound();
            return new ObjectResult(tasks);
        }

        [HttpPost, Route("")]
        public IActionResult Add([FromBody] CreateTask task)
        {
            if (task == null)
            {
                ModelState.AddModelError("", "No data for employee");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var insertedId = _taskService.Add(task);
            var task1 = _taskService.GetViewModel(insertedId);

            return Ok(task1);
        }

        [HttpPut, Route("")]
        public IActionResult Edit([FromBody] CreateTask task)
        {
            if (task == null)
            {
                ModelState.AddModelError("", "No data for task");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_taskService.Get(task.Id) == null)
            {
                return NotFound();
            }

            _taskService.Update(task);
            return NoContent();
        }

        [HttpDelete, Route("{id:int:min(1)}")]
        public IActionResult Delete(int id)
        {
            var task = _taskService.Get(id);
            if (task == null)
            {
                return NotFound();
            }

            _taskService.Delete(id);
            return NoContent();
        }
    }
}