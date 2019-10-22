using HRIS.General.Model.PersonalAdmin;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.Service.Interface
{
    public interface IPersonalAdminManager
    {
        IList<PeopleModel> GetAllPeople();


        IList<EmployeeQuotaModel> GetAllEmployeeQuota();
        EmployeeQuotaModel GetEmployeeQuota(EmployeeQuotaModel model);
        EmployeeQuotaModel CreateEmployeeQuota(EmployeeQuotaModel model);
        EmployeeQuotaModel UpdateEmployeeQuota(EmployeeQuotaModel model);
        void DeleteEmployeeQuota(int id);
    }
}
