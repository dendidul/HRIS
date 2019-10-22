using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using HRIS.General.Model.ViewModel;
using HRIS.Service.Interface;
using HRIS.Service.Manager;
using HRIS.Web.Helper;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace HRIS.Web.Controllers
{
    public class GlobalAccessMenuController : Controller
    {
        private readonly ICookie _cookie;
        private readonly HelperController HelperController;
        private readonly IUserManagementManager IUserManagementManager;

        public GlobalAccessMenuController(IConfiguration _config, ICookie cookie)
        {
            this._cookie = cookie;
            this.HelperController = new HelperController(_config, cookie);
            this.IUserManagementManager = new UserManagementManager(_config);
        }


        //public override void OnActionExecuting(ActionExecutingContext ctx)
        //{
        //    base.OnActionExecuting(ctx);
        //    ValidateUserRole(ctx);
        //    //  ViewBagItem(ctx);
        //}

        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            ValidateUserRole(ctx);
            //  ViewBagItem(ctx);
        }

        public List<RoleMenuViewModel> BuildRoleMenu()
        {
            //HttpContext.Session.SetString("Role_ID", "1");           
            //var data = HttpContext.Session.GetString("Role_ID");
            //var RolesID = Convert.ToInt32(data);
            //var UserName = _cookie["UserName"];
            //var RolesID = _cookie["Role_ID"];
            var RolesID = HelperController.GetCookie("Role_ID");

            //var RolesID1 = _cookie["Role_ID"];
            var datalist = IUserManagementManager.BuildRoleMenu(Convert.ToInt32(RolesID));
            return datalist.ToList();

        }

        public IActionResult SetMenu()
        {
            //var UserName = _cookie["UserName"];
            //var RolesID = _cookie["Role_ID"];

            //var UserName = HelperController.GetCookie("UserName");
            var RolesID = HelperController.GetCookie("Role_ID");
            //var RolesID1 = _cookie["Role_ID"];
            var datalist = IUserManagementManager.BuildRoleMenu(Convert.ToInt32(RolesID));
            ViewBag.Menu = datalist;
            return PartialView("Menu");
        }

        public ActionResult ValidateUserRole(ActionExecutingContext ctx)
        {

            if (HelperController.GetCookie("Role_ID") != null)
            {
                var session_role_id = HelperController.GetCookie("Role_ID");
                if (string.IsNullOrEmpty(session_role_id))
                {
                    ctx.Result = new RedirectResult(Url.Action("Login", "Account"));
                    return RedirectToAction("Login", "Account");
                }

                var role_id = Convert.ToInt32(session_role_id);

                var URL = HttpContext.Request.GetDisplayUrl();
                var fullUrl = HttpContext.Request.GetDisplayUrl().ToString();

                var questionMarkIndex = fullUrl.IndexOf('?');
                string queryString = null;
                string url = fullUrl;
                if (questionMarkIndex != -1) // There is a QueryString
                {
                    url = fullUrl.Substring(0, questionMarkIndex);
                    queryString = fullUrl.Substring(questionMarkIndex + 1);
                }

             
                var routeData = ControllerContext.ActionDescriptor.ControllerName;              
                var AccessingController = HelperController.ControllerLists();
                var count = AccessingController.Where(x => x.ToLower() == routeData.ToString().ToLower()).Count();

                if (count < 1)
                {
                    var dataCount = IUserManagementManager.CheckMenuForFoles(role_id, routeData);
                    if (dataCount == false)
                    {
                        ctx.Result = new RedirectResult(Url.Action("Index", "NoAccess"));
                        return RedirectToAction("Index", "NoAccess");
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
                //return null;

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}