using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Master
{
    public class EducationModel
    {
        public int id { get; set; }

        public int person_id { get; set; }

        public string edu_type { get; set; }

        public string edu_institution { get; set; }

        public string edu_subject { get; set; }

        public double? edu_grade { get; set; }

        public double? edu_maxgrade { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public DateTime? begin_date { get; set; }

        public DateTime? end_date { get; set; }

        public int? begin_year { get; set; }

        public int? end_year { get; set; }

        public string TRIAL271 { get; set; }

    }
}
