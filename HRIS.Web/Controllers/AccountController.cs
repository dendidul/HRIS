using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using HRIS.General.Model.UserManagement;
using HRIS.Service.Interface;
using HRIS.Service.Manager;
using HRIS.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HRIS.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICookie _cookie;
        private readonly IUserManagementManager IUserManagementManager;
        private readonly HelperController HelperController;

        public AccountController(IConfiguration _config, ICookie cookie)
        {
            this._cookie = cookie;
            this.IUserManagementManager = new UserManagementManager(_config);
            this.HelperController = new HelperController(_config,cookie);
        }

        public IActionResult Login()
        {
            var RoleId = HelperController.GetCookie("Role_ID");
            if(RoleId != "")
            {
                return RedirectToAction("DashBoard", "Home");
            }
            else
            {
                return View();
            }
           
        }

        [HttpPost]
        public IActionResult Login(UserModel model)
        {

            //var Password = model.Password;
            //    //Encryptor.Encrypt(model.Password);
            //model.Password = Password;
            var checkdata = IUserManagementManager.CheckValidUser(model);

            if (checkdata != null)
            {
                //CookieOptions options = new CookieOptions();
                //options.Expires = DateTime.Now.AddDays(1);
                //Response.Cookies.Append("Role_ID", checkdata != null ? checkdata.RoleId.ToString():"", options);
                //Response.Cookies.Append("UserName", model.UserName, options);

                //string FirstName = checkdata. != null ? checkdata.FirstName : "";
                //string LastName = checkdata.LastName != null ? checkdata.LastName : "";
                //string UserName =  + " " + LastName;
                HelperController.SetCookie("UserId", checkdata != null ? checkdata.id.ToString() : "");
                HelperController.SetCookie("UserName", checkdata != null ? checkdata.username.ToString() : "");
                HelperController.SetCookie("Role_ID", checkdata != null ? checkdata.role_id.ToString() : "");

                return Json(new { User = "Valid", RoleId = checkdata.role_id });
            }
            else
            {
                return Json(new { User = "NotValid", RoleId = checkdata != null ? checkdata.role_id.ToString() : "" });
            }

            // return RedirectToAction("Dashboard", "CRMHome");



        }

        public IActionResult LogOut()
        {
            try
            {
                HelperController.RemoveCookie("UserId");
                HelperController.RemoveCookie("UserName");
                HelperController.RemoveCookie("Role_ID");
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "LogOut", ex.Message);

            }

            //return RedirectToAction("Onboarding", "Account");
            return RedirectToAction("Login");
        }

    }
}