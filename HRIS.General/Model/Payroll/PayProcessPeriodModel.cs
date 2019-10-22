using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Payroll
{
    public class PayProcessPeriodModel
    {
        public int id { get; set; }

        public int payroll_process_id { get; set; }

        public int payroll_period_id { get; set; }

        public DateTime? created { get; set; }

        public int? created_by { get; set; }

        public DateTime? modified { get; set; }

        public int? modified_by { get; set; }

        public int del_flag { get; set; }

        public short? is_active { get; set; }

        public string TRIAL281 { get; set; }
    }
}
