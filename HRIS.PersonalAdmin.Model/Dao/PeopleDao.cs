using Dapper;
using HRIS.General.Model.PersonalAdmin;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Linq;

namespace HRIS.PersonalAdmin.Model.Dao
{
    public class PeopleDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public PeopleDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public PeopleModel GetPeople(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT p.id, p.person_code, p.first_name, p.middle_name, p.last_name, 
                               p.birth_place, p.gender, p.birth_date, p.religion, 
                               p.marital_status_id, p.photo_path, p.photo_name,
                               p.majority, p.education, p.institution, p.begin_year, p.end_year,
                               p.grade, p.max_grade, p.created_by, p.created_date, p.modified_by, p.modified_date,
                               COALESCE(epa.nik, p.person_code) AS nik,
                               COALESCE(epa.status_employee, 'AKTIF') AS status_employee,
                               epa.hire_date AS hire_date,
                               epa.terminate_date AS terminate_date
                        FROM bos_people p
                        LEFT JOIN bos_employee_pay_attributes epa ON p.id = epa.people_id AND (epa.flag_delete = 0 OR epa.flag_delete IS NULL)
                        WHERE p.id = @id;";
                    return conn.Query<PeopleModel>(sql, new { id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public EmployeeAttributeViewModel GetEmployeeDetail(int peopleId)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var vm = new EmployeeAttributeViewModel();

                    // 1. People
                    const string sqlPeople = @"
                        SELECT id, person_code, first_name, middle_name, last_name, 
                               birth_place, gender, birth_date, religion, marital_status_id, 
                               photo_path, photo_name, majority, education, institution, 
                               begin_year, end_year, grade, max_grade, 
                               created_by, created_date, modified_by, modified_date
                        FROM bos_people 
                        WHERE id = @peopleId;";
                    vm.People = conn.Query<PeopleModel>(sqlPeople, new { peopleId }).FirstOrDefault() ?? new PeopleModel();

                    // 2. Employee Pay Attributes
                    const string sqlAttr = @"
                        SELECT id, people_id, status_employee_id, employee_subgroup_id, company_id, 
                               unit_id, position_id, job_id, personal_area_id, flag_delete, 
                               end_contract, supervisor_empid, email, employee_grade_id, 
                               employee_basic_wage, employee_currency, payroll_area_id, 
                               payroll_component_insurance_id, payroll_group_id, cost_center_id, 
                               bank_source_id, level_grade_id, nik, company_desc, 
                               employee_subgroup_desc, unit_desc, position_desc, job_desc, 
                               personal_area_desc, payroll_area_desc, payroll_group_desc, 
                               level_grade_desc, employee_grade_desc, cost_center_desc, 
                               bank_source_desc, status_employee, hire_date, terminate_date
                        FROM bos_employee_pay_attributes 
                        WHERE people_id = @peopleId AND (flag_delete = 0 OR flag_delete IS NULL)
                        ORDER BY id DESC LIMIT 1;";
                    vm.EmployeeAttributes = conn.Query<EmployeeAttributesModel>(sqlAttr, new { peopleId }).FirstOrDefault() ?? new EmployeeAttributesModel();

                    int empAttrId = vm.EmployeeAttributes.id;

                    if (empAttrId > 0)
                    {
                        // 3. Employee Times
                        const string sqlTime = @"
                            SELECT id, employee_id, workgroup_id, starting_date, start_seq, 
                                   begin_date, end_date, finger_id
                            FROM bos_employee_times 
                            WHERE employee_id = @empAttrId AND (delete_flag = 0 OR delete_flag IS NULL)
                            ORDER BY id DESC LIMIT 1;";
                        vm.EmpTime = conn.Query<EmployeeTimesModel>(sqlTime, new { empAttrId }).FirstOrDefault() ?? new EmployeeTimesModel();

                        // 4. Employee Taxes
                        const string sqlTax = @"
                            SELECT id, employee_tax_id, employee_id, tax_status_id, tax_type, 
                                   begin_date, end_date, tax_service_office_id
                            FROM bos_employee_taxes 
                            WHERE employee_id = @empAttrId 
                            ORDER BY id DESC LIMIT 1;";
                        vm.EmpTax = conn.Query<EmployeeTaxesModel>(sqlTax, new { empAttrId }).FirstOrDefault() ?? new EmployeeTaxesModel();

                        // 5. Bank Accounts
                        const string sqlBank = @"
                            SELECT id, employee_id, bank_account, bank_name, transfer_porsentase
                            FROM bos_bank_accounts 
                            WHERE employee_id = @empAttrId AND (flag_delete = 0 OR flag_delete IS NULL);";
                        vm.BankList = conn.Query<EmployeeBankAccount>(sqlBank, new { empAttrId }).ToList();

                        // 6. Address
                        const string sqlAddr = @"
                            SELECT id, employee_id, ""AddressType"", address_employee AS ""Address"", ""Telp""
                            FROM bos_employee_address 
                            WHERE employee_id = @empAttrId AND (del_flag = 0 OR del_flag IS NULL);";
                        vm.AddressList = conn.Query<EmployeeBankAddressModel>(sqlAddr, new { empAttrId }).ToList();

                        // 7. Family
                        const string sqlFamily = @"
                            SELECT id, employee_id, name, relation_type, birthdate, education
                            FROM bos_employee_family 
                            WHERE employee_id = @empAttrId AND (del_flag = 0 OR del_flag IS NULL);";
                        vm.FamilyList = conn.Query<EmployeeFamilyModel>(sqlFamily, new { empAttrId }).ToList();

                        // 8. Quotas
                        const string sqlQuota = @"
                            SELECT q.id, q.status_employee_id, q.unattendance_id, q.quota, q.begdt, q.enddt,
                                   u.short_desc AS ""UnattendanceCode""
                            FROM bos_employee_quota q
                            LEFT JOIN bos_unattendances u ON q.unattendance_id = u.id
                            WHERE q.status_employee_id = @empAttrId AND (q.flag_delete = 0 OR q.flag_delete IS NULL);";
                        vm.QuotesList = conn.Query<EmployeeQuotaModel>(sqlQuota, new { empAttrId }).ToList();

                        // 9. Workgroup & KPP desc
                        if (vm.EmpTime?.workgroup_id != null && vm.EmpTime.workgroup_id > 0)
                        {
                            vm.WorkgroupName = conn.Query<string>("SELECT short_desc FROM bos_workgroups WHERE id = @wId;", new { wId = vm.EmpTime.workgroup_id }).FirstOrDefault() ?? "";
                        }
                        if (vm.EmpTax?.tax_service_office_id != null && vm.EmpTax.tax_service_office_id > 0)
                        {
                            vm.Kpp = conn.Query<string>("SELECT tax_office_code FROM bos_tax_service_offices WHERE id = @tsoId;", new { tsoId = vm.EmpTax.tax_service_office_id }).FirstOrDefault() ?? "";
                        }
                    }

                    return vm;
                }
            }
            catch
            {
                throw;
            }
        }

        public int CreateEmployee(EmployeeAttributeViewModel vm)
        {
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Insert bos_people
                        const string sqlInsertPeople = @"
                            INSERT INTO bos_people (
                                person_code, first_name, middle_name, last_name, birth_place, gender, 
                                birth_date, religion, marital_status_id, photo_path, photo_name,
                                majority, education, institution, begin_year, end_year, grade, max_grade,
                                created_by, created_date
                            ) VALUES (
                                @person_code, @first_name, @middle_name, @last_name, @birth_place, @gender, 
                                @birth_date, @religion, @marital_status_id, @photo_path, @photo_name,
                                @majority, @education, @institution, @begin_year, @end_year, @grade, @max_grade,
                                @created_by, @created_date
                            ) RETURNING id;";

                        vm.People.created_by = "admin";
                        vm.People.created_date = DateTime.Now;
                        if (string.IsNullOrEmpty(vm.People.person_code))
                        {
                            vm.People.person_code = (vm.People.first_name ?? "EMP") + (vm.People.birth_date?.ToString("ddMMyy") ?? DateTime.Now.ToString("ddMMyy"));
                        }

                        int peopleId = conn.ExecuteScalar<int>(sqlInsertPeople, vm.People, trans);
                        vm.People.id = peopleId;

                        // 2. Insert bos_employee_pay_attributes
                        const string sqlInsertAttr = @"
                            INSERT INTO bos_employee_pay_attributes (
                                people_id, nik, status_employee, company_id, company_desc,
                                unit_id, unit_desc, position_id, position_desc, job_id, job_desc,
                                personal_area_id, personal_area_desc, payroll_area_id, payroll_area_desc,
                                payroll_group_id, payroll_group_desc, level_grade_id, level_grade_desc,
                                cost_center_id, cost_center_desc, bank_source_id, bank_source_desc,
                                employee_basic_wage, employee_currency, email, hire_date, end_contract,
                                created_by, created_date, flag_delete
                            ) VALUES (
                                @people_id, @nik, @status_employee, @company_id, @company_desc,
                                @unit_id, @unit_desc, @position_id, @position_desc, @job_id, @job_desc,
                                @personal_area_id, @personal_area_desc, @payroll_area_id, @payroll_area_desc,
                                @payroll_group_id, @payroll_group_desc, @level_grade_id, @level_grade_desc,
                                @cost_center_id, @cost_center_desc, @bank_source_id, @bank_source_desc,
                                @employee_basic_wage, @employee_currency, @email, @hire_date, @end_contract,
                                @created_by, @created_date, 0
                            ) RETURNING id;";

                        vm.EmployeeAttributes.people_id = peopleId;
                        vm.EmployeeAttributes.status_employee = string.IsNullOrEmpty(vm.EmployeeAttributes.status_employee) ? "AKTIF" : vm.EmployeeAttributes.status_employee;
                        vm.EmployeeAttributes.created_by = "admin";
                        vm.EmployeeAttributes.created_date = DateTime.Now;

                        int empAttrId = conn.ExecuteScalar<int>(sqlInsertAttr, vm.EmployeeAttributes, trans);
                        vm.EmployeeAttributes.id = empAttrId;

                        // 3. Insert bos_employee_times
                        if (vm.EmpTime != null)
                        {
                            const string sqlInsertTime = @"
                                INSERT INTO bos_employee_times (
                                    employee_id, workgroup_id, starting_date, start_seq, 
                                    begin_date, finger_id, created_by, created_date, delete_flag
                                ) VALUES (
                                    @employee_id, @workgroup_id, @starting_date, @start_seq, 
                                    @begin_date, @finger_id, @created_by, @created_date, 0
                                );";
                            vm.EmpTime.employee_id = empAttrId;
                            vm.EmpTime.created_by = "admin";
                            vm.EmpTime.created_date = DateTime.Now;
                            conn.Execute(sqlInsertTime, vm.EmpTime, trans);
                        }

                        // 4. Insert bos_employee_taxes
                        if (vm.EmpTax != null)
                        {
                            const string sqlInsertTax = @"
                                INSERT INTO bos_employee_taxes (
                                    employee_id, employee_tax_id, tax_status_id, tax_type, 
                                    tax_service_office_id, begin_date, created_by, created_date
                                ) VALUES (
                                    @employee_id, @employee_tax_id, @tax_status_id, @tax_type, 
                                    @tax_service_office_id, @begin_date, @created_by, @created_date
                                );";
                            vm.EmpTax.employee_id = empAttrId;
                            vm.EmpTax.employee_tax_id = vm.EmployeeAttributes.nik;
                            vm.EmpTax.created_by = "admin";
                            vm.EmpTax.created_date = DateTime.Now;
                            conn.Execute(sqlInsertTax, vm.EmpTax, trans);
                        }

                        trans.Commit();
                        return peopleId;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdateEmployee(EmployeeAttributeViewModel vm)
        {
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Update bos_people
                        const string sqlUpdatePeople = @"
                            UPDATE bos_people SET
                                first_name = @first_name,
                                middle_name = @middle_name,
                                last_name = @last_name,
                                birth_place = @birth_place,
                                gender = @gender,
                                birth_date = @birth_date,
                                religion = @religion,
                                marital_status_id = @marital_status_id,
                                photo_path = COALESCE(@photo_path, photo_path),
                                photo_name = COALESCE(@photo_name, photo_name),
                                majority = @majority,
                                education = @education,
                                institution = @institution,
                                modified_by = @modified_by,
                                modified_date = @modified_date
                            WHERE id = @id;";

                        vm.People.modified_by = "admin";
                        vm.People.modified_date = DateTime.Now;
                        conn.Execute(sqlUpdatePeople, vm.People, trans);

                        // 2. Update bos_employee_pay_attributes
                        const string sqlUpdateAttr = @"
                            UPDATE bos_employee_pay_attributes SET
                                nik = @nik,
                                status_employee = @status_employee,
                                unit_id = @unit_id,
                                unit_desc = @unit_desc,
                                position_id = @position_id,
                                position_desc = @position_desc,
                                job_id = @job_id,
                                job_desc = @job_desc,
                                payroll_group_id = @payroll_group_id,
                                payroll_group_desc = @payroll_group_desc,
                                level_grade_id = @level_grade_id,
                                level_grade_desc = @level_grade_desc,
                                cost_center_id = @cost_center_id,
                                cost_center_desc = @cost_center_desc,
                                bank_source_id = @bank_source_id,
                                bank_source_desc = @bank_source_desc,
                                employee_basic_wage = @employee_basic_wage,
                                employee_currency = @employee_currency,
                                email = @email,
                                hire_date = @hire_date,
                                end_contract = @end_contract,
                                terminate_date = @terminate_date,
                                modified_by = @modified_by,
                                modified_date = @modified_date
                            WHERE people_id = @people_id AND (flag_delete = 0 OR flag_delete IS NULL);";

                        vm.EmployeeAttributes.people_id = vm.People.id;
                        vm.EmployeeAttributes.modified_by = "admin";
                        vm.EmployeeAttributes.modified_date = DateTime.Now;
                        conn.Execute(sqlUpdateAttr, vm.EmployeeAttributes, trans);

                        // 3. Update or Insert bos_employee_times
                        if (vm.EmpTime != null && vm.EmployeeAttributes.id > 0)
                        {
                            const string sqlUpdateTimes = @"
                                UPDATE bos_employee_times SET
                                    workgroup_id = @workgroup_id,
                                    starting_date = @starting_date,
                                    finger_id = @finger_id
                                WHERE employee_id = @employee_id;";
                            vm.EmpTime.employee_id = vm.EmployeeAttributes.id;
                            int affected = conn.Execute(sqlUpdateTimes, vm.EmpTime, trans);
                            if (affected == 0)
                            {
                                const string sqlInsertTime = @"
                                    INSERT INTO bos_employee_times (
                                        employee_id, workgroup_id, starting_date, start_seq, 
                                        begin_date, finger_id, created_by, created_date, delete_flag
                                    ) VALUES (
                                        @employee_id, @workgroup_id, @starting_date, 1, 
                                        @starting_date, @finger_id, 'admin', NOW(), 0
                                    );";
                                conn.Execute(sqlInsertTime, vm.EmpTime, trans);
                            }
                        }

                        // 4. Update or Insert bos_employee_taxes
                        if (vm.EmpTax != null && vm.EmployeeAttributes.id > 0)
                        {
                            const string sqlUpdateTaxes = @"
                                UPDATE bos_employee_taxes SET
                                    tax_status_id = @tax_status_id,
                                    tax_type = @tax_type,
                                    tax_service_office_id = @tax_service_office_id
                                WHERE employee_id = @employee_id;";
                            vm.EmpTax.employee_id = vm.EmployeeAttributes.id;
                            int affectedTax = conn.Execute(sqlUpdateTaxes, vm.EmpTax, trans);
                            if (affectedTax == 0)
                            {
                                const string sqlInsertTax = @"
                                    INSERT INTO bos_employee_taxes (
                                        employee_id, employee_tax_id, tax_status_id, tax_type, 
                                        tax_service_office_id, begin_date, created_by, created_date
                                    ) VALUES (
                                        @employee_id, @employee_tax_id, @tax_status_id, @tax_type, 
                                        @tax_service_office_id, NOW(), 'admin', NOW()
                                    );";
                                vm.EmpTax.employee_tax_id = vm.EmployeeAttributes.nik;
                                conn.Execute(sqlInsertTax, vm.EmpTax, trans);
                            }
                        }

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public void TerminateEmployee(int peopleId, DateTime terminateDate)
        {
            using (IDbConnection conn = Connection)
            {
                const string sql = @"
                    UPDATE bos_employee_pay_attributes SET
                        status_employee = 'TERMINATE',
                        terminate_date = @terminateDate,
                        modified_by = 'admin',
                        modified_date = NOW()
                    WHERE people_id = @peopleId AND (flag_delete = 0 OR flag_delete IS NULL);";
                conn.Execute(sql, new { peopleId, terminateDate });
            }
        }

        public void DeleteEmployee(int peopleId)
        {
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Soft delete on pay attributes & times
                        const string sqlAttr = @"UPDATE bos_employee_pay_attributes SET flag_delete = 1, modified_date = NOW() WHERE people_id = @peopleId;";
                        conn.Execute(sqlAttr, new { peopleId }, trans);

                        // Soft delete people if del_flag exists, otherwise delete
                        const string sqlPeople = @"DELETE FROM bos_people WHERE id = @peopleId;";
                        conn.Execute(sqlPeople, new { peopleId }, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public IList<PeopleModel> GetAllPeople()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT p.id, p.person_code, p.first_name, p.middle_name, p.last_name, 
                               p.birth_place, p.gender, p.birth_date, p.religion, 
                               p.marital_status_id, p.photo_path, p.photo_name,
                               COALESCE(epa.nik, p.person_code) AS nik,
                               COALESCE(epa.status_employee, 'AKTIF') AS status_employee,
                               epa.hire_date AS hire_date,
                               epa.terminate_date AS terminate_date
                        FROM bos_people p
                        LEFT JOIN bos_employee_pay_attributes epa ON p.id = epa.people_id AND (epa.flag_delete = 0 OR epa.flag_delete IS NULL)
                        ORDER BY p.id ASC;";
                    return conn.Query<PeopleModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
