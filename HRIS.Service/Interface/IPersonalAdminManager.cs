using HRIS.General.Model.PersonalAdmin;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.Service.Interface
{
    public interface IPersonalAdminManager
    {
        IList<PeopleModel> GetAllPeople();
        PeopleModel GetPeople(int id);
        EmployeeAttributeViewModel GetEmployeeDetail(int peopleId);
        int CreateEmployee(EmployeeAttributeViewModel vm);
        void UpdateEmployee(EmployeeAttributeViewModel vm);
        void TerminateEmployee(int peopleId, DateTime terminateDate);
        void DeleteEmployee(int peopleId);

        IList<EmployeeQuotaModel> GetAllEmployeeQuota();
        EmployeeQuotaModel GetEmployeeQuota(EmployeeQuotaModel model);
        EmployeeQuotaModel CreateEmployeeQuota(EmployeeQuotaModel model);
        EmployeeQuotaModel UpdateEmployeeQuota(EmployeeQuotaModel model);
        void DeleteEmployeeQuota(int id);
    }
}
