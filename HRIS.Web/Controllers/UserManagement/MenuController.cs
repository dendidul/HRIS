using CookieManager;
using HRIS.General.Model.UserManagement;
using HRIS.Service.Interface;
using HRIS.Service.Manager;
using HRIS.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace HRIS.Web.Controllers.UserManagement
{
    public class MenuController : GlobalAccessMenuController
    {
        private readonly IUserManagementManager _userManagementManager;
        private readonly HelperController _helperController;

        public MenuController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._userManagementManager = new UserManagementManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            var data = _userManagementManager.GetAllMenu();
            return View(data);
        }

        public IActionResult IndexMenu()
        {
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            ViewBag.ParentMenus = _userManagementManager.GetAllMenu();
            return View(new MenuModel());
        }

        [HttpPost]
        public IActionResult Create(MenuModel model)
        {
            try
            {
                _userManagementManager.CreateMenu(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.ParentMenus = _userManagementManager.GetAllMenu();
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            var data = _userManagementManager.GetMenu(id);
            ViewBag.ParentMenus = _userManagementManager.GetAllMenu();
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MenuModel model)
        {
            try
            {
                _userManagementManager.UpdateMenu(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.ParentMenus = _userManagementManager.GetAllMenu();
                return View(model);
            }
        }

        public IActionResult Delete(int id)
        {
            var data = _userManagementManager.GetMenu(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult Delete(MenuModel model)
        {
            try
            {
                _userManagementManager.DeleteMenu(model.id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}
