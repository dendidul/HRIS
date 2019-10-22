using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Payroll
{
    public class PayProportionalModel
    {
        public int id { get; set; }

        public string proportional_id { get; set; }

        public string short_desc { get; set; }

        public string long_desc { get; set; }

        public string numerator_type { get; set; }

        public string numerator_amount { get; set; }

        public string denominator_type { get; set; }

        public string denominator_amount { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public short? delete_flag { get; set; }

        public string TRIAL284 { get; set; }


    }
}
