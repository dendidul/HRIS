using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Tax
{
    public class TaxLineComponent
    {
        public int id { get; set; }

        public string tax_line { get; set; }

        public int payroll_component_id { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public short? flag_delete { get; set; }

        public string TRIAL288 { get; set; }
    }
}
