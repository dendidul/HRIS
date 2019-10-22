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
    public class LevelGradeController : GlobalAccessMenuController
    {
        private readonly IMasterManager IMasterManager;
        private readonly HelperController HelperController;

        public LevelGradeController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);

        }

        public IActionResult Index()
        {
            IList<LevelGradeModel> model = new List<LevelGradeModel>();
            model = IMasterManager.GetAllLevelGrade();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(LevelGradeModel model)
        {
            IMasterManager.GetLevelGrade(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            LevelGradeModel model = new LevelGradeModel();
            model.id = id;
            model = IMasterManager.GetLevelGrade(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LevelGradeModel model)
        {
            try
            {

                // TODO: Add update logic here
                IMasterManager.UpdateLevelGrade(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            LevelGradeModel model = new LevelGradeModel();
            model.id = id;
            model = IMasterManager.GetLevelGrade(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            LevelGradeModel model = new LevelGradeModel();
            model.id = id;
            model = IMasterManager.GetLevelGrade(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(LevelGradeModel model)
        {


            IMasterManager.DeleteLevelGrade(model.id);
            return RedirectToAction("Index");
        }

    }
}