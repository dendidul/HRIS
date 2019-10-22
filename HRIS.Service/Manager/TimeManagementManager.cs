using System;
using System.Collections.Generic;
using System.Text;
using HRIS.General.Model.TimeManagement;
using HRIS.General.Utility;
using HRIS.Service.Interface;
using HRIS.TimeManagement.Model.Dao;
using Microsoft.Extensions.Configuration;

namespace HRIS.Service.Manager
{
    public class TimeManagementManager:ITimeManagementManager
    {
        private readonly IConfiguration _config;
        private readonly Logger _logger;
        private readonly TimeFrameDao TimeFrameDao;
        private readonly ScheduleVariantDao ScheduleVariantDao;
        private readonly WorkgroupDao WorkgroupDao;
        private readonly UnattendanceConfigDao UnattendanceConfigDao;
        private readonly CompanyCalendarDao CompanyCalendarDao;
        private readonly UnattendanceReqDao UnattendanceReqDao;
        private readonly TimesheetReqDao TimesheetReqDao;
        private readonly OvertimeReqDao OvertimeReqDao;

        public TimeManagementManager(IConfiguration _config)
        {
            this._config = _config;
            this._logger = new Logger(_config);
            this.TimeFrameDao = new TimeFrameDao(_config);
            this.ScheduleVariantDao = new ScheduleVariantDao(_config);
            this.WorkgroupDao = new WorkgroupDao(_config);
            this.UnattendanceConfigDao = new UnattendanceConfigDao(_config);
            this.CompanyCalendarDao = new CompanyCalendarDao(_config);
            this.UnattendanceReqDao = new UnattendanceReqDao(_config);
            this.TimesheetReqDao = new TimesheetReqDao(_config);
            this.OvertimeReqDao = new OvertimeReqDao(_config);

        }

        public string DestinationLogFolder()
        {
            return _config.GetSection("Logging:DestinationFolder:Service").Value.ToString();
        }

        public TimeFrameModel CreateTimeFrame(TimeFrameModel model)
        {
            var data = new TimeFrameModel();

            try
            {
                data = TimeFrameDao.CreateTimeFrame(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateTimeFrame", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteTimeFrame(int id)
        {
            try
            {
                TimeFrameDao.DeleteTimeFrame(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteTimeFrame", ex.Message, "Service");

            }
        }       

        public IList<TimeFrameModel> GetAllTimeFrame()
        {
            try
            {
                var data = TimeFrameDao.GetAllTimeFrame();
                return data;
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllTimeFrame", ex.Message, "Service");
                return null;
            }
        }

        public TimeFrameModel GetTimeFrame(TimeFrameModel model)
        {
            var data = new TimeFrameModel();

            try
            {
                data = TimeFrameDao.GetTimeFrame(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTimeFrame", ex.Message, "Service");

            }

            return data;
        }

        public TimeFrameModel UpdateTimeFrame(TimeFrameModel model)
        {
            var data = new TimeFrameModel();

            try
            {
                data = TimeFrameDao.UpdateTimeFrame(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateTimeFrame", ex.Message, "Service");

            }

            return data;
        }

        public IList<ScheduleVariantModel> GetAllScheduleVariant()
        {
            IList<ScheduleVariantModel> data = new List<ScheduleVariantModel>();

            try
            {
                data = ScheduleVariantDao.GetAllScheduleVariant();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllScheduleVariant", ex.Message, "Service");

            }

            return data;
        }

        public ScheduleVariantModel GetScheduleVariant(ScheduleVariantModel model)
        {
            var data = new ScheduleVariantModel();

            try
            {
                data = ScheduleVariantDao.GetScheduleVariant(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetScheduleVariant", ex.Message, "Service");

            }

            return data;
        }

        public ScheduleVariantModel CreateScheduleVariant(ScheduleVariantModel model)
        {
            var data = new ScheduleVariantModel();

            try
            {
                data = ScheduleVariantDao.CreateScheduleVariant(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateScheduleVariant", ex.Message, "Service");

            }

            return data;
        }

        public ScheduleVariantModel UpdateScheduleVariant(ScheduleVariantModel model)
        {
            var data = new ScheduleVariantModel();

            try
            {
                data = ScheduleVariantDao.UpdateScheduleVariant(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateScheduleVariant", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteScheduleVariant(int id)
        {
            try
            {
                ScheduleVariantDao.DeleteScheduleVariant(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteScheduleVariant", ex.Message, "Service");

            }
        }

        public IList<WorkGroupModel> GetAllWorkgroup()
        {
            IList<WorkGroupModel> data = new List<WorkGroupModel>();

            try
            {
                data = WorkgroupDao.GetAllWorkgroup();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllWorkgroup", ex.Message, "Service");

            }

            return data;
        }

        public WorkGroupModel GetWorkGroup(WorkGroupModel model)
        {
            var data = new WorkGroupModel();

            try
            {
                data = WorkgroupDao.GetWorkGroup(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetWorkGroup", ex.Message, "Service");

            }

            return data;
        }

        public WorkGroupModel CreateWorkGroup(WorkGroupModel model)
        {
            var data = new WorkGroupModel();

            try
            {
                data = WorkgroupDao.CreateWorkGroup(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateWorkGroup", ex.Message, "Service");

            }

            return data;
        }

        public WorkGroupModel UpdateWorkGroup(WorkGroupModel model)
        {
            var data = new WorkGroupModel();

            try
            {
                data = WorkgroupDao.UpdateWorkGroup(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateWorkGroup", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteWorkGroup(int id)
        {
            try
            {
                WorkgroupDao.DeleteWorkGroup(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteWorkGroup", ex.Message, "Service");

            }
        }

        public IList<UnattendanceModel> GetAllUnattendanceConfig()
        {

            IList<UnattendanceModel> data = new List<UnattendanceModel>();

            try
            {
                data = UnattendanceConfigDao.GetAllUnattendanceConfig();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllUnattendanceConfig", ex.Message, "Service");

            }

            return data;
        }

        public UnattendanceModel GetUnattendanceConfig(UnattendanceModel model)
        {
            var data = new UnattendanceModel();

            try
            {
                data = UnattendanceConfigDao.GetUnattendanceConfig(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetUnattendanceConfig", ex.Message, "Service");

            }

            return data;
        }

        public UnattendanceModel CreateUnattendanceConfig(UnattendanceModel model)
        {
            var data = new UnattendanceModel();

            try
            {
                data = UnattendanceConfigDao.CreateUnattendanceConfig(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateUnattendanceConfig", ex.Message, "Service");

            }

            return data;
        }

        public UnattendanceModel UpdateUnattendanceConfig(UnattendanceModel model)
        {
            var data = new UnattendanceModel();

            try
            {
                data = UnattendanceConfigDao.UpdateUnattendanceConfig(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateUnattendanceConfig", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteUnattendanceConfig(int id)
        {
            try
            {
                UnattendanceConfigDao.DeleteUnattendanceConfig(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteUnattendanceConfig", ex.Message, "Service");

            }
        }

        public IList<CompanyCalendarModel> GetAllCompanyCalendar()
        {
            IList<CompanyCalendarModel> data = new List<CompanyCalendarModel>();

            try
            {
                data = CompanyCalendarDao.GetAllCompanyCalendar();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllCompanyCalendar", ex.Message, "Service");

            }

            return data;
        }

        public CompanyCalendarModel GetCompanyCalendar(CompanyCalendarModel model)
        {
            var data = new CompanyCalendarModel();

            try
            {
                data = CompanyCalendarDao.GetCompanyCalendar(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetCompanyCalendar", ex.Message, "Service");

            }

            return data;
        }

        public CompanyCalendarModel CreateCompanyCalendar(CompanyCalendarModel model)
        {
            var data = new CompanyCalendarModel();

            try
            {
                data = CompanyCalendarDao.CreateCompanyCalendar(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateCompanyCalendar", ex.Message, "Service");

            }

            return data;
        }

        public CompanyCalendarModel UpdateCompanyCalendar(CompanyCalendarModel model)
        {
            var data = new CompanyCalendarModel();

            try
            {
                data = CompanyCalendarDao.UpdateCompanyCalendar(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateCompanyCalendar", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteCompanyCalendar(int id)
        {
            try
            {
                CompanyCalendarDao.DeleteCompanyCalendar(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteCompanyCalendar", ex.Message, "Service");

            }
        }

        public IList<TimesheetReqModel> GetAllTimesheetReq()
        {

            IList<TimesheetReqModel> data = new List<TimesheetReqModel>();

            try
            {
                data = TimesheetReqDao.GetAllTimesheetReq();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllTimesheetReq", ex.Message, "Service");

            }

            return data;
        }

        public TimesheetReqModel GetTimesheetReq(TimesheetReqModel model)
        {
            var data = new TimesheetReqModel();

            try
            {
                data = TimesheetReqDao.GetTimesheetReq(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTimesheetReq", ex.Message, "Service");

            }

            return data;
        }

        public TimesheetReqModel CreateTimesheetReq(TimesheetReqModel model)
        {
            var data = new TimesheetReqModel();

            try
            {
                data = TimesheetReqDao.CreateTimesheetReq(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateTimesheetReq", ex.Message, "Service");

            }

            return data;
        }

        public TimesheetReqModel UpdateTimesheetReq(TimesheetReqModel model)
        {
            var data = new TimesheetReqModel();

            try
            {
                data = TimesheetReqDao.UpdateTimesheetReq(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateTimesheetReq", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteTimesheetReq(int id)
        {
            try
            {
                TimesheetReqDao.DeleteTimesheetReq(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteTimesheetReq", ex.Message, "Service");

            }
        }

        public IList<UnattendanceReqModel> GetAllUnattendanceReq()
        {
            IList<UnattendanceReqModel> data = new List<UnattendanceReqModel>();

            try
            {
                data = UnattendanceReqDao.GetAllUnattendanceReq();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllUnattendanceReq", ex.Message, "Service");

            }

            return data;
        }

        public UnattendanceReqModel GetUnattendanceReq(UnattendanceReqModel model)
        {
            var data = new UnattendanceReqModel();

            try
            {
                data = UnattendanceReqDao.GetUnattendanceReq(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetUnattendanceReq", ex.Message, "Service");

            }

            return data;
        }

        public UnattendanceReqModel CreateUnattendanceReq(UnattendanceReqModel model)
        {
            var data = new UnattendanceReqModel();

            try
            {
                data = UnattendanceReqDao.CreateUnattendanceReq(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateUnattendanceReq", ex.Message, "Service");

            }

            return data;
        }

        public UnattendanceReqModel UpdateUnattendanceReq(UnattendanceReqModel model)
        {
            var data = new UnattendanceReqModel();

            try
            {
                data = UnattendanceReqDao.UpdateUnattendanceReq(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateUnattendanceReq", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteUnattendanceReq(int id)
        {
            try
            {
                UnattendanceReqDao.DeleteUnattendanceReq(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteUnattendanceReq", ex.Message, "Service");

            }
        }

        public IList<OvertimeRequestModel> GetAllOvertimeRequest()
        {
            IList<OvertimeRequestModel> data = new List<OvertimeRequestModel>();

            try
            {
                data = OvertimeReqDao.GetAllOvertimeRequest();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllOvertimeRequest", ex.Message, "Service");

            }

            return data;
        }

        public OvertimeRequestModel GetOvertimeRequest(OvertimeRequestModel model)
        {

            var data = new OvertimeRequestModel();

            try
            {
                data = OvertimeReqDao.GetOvertimeRequest(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetOvertimeRequest", ex.Message, "Service");

            }

            return data;
        }

        public OvertimeRequestModel CreateOvertimeRequest(OvertimeRequestModel model)
        {
            var data = new OvertimeRequestModel();

            try
            {
                data = OvertimeReqDao.CreateOvertimeRequest(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateOvertimeRequest", ex.Message, "Service");

            }

            return data;
        }

        public OvertimeRequestModel UpdateOvertimeRequest(OvertimeRequestModel model)
        {
            var data = new OvertimeRequestModel();

            try
            {
                data = OvertimeReqDao.UpdateOvertimeRequest(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateOvertimeRequest", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteOvertimeRequest(int id)
        {
            try
            {
                OvertimeReqDao.DeleteOvertimeRequest(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteOvertimeRequest", ex.Message, "Service");

            }
        }
    }
}
