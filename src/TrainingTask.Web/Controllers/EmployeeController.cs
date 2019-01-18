using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Common.Models;

namespace TrainingTask.Web.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IConfiguration _configuration;

        public EmployeeController(IEmployeeService employeeService, IConfiguration configuration)
        {
            _employeeService = employeeService;
            _configuration = configuration;
        }

        [HttpGet, Route("all")]
        public IActionResult GetAll()
        {
            return Ok(_employeeService.GetAll());
        }

        [HttpGet, Route("")]
        public ActionResult GetPage(int? page, int? limit)
        {
            var pageIndex = page ?? Constants.FirstPage;
            var pageSize = limit ?? _configuration.GetSection(Constants.SectionName).GetValue<int>(Constants.Property);

            return Ok(_employeeService.Get(pageIndex, pageSize));
        }

        [HttpGet, Route("{id:int:min(1)}")]
        public IActionResult GetById(int id)
        {
            Employee employee = _employeeService.Get(id);
            if (employee == null)
                return NotFound();
            return new ObjectResult(employee);
        }

        [HttpPost, Route("")]
        public IActionResult Add([FromBody] Employee employee)
        {
            if (employee == null)
            {
                ModelState.AddModelError("", "No data for employee");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var insertedId = _employeeService.Add(employee);
            employee.Id = insertedId;

            return Ok(employee);
        }

        [HttpPut, Route("")]
        public IActionResult Edit([FromBody] Employee employee)
        {
            if (employee == null)
            {
                ModelState.AddModelError("", "No data for employee");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_employeeService.Get(employee.Id) == null)
            {
                return NotFound();
            }

            _employeeService.Update(employee);
            return NoContent();
        }

        [HttpDelete, Route("{id:int:min(1)}")]
        public IActionResult Delete(int id)
        {
            Employee employee = _employeeService.Get(id);
            if (employee == null)
            {
                return NotFound();
            }

            _employeeService.Delete(id);
            return NoContent();
        }
    }
}