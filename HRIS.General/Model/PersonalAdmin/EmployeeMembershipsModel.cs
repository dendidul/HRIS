using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.PersonalAdmin
{
   public class EmployeeMembershipsModel
    {
        public int id { get; set; }

        public int? employee_id { get; set; }

        public int? membership_id { get; set; }

        public string employee_membership_id { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public string TRIAL271 { get; set; }
    }
}
