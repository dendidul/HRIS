# HRIS Explicit Columns & Comprehensive NULL Handling Fix Report

## 1. Executive Summary

This report documents the project-wide elimination of all wildcard queries (`SELECT *` and `SELECT [alias].*`) and the implementation of comprehensive NULL handling throughout **HRIS** (`D:\SourceCode\hris`), benchmarked against the legacy **Booster HR** application (`D:\SourceCode\Booster HR`) and PostgreSQL database schema reference (`db25april2016sore.sql`).

Every query across all modules was converted to use explicit, verified column projections matching the target DTOs/ViewModels. Concurrently, defensive NULL handling and empty-state safeguards were implemented across Data Access Objects (DAOs), managers, services, and Razor views.

---

## 2. Metrics & Scope

- **Total `SELECT *` / Wildcard queries found:** **73 occurrences** across 40 DAO and service manager files.
- **Total wildcard queries replaced with explicit columns:** **73 (100%)**.
- **Remaining applicable wildcard queries:** **0**.
- **Razor views audited and safeguarded with Model/Collection NULL checks:** **20 Index and Config views**.
- **Build Status:** All 9 projects in `HRIS.sln` built with **0 Errors**.

---

## 3. Detailed Changes per Component / Module

### 3.1 HRIS.Master.Model (Master Data DAOs)
1. **`AreaDao.cs`**:
   - `GetAllArea()` & `GetArea()`: Replaced `SELECT *` with explicit columns `id, area_code, short_desc, long_desc, begin_date, end_date, del_flag, created_by, created_date, modified_by, modified_date, hierarchy_id, tax_service_office_id`.
2. **`BankSourceDao.cs`**:
   - `GetAllBankSource()` & `GetBankSource()`: Replaced `SELECT *` with `id, bank_source_id, rec_number, bank_name, del_flag, created_by, created_date, modified_by, modified_date`.
3. **`CostCenterDao.cs`**:
   - `GetAllCostCenter()` & `GetCostCenter()`: Replaced `SELECT *` with `id, cost_center_code, description, begin_date, end_date, del_flag`.
4. **`EmployeeGroupDao.cs`**:
   - `GetAllEmployeeGroup()` & `GetEmployeeGroup()`: Replaced `SELECT *` with `id, group_code AS "Group_code", short_desc, long_desc, del_flag`.
5. **`JobDao.cs`**:
   - `GetAllJob()` & `GetJob()`: Replaced `SELECT *` with `id, job_id, short_desc, long_desc, position_id, begin_date, end_date, del_flag, created_by, created_date, modified_by, modified_date`.
6. **`LevelGradeDao.cs`**:
   - `GetAllLevelGrade()` & `GetLevelGrade()`: Replaced `SELECT *` with `id, level_id, short_desc, long_desc, begin_date, end_date, del_flag, created_by, created_date, modified_by, modified_date`.
7. **`ObjectMasterDao.cs`**:
   - `GetAllObjectMaster()` & `GetObjectMaster()`: Replaced `SELECT *` with `id, object_type, format_number, last_number, controller_name, ess_controller_name, view_admin, view_ess, del_flag`.
8. **`PositionDao.cs`**:
   - `GetAllPosition()` & `GetPosition()`: Replaced `SELECT *` with `id, position_id, short_description, long_description, begin_date, end_date, del_flag, created_by, created_date, modified_by, modified_date`.
9. **`UnitDao.cs`**:
   - `GetAllUnit()` & `GetUnit()`: Replaced `SELECT *` with `id, unit_id, short_desc, long_desc, position_id, begin_date, end_date, del_flag, cost_center_id, head_unit_id, created_by, created_date, modified_by, modified_date`.

### 3.2 HRIS.UserManagement.Model (User Management DAOs)
1. **`MenuDao.cs`**:
   - `GetAllMenu()` & `GetMenu()`: Replaced `SELECT *` with `id, menu_name, link, icon, parent_id, index, del_flag, controller_name`.
2. **`ManageRolesMenuDao.cs`**:
   - `GetAllRoleAccess()`: Replaced `SELECT *` with `id, role_id, menu_id, del_flag`.
3. **`UserDao.cs`**:
   - `CheckValidUser()`, `GetAllUsers()`, `GetUsers()`: Replaced `SELECT u.*` with explicit columns `u.id, u.username, u.password, u.role_id, u.status_employee_id, u.activate, u.flag_delete AS del_flag, u.created, u.modified, u.created_by, u.modified_by, u.created_date AS "createdDate", u.modified_date AS "modifiedDate", p.first_name AS employeename, p.person_code AS employeeid`.

### 3.3 HRIS.TimeManagementModel (Time Management DAOs)
1. **`CompanyCalendarDao.cs`**:
   - `GetAllCompanyCalendar()` & `GetCompanyCalendar()`: Replaced `SELECT c.*` and `SELECT *` with explicit columns: `c.id, c.personal_area_id, c.workgroup_id, c.starting_time, c.delete_flag AS del_flag, c.unattendance_id, c.created_by, c.modified_by, c.created_date, c.modified_date, c.remark, c.begin_date, c.end_date, a.short_desc AS area_desc, w.short_desc AS workgroup_desc, u.short_desc AS unattendance_desc`.
2. **`OvertimeReqDao.cs`**:
   - `GetAllOvertimeRequest()` & `GetOvertimeRequest()`: Replaced `SELECT o.*` with explicit column list `o.id, o.description, o.unit_id, o.overtime_date, o.doc_number, o.running_status, o.created_id, o.rejected, o.app_level, o.revision, o.object_type, o.note, o.approved, o.request_status, o.created AS created_date, o.modified AS modified_date, COALESCE(epa.nik, p.person_code) AS NIK, (p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS EmployeeName, u.short_desc AS UnitCode`.
3. **`ScheduleVariantDao.cs`**:
   - `GetAllScheduleVariant()` & `GetScheduleVariant()`: Replaced wildcard with `id, schedule_variant_id, short_desc, long_desc, delete_flag AS del_flag, created_by, created_date, modified_by, modified_date`.
4. **`TimeFrameDao.cs`**:
   - `GetAllTimeFrame()` & `GetTimeFrame()`: Replaced wildcard with `id, time_frame_id, short_desc, long_desc, starting_time, tolerance_time, duration_time, time_frame_type, delete_flag AS del_flag, created_by, created_date, modified_by, modified_date`.
5. **`TimesheetReqDao.cs`**:
   - `GetAllTimesheetReq()` & `GetTimesheetReq()`: Replaced `SELECT t.*` with explicit column projection including `COALESCE(epa.nik, p.person_code) AS NIK`, employee full name concatenation, and audit timestamps.
6. **`UnattendanceConfigDao.cs`**:
   - `GetAllUnattendanceConfig()` & `GetUnattendanceConfig()`: Replaced wildcard with explicit columns `id, unattendance_id, short_desc, long_desc, un_attendance_unit, un_attendance_quota, amount_of_increment, request_flag, default_timesheet_flag, un_attendance_period, increment_period, delete_flag AS del_flag, created_by, created_date, modified_by, modified_date`.
7. **`UnattendanceReqDao.cs`**:
   - `GetAllUnattendanceReq()` & `GetUnattendanceReq()`: Replaced `SELECT u.*` with explicit columns `u.id, u.employee_id, u.unattendance_id, u.start_date, u.last_date, u.no_unit, u.doc_number, u.object_type, u.revision, u.app_level, u.approved, u.rejected, u.note, u.request_status, u.note_admin, u.created_by, u.created_date, u.modified_by, u.modified_date, u.flag_delete AS del_flag, COALESCE(epa.nik, p.person_code) AS NIK, (p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS EmployeeName, att.unattendance_id AS UnattendanceCode`.
8. **`WorkgroupDao.cs`**:
   - `GetAllWorkgroup()` & `GetWorkGroup()`: Replaced wildcard with `id, workgroup_id, short_desc, long_desc, schedule_variant_id, schedule_seq, begin_date, end_date, delete_flag AS del_flag, created_by, created_date, modified_by, modified_date`.

### 3.4 HRIS.PersonalAdmin.Model (Personal Admin DAOs)
1. **`PeopleDao.cs`**:
   - `GetAllPeople()`: Replaced `SELECT p.*` with explicit columns `p.id, p.person_code, p.first_name, p.middle_name, p.last_name, p.birth_place, p.gender, p.birth_date, p.religion, p.marital_status_id, p.photo_path, p.photo_name, COALESCE(epa.nik, p.person_code) AS nik, COALESCE(epa.status_employee, 'AKTIF') AS status_employee, epa.hire_date AS hire_date, epa.terminate_date AS terminate_date`.
2. **`EmployeeLeaveQuotaDao.cs`**:
   - `GetAllEmployeeQuota()` & `GetEmployeeQuota()`: Replaced `SELECT q.*` with explicit columns `q.id, q.status_employee_id, q.unattendance_id, q.quota, q.actfl, q.begdt AS begin_date, q.enddt AS end_date, q.flag_delete AS del_flag, q.modby AS modified_by, q.moddt AS modified_date, COALESCE(epa.nik, p.person_code) AS "EmployeeId", (p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS "EmployeeName", u.short_desc AS "UnattendanceCode"`.

### 3.5 HRIS.Payroll.Model & Services (Payroll DAOs & Managers)
1. **`OneTimeTransactionDao.cs`**:
   - `GetAllOneTimeTransaction()` & `GetOneTimeTransaction()`: Replaced `SELECT *` with explicit columns `id, employee_id, number_period, fiscal_year, payroll_component_id, comp_trans_type, comp_amount, begin_date, end_date, payroll_process_id, currency_id, del_flag, created_by, created_date, modifed_by, modifed_date`.
2. **`PayrollAreaDao.cs`**:
   - `GetAllPayrollArea()` & `GetPayrollArea()`: Replaced `SELECT *` with `id, payroll_area_id, short_desc, long_desc, begin_date, end_date, del_flag, created_by, modified_by, created_date, modified_date`.
3. **`PayrollCalculationDao.cs`**:
   - `GetAllPayrollCalculation()` & `GetPayrollCalculation()`: Replaced `SELECT *` with `id, seq_number, year_period, payroll_periode_id, payroll_area_id, payroll_process_id, begin_date, end_date, running_status, running_date, running_by, total_employee, processed_employee, is_logged, pdf_log_generated, start_time, end_time, total_time`.
   - `GetCalculationResults()`: Replaced `SELECT *` with `id, payroll_calculation_id, employee_id, payroll_component_id, transaction_type, payroll_component_amount, payroll_component, flag`.
4. **`PayrollCalculationDetailDao.cs`**:
   - `ReadAll()`: Replaced `SELECT a.*` with explicit projection for `bos_payroll_calculations` and descriptions.
5. **`PayrollComponentDao.cs`**:
   - `GetAllPayrollComponent()` & `GetPayrollComponent()`: Replaced `SELECT *` with `id, payroll_component_id, short_desc, long_desc, amount_type, amount, amount_minimum, amount_maximum, object_id, proportional_type, begin_date, end_date, transaction_flag, proportional_id, delete_flag, marital_status, currency_id, created_by, created_date, modified_by, modified_date`.
6. **`PayrollGroupDao.cs`**:
   - `GetAllPayrollGroup()` & `GetPayrollGroup()`: Replaced `SELECT *` with `id, pgrid, sdesc, payroll_periode_id, begdt, enddt, delfl, created_by, created_date, modified_by, modified_date`.
7. **`PayrollPeriodDao.cs`**:
   - `GetAllPayrollPeriod()` & `GetPayrollPeriod()`: Replaced `SELECT *` with `id, payroll_periode_id, short_desc, long_desc, max_period, begin_date, end_date, del_flag, created_by, created_date, modified_by, modified_date`.
8. **`PayrollProcessDao.cs`**:
   - `GetAllPayrollProcess()` & `GetPayrollProcess()`: Replaced `SELECT *` with `id, payroll_process_id, short_desc, payroll_periode_id, calculation_function, begin_date, end_date, long_desc, working_period, del_flag, on_cycle, employee_status`.
9. **`PayrollRecurringComponentDao.cs`**:
   - `GetAllRecurringComponent()` & `GetRecurringComponent()`: Replaced `SELECT *` with `id, employee_id, payroll_component_id, comp_amount, begin_date, end_date, delete_flag, ref, currency_id, created_by, created_date, updated_by, updated_date`.
10. **`ProportionalDao.cs`**:
    - `GetAllProportional()` & `GetProportional()`: Replaced `SELECT *` with `id, proportional_id, short_desc, long_desc, numerator_type, numerator_amount, denominator_type, denominator_amount, delete_flag, created_by, created_date, modified_by, modified_date`.
11. **`PayrollManager.cs`**:
    - Refactored recurring component subquery in calculation engine to explicit projection: `SELECT id, employee_id, payroll_component_id, comp_amount, begin_date, end_date, delete_flag, ref, currency_id`.
    - Refactored one-time transaction subquery in calculation engine to explicit projection: `SELECT id, employee_id, number_period, fiscal_year, payroll_component_id, comp_trans_type, comp_amount, begin_date, end_date, payroll_process_id, currency_id, del_flag`.

### 3.6 HRIS.Tax.Model (Tax Suite DAOs)
1. **`TaxCalculationDao.cs`**:
   - `GetAllTaxCalculation()` & `GetTaxCalculation()`: Replaced `SELECT *` with `id, status_employee_id, sequence, year_period, regular_net, regular_gross, irregular_net, irregular_gross, ptkp, tax_paid, payroll_process_id, position_cost, kpp, tax_allowance, payment_regular_net, deduction_regular_net, tax_regular_net, tax_service_office_id`.
2. **`TaxLineComponentDao.cs`**:
   - `GetAllTaxLineComponent()` & `GetTaxLineComponent()`: Replaced `SELECT *` with `id, tax_line, payroll_component_id, flag_delete, created_by, created_date, modified_by, modified_date`.
3. **`TaxOfficerDao.cs`**:
   - `GetAllTaxOfficer()` & `GetTaxOfficer()`: Replaced `SELECT *` with `id, address_id, tax_office_code, short_desc, long_desc, flag_delete`.
4. **`TaxParameterDao.cs`**:
   - `GetAllTaxParameter()` & `GetTaxParameter()`: Replaced `SELECT *` with `id, tax_parameter_id, short_desc, long_desc, begin_date, end_date, max_position_tax, position_tax_percentage, tax_component_id, tax_allowance_id, flag_delete, created_by, created_date, modified_by, modified_date`.

---

## 4. Comprehensive NULL Handling Safeguards

1. **Razor View Loop Protection:**
   - 20 Index and management views were updated to wrap collection rendering with `@if (Model != null && Model.Any())`.
   - In `Views/Shared/Menu.cshtml`, `ViewBag.Menu` nullability was guarded with fallback initialization (`?? new List<RoleMenuViewModel>()`), and LINQ filters were updated with `p != null` to prevent `NullReferenceException` during session transitions.
   - In `Views/AccessModule/Config.cshtml`, null navigation over `Model?.ListMenu` and `Model?.ListSelectedMenu` was implemented with pre-indexed `HashSet<int>` lookups.
2. **Nullable Value Formattings:**
   - Date properties safely formatted using `.HasValue` checks or null-conditional `item.created_date?.ToString("dd-MM-yyyy") ?? "-"`.
3. **Database Null Coalescing:**
   - `COALESCE` safely applied for string concatenation (`p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')`), guaranteeing non-null display strings in views without overwriting underlying valid empty fields.

---

## 5. Verification & Verification Output

- **Solution Build Command:** `dotnet build D:/SourceCode/hris/HRIS.sln`
- **Result:** **Build succeeded: 0 Warning(s) / 0 Error(s)** for models, services, and web projects.
- **Source Verification:** Automated re-scan confirms **0** instances of `SELECT *` or `SELECT alias.*` remain anywhere in `D:\SourceCode\hris`.
