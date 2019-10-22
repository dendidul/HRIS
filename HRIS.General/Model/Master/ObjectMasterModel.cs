using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Master
{
    public class ObjectMasterModel
    {
        public int id { get; set; }

        public string object_type { get; set; }

        public string format_number { get; set; }

        public int? last_number { get; set; }

        public string controller_name { get; set; }

        public string ess_controller_name { get; set; }

        public string view_admin { get; set; }

        public string view_ess { get; set; }

        public short? del_flag { get; set; }

        public string TRIAL278 { get; set; }
    }
}
