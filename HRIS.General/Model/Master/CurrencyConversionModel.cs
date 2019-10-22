using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Master
{
   public class CurrencyConversionModel
    {
        public int id { get; set; }

        public int? currency_id { get; set; }

        public int? currency_value { get; set; }

        public int? currency_default { get; set; }

        public short? flag_delete { get; set; }

        public string created_by { get; set; }

        public string modified_by { get; set; }

        public DateTime? created_date { get; set; }

        public DateTime? modified_date { get; set; }

        public string currency_base { get; set; }

        public string TRIAL271 { get; set; }
    }
}
