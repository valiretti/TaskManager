using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Common.Models;
using TrainingTask.Web.MVC.Filters;
using TrainingTask.Web.MVC.Models;

namespace TrainingTask.Web.MVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var employees = _employeeService.GetAll();
            return View(_mapper.Map<IEnumerable<EmployeeViewModel>>(employees));
        }

        public ActionResult Success(string action)
        {
            return View();
        }

        [ImportModelState]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ExportModelState]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeViewModel employee)
        {
            if (employee == null)
                return BadRequest("No data for model");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }

            _employeeService.Add(_mapper.Map<Employee>(employee));
            return RedirectToAction("Success");
        }

        [ImportModelState]
        public ActionResult Edit(int id)
        {
            var employee = _employeeService.Get(id);
            if (employee == null)
                return NotFound();

            return View(_mapper.Map<EmployeeViewModel>(employee));
        }

        [HttpPost]
        [ExportModelState]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeViewModel model)
        {
            if (model == null)
                return BadRequest("No data for model");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit");
            }

            var employee = _mapper.Map<Employee>(model);
            if (_employeeService.Get(employee.Id) == null)
            {
                return NotFound();
            }

            _employeeService.Update(employee);
            return RedirectToAction("Success");
        }

        [ImportModelState]
        public ActionResult Delete(int id)
        {
            var employee = _employeeService.Get(id);
            if (employee == null)
                return NotFound();

            return View(_mapper.Map<EmployeeViewModel>(employee));
        }

        [HttpPost]
        [ExportModelState]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(EmployeeViewModel employee)
        {
            if (_employeeService.Get(employee.Id) == null)
                return NotFound();

            _employeeService.Delete(employee.Id);
            return RedirectToAction("Success");
        }
    }
}