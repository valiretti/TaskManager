using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Common.Interfaces;
using TrainingTask.Common.Models;
using TrainingTask.Web.MVC.Filters;
using TrainingTask.Web.MVC.Models;
using X.PagedList;

namespace TrainingTask.Web.MVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly ILog _log;
        private readonly IConfiguration _configuration;


        public EmployeeController(IEmployeeService employeeService, IMapper mapper, ILog log, IConfiguration configuration)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _log = log;
            _configuration = configuration;
        }

        public ActionResult GetAll()
        {
            try
            {
                var employees = _employeeService.GetAll();
                return View(_mapper.Map<IEnumerable<EmployeeViewModel>>(employees));
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            return BadRequest();
        }

        public ActionResult Index(int? page, int? limit)
        {
            try
            {
                var pageIndex = page ?? Constants.FirstPage;
                var pageSize = limit ?? _configuration.GetSection(Constants.SectionName).GetValue<int>(Constants.Property);
                ViewBag.PageSize = pageSize;

                var result = _employeeService.Get(pageIndex, pageSize);
                var employeesAsIPagedList = new StaticPagedList<EmployeeViewModel>(_mapper.Map<IEnumerable<EmployeeViewModel>>(result.Items), pageIndex, pageSize, result.Total);
                return View(employeesAsIPagedList);
            }

            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            return BadRequest();
        }

        public ActionResult Success()
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

            try
            {
                _employeeService.Add(_mapper.Map<Employee>(employee));
                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToAction("Create");
        }

        [ImportModelState]
        public ActionResult Edit(int id)
        {
            try
            {
                var employee = _employeeService.Get(id);
                if (employee == null)
                    return NotFound();

                return View(_mapper.Map<EmployeeViewModel>(employee));
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                ModelState.AddModelError("", ex.Message);
            }

            return BadRequest();
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

            try
            {
                var employee = _mapper.Map<Employee>(model);
                if (_employeeService.Get(employee.Id) == null)
                {
                    return NotFound();
                }

                _employeeService.Update(employee);
                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToAction("Edit");
        }

        [ImportModelState]
        public ActionResult Delete(int id)
        {
            try
            {
                var employee = _employeeService.Get(id);
                if (employee == null)
                    return NotFound();

                return View(_mapper.Map<EmployeeViewModel>(employee));
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            return BadRequest();
        }

        [HttpPost]
        [ExportModelState]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(EmployeeViewModel employee)
        {
            try
            {
                if (_employeeService.Get(employee.Id) == null)
                    return NotFound();

                _employeeService.Delete(employee.Id);
                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToAction("Delete");
        }
    }
}