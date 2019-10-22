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
    public class EmployeeGroupController : GlobalAccessMenuController
    {

        private readonly IMasterManager IMasterManager;
        private readonly HelperController HelperController;

        public EmployeeGroupController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);

        }

        public IActionResult Index()
        {
            IList<EmployeeGroupModel> model = new List<EmployeeGroupModel>();
            model = IMasterManager.GetAllEmployeeGroup();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeGroupModel model)
        {
            IMasterManager.CreateEmployeeGroup(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            EmployeeGroupModel model = new EmployeeGroupModel();
            model.id = id;
            model = IMasterManager.GetEmployeeGroup(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeGroupModel model)
        {
            try
            {

                // TODO: Add update logic here
                IMasterManager.UpdateEmployeeGroup(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            EmployeeGroupModel model = new EmployeeGroupModel();
            model.id = id;
            model = IMasterManager.GetEmployeeGroup(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            EmployeeGroupModel model = new EmployeeGroupModel();
            model.id = id;
            model = IMasterManager.GetEmployeeGroup(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(EmployeeGroupModel model)
        {


            IMasterManager.DeletePosition(model.id);


            return RedirectToAction("Index");
        }


    }
}