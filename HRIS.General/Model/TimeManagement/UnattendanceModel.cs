using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.TimeManagement
{
    public class UnattendanceModel
    {
        public int id { get; set; }

        public string unattendance_id { get; set; }

        public string short_desc { get; set; }

        public string long_desc { get; set; }

        public string un_attendance_period { get; set; }

        public string un_attendance_unit { get; set; }

        public string un_attendance_quota { get; set; }

        public string amount_of_increment { get; set; }

        public string increment_period { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public short? del_flag { get; set; }
    }
}
