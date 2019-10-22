using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.TimeManagement
{
    public class TimeUnitDetailsModel
    {
        public int id { get; set; }

        public int? time_unit_id { get; set; }

        public string time_event_type { get; set; }

        public string day_type { get; set; }

        public int? unattendance_id { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public string overtime_start { get; set; }

        public string overtime_end { get; set; }

        public string maximum { get; set; }

        public string minimum { get; set; }

        public string TRIAL288 { get; set; }

    }
}
