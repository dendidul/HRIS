using System;
using System.Collections.Generic;
using System.Text;
using HRIS.General.Model.Master;

namespace HRIS.Service.Interface
{
    public interface IMasterManager
    {

        // ================= Area ======================= 

        IList<AreaModel> GetAllArea();
        AreaModel GetArea(AreaModel model);
        AreaModel CreateArea(AreaModel model);
        AreaModel UpdateArea(AreaModel model);
        void DeleteArea(int id);

        // ===================||==================== 

        // ================= CostCenter =======================

        IList<CostCenterModel> GetAllCostCenter();
        CostCenterModel GetCostCenter(CostCenterModel model);
        CostCenterModel CreateCostCenter(CostCenterModel model);
        CostCenterModel UpdateCostCenter(CostCenterModel model);
        void DeleteCostCenter(int id);

        // ===================||==================== 

        // ================= Job =======================

        IList<JobModel> GetAllJob();
        JobModel GetJob(JobModel model);
        JobModel CreateJob(JobModel model);
        JobModel UpdateJob(JobModel model);
        void DeleteJob(int id);

        // ===================||==================== 

        // ================= LevelGrade =======================

        IList<LevelGradeModel> GetAllLevelGrade();
        LevelGradeModel GetLevelGrade(LevelGradeModel model);
        LevelGradeModel CreateLevelGrade(LevelGradeModel model);
        LevelGradeModel UpdateLevelGrade(LevelGradeModel model);
        void DeleteLevelGrade(int id);

        // ===================||==================== 

        // ================= Object Master =======================

        IList<ObjectMasterModel> GetAllObjectMaster();
        ObjectMasterModel GetObjectMaster(ObjectMasterModel model);
        ObjectMasterModel CreateObjectMaster(ObjectMasterModel model);
        ObjectMasterModel UpdateObjectMaster(ObjectMasterModel model);
        void DeleteObjectMaster(int id);

        // ===================||==================== 


        // ================= Position =======================

        IList<PositionModel> GetAllPosition();
        PositionModel GetPosition(PositionModel model);
        PositionModel CreatePosition(PositionModel model);
        PositionModel UpdatePosition(PositionModel model);
        void DeletePosition(int id);

        // ===================||==================== 

        // ================= Unit =======================

        IList<UnitModel> GetAllUnit();
        UnitModel GetUnit(UnitModel model);
        UnitModel CreateUnit(UnitModel model);
        UnitModel UpdateUnit(UnitModel model);
        void DeleteUnit(int id);

        // ===================||==================== 


        // ================= EmployeeGroup =======================

        IList<EmployeeGroupModel> GetAllEmployeeGroup();
        EmployeeGroupModel GetEmployeeGroup(EmployeeGroupModel model);
        EmployeeGroupModel CreateEmployeeGroup(EmployeeGroupModel model);
        EmployeeGroupModel UpdateEmployeeGroup(EmployeeGroupModel model);
        void DeleteEmployeeGroup(int id);


        // ===================||==================== 



    }
}
