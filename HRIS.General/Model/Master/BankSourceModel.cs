using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Master
{
    public class BankSourceModel
    {

        public int id { get; set; }

        public string bank_source_id { get; set; }

        public string rec_number { get; set; }

        public string bank_name { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public short? del_flag { get; set; }

        public string TRIAL268 { get; set; }

    }
}
