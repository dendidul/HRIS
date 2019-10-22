using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.PersonalAdmin
{
    public class EmployeeTimesModel
    {
        public int id { get; set; }

        public int workgroup_id { get; set; }

        public DateTime? starting_date { get; set; }

        public int? start_seq { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public int employee_id { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public short? delete_flag { get; set; }

        public int? finger_id { get; set; }

        public string TRIAL274 { get; set; }
    }
}
