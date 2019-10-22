using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.PersonalAdmin
{
   public class EmployeeFamilyModel
    {
        public int id { get; set; }

        public int? employee_id { get; set; }

        public string relation_type { get; set; }

        public string name { get; set; }

        public DateTime? birthdate { get; set; }

        public string education { get; set; }

        public int? del_flag { get; set; }

        public string TRIAL271 { get; set; }

    }
}
