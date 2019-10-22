using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using HRIS.General.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HRIS.Web.Helper
{
    public class HelperController : Controller
    {

        private ICookie _cookie;
        private IConfiguration _config;
        private readonly Logger _logger;

        public HelperController(IConfiguration config, ICookie cookie)
        {

            this._config = config;
            this._cookie = cookie;
            this._logger = new Logger(config);
        }

        public List<string> ControllerList
        {
            get
            {
                List<string> listItems = new List<string>();
                listItems.Add("GlobalAccessMenu");
                listItems.Add("Account");
                listItems.Add("Error");
                listItems.Add("Home");
                return listItems;
            }
        }

        public List<string> ControllerLists()
        {
            return ControllerList;
        }

        public string DestinationLogFolder()
        {
            return _config.GetSection("Logging:DestinationFolder:Web").Value.ToString();
        }

        public void RemoveCookie(string key)
        {
            _cookie.Remove(key);
            //Response.Cookies.Delete(key);
        }


        public string GetCookie(string key)
        {
            //return _cookie.Get(key);
            if (key == "UserId")
            {
                var cookie = _cookie.Get(key);
                if (cookie == "")
                {
                    return "0";
                }
                else
                {
                    return _cookie.Get(key);
                }
            }
            else
            {
                return _cookie.Get(key);
            }

        }

       

        public void SetCookie(string key, string value)
        {
            CookieOptions option = new CookieOptions();
            //option.Expires = DateTime.Now.AddDays(Convert.ToInt32(CookieTimeOut()));
            option.Expires = DateTime.Now.AddDays(30);
            _cookie.Set(key, value, option);
            //_cookie.Set()



        }

        public void InsertLog(int userId, string function, string message)
        {
            //UserModel model = new UserModel();
            //model.Id = userId;
            //var userdata = IUserManagementManager.GetUser(model);
            //var userName = userdata != null ? userdata.UserName : "";

            _logger.WriteFunctionLog(DestinationLogFolder(), "", function, message, "Web");

            //Response.Cookies.Delete(key);
        }



    }



    
}