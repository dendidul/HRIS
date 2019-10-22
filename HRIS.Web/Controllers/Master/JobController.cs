using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using HRIS.General.Model.Master;
using HRIS.Service.Interface;
using HRIS.Service.Manager;
using HRIS.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HRIS.Web.Controllers.Master
{
    public class JobController : GlobalAccessMenuController
    {
        private readonly IMasterManager IMasterManager;
        private readonly HelperController HelperController;

        public JobController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);

        }

        public IActionResult Index()
        {
            IList<JobModel> model = new List<JobModel>();
            model = IMasterManager.GetAllJob();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(JobModel model)
        {
            IMasterManager.CreateJob(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            JobModel model = new JobModel();
            model.id = id;
            model = IMasterManager.GetJob(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobModel model)
        {
            try
            {

                // TODO: Add update logic here
                IMasterManager.UpdateJob(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            JobModel model = new JobModel();
            model.id = id;
            model = IMasterManager.GetJob(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            JobModel model = new JobModel();
            model.id = id;
            model = IMasterManager.GetJob(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(JobModel model)
        {


            IMasterManager.DeleteJob(model.id);
            return RedirectToAction("Index");
        }

    }
}