using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.PersonalAdmin
{
    public class EmployeeGradeModel
    {
        public int id { get; set; }

        public string grade_id { get; set; }

        public string grade_desc { get; set; }

        public string min_rate { get; set; }

        public string max_rate { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public int? level_grade_id { get; set; }

        public short? flag_delete { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public string TRIAL271 { get; set; }
    }
}
