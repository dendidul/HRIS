using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.TimeManagement
{
   public class EmployeeOvertimeModel
    {
        public int id { get; set; }

        public int? status_employee_id { get; set; }

        public DateTime? transaction_date { get; set; }

        public string time_in { get; set; }

        public string time_out { get; set; }

        public string tolerance { get; set; }

        public string request_begin { get; set; }

        public string request_end { get; set; }

        public string overtime_duration { get; set; }

        public string request_approval { get; set; }

        public string time_dev_dur { get; set; }

        public DateTime? created_date { get; set; }

        public string created_by { get; set; }

        public string updated_by { get; set; }

        public DateTime? updated_date { get; set; }

        public short? del_flag { get; set; }

    }
}
