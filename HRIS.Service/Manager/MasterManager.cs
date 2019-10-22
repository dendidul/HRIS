using System;
using System.Collections.Generic;
using System.Text;
using HRIS.General.Model.Master;
using HRIS.General.Utility;
using HRIS.Master.Model.Dao;
using HRIS.Service.Interface;
using Microsoft.Extensions.Configuration;

namespace HRIS.Service.Manager
{
    public class MasterManager : IMasterManager
    {

        private readonly IConfiguration _config;
        private readonly Logger _logger;
        private readonly AreaDao AreaDao;
        private readonly CostCenterDao CostCenterDao;
        private readonly JobDao JobDao;
        private readonly LevelGradeDao LevelGradeDao;
        private readonly ObjectMasterDao ObjectMasterDao;
        private readonly PositionDao PositionDao;
        private readonly UnitDao UnitDao;
        private readonly EmployeeGroupDao EmployeeGroupDao;


        public MasterManager(IConfiguration _config)
        {
            this._config = _config;
            this._logger = new Logger(_config);
            this.AreaDao = new AreaDao(_config);
            this.CostCenterDao = new CostCenterDao(_config);
            this.JobDao = new JobDao(_config);
            this.LevelGradeDao = new LevelGradeDao(_config);
            this.ObjectMasterDao = new ObjectMasterDao(_config);
            this.PositionDao = new PositionDao(_config);
            this.UnitDao = new UnitDao(_config);
            this.EmployeeGroupDao = new EmployeeGroupDao(_config);



        }

        public string DestinationLogFolder()
        {
            return _config.GetSection("Logging:DestinationFolder:Service").Value.ToString();
        }


        public AreaModel CreateArea(AreaModel model)
        {
            var data = new AreaModel();

            try
            {
                data = AreaDao.CreateArea(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateArea", ex.Message, "Service");

            }

            return data;
        }

        public CostCenterModel CreateCostCenter(CostCenterModel model)
        {
            var data = new CostCenterModel();

            try
            {
                data = CostCenterDao.CreateCostCenter(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateCostCenter", ex.Message, "Service");

            }

            return data;
        }

        public JobModel CreateJob(JobModel model)
        {
            var data = new JobModel();

            try
            {
                data = JobDao.CreateJob(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateJob", ex.Message, "Service");

            }

            return data;
        }

        public LevelGradeModel CreateLevelGrade(LevelGradeModel model)
        {
            var data = new LevelGradeModel();

            try
            {
                data = LevelGradeDao.CreateLevelGrade(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateLevelGrade", ex.Message, "Service");

            }

            return data;
        }

        public ObjectMasterModel CreateObjectMaster(ObjectMasterModel model)
        {
            var data = new ObjectMasterModel();

            try
            {
                data = ObjectMasterDao.CreateObjectMaster(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateObjectMaster", ex.Message, "Service");

            }

            return data;
        }

        public PositionModel CreatePosition(PositionModel model)
        {
            var data = new PositionModel();

            try
            {
                data = PositionDao.CreatePosition(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreatePosition", ex.Message, "Service");

            }

            return data;
        }

        public UnitModel CreateUnit(UnitModel model)
        {
            var data = new UnitModel();

            try
            {
                data = UnitDao.CreateUnit(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UnitModel", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteArea(int id)
        {
            try
            {
                AreaDao.DeleteArea(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteArea", ex.Message, "Service");

            }
        }

        public void DeleteCostCenter(int id)
        {
            try
            {
                CostCenterDao.DeleteCostCenter(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteCostCenter", ex.Message, "Service");

            }
        }

        public void DeleteJob(int id)
        {
            try
            {
                JobDao.DeleteJob(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteJob", ex.Message, "Service");

            }
        }

        public void DeleteLevelGrade(int id)
        {
            try
            {
                LevelGradeDao.DeleteLevelGrade(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteLevelGrade", ex.Message, "Service");

            }
        }

        public void DeleteObjectMaster(int id)
        {
            try
            {
                ObjectMasterDao.DeleteObjectMaster(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteObjectMaster", ex.Message, "Service");

            }
        }

        public void DeletePosition(int id)
        {
            try
            {
                PositionDao.DeletePosition(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeletePosition", ex.Message, "Service");

            }
        }

        public void DeleteUnit(int id)
        {
            try
            {
                UnitDao.DeleteUnit(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteUnit", ex.Message, "Service");

            }
        }

        public IList<AreaModel> GetAllArea()
        {
            IList<AreaModel> data = new List<AreaModel>();

            try
            {
                data = AreaDao.GetAllArea();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllArea", ex.Message, "Service");

            }

            return data;
        }

        public IList<CostCenterModel> GetAllCostCenter()
        {
            IList<CostCenterModel> data = new List<CostCenterModel>();

            try
            {
                data = CostCenterDao.GetAllCostCenter();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllCostCenter", ex.Message, "Service");

            }

            return data;
        }

        public IList<JobModel> GetAllJob()
        {
            IList<JobModel> data = new List<JobModel>();

            try
            {
                data = JobDao.GetAllJob();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllJob", ex.Message, "Service");

            }

            return data;
        }

        public IList<LevelGradeModel> GetAllLevelGrade()
        {

            IList<LevelGradeModel> data = new List<LevelGradeModel>();

            try
            {
                data = LevelGradeDao.GetAllLevelGrade();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllLevelGrade", ex.Message, "Service");

            }

            return data;
        }

        public IList<ObjectMasterModel> GetAllObjectMaster()
        {
            IList<ObjectMasterModel> data = new List<ObjectMasterModel>();

            try
            {
                data = ObjectMasterDao.GetAllObjectMaster();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllObjectMaster", ex.Message, "Service");

            }

            return data;
        }

        public IList<PositionModel> GetAllPosition()
        {
            IList<PositionModel> data = new List<PositionModel>();

            try
            {
                data = PositionDao.GetAllPosition();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllPosition", ex.Message, "Service");

            }

            return data;
        }

        public IList<UnitModel> GetAllUnit()
        {
            IList<UnitModel> data = new List<UnitModel>();

            try
            {
                data = UnitDao.GetAllUnit();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllUnit", ex.Message, "Service");

            }

            return data;
        }

        public AreaModel GetArea(AreaModel model)
        {
            var data = new AreaModel();

            try
            {
                data = AreaDao.GetArea(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetArea", ex.Message, "Service");

            }

            return data;
        }

        public CostCenterModel GetCostCenter(CostCenterModel model)
        {
            var data = new CostCenterModel();

            try
            {
                data = CostCenterDao.GetCostCenter(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetCostCenter", ex.Message, "Service");

            }

            return data;
        }

        public JobModel GetJob(JobModel model)
        {
            var data = new JobModel();

            try
            {
                data = JobDao.GetJob(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetJob", ex.Message, "Service");

            }

            return data;
        }

        public LevelGradeModel GetLevelGrade(LevelGradeModel model)
        {
            var data = new LevelGradeModel();

            try
            {
                data = LevelGradeDao.GetLevelGrade(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetLevelGrade", ex.Message, "Service");

            }

            return data;
        }

        public ObjectMasterModel GetObjectMaster(ObjectMasterModel model)
        {
            var data = new ObjectMasterModel();

            try
            {
                data = ObjectMasterDao.GetObjectMaster(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetObjectMaster", ex.Message, "Service");

            }

            return data;
        }

        public PositionModel GetPosition(PositionModel model)
        {
            var data = new PositionModel();

            try
            {
                data = PositionDao.GetPosition(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetPosition", ex.Message, "Service");

            }

            return data;
        }

        public UnitModel GetUnit(UnitModel model)
        {
            var data = new UnitModel();

            try
            {
                data = UnitDao.GetUnit(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetUnit", ex.Message, "Service");

            }

            return data;
        }

        public AreaModel UpdateArea(AreaModel model)
        {
            var data = new AreaModel();

            try
            {
                data = AreaDao.UpdateArea(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateArea", ex.Message, "Service");

            }

            return data;
        }

        public CostCenterModel UpdateCostCenter(CostCenterModel model)
        {
            var data = new CostCenterModel();

            try
            {
                data = CostCenterDao.UpdateCostCenter(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateCostCenter", ex.Message, "Service");

            }

            return data;
        }

        public JobModel UpdateJob(JobModel model)
        {
            var data = new JobModel();

            try
            {
                data = JobDao.UpdateJob(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateJob", ex.Message, "Service");

            }

            return data;
        }

        public LevelGradeModel UpdateLevelGrade(LevelGradeModel model)
        {
            var data = new LevelGradeModel();

            try
            {
                data = LevelGradeDao.UpdateLevelGrade(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateLevelGrade", ex.Message, "Service");

            }

            return data;
        }

        public ObjectMasterModel UpdateObjectMaster(ObjectMasterModel model)
        {
            var data = new ObjectMasterModel();

            try
            {
                data = ObjectMasterDao.UpdateObjectMaster(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateObjectMaster", ex.Message, "Service");

            }

            return data;
        }

        public PositionModel UpdatePosition(PositionModel model)
        {
            var data = new PositionModel();

            try
            {
                data = PositionDao.UpdatePosition(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdatePosition", ex.Message, "Service");

            }

            return data;
        }

        public UnitModel UpdateUnit(UnitModel model)
        {
            var data = new UnitModel();

            try
            {
                data = UnitDao.UpdateUnit(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateUnit", ex.Message, "Service");

            }

            return data;
        }

        public IList<EmployeeGroupModel> GetAllEmployeeGroup()
        {
            IList<EmployeeGroupModel> data = new List<EmployeeGroupModel>();

            try
            {
                data = EmployeeGroupDao.GetAllEmployeeGroup();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllEmployeeGroup", ex.Message, "Service");

            }

            return data;
        }

        public EmployeeGroupModel GetEmployeeGroup(EmployeeGroupModel model)
        {
            var data = new EmployeeGroupModel();

            try
            {
                data = EmployeeGroupDao.GetEmployeeGroup(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetEmployeeGroup", ex.Message, "Service");

            }

            return data;
        }

        public EmployeeGroupModel CreateEmployeeGroup(EmployeeGroupModel model)
        {
            var data = new EmployeeGroupModel();

            try
            {
                data = EmployeeGroupDao.CreateEmployeeGroup(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateEmployeeGroup", ex.Message, "Service");

            }

            return data;
        }

        public EmployeeGroupModel UpdateEmployeeGroup(EmployeeGroupModel model)
        {
            var data = new EmployeeGroupModel();

            try
            {
                data = EmployeeGroupDao.UpdateEmployeeGroup(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateEmployeeGroup", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteEmployeeGroup(int id)
        {
            try
            {
                EmployeeGroupDao.DeleteEmployeeGroup(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteEmployeeGroup", ex.Message, "Service");

            }
        }
    }
}
