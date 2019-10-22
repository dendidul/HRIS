using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.TimeManagement
{
    public class TimeCalculationDetailsModel
    {
        public int id { get; set; }

        public int time_calculation_id { get; set; }

        public int status_employee_id { get; set; }

        public int time_unit_id { get; set; }

        public int time_unit_amount { get; set; }

        public string time_unit_desc { get; set; }

        public string TRIAL288 { get; set; }

    }
}
