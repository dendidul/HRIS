using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.TimeManagement
{
   public class TimeFrameModel

    {
        public int id { get; set; }

        public string time_frame_id { get; set; }

        public string short_desc { get; set; }

        public string long_desc { get; set; }

        public string starting_time { get; set; }

        public string tolerance_time { get; set; }

        public string duration_time { get; set; }
        

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public short? del_flag { get; set; }



    }
}
