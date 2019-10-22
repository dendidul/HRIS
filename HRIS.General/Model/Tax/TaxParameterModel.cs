using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Tax
{
    public class TaxParameterModel
    {
        public int id { get; set; }

        public string tax_parameter_id { get; set; }

        public string short_desc { get; set; }

        public string long_desc { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public decimal? max_position_tax { get; set; }

        public short? position_tax_percentage { get; set; }

        public int? tax_component_id { get; set; }

        public int? tax_allowance_id { get; set; }

        public short? flag_delete { get; set; }

        public string TRIAL288 { get; set; }
    }
}
