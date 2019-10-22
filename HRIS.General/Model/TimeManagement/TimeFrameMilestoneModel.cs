using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.TimeManagement
{
    public class TimeFrameMilestoneModel
    {
        public int id { get; set; }

        public string mils_time { get; set; }

        public string mils_type { get; set; }

        public int time_frame_id { get; set; }

        public short? del_flag { get; set; }

        public string TRIAL288 { get; set; }
    }
}
