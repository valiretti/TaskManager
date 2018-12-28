using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Common.Models;
using Task = System.Threading.Tasks.Task;

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
            var task = _taskService.Get(id);
            if (task == null)
                return NotFound();
            return new ObjectResult(task);
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

            var result = _taskService.Add(task);
            task.Id = result;

            return Ok(task);
        }

        [HttpPut, Route("")]
        public IActionResult Edit([FromBody] CreateTask task)
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