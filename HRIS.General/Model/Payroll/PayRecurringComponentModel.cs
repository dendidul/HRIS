using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Payroll
{
    public class PayRecurringComponentModel
    {
        public int id { get; set; }

        public int? employee_id { get; set; }

        public int? payroll_component_id { get; set; }

        public string comp_amount { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string updated_by { get; set; }

        public DateTime? updated_date { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public short? delete_flag { get; set; }

        public short? referensi { get; set; }

        public int? currency_id { get; set; }

        public string TRIAL281 { get; set; }

    }
}
