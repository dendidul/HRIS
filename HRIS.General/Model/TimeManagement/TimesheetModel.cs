using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.TimeManagement
{
    public class TimesheetModel
    {
        public int employee_id { get; set; }

        public DateTime? transaction_date { get; set; }

        public string time_dev_in { get; set; }

        public string time_dev_out { get; set; }

        public string emp_duration { get; set; }

        public string emp_dev_duration { get; set; }

        public string unattendance_type { get; set; }

        public string is_work_day { get; set; }

        public int? time_dev_in_int { get; set; }

        public int? time_dev_out_int { get; set; }

        public DateTime? time_in { get; set; }

        public DateTime? time_out { get; set; }

        public DateTime? time_sch_in { get; set; }

        public DateTime? time_sch_out { get; set; }

        public int? unattendance_id { get; set; }

        public int id { get; set; }

        public string TRIAL291 { get; set; }

    }
}
