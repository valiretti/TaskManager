using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
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
    public class ProjectController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IEmployeeService _employeeService;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        private readonly ILog _log;

        public ProjectController(ITaskService taskService, IEmployeeService employeeService, IProjectService projectService, IMapper mapper, ILog log)
        {
            _taskService = taskService;
            _employeeService = employeeService;
            _projectService = projectService;
            _mapper = mapper;
            _log = log;
        }

        public ActionResult GetAll()
        {
            try
            {
                var projects = _projectService.GetAll();
                return View(_mapper.Map<IEnumerable<ProjectViewModel>>(projects));
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
                var pageIndex = page ?? 1;
                var pageSize = limit ?? 5;

                var result = _projectService.Get(pageIndex, pageSize);
                var projectsAsIPagedList = new StaticPagedList<ProjectViewModel>(_mapper.Map<IEnumerable<ProjectViewModel>>(result.Items), pageIndex, pageSize, result.Total);
                return View(projectsAsIPagedList);
            }

            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            return BadRequest();
        }

        [ImportModelState]
        public ActionResult CreateTask()
        {
            try
            {
                FillComboBoxes();
                return View();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            return RedirectToAction("Index");
        }

        [ImportModelState]
        public ActionResult EditTask(string json)
        {
            try
            {
                FillComboBoxes();
                var task = JsonConvert.DeserializeObject<CreateTask>(json);
                return View(_mapper.Map<TaskCreationModel>(task));
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

        [HttpGet]
        public JsonResult GetEmployees()
        {
            try
            {
                var employees = _employeeService.GetAll();
                return Json(employees);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            return Json(new List<EmployeeViewModel>());
        }


        [HttpGet]
        public JsonResult GetTasksByProjectId(int id)
        {
            try
            {
                var tasks = _taskService.GetByProjectId(id);
                return Json(_mapper.Map<IEnumerable<Models.TaskViewModel>>(tasks));
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            return Json(new List<Models.TaskViewModel>());

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
                var project = _projectService.Get(id);
                if (project == null)
                    return NotFound();

                return View(_mapper.Map<ProjectCreationModel>(project));
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
        public ActionResult Edit(ProjectCreationModel model)
        {
            if (model == null)
                return BadRequest("No data for model");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit");
            }

            try
            {
                var project = _mapper.Map<CreateProject>(model);
                if (_projectService.Get(project.Id) == null)
                {
                    return NotFound();
                }

                _projectService.Update(project);
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
                var project = _projectService.Get(id);
                if (project == null)
                    return NotFound();

                return View(_mapper.Map<ProjectViewModel>(project));
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
        public ActionResult Delete(ProjectViewModel project)
        {
            try
            {
                if (_projectService.Get(project.Id) == null)
                    return NotFound();

                _projectService.Delete(project.Id);
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
