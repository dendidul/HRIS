using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.TimeManagement
{
    public class CompanyCalendarModel
    {
        public int id { get; set; }

        public int? personal_area_id { get; set; }

        public int? workgroup_id { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public int? unattendance_id { get; set; }

        public string remark { get; set; }

        public string created_by { get; set; }

        public string modified_by { get; set; }

        public DateTime? created_date { get; set; }

        public DateTime? modified_date { get; set; }

        public int? del_flag { get; set; }

        public string area_code { get; set; }
        public string workgroup_code { get; set; }
        public string attendance_code { get; set; }



    }
}
