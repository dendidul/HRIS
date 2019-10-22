using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.TimeManagement
{
    public class OvertimeRequestDetailsModel
    {
        public int id { get; set; }

        public int? overtime_request_id { get; set; }

        public DateTime overtime_begin { get; set; }

        public DateTime overtime_end { get; set; }

        public DateTime? final_overtime_begin { get; set; }

        public DateTime? final_overtime_end { get; set; }

    }
}
