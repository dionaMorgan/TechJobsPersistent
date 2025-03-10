﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;
using TechJobsPersistent.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TechJobsPersistent.Controllers
{
    public class HomeController : Controller
    {
        private JobDbContext context;

        public HomeController(JobDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Job> jobs = context.Jobs.Include(j => j.Employer).ToList();

            return View(jobs);
        }

        [HttpGet("/Add")]
        public IActionResult AddJob()
        {
            AddJobViewModel addJobViewModel = new AddJobViewModel(context.Employers.ToList(), context.Skills.ToList());

            return View(addJobViewModel);
        }

        public IActionResult ProcessAddJobForm(AddJobViewModel addJobViewModel, string[] selectedSkills)
        {
            if (ModelState.IsValid)  
            {
                Job newJob = new Job
                {
                    Name = addJobViewModel.Name,
                    EmployerId = addJobViewModel.EmployerId,
                    Employer = context.Employers.Find(addJobViewModel.EmployerId)

                };

                for (int i = 0; i < selectedSkills.Length; i++)
                {
                    JobSkill jobSkill = new JobSkill
                    {
                        JobId = newJob.Id,
                        SkillId = Int32.Parse(selectedSkills[i]), //need to parse each item in selectedSkills as an integer to use for SkillId, .ToInt32? .Int32?
                        Job = newJob,
                    };
                    context.JobSkills.Add(jobSkill);

                };

                context.Jobs.Add(newJob);
                context.SaveChanges();

                return Redirect("/Employer");
            }

            return View("AddJob", addJobViewModel);
        }

        public IActionResult Detail(int id)
        {
            Job theJob = context.Jobs
                .Include(j => j.Employer)
                .Single(j => j.Id == id);

            List<JobSkill> jobSkills = context.JobSkills
                .Where(js => js.JobId == id)
                .Include(js => js.Skill)
                .ToList();

            JobDetailViewModel viewModel = new JobDetailViewModel(theJob, jobSkills);
            return View(viewModel);
        }
    }

        
    
}

