using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.PersonalAdmin
{
    public class EmployeeBankAccount
    {
        public int id { get; set; }

        public int? employee_id { get; set; }

        public string bank_name { get; set; }

        public string bank_account { get; set; }

        public int? transfer_porsentase { get; set; }

        public string created_by { get; set; }

        public string modified_by { get; set; }

        public DateTime? created_date { get; set; }

        public DateTime? modified_date { get; set; }

        public short? flag_delete { get; set; }

        public string TRIAL268 { get; set; }
    }
}
