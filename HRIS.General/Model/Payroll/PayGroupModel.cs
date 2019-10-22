using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Payroll
{
    public class PayGroupModel
    {
        public int id { get; set; }

        public string pgrid { get; set; }

        public string sdesc { get; set; }

        public int? payroll_periode_id { get; set; }

        public DateTime? begdt { get; set; }

        public DateTime? enddt { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public short? delfl { get; set; }

        public string TRIAL281 { get; set; }

    }
}
