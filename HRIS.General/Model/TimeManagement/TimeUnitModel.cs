using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.TimeManagement
{
    public class TimeUnitModel
    {
        public int id { get; set; }

        public string time_unit_id { get; set; }

        public string short_desc { get; set; }

        public string long_desc { get; set; }

        public short? delete_flag { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public string measure_type { get; set; }

        public int? lookup_id { get; set; }

        public string TRIAL291 { get; set; }

    }
}
