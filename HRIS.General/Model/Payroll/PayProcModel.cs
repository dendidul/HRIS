using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Payroll
{
    public class PayProcModel
    {
        public int id { get; set; }

        public string payroll_process_id { get; set; }

        public string short_desc { get; set; }

        public string payroll_periode_id { get; set; }

        public string calculation_function { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public string long_desc { get; set; }

        public string working_period { get; set; }

        public short? del_flag { get; set; }

        public short? on_cycle { get; set; }

        public string employee_status { get; set; }

        public string TRIAL284 { get; set; }
    }
}
