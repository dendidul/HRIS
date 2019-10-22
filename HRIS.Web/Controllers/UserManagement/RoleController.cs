using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using HRIS.General.Model.UserManagement;
using HRIS.General.Model.ViewModel;
using HRIS.Service.Interface;
using HRIS.Service.Manager;
using HRIS.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HRIS.Web.Controllers.UserManagement
{
    public class RoleController : GlobalAccessMenuController
    {
        private readonly IUserManagementManager IUserManagementManager;
        private readonly HelperController HelperController;

        public RoleController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IUserManagementManager = new UserManagementManager(config);
            this.HelperController = new HelperController(config, cookie);

        }


        public IActionResult Index()
        {
            IList<RolesModel> model = new List<RolesModel>();
            model = IUserManagementManager.GetAllRoles();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RolesModel model)
        {
            IUserManagementManager.CreateRoles(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            RolesModel model = new RolesModel();
            model.id = id;
            model = IUserManagementManager.GetRoles(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RolesModel model)
        {
            try
            {

                // TODO: Add update logic here
                IUserManagementManager.UpdateRoles(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            RolesModel model = new RolesModel();
            model.id = id;
            model = IUserManagementManager.GetRoles(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            RolesModel model = new RolesModel();
            model.id = id;
            model = IUserManagementManager.GetRoles(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(RolesModel model)
        {

            IUserManagementManager.DeleteRoles(model.id);
            return RedirectToAction("Index");
        }
    }
}