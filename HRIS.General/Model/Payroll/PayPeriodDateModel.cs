using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Payroll
{
   public class PayPeriodDateModel
    {
        public int id { get; set; }

        public int? seq_number { get; set; }

        public DateTime? begin_period_date { get; set; }

        public string first_day_of_month { get; set; }

        public DateTime? end_period_date { get; set; }

        public string last_day_of_month { get; set; }

        public int? payroll_periode_id { get; set; }

        public DateTime? time_period_begin { get; set; }

        public DateTime? time_period_end { get; set; }

        public int? del_flag { get; set; }

        public string TRIAL281 { get; set; }

    }
}
