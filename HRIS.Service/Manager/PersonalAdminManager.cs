using System;
using System.Collections.Generic;
using System.Text;
using HRIS.General.Model.PersonalAdmin;
using HRIS.General.Utility;
using HRIS.PersonalAdmin.Model.Dao;
using HRIS.Service.Interface;
using Microsoft.Extensions.Configuration;

namespace HRIS.Service.Manager
{
    public class PersonalAdminManager:IPersonalAdminManager
    {
        private readonly IConfiguration _config;
        private readonly Logger _logger;
        private readonly PeopleDao PeopleDao;
        private readonly EmployeeLeaveQuotaDao EmployeeLeaveQuotaDao;

        public PersonalAdminManager(IConfiguration _config)
        {
            this._config = _config;
            this._logger = new Logger(_config);
            this.PeopleDao = new PeopleDao(_config);
            this.EmployeeLeaveQuotaDao = new EmployeeLeaveQuotaDao(_config);



        }

 

        public string DestinationLogFolder()
        {
            return _config.GetSection("Logging:DestinationFolder:Service").Value.ToString();
        }

        

        public IList<PeopleModel> GetAllPeople()
        {
            IList<PeopleModel> data = new List<PeopleModel>();

            try
            {
                data = PeopleDao.GetAllPeople();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllPeople", ex.Message, "Service");

            }

            return data;
        }


        public IList<EmployeeQuotaModel> GetAllEmployeeQuota()
        {
            IList<EmployeeQuotaModel> data = new List<EmployeeQuotaModel>();

            try
            {
                data = EmployeeLeaveQuotaDao.GetAllEmployeeQuota();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllEmployeeQuota", ex.Message, "Service");

            }

            return data;

        }

        public EmployeeQuotaModel GetEmployeeQuota(EmployeeQuotaModel model)
        {
            var data = new EmployeeQuotaModel();

            try
            {
                data = EmployeeLeaveQuotaDao.GetEmployeeQuota(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetEmployeeQuota", ex.Message, "Service");

            }

            return data;
        }

        public EmployeeQuotaModel UpdateEmployeeQuota(EmployeeQuotaModel model)
        {
            var data = new EmployeeQuotaModel();

            try
            {
                data = EmployeeLeaveQuotaDao.UpdateEmployeeQuota(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateEmployeeQuota", ex.Message, "Service");

            }

            return data;
        }

        public EmployeeQuotaModel CreateEmployeeQuota(EmployeeQuotaModel model)
        {
            var data = new EmployeeQuotaModel();

            try
            {
                data = EmployeeLeaveQuotaDao.CreateEmployeeQuota(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateEmployeeQuota", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteEmployeeQuota(int id)
        {
            try
            {
                EmployeeLeaveQuotaDao.DeleteEmployeeQuota(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteEmployeeQuota", ex.Message, "Service");

            }
        }



    }
}
