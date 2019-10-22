using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Master
{
   public  class CurrencyModel
    {

        public int id { get; set; }

        public string currency_code { get; set; }

        public string description { get; set; }

        public string created_by { get; set; }

        public string modified_by { get; set; }

        public DateTime? created_date { get; set; }

        public DateTime? modified_date { get; set; }

        public short? flag_delete { get; set; }

        public int? order_no { get; set; }

        public string TRIAL271 { get; set; }
    }
}
