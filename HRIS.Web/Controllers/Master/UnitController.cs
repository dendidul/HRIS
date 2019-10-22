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
    public class UnitController : GlobalAccessMenuController
    {
        private readonly IMasterManager IMasterManager;
        private readonly HelperController HelperController;

        public UnitController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<UnitModel> model = new List<UnitModel>();
            model = IMasterManager.GetAllUnit();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UnitModel model)
        {
            IMasterManager.CreateUnit(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            UnitModel model = new UnitModel();
            model.id = id;
            model = IMasterManager.GetUnit(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UnitModel model)
        {
            try
            {

                // TODO: Add update logic here
                IMasterManager.UpdateUnit(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            UnitModel model = new UnitModel();
            model.id = id;
            model = IMasterManager.GetUnit(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            UnitModel model = new UnitModel();
            model.id = id;
            model = IMasterManager.GetUnit(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(UnitModel model)
        {


            IMasterManager.DeleteUnit(model.id);


            return RedirectToAction("Index");
        }
    }
}