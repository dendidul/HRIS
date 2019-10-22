using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Payroll
{
   public class PayGroupComponentModel
    {
        public int id { get; set; }

        public int? payroll_group_id { get; set; }

        public int? payroll_component_id { get; set; }

        public DateTime? begdt { get; set; }

        public DateTime? enddt { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public short? delfl { get; set; }

        public int? order_no { get; set; }

        public string TRIAL278 { get; set; }

    }
}
