using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.TimeManagement
{
   public class TimesheetReqDetailsModel
    {
        public int id { get; set; }

        public int timesheet_id { get; set; }

        public int? unattendance_id { get; set; }

        public DateTime? transaction_date { get; set; }

        public string begin_time { get; set; }

        public string end_time { get; set; }

    }
}
