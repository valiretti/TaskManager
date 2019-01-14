using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Web.MVC.Models;

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

      
        // GET: Project/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
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
    }
}