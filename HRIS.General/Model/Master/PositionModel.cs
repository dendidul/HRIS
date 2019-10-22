using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Master
{
    public class PositionModel
    {

        public int id { get; set; }

        public string position_id { get; set; }

        public string short_description { get; set; }

        public string long_description { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public short? del_flag { get; set; }

        public string TRIAL284 { get; set; }
    }
}
