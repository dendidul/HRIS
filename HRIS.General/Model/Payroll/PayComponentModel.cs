using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Payroll
{
    public class PayComponentModel
    {
        public int id { get; set; }

        public string payroll_component_id { get; set; }

        public string short_desc { get; set; }

        public string long_desc { get; set; }

        public string amount_type { get; set; }

        public string amount { get; set; }

        public string amount_minimum { get; set; }

        public string amount_maximum { get; set; }

        public string object_id { get; set; }

        public string proportional_type { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public string transaction_flag { get; set; }

        public string proportional_id { get; set; }

        public short? delete_flag { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public int? rapel_component_id { get; set; }

        public short? is_simulation { get; set; }

        public string marital_status { get; set; }

        public int? currency_id { get; set; }

        public string TRIAL281 { get; set; }
    }
}
