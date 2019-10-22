using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.TimeManagement
{
   public class TimeCalculationModel
    {
        public int id { get; set; }

        public string status { get; set; }

        public int payroll_area_id { get; set; }

        public int payroll_process_id { get; set; }

        public int sequence { get; set; }

        public int year { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public int? total_employee { get; set; }

        public int? processed_employee { get; set; }

        public DateTime? start_time { get; set; }

        public DateTime? end_time { get; set; }

        public int? total_time { get; set; }

        public string TRIAL288 { get; set; }


    }
}
