using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Payroll
{
    public class PayCalculationModel
    {
        public int id { get; set; }

        public int? seq_number { get; set; }

        public string year_period { get; set; }

        public int? payroll_periode_id { get; set; }

        public int? payroll_area_id { get; set; }

        public int? payroll_process_id { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public string running_status { get; set; }

        public DateTime? running_date { get; set; }

        public string running_by { get; set; }

        public int? total_employee { get; set; }

        public int? processed_employee { get; set; }

        public short? is_logged { get; set; }

        public short? pdf_log_generated { get; set; }

        public DateTime? start_time { get; set; }

        public DateTime? end_time { get; set; }

        public int? total_time { get; set; }

        public string TRIAL281 { get; set; }

    }
}
