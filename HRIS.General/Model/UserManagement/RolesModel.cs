using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.UserManagement
{
    public class RolesModel
    {
        public int id { get; set; }

        public DateTime? createdDate { get; set; }

        public DateTime? modifiedDate { get; set; }

        public string role_id { get; set; }

        public string role_desc { get; set; }

        public string created_by { get; set; }

        public string modified_by { get; set; }

        public int? del_flag { get; set; }

        public string TRIAL284 { get; set; }
    }
}
