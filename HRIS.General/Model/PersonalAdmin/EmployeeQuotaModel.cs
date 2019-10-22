using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.PersonalAdmin
{
    public class EmployeeQuotaModel
    {
        public int id { get; set; }

        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }
        public string UnattendanceCode { get; set; }

        public int? status_employee_id { get; set; }

        public int? unattendance_id { get; set; }

        public int? quota { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public DateTime? created_date { get; set; }

        public string created_by { get; set; }

        public DateTime? modified_date { get; set; }

        public string modified_by { get; set; }

        public short? del_flag { get; set; }

        

    }
}
