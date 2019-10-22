using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.PersonalAdmin
{
    public class EmployeeBankAddressModel
    {
        public int id { get; set; }

        public int? employee_id { get; set; }

        public string AddressType { get; set; }

        public string Address { get; set; }

        public string Telp { get; set; }

        public int? del_flag { get; set; }

        public string TRIAL271 { get; set; }
    }
}
