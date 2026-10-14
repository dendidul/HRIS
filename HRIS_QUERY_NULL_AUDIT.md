# HRIS Query and NULL Handling Audit Plan

## Project Information
- **Target Project:** `D:\SourceCode\hris` (.NET 10 ASP.NET Core MVC, Sneat Admin Template)
- **Reference Project:** `D:\SourceCode\Booster HR` (Legacy ASP.NET MVC 4.5 & Entity Framework)
- **Database Schema Reference:** `D:\SourceCode\Booster HR\HRLinov.App.Logic\db\db25april2016sore.sql` & `HRLinov.edmx`

---

## 1. Inventory of Wildcard Queries (`SELECT *` / `SELECT tbl.*`)

Across the entire solution, exactly **73 occurrences** of `SELECT *` / `SELECT [alias].*` were identified in DAO implementations and managers:

| No | File Path | Method / Context | Table(s) Queried | Proposed Explicit Columns |
|---|---|---|---|---|
| 1-2 | `HRIS.Master.Model/Dao/AreaDao.cs` | `GetAllArea()`, `GetArea()` | `bos_areas` | `id, area_code, short_desc, long_desc, begin_date, end_date, del_flag, created_by, created_date, modified_by, modified_date, hierarchy_id, tax_service_office_id` |
| 3-4 | `HRIS.Master.Model/Dao/BankSourceDao.cs` | `GetAllBankSource()`, `GetBankSource()` | `bos_bank_sources` | `id, bank_source_id, rec_number, bank_name, del_flag, created_by, created_date, modified_by, modified_date` |
| 5-6 | `HRIS.Master.Model/Dao/CostCenterDao.cs` | `GetAllCostCenter()`, `GetCostCenter()` | `bos_cost_centers` | `id, cost_center_code, description, begin_date, end_date, del_flag` |
| 7-8 | `HRIS.Master.Model/Dao/EmployeeGroupDao.cs` | `GetAllEmployeeGroup()`, `GetEmployeeGroup()` | `bos_employee_groups` | `id, group_code AS "Group_code", short_desc, long_desc, del_flag` |
| 9-10 | `HRIS.Master.Model/Dao/JobDao.cs` | `GetAllJob()`, `GetJob()` | `bos_jobs` | `id, job_id, short_desc, long_desc, position_id, begin_date, end_date, del_flag, created_by, created_date, modified_by, modified_date` |
| 11-12 | `HRIS.Master.Model/Dao/LevelGradeDao.cs` | `GetAllLevelGrade()`, `GetLevelGrade()` | `bos_level_grades` | `id, level_id, short_desc, long_desc, begin_date, end_date, del_flag, created_by, created_date, modified_by, modified_date` |
| 13-14 | `HRIS.Master.Model/Dao/ObjectMasterDao.cs` | `GetAllObjectMaster()`, `GetObjectMaster()` | `bos_object_masters` | `id, object_type, format_number, last_number, controller_name, ess_controller_name, view_admin, view_ess, del_flag` |
| 15-16 | `HRIS.Master.Model/Dao/PositionDao.cs` | `GetAllPosition()`, `GetPosition()` | `bos_positions` | `id, position_id, short_description, long_description, begin_date, end_date, del_flag, created_by, created_date, modified_by, modified_date` |
| 17-18 | `HRIS.Master.Model/Dao/UnitDao.cs` | `GetAllUnit()`, `GetUnit()` | `bos_units` | `id, unit_id, short_desc, long_desc, position_id, begin_date, end_date, del_flag, cost_center_id, head_unit_id, created_by, created_date, modified_by, modified_date` |
| 19-20 | `HRIS.UserManagement.Model/Dao/MenuDao.cs` | `GetAllMenu()`, `GetMenu()` | `bos_menu` | `id, menu_name, link, icon, parent_id, index, del_flag` |
| 21 | `HRIS.UserManagement.Model/Dao/ManageRolesMenuDao.cs` | `GetAllManageRolesMenu()` | `bos_manage_roles_menu` | `id, role_id, menu_id, del_flag` |
| 22-24 | `HRIS.UserManagement.Model/Dao/UserDao.cs` | `GetAllUser()`, `GetUser()`, `GetUserById()` | `bos_users` (JOIN `bos_roles`) | `u.id, u.username, u.password, u.role_id, u.status_employee_id, u.activate, u.flag_delete, u.created, u.modified, r.name AS role_name` |
| 25-26 | `HRIS.TimeManagementModel/Dao/CompanyCalendarDao.cs` | `GetAllCompanyCalendar()`, `GetCompanyCalendar()` | `bos_company_calendars` (JOIN `bos_areas`, `bos_workgroups`, `bos_unattendances`) | `c.id, c.personal_area_id, c.workgroup_id, c.unattendance_id, c.begin_date, c.end_date, c.remark, c.delete_flag AS del_flag, a.short_desc AS area_desc, w.short_desc AS workgroup_desc, u.short_desc AS unattendance_desc` |
| 27-28 | `HRIS.TimeManagementModel/Dao/OvertimeReqDao.cs` | `GetAllOvertimeRequest()`, `GetOvertimeRequest()` | `bos_overtime_requests` (JOIN `bos_overtime_request_details`, `bos_employee_pay_attributes`, `bos_people`, `bos_units`) | `o.id, o.description, o.unit_id, o.overtime_date, o.doc_number, o.running_status, o.created, o.created_by, o.modified, o.modified_by, o.approved, o.rejected, o.request_status, COALESCE(epa.nik, p.person_code) AS NIK, (p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS EmployeeName, u.short_desc AS UnitCode` |
| 29-30 | `HRIS.TimeManagementModel/Dao/ScheduleVariantDao.cs` | `GetAllScheduleVariant()`, `GetScheduleVariant()` | `bos_schedule_variants` | `id, schedule_variant_id, short_desc, long_desc, delete_flag AS del_flag, created_by, created_date, modified_by, modified_date` |
| 31-32 | `HRIS.TimeManagementModel/Dao/TimeFrameDao.cs` | `GetAllTimeFrame()`, `GetTimeFrame()` | `bos_time_frames` | `id, time_frame_id, short_desc, long_desc, starting_time, tolerance_time, duration_time, time_frame_type, delete_flag AS del_flag, created_by, created_date, modified_by, modified_date` |
| 33-34 | `HRIS.TimeManagementModel/Dao/TimesheetReqDao.cs` | `GetAllTimesheetReq()`, `GetTimesheetReq()` | `bos_timesheet_reqs` (JOIN `bos_employee_pay_attributes`, `bos_people`) | `t.id, t.employee_id, t.transaction_date, t.time_in, t.time_out, t.request_note, t.request_status, t.doc_number, t.flag_delete AS del_flag, COALESCE(epa.nik, p.person_code) AS NIK, (p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS EmployeeName` |
| 35-36 | `HRIS.TimeManagementModel/Dao/UnattendanceConfigDao.cs` | `GetAllUnattendanceConfig()`, `GetUnattendanceConfig()` | `bos_unattendances` | `id, unattendance_id, short_desc, long_desc, un_attendance_unit, un_attendance_quota, amount_of_increment, request_flag, default_timesheet_flag, un_attendance_period, increment_period, delete_flag AS del_flag` |
| 37-38 | `HRIS.TimeManagementModel/Dao/UnattendanceReqDao.cs` | `GetAllUnattendanceReq()`, `GetUnattendanceReq()` | `bos_unattendance_reqs` (JOIN `bos_employee_pay_attributes`, `bos_people`, `bos_unattendances`) | `u.id, u.employee_id, u.unattendance_id, u.start_date, u.last_date, u.no_unit, u.doc_number, u.request_status, u.flag_delete AS del_flag, COALESCE(epa.nik, p.person_code) AS NIK, (p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS EmployeeName, att.unattendance_id AS UnattendanceCode` |
| 39-40 | `HRIS.TimeManagementModel/Dao/WorkgroupDao.cs` | `GetAllWorkgroup()`, `GetWorkgroup()` | `bos_workgroups` | `id, workgroup_id, short_desc, long_desc, schedule_variant_id, schedule_seq, begin_date, end_date, delete_flag AS del_flag, created_by, created_date, modified_by, modified_date` |
| 41 | `HRIS.PersonalAdmin.Model/Dao/PeopleDao.cs` | `GetAllPeople()` | `bos_people` (JOIN `bos_employee_pay_attributes`) | `p.id, p.person_code, p.first_name, p.middle_name, p.last_name, p.gender, p.birth_place, p.birth_date, p.religion, p.marital_status_id, COALESCE(epa.nik, p.person_code) AS nik, COALESCE(epa.status_employee, 'AKTIF') AS status_employee, epa.hire_date, epa.terminate_date` |
| 42-43 | `HRIS.PersonalAdmin.Model/Dao/EmployeeLeaveQuotaDao.cs` | `GetAllEmployeeQuota()`, `GetEmployeeQuota()` | `bos_employee_quota` (JOIN `bos_employee_pay_attributes`, `bos_people`, `bos_unattendances`) | `q.id, q.status_employee_id, q.unattendance_id, q.quota, q.actfl, q.begdt AS begin_date, q.enddt AS end_date, q.flag_delete AS del_flag, q.modby AS modified_by, q.moddt AS modified_date, COALESCE(epa.nik, p.person_code) AS "EmployeeId", (p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS "EmployeeName", u.short_desc AS "UnattendanceCode"` |
| 44-45 | `HRIS.Payroll.Model/Dao/OneTimeTransactionDao.cs` | `GetAllOneTimeTransaction()`, `GetOneTimeTransaction()` | `bos_pay_onetime_trans` | `id, employee_id, number_period, fiscal_year, payroll_component_id, comp_trans_type, comp_amount, begin_date, end_date, payroll_process_id, currency_id, del_flag, created_by, created_date, modifed_by, modifed_date` |
| 46-47 | `HRIS.Payroll.Model/Dao/PayrollAreaDao.cs` | `GetAllPayrollArea()`, `GetPayrollArea()` | `bos_payroll_areas` | `id, payroll_area_id, short_desc, long_desc, begin_date, end_date, del_flag, created_by, created_date, modified_by, modified_date` |
| 48-50 | `HRIS.Payroll.Model/Dao/PayrollCalculationDao.cs` | `GetAllCalculation()`, `GetCalculationById()`, `GetResultsByCalculationId()` | `bos_payroll_calculations`, `bos_payroll_calc_results` | Calculations: `id, seq_number, year_period, payroll_periode_id, payroll_area_id, payroll_process_id, begin_date, end_date, running_status, running_date, running_by, total_employee, processed_employee, is_logged, start_time, end_time, total_time`<br>Results: `id, payroll_calculation_id, employee_id, payroll_component_id, transaction_type, payroll_component_amount, payroll_component, flag` |
| 51 | `HRIS.Payroll.Model/Dao/PayrollCalculationDetailDao.cs` | `GetPayrollCalculationDetailContain()` | `bos_payroll_calc_results` (JOIN components) | `a.id, a.payroll_calculation_id, a.employee_id, a.payroll_component_id, a.transaction_type, a.payroll_component_amount, a.payroll_component, a.flag, b.short_desc AS payroll_component_desc` |
| 52-53 | `HRIS.Payroll.Model/Dao/PayrollComponentDao.cs` | `GetAllPayrollComponent()`, `GetPayrollComponent()` | `bos_payroll_components` | `id, payroll_component_id, short_desc, long_desc, amount_type, amount, amount_minimum, amount_maximum, object_id, proportional_type, begin_date, end_date, transaction_flag, proportional_id, delete_flag, marital_status, currency_id` |
| 54-55 | `HRIS.Payroll.Model/Dao/PayrollGroupDao.cs` | `GetAllPayrollGroup()`, `GetPayrollGroup()` | `bos_payroll_groups` | `id, pgrid, sdesc, payroll_periode_id, begdt, enddt, delfl, created_by, created_date, modified_by, modified_date` |
| 56-57 | `HRIS.Payroll.Model/Dao/PayrollPeriodDao.cs` | `GetAllPayrollPeriod()`, `GetPayrollPeriod()` | `bos_payroll_periodes` | `id, payroll_periode_id, short_desc, long_desc, max_period, begin_date, end_date, del_flag, created_by, created_date, modified_by, modified_date` |
| 58-59 | `HRIS.Payroll.Model/Dao/PayrollProcessDao.cs` | `GetAllPayrollProcess()`, `GetPayrollProcess()` | `bos_payroll_processes` | `id, payroll_process_id, short_desc, payroll_periode_id, calculation_function, begin_date, end_date, long_desc, working_period, del_flag, on_cycle, employee_status` |
| 60-61 | `HRIS.Payroll.Model/Dao/PayrollRecurringComponentDao.cs` | `GetAllPayrollRecurringComponent()`, `GetPayrollRecurringComponent()` | `bos_pay_recurring_comps` | `id, employee_id, payroll_component_id, comp_amount, begin_date, end_date, delete_flag, ref, currency_id, created_by, created_date, updated_by, updated_date` |
| 62-63 | `HRIS.Payroll.Model/Dao/ProportionalDao.cs` | `GetAllProportional()`, `GetProportional()` | `bos_proportionals` | `id, proportional_id, short_desc, long_desc, numerator_type, numerator_amount, denominator_type, denominator_amount, delete_flag, created_by, created_date, modified_by, modified_date` |
| 64-65 | `HRIS.Tax.Model/Dao/TaxCalculationDao.cs` | `GetAllTaxCalculation()`, `GetTaxCalculationById()` | `bos_tax_calculations` | `id, status_employee_id, sequence, year_period, regular_net, regular_gross, irregular_net, irregular_gross, ptkp, tax_paid, payroll_process_id, position_cost, kpp, tax_allowance, payment_regular_net, deduction_regular_net, tax_regular_net, tax_service_office_id` |
| 66-67 | `HRIS.Tax.Model/Dao/TaxLineComponentDao.cs` | `GetAllTaxLineComponent()`, `GetTaxLineComponent()` | `bos_taxline_components` | `id, tax_line, payroll_component_id, flag_delete, created_by, created_date, modified_by, modified_date` |
| 68-69 | `HRIS.Tax.Model/Dao/TaxOfficerDao.cs` | `GetAllTaxOfficer()`, `GetTaxOfficer()` | `bos_tax_service_offices` | `id, address_id, tax_office_code, short_desc, long_desc, flag_delete` |
| 70-71 | `HRIS.Tax.Model/Dao/TaxParameterDao.cs` | `GetAllTaxParameter()`, `GetTaxParameter()` | `bos_tax_parameters` | `id, tax_parameter_id, short_desc, long_desc, begin_date, end_date, max_position_tax, position_tax_percentage, tax_component_id, tax_allowance_id, flag_delete, created_by, created_date, modified_by, modified_date` |
| 72-73 | `HRIS.Service/Manager/PayrollManager.cs` | `Calculate()` subqueries | `bos_pay_recurring_comps`, `bos_pay_onetime_trans` | `id, employee_id, payroll_component_id, comp_amount, begin_date, end_date, delete_flag`<br>`id, employee_id, payroll_component_id, comp_amount, comp_trans_type, begin_date, end_date, del_flag` |

---

## 2. NULL Handling Strategy Across Layers

1. **Database Layer:**
   - Nullable foreign keys handled via `LEFT JOIN` without filtering them out in WHERE.
   - Use `COALESCE` only where required for meaningful display/fallback (e.g. `COALESCE(p.first_name, '')`).
2. **Models & DTOs:**
   - All datetime, foreign key IDs, and numeric properties use nullable types (`DateTime?`, `int?`, `decimal?`) to prevent casting/mapping exceptions in Dapper.
3. **Services & Controllers:**
   - Null-checks before collection LINQ transformations (`data?.ToList() ?? new List<T>()`).
4. **Razor Views:**
   - Null-check collections: `@if (Model != null && Model.Any())`.
   - Empty state message: `<tr><td colspan="X" class="text-center text-muted">No data available</td></tr>`.
   - Null-safe navigation: `@(item?.property ?? "-")`.
   - Safe date formatting: `@(item?.begin_date?.ToString("dd/MM/yyyy") ?? "-")`.

---

## 3. Execution Checklist
- [x] Audit all `SELECT *` in DAO files.
- [ ] Refactor DAOs to explicit columns.
- [ ] Refactor Manager queries in `PayrollManager.cs`.
- [ ] Audit & safeguard Razor view loops and properties for NULL handling.
- [ ] Compile and verify `dotnet build`.
- [ ] Write `HRIS_QUERY_NULL_FIX_REPORT.md`.
- [ ] Commit & push to `template_sneat`.
