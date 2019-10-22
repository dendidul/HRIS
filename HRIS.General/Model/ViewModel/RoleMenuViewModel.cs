using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.ViewModel
{
    public class RoleMenuViewModel
    {
       
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public int Level { get; set; }
        public int Sequence { get; set; }
        public int ParentMenuId { get; set; }
        public string Link { get; set; }
        public string MenuNm { get; set; }
        public string icon { get; set; }
        public string ControllerName { get; set; }
    }
}
