using HRIS.General.Model.UserManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.ViewModel
{
   public class RoleAccessMenuViewModel
    {
        public int RoleId { get; set; }
        public string RoleNm { get; set; }
        public IList<MenuModel> ListMenu { get; set; }
        public IList<MenuModel> ListSelectedMenu { get; set; }
    }
}
