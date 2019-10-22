using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Tax
{
   public class TaxComponentAssignmentModel
    {

        public int id { get; set; }

        public int tax_parameter_id { get; set; }

        public int? payroll_component_id { get; set; }

        public string reguller_component { get; set; }

        public string net_flag { get; set; }

        public short? tax_line { get; set; }

        public string tax_component { get; set; }

        public int? del_flag { get; set; }

        public string TRIAL284 { get; set; }
    }
}
