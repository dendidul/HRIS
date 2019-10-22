using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.TimeManagement
{
    public class OvertimeRequestModel
    {
        public int id { get; set; }

        public string doc_number { get; set; }

        public int? employee_id { get; set; }

        public int? unit_id { get; set; }

        public string description { get; set; }

        public DateTime? created_date { get; set; }

        public string created_by { get; set; }

        public DateTime? modified_date { get; set; }

        public string modified_by { get; set; }

        public string request_status { get; set; }

        public int del_flag { get; set; }

        public string NIK { get; set; }

        public string EmployeeName { get; set; }
        public string UnitCode { get; set; }
    }
}
