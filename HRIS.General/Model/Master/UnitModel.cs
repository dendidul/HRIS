using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Master
{
    public class UnitModel
    {
        public int id { get; set; }

        public string unit_id { get; set; }

        public string short_desc { get; set; }

        public string long_desc { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public int? position_id { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public short? del_flag { get; set; }

        public int? cost_center_id { get; set; }

        public int? head_unit_id { get; set; }

        public string TRIAL291 { get; set; }

    }
}
