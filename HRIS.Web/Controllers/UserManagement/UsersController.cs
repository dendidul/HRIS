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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace HRIS.Web.Controllers.UserManagement
{
    public class UsersController : GlobalAccessMenuController
    {
        private readonly IUserManagementManager IUserManagementManager;
        private readonly HelperController HelperController;

        public UsersController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IUserManagementManager = new UserManagementManager(config);
            this.HelperController = new HelperController(config, cookie);

        }

        public IActionResult Index()
        {
            IList<UserModel> model = new List<UserModel>();
            model = IUserManagementManager.GetAllUsers();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.ListRole = new SelectList(IUserManagementManager.GetAllRoles(), "id", "role_id");
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserModel model)
        {
            
            IUserManagementManager.CreateUser(model);
            
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            UserModel model = new UserModel();
            model.id = id;
            model = IUserManagementManager.GetUsers(model);
            ViewBag.ListRole = new SelectList(IUserManagementManager.GetAllRoles(), "id", "role_id",model.role_id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserModel model)
        {
            try
            {

                // TODO: Add update logic here
                IUserManagementManager.UpdateUser(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            UserModel model = new UserModel();
            model.id = id;
            model = IUserManagementManager.GetUsers(model);
            ViewBag.ListRole = new SelectList(IUserManagementManager.GetAllRoles(), "id", "role_id", model.role_id);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            UserModel model = new UserModel();
            model.id = id;
            model = IUserManagementManager.GetUsers(model);
            ViewBag.ListRole = new SelectList(IUserManagementManager.GetAllRoles(), "id", "role_id", model.role_id);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(UserModel model)
        {

            IUserManagementManager.DeleteUsers(model.id);
            return RedirectToAction("Index");
        }
    }
}