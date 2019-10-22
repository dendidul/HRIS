using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Tax
{
    public class TaxServiceOfficeModel
    {
        public int id { get; set; }

        public int? address_id { get; set; }

        public string tax_office_code { get; set; }

        public string short_desc { get; set; }

        public string long_desc { get; set; }

        public short? flag_delete { get; set; }

        public string TRIAL288 { get; set; }
    }
}
