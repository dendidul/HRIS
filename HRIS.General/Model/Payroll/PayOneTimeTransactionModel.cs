using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Payroll
{
    public class PayOneTimeTransactionModel
    {
        public int id { get; set; }

        public int? employee_id { get; set; }

        public int? number_period { get; set; }

        public string fiscal_year { get; set; }

        public int? payroll_component_id { get; set; }

        public string comp_trans_type { get; set; }

        public string comp_amount { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modifed_by { get; set; }

        public DateTime? modifed_date { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public int? payroll_process_id { get; set; }

        public int? currency_id { get; set; }

        public short? del_flag { get; set; }

        public string TRIAL278 { get; set; }
    }
}
