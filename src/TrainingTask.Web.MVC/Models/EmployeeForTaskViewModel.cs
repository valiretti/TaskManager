using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingTask.Web.MVC.Models
{
    public class EmployeeForTaskViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; }
    }
}
