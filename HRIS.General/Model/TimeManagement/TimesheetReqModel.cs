using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.TimeManagement
{
    public class TimesheetReqModel
    {
        public int id { get; set; }

        public int employee_id { get; set; }

        public string NIK { get; set; }

        public string EmployeeName { get; set; }

        public DateTime? transaction_date { get; set; }

        public string time_in { get; set; }

        public string time_out { get; set; }

        public string request_note { get; set; }

        public string request_status { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public short? del_flag { get; set; }

        public string remarks { get; set; }
    }
}
