using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Common.Exceptions;
using TrainingTask.Common.Models;
using TrainingTask.Web.MVC.Filters;
using TrainingTask.Web.MVC.Models;
using TaskViewModel = TrainingTask.Common.Models.TaskViewModel;

namespace TrainingTask.Web.MVC.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IEmployeeService _employeeService;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectController(ITaskService taskService, IEmployeeService employeeService, IProjectService projectService, IMapper mapper)
        {
            _taskService = taskService;
            _employeeService = employeeService;
            _projectService = projectService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var projects = _projectService.GetAll();
            return View(_mapper.Map<IEnumerable<ProjectViewModel>>(projects));
        }

        [ImportModelState]
        public ActionResult CreateTask()
        {
            FillComboBoxes();
            return View();
        }

        [ImportModelState]
        public ActionResult EditTask(string json)
        {
            FillComboBoxes();
            var task = JsonConvert.DeserializeObject<CreateTask>(json);
            return View(_mapper.Map<TaskCreationModel>(task));
        }

        public ActionResult Success()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetEmployees()
        {
            var employees = _employeeService.GetAll();
            return Json(employees);
        }

        [ImportModelState]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ExportModelState]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectCreationModel project)
        {
            if (project == null)
                return BadRequest("No data for model");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }

            try
            {
                _projectService.Add(_mapper.Map<CreateProject>(project));
                return RedirectToAction("Success");
            }
            catch (UniquenessViolationException ex)
            {
                ModelState.AddModelError("", ex.Message);
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
            var project = _projectService.Get(id);
            if (project == null)
                return NotFound();

            return View(_mapper.Map<ProjectViewModel>(project));
        }

        // POST: Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Project/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
