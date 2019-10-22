using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Tax
{
   public class TaxStatusModel
    {
        public int id { get; set; }

        public int tax_parameter_id { get; set; }

        public string tax_status_id { get; set; }

        public string tax_status_desc { get; set; }

        public string amount { get; set; }

        public int? del_flag { get; set; }

        public string TRIAL288 { get; set; }

    }
}
