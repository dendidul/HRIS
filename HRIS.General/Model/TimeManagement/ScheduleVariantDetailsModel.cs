using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.TimeManagement
{
    public class ScheduleVariantDetailsModel
    {
        public int id { get; set; }

        public int? seq_number { get; set; }

        public int? time_frame_id { get; set; }

        public int? schedule_variant_id { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public short? del_flag { get; set; }

    }
}
