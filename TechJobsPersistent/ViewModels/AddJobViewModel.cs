using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechJobsPersistent.Models;

namespace TechJobsPersistent.ViewModels
{
    public class AddJobViewModel
    {
        public string Name { get; set; }

        public int EmployerId { get; set; }

        public List<SelectListItem> Employers { get; set; }


        public AddJobViewModel(List<Employer> employers)
        {
            Employers = new List<SelectListItem>();

            foreach (var employer in employers)
            {
                Employers.Add(new SelectListItem
                {
                    Value = employer.Id.ToString(), //need to convert to string, because value property of SelectListItem expects a string
                    Text = employer.Name
                });
            }
        }

        public AddJobViewModel()
        {
        }
    }
}
