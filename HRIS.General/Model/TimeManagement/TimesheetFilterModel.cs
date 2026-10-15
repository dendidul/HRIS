using System;

namespace HRIS.General.Model.TimeManagement
{
    public class TimesheetFilterModel
    {
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public int? AreaId { get; set; }
        public int? WorkGroupId { get; set; }
        public int? EmployeeId { get; set; }
        public string UnattendanceType { get; set; }
        public string EmployeeName { get; set; }
        public string NIK { get; set; }
    }
}

