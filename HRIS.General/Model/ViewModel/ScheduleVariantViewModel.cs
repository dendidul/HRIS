using HRIS.General.Model.TimeManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.General.Model.ViewModel
{
   public class ScheduleVariantViewModel
    {
        public ScheduleVariantModel Model { get; set; }
        public List<ScheduleVariantDetailsModel> Details { get; set; }

    }
}
