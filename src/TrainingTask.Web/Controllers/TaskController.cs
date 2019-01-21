using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Common.Exceptions;
using TrainingTask.Common.Models;

namespace TrainingTask.Web.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IConfiguration _configuration;

        public TaskController(ITaskService taskService, IConfiguration configuration)
        {
            _taskService = taskService;
            _configuration = configuration;
        }

        [HttpGet, Route("all")]
        public IActionResult GetAll()
        {
            return Ok(_taskService.GetAll());
        }

        [HttpGet, Route("")]
        public ActionResult GetPage(int? page, int? limit)
        {
            var pageIndex = page ?? Constants.FirstPage;
            var pageSize = limit ?? _configuration.GetSection(Constants.SectionName).GetValue<int>(Constants.Property);

            return Ok(_taskService.Get(pageIndex, pageSize));
        }

        [HttpGet, Route("{id:int:min(1)}")]
        public IActionResult GetById(int id)
        {
            var task = _taskService.GetViewModel(id);
            if (task == null)
                return NotFound();
            return new ObjectResult(task);
        }

        [HttpPost, Route("")]
        public IActionResult Add([FromBody] CreateTask model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "No data for task");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var insertedId = _taskService.Add(model);
                var task = _taskService.GetViewModel(insertedId);
                return Ok(task);
            }
            catch (ForeignKeyViolationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return BadRequest(ModelState);
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
                ModelState.AddModelError("", "The task not found.");
                return NotFound(ModelState);
            }

            try
            {
                _taskService.Update(task);
                return NoContent();
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