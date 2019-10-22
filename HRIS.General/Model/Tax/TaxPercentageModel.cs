using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Tax
{
    public class TaxPercentageModel
    {
        public int id { get; set; }

        public int tax_parameter_id { get; set; }

        public decimal? min_amount { get; set; }

        public decimal? max_amount { get; set; }

        public string tax_tarif { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public short? npwp_flag { get; set; }

        public int? del_flag { get; set; }

        public string TRIAL288 { get; set; }
    }
}
