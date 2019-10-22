using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.PersonalAdmin
{
    public class EmployeeStatusModel
    {
        public int id { get; set; }

        public string employee_status { get; set; }

        public DateTime? begin_date { get; set; }

        public int person_id { get; set; }

        public DateTime? end_date { get; set; }

        public string code { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public short? flag_delete { get; set; }

        public int? user_id { get; set; }

        public string TRIAL284 { get; set; }
    }
}
