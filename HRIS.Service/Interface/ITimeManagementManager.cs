using HRIS.General.Model.TimeManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.Service.Interface
{
    public interface ITimeManagementManager
    {
        IList<TimeFrameModel> GetAllTimeFrame();
        TimeFrameModel GetTimeFrame(TimeFrameModel model);
        TimeFrameModel CreateTimeFrame(TimeFrameModel model);
        TimeFrameModel UpdateTimeFrame(TimeFrameModel model);
        void DeleteTimeFrame(int id);


        IList<ScheduleVariantModel> GetAllScheduleVariant();
        ScheduleVariantModel GetScheduleVariant(ScheduleVariantModel model);
        ScheduleVariantModel CreateScheduleVariant(ScheduleVariantModel model);
        ScheduleVariantModel UpdateScheduleVariant(ScheduleVariantModel model);
        void DeleteScheduleVariant(int id);


        IList<WorkGroupModel> GetAllWorkgroup();
        WorkGroupModel GetWorkGroup(WorkGroupModel model);
        WorkGroupModel CreateWorkGroup(WorkGroupModel model);
        WorkGroupModel UpdateWorkGroup(WorkGroupModel model);
        void DeleteWorkGroup(int id);

        IList<UnattendanceModel> GetAllUnattendanceConfig();
        UnattendanceModel GetUnattendanceConfig(UnattendanceModel model);
        UnattendanceModel CreateUnattendanceConfig(UnattendanceModel model);
        UnattendanceModel UpdateUnattendanceConfig(UnattendanceModel model);
        void DeleteUnattendanceConfig(int id);

        IList<CompanyCalendarModel> GetAllCompanyCalendar();
        CompanyCalendarModel GetCompanyCalendar(CompanyCalendarModel model);
        CompanyCalendarModel CreateCompanyCalendar(CompanyCalendarModel model);
        CompanyCalendarModel UpdateCompanyCalendar(CompanyCalendarModel model);
        void DeleteCompanyCalendar(int id);

        IList<TimesheetReqModel> GetAllTimesheetReq();
        TimesheetReqModel GetTimesheetReq(TimesheetReqModel model);
        TimesheetReqModel CreateTimesheetReq(TimesheetReqModel model);
        TimesheetReqModel UpdateTimesheetReq(TimesheetReqModel model);
        void DeleteTimesheetReq(int id);

        IList<UnattendanceReqModel> GetAllUnattendanceReq();
        UnattendanceReqModel GetUnattendanceReq(UnattendanceReqModel model);
        UnattendanceReqModel CreateUnattendanceReq(UnattendanceReqModel model);
        UnattendanceReqModel UpdateUnattendanceReq(UnattendanceReqModel model);
        void DeleteUnattendanceReq(int id);

        IList<OvertimeRequestModel> GetAllOvertimeRequest();
        OvertimeRequestModel GetOvertimeRequest(OvertimeRequestModel model);
        OvertimeRequestModel CreateOvertimeRequest(OvertimeRequestModel model);
        OvertimeRequestModel UpdateOvertimeRequest(OvertimeRequestModel model);
        void DeleteOvertimeRequest(int id);



    }
}
