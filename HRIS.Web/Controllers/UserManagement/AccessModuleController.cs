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
    public class AccessModuleController : GlobalAccessMenuController
    {
        private readonly IUserManagementManager IUserManagementManager;
        private readonly HelperController HelperController;

        public AccessModuleController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IUserManagementManager = new UserManagementManager(config);
            this.HelperController = new HelperController(config, cookie);

        }

        public IActionResult Index()
        {
            var data = IUserManagementManager.GetAllRoles();
            return View(data);
        }

        public IActionResult Config(int id)
        {

            RoleAccessMenuViewModel model = new RoleAccessMenuViewModel();
            RolesModel roleModel = new RolesModel();
            IList<MenuModel> SelectedList = new List<MenuModel>();
            roleModel.id = id;
            var getRole = IUserManagementManager.GetRoles(roleModel);
            model.RoleId = getRole.id;
            model.RoleNm = getRole.role_id;
            model.ListMenu = IUserManagementManager.GetAllMenu();
            var selectedMenu = IUserManagementManager.GetAllRoleAccess().Where(x => x.role_id == id).ToList();

            foreach (var i in selectedMenu)
            {
                MenuModel menuModel = new MenuModel();
                menuModel.id = (int)i.menu_id;
                SelectedList.Add(menuModel);
            }
            model.ListSelectedMenu = SelectedList;



            return View(model);
        }

        [HttpPost]
        public ActionResult Config(RoleAccessMenuViewModel model, string[] SelectedMenu, string[] SelectedChildMenu)
        {

            var getdata = IUserManagementManager.GetAllRoleAccess().Where(x => x.role_id == model.RoleId).ToList();
            foreach (var i in getdata)
            {
                IUserManagementManager.DeleteManageRoleAccessMenu(i.id);
            }

            if (SelectedMenu != null)
            {
                foreach (var j in SelectedMenu)
                {
                    ManageRolesMenuModel form = new ManageRolesMenuModel();
                    form.menu_id = int.Parse(j);
                    form.role_id = model.RoleId;
                    IUserManagementManager.CreateRoleAccessMenu(form);

                }
            }


            if (SelectedChildMenu != null)
            {
                foreach (var k in SelectedChildMenu)
                {
                    ManageRolesMenuModel form = new ManageRolesMenuModel();
                    form.menu_id = int.Parse(k);
                    form.role_id = model.RoleId;
                    IUserManagementManager.CreateRoleAccessMenu(form);

                }
            }



            //var data = RolesMenuLogic.ReadRoleMenuByRoleID(id);
            //return View(data);
            return RedirectToAction("Index");
        }

    }
}