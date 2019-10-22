using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Master
{
    public class CostCenterModel
    {

        public int id { get; set; }

        public string cost_center_code { get; set; }

        public string description { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public short? del_flag { get; set; }

        public string TRIAL271 { get; set; }
    }
}
