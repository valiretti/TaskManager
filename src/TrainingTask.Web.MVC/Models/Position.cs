using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingTask.Web.MVC.Models
{
    public enum Position
    {
        Developer = 0,
        Tester = 1,
        [Display(Name = "Business Analyst")]
        BusinessAnalyst = 2,
        Manager = 3,
        Administrative = 4
    }
}
