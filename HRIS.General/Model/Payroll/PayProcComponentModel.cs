using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Payroll
{
    public class PayProcComponentModel
    {
        public int id { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public int? payroll_process_id { get; set; }

        public int? payroll_component_id { get; set; }

        public short del_flag { get; set; }

        public string TRIAL281 { get; set; }
    }
}
