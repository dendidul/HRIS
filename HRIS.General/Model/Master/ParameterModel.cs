using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.Master
{
    public class ParameterModel
    {
        public int Id { get; set; }

        public string ParamKey { get; set; }

        public string ParamName { get; set; }

        public string ParamValue { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public bool? del_flag { get; set; }
    }
}
