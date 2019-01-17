using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Common.Exceptions;
using TrainingTask.Common.Interfaces;
using TrainingTask.Common.Models;
using TrainingTask.Web.MVC.Filters;
using TrainingTask.Web.MVC.Models;
using X.PagedList;
using TaskViewModel = TrainingTask.Common.Models.TaskViewModel;

namespace TrainingTask.Web.MVC.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IEmployeeService _employeeService;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        private readonly ILog _log;
        private readonly IConfiguration _configuration;

        public TaskController(ITaskService taskService, IEmployeeService employeeService, IProjectService projectService, IMapper mapper, ILog log, IConfiguration configuration)
        {
            _taskService = taskService;
            _employeeService = employeeService;
            _projectService = projectService;
            _mapper = mapper;
            _log = log;
            _configuration = configuration;
        }

        public ActionResult GetAll()
        {
            try
            {
                var tasks = _taskService.GetAll();
                return View(_mapper.Map<IEnumerable<Models.TaskViewModel>>(tasks));
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

                var result = _taskService.Get(pageIndex, pageSize);
                var tasksAsIPagedList = new StaticPagedList<Models.TaskViewModel>(_mapper.Map<IEnumerable<Models.TaskViewModel>>(result.Items), pageIndex, pageSize, result.Total);
                return View(tasksAsIPagedList);
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
            try
            {
                FillComboBoxes();
                return View("CreateTask");
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ExportModelState]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskCreationModel task)
        {
            if (task == null)
                return BadRequest("No data for model");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }

            try
            {
                _taskService.Add(_mapper.Map<CreateTask>(task));
                return RedirectToAction("Success");
            }
            catch (ForeignKeyViolationException ex)
            {
                _log.Error(ex.Message);
                ModelState.AddModelError("", ex.Message);
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
                var task = _taskService.GetViewModel(id);
                if (task == null)
                    return NotFound();

                FillComboBoxes(task);
                return View("EditTask", _mapper.Map<TaskCreationModel>(task));
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
        public ActionResult Edit(TaskCreationModel model)
        {
            if (model == null)
                return BadRequest("No data for model");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit");
            }

            var task = _mapper.Map<CreateTask>(model);
            if (_taskService.Get(task.Id) == null)
            {
                return NotFound();
            }

            try
            {
                _taskService.Update(task);
                return RedirectToAction("Success");
            }
            catch (ForeignKeyViolationException ex)
            {
                _log.Error(ex.Message);
                ModelState.AddModelError("", ex.Message);
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
                var task = _taskService.GetViewModel(id);
                if (task == null)
                    return NotFound();

                return View(_mapper.Map<Models.TaskViewModel>(task));
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
        public ActionResult Delete(Models.TaskViewModel task)
        {
            try
            {
                if (_taskService.Get(task.Id) == null)
                    return NotFound();

                _taskService.Delete(task.Id);
                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToAction("Delete");
        }

        private void FillComboBoxes(TaskViewModel task = null)
        {
            SelectList projects = new SelectList(_projectService.GetAll(), "Id", "Name");
            ViewBag.Projects = projects;

            var employees = _mapper.Map<IEnumerable<EmployeeForTaskViewModel>>(_employeeService.GetAll());
            MultiSelectList employeesForSelect = new MultiSelectList(employees, "Id", "FullName", task != null ? task.Employees : new List<int>());
            ViewBag.Employees = employeesForSelect;
        }
    }
}