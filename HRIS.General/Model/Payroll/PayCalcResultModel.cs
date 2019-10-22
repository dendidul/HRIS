using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Payroll
{
    public class PayCalcResultModel
    {
        public int id { get; set; }

        public int payroll_calculation_id { get; set; }

        public int? employee_id { get; set; }

        public int? payroll_component_id { get; set; }

        public string transaction_type { get; set; }

        public decimal? payroll_component_amount { get; set; }

        public decimal? payroll_component { get; set; }

        public decimal? flag { get; set; }

        public string TRIAL281 { get; set; }
    }
}
