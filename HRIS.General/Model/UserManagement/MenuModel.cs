using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.UserManagement
{
    public class MenuModel
    {
        public int id { get; set; }

        public string menu_name { get; set; }

        public string link { get; set; }

        public string icon { get; set; }

        public int? parent_id { get; set; }

        public int? index { get; set; }

        public short? del_flag { get; set; }

        public string TRIAL278 { get; set; }

    }
}
