using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.PersonalAdmin
{
    public class EmployeeAttributesModel
    {
        public int id { get; set; }

        public int people_id { get; set; }

        public int? status_employee_id { get; set; }

        public int? employee_subgroup_id { get; set; }

        public int? company_id { get; set; }

        public int? unit_id { get; set; }

        public int? position_id { get; set; }

        public int? job_id { get; set; }

        public int? personal_area_id { get; set; }

        public short? flag_delete { get; set; }

        public DateTime? end_contract { get; set; }

        public string supervisor_empid { get; set; }

        public string email { get; set; }

        public int? employee_grade_id { get; set; }

        public string employee_basic_wage { get; set; }

        public string employee_currency { get; set; }

        public int? payroll_area_id { get; set; }

        public int? payroll_component_insurance_id { get; set; }

        public int? payroll_group_id { get; set; }

        public string created_by { get; set; }

        public string modified_by { get; set; }

        public int? cost_center_id { get; set; }

        public int? bank_source_id { get; set; }

        public DateTime? created_date { get; set; }

        public DateTime? modified_date { get; set; }

        public int? level_grade_id { get; set; }

        public string nik { get; set; }

        public string company_desc { get; set; }

        public string employee_subgroup_desc { get; set; }

        public string unit_desc { get; set; }

        public string position_desc { get; set; }

        public string job_desc { get; set; }

        public string personal_area_desc { get; set; }

        public string payroll_area_desc { get; set; }

        public string payroll_group_desc { get; set; }

        public string level_grade_desc { get; set; }

        public string employee_grade_desc { get; set; }

        public string cost_center_desc { get; set; }

        public string bank_source_desc { get; set; }

        public string status_employee { get; set; }

        public DateTime? hire_date { get; set; }

        public DateTime? terminate_date { get; set; }

        public string TRIAL271 { get; set; }

    }
}
