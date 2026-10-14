using System;

namespace HRIS.General.Model.PersonalAdmin
{
    public class EmployeeTaxesModel
    {
        public int id { get; set; }
        public string employee_tax_id { get; set; }
        public int? employee_id { get; set; }
        public int? tax_status_id { get; set; }
        public string tax_type { get; set; }
        public DateTime? begin_date { get; set; }
        public DateTime? end_date { get; set; }
        public string created_by { get; set; }
        public DateTime? created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public int? tax_service_office_id { get; set; }
    }
}
