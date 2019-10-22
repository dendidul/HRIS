using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Tax
{
   public  class TaxCalcModel
    {

        public int id { get; set; }

        public int status_employee_id { get; set; }

        public int sequence { get; set; }

        public int? year_period { get; set; }

        public decimal? regular_net { get; set; }

        public decimal? regular_gross { get; set; }

        public decimal? irregular_net { get; set; }

        public decimal? irregular_gross { get; set; }

        public decimal? ptkp { get; set; }

        public decimal? tax_paid { get; set; }

        public int? payroll_process_id { get; set; }

        public decimal? position_cost { get; set; }

        public string kpp { get; set; }

        public decimal? tax_allowance { get; set; }

        public decimal? payment_regular_net { get; set; }

        public decimal? deduction_regular_net { get; set; }

        public decimal? tax_regular_net { get; set; }

        public int? tax_service_office_id { get; set; }

        public string TRIAL284 { get; set; }
    }
}
