using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Payroll
{
   public class PayMembershipsComponent
    {

        public int id { get; set; }

        public int? membership_id { get; set; }

        public string membership_type { get; set; }

        public int? payroll_component_id { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public string short_desc { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public short? flag_delete { get; set; }

        public short? sequence_no { get; set; }

        public string TRIAL278 { get; set; }

    }
}
