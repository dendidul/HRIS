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
    public class AreaController : GlobalAccessMenuController
    {
        private readonly IMasterManager IMasterManager;    
        private readonly HelperController HelperController;

        public AreaController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<AreaModel> model = new List<AreaModel>();
            model = IMasterManager.GetAllArea();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AreaModel model)
        {
            IMasterManager.CreateArea(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            AreaModel model = new AreaModel();
            model.id = id;
            model = IMasterManager.GetArea(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AreaModel model)
        {
            try
            {

                // TODO: Add update logic here
                IMasterManager.UpdateArea(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            AreaModel model = new AreaModel();
            model.id = id;
            model = IMasterManager.GetArea(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            AreaModel model = new AreaModel();
            model.id = id;
            model = IMasterManager.GetArea(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(AreaModel model)
        {


            IMasterManager.DeleteArea(model.id);


            return RedirectToAction("Index");
        }

    }
}