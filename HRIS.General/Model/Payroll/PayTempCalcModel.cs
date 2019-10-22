using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Payroll
{
    public class PayTempCalcModel
    {
        public int id { get; set; }

        public int? employee_id { get; set; }

        public int? process_id { get; set; }

        public int? period_id { get; set; }

        public int? payroll_area_id { get; set; }

        public int? payroll_calculation_id { get; set; }

        public int? year { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public int? sequence { get; set; }

        public int? thread_no { get; set; }

        public string TRIAL288 { get; set; }

    }
}
