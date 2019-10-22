using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.PersonalAdmin
{
   public class PeopleModel
    {
        public string person_code { get; set; }

        public string first_name { get; set; }

        public string middle_name { get; set; }

        public string last_name { get; set; }

        public string birth_place { get; set; }

        public string gender { get; set; }

        public DateTime? birth_date { get; set; }

        public DateTime? die_date { get; set; }

        public string die_place { get; set; }

        public int id { get; set; }

        public string religion { get; set; }

        public string marital_status_id { get; set; }

        public int? applicant_id { get; set; }

        public string photo_path { get; set; }

        public string photo_name { get; set; }

        public string created_by { get; set; }

        public DateTime? created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime? modified_date { get; set; }

        public string applicant_ids { get; set; }

        public string blacklist_id { get; set; }

        public string majority { get; set; }

        public string education { get; set; }

        public string institution { get; set; }
        public string status_employee { get; set; }
        public string nik { get; set; }

        public DateTime? hire_date { get; set; }

        public DateTime? terminate_date { get; set; }

        public int? begin_year { get; set; }

        public int? end_year { get; set; }

        public double? grade { get; set; }

        public double? max_grade { get; set; }

        public string TRIAL284 { get; set; }

    }
}
