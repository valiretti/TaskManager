using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Common.Exceptions;
using TrainingTask.Common.Models;
using TrainingTask.Web.MVC.Filters;
using TrainingTask.Web.MVC.Models;
using TaskViewModel = TrainingTask.Common.Models.TaskViewModel;

namespace TrainingTask.Web.MVC.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IEmployeeService _employeeService;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public TaskController(ITaskService taskService, IEmployeeService employeeService, IProjectService projectService, IMapper mapper)
        {
            _taskService = taskService;
            _employeeService = employeeService;
            _projectService = projectService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var tasks = _taskService.GetAll();
            return View(_mapper.Map<IEnumerable<Models.TaskViewModel>>(tasks));
        }

        public ActionResult Success()
        {
            return View();
        }

        [ImportModelState]
        public ActionResult Create()
        {
            FillComboBoxes();
            return View(;
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
                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToAction("Create");
        }

        [ImportModelState]
        public ActionResult Edit(int id)
        {
            var task = _taskService.GetViewModel(id);
            if (task == null)
                return NotFound();

            FillComboBoxes(task);
            return View(_mapper.Map<TaskCreationModel>(task));
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
                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToAction("Create");
        }

        [ImportModelState]
        public ActionResult Delete(int id)
        {
            var task = _taskService.GetViewModel(id);
            if (task == null)
                return NotFound();

            return View(_mapper.Map<Models.TaskViewModel>(task));
        }

        [HttpPost]
        [ExportModelState]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Models.TaskViewModel task)
        {
            if (_taskService.Get(task.Id) == null)
                return NotFound();

            _taskService.Delete(task.Id);
            return RedirectToAction("Success");
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