using System;
using System.Collections.Generic;

namespace HRIS.General.Model.PersonalAdmin
{
    public class EmployeeAttributeViewModel
    {
        public PeopleModel People { get; set; } = new PeopleModel();
        public EmployeeAttributesModel EmployeeAttributes { get; set; } = new EmployeeAttributesModel();
        public EmployeeTimesModel EmpTime { get; set; } = new EmployeeTimesModel();
        public EmployeeTaxesModel EmpTax { get; set; } = new EmployeeTaxesModel();

        public List<EmployeeQuotaModel> QuotesList { get; set; } = new List<EmployeeQuotaModel>();
        public List<EmployeeFamilyModel> FamilyList { get; set; } = new List<EmployeeFamilyModel>();
        public List<EmployeeBankAddressModel> AddressList { get; set; } = new List<EmployeeBankAddressModel>();
        public List<EmployeeBankAccount> BankList { get; set; } = new List<EmployeeBankAccount>();
        public List<EmployeeMembershipsModel> MembershipList { get; set; } = new List<EmployeeMembershipsModel>();

        public string SupervisorName { get; set; }
        public string WorkgroupName { get; set; }
        public string Kpp { get; set; }
        public string TaxStatus { get; set; }
    }
}
