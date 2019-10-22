using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.UserManagement
{
    public class UserModel
    {
        public int id { get; set; }

        public string username { get; set; }

        public string password { get; set; }
        public string employeename { get; set; }
        public string employeeid { get; set; }

        public int? role_id { get; set; }

        public int? status_employee_id { get; set; }

        public DateTime? createdDate { get; set; }

        public DateTime? modifiedDate { get; set; }

        public string created_by { get; set; }

        public string modified_by { get; set; }

        public short? del_flag { get; set; }

        public string TRIAL291 { get; set; }
    }
}
