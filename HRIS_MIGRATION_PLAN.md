# HRIS Migration Plan & Feature Matrix

## 1. Executive Summary

- **Source Application (Legacy):** `D:\SourceCode\Booster HR` (ASP.NET MVC 5, Entity Framework 6 / Npgsql, PostgreSQL schema reference `db25april2016sore.sql`).
- **Target Application (New):** `D:\SourceCode\hris` (ASP.NET Core MVC on .NET 10, Sneat Bootstrap 5 Admin Theme, Dapper + ADO.NET / Stored Procedures / EF Core).
- **Primary Architect & Lead Developer:** Erdendi Lesmana (`@dendidul`).
- **Objective:** Complete migration and implementation of all functional HRIS features from the legacy system into the target application while respecting the target's clean multi-tier architecture, Sneat UI, and .NET 10 runtime compatibility.

---

## 2. Architecture & Tech Stack Comparison

| Architectural Aspect | Legacy Application (`Booster HR`) | Target Application (`HRIS`) |
|---|---|---|
| **Framework** | .NET Framework 4.5 / ASP.NET MVC 5 | ASP.NET Core MVC (.NET 10) |
| **Data Access** | Entity Framework 6 (`hris_linovEntities`), Npgsql, raw SQL | Dapper (`IDbConnection`), Npgsql, EF Core Identity with PostgreSQL |
| **Database Engine** | PostgreSQL (`booster_hr`) | PostgreSQL (`booster_hr`) |
| **UI Framework** | Bootstrap 3, GridMvc, jQuery EasyUI / ComboGrid | Sneat Responsive Bootstrap 5, Boxicons, DataTables |
| **Authentication & Auth** | FormsAuthentication, Custom `MenuController` RBAC | ASP.NET Core Identity & CookieManager, `GlobalAccessMenuController` |
| **Layering** | MVC (`HRLinov.App.Mvc`) -> Presenter/Logic (`HRLinov.App.Logic`) | `HRIS.Web` -> `HRIS.Service` -> `HRIS.*.Model` (DAOs) -> `HRIS.General` |

---

## 3. Legacy-to-Target Feature Migration Matrix

| Module / Feature | Legacy Implementation | Target Implementation | Database Dependencies | Status |
|---|---|---|---|---|
| **Auth: Login & Logout** | `LoginController`, `MembershipController` | `AccountController`, `HelperController` | `bos_users`, `bos_roles` | Implemented / Verified |
| **Auth: RBAC & Permissions** | `MenuController`, `GlobalController`, `NoAccessController` | `GlobalAccessMenuController`, `AccessModuleController` | `bos_menu`, `bos_manage_roles_menu`, `bos_roles` | Implemented / Verified |
| **User Mgmt: Users** | `UserController`, `UserLogic`, `UserPresenter` | `UsersController`, `UserManagementManager`, `UserDao` | `bos_users`, `sp_Usm_users*` | Implemented / Verified |
| **User Mgmt: Roles** | `RolesController`, `RolesLogic`, `RolesPresenter` | `RoleController`, `UserManagementManager`, `RoleDao` | `bos_roles`, `sp_Usm_roles*` | Implemented / Verified |
| **User Mgmt: Menu Permissions** | `MenuController`, `ManageRoleMenuLogic` | `AccessModuleController`, `ManageRolesMenuDao` | `bos_manage_roles_menu`, `sp_Usm_manage_roles_menu*` | Implemented / Verified |
| **Master: Area** | `AreaController`, `AreaLogic`, `AreaPresenter` | `Master/AreaController`, `MasterManager`, `AreaDao`, Sneat Views | `bos_areas`, `sp_Mst_areas*` | Implemented / Verified |
| **Master: Bank Source** | `BankSourceController`, `BankSourceLogic` | `Master/BankSourceController`, `MasterManager`, `BankSourceDao`, Sneat Views | `bos_bank_sources`, `sp_Mst_bank_sources*` | Implemented / Verified |
| **Master: Cost Center** | `CostCenterController`, `CostCenterLogic` | `Master/CostCenterController`, `MasterManager`, `CostCenterDao`, Sneat Views | `bos_cost_centers`, `sp_Mst_cost_centers*` | Implemented / Verified |
| **Master: Employee Group** | `EmployeeGroupController`, `EmployeeGroupLogic` | `Master/EmployeeGroupController`, `MasterManager`, `EmployeeGroupDao`, Sneat Views | `bos_employee_groups`, `sp_Mst_employee_groups*` | Implemented / Verified |
| **Master: Job Title** | `JobController`, `JobLogic`, `JobPresenter` | `Master/JobController`, `MasterManager`, `JobDao`, Sneat Views | `bos_jobs`, `sp_Mst_jobs*` | Implemented / Verified |
| **Master: Level & Grade** | `LevelGradeController`, `LevelGradeLogic` | `Master/LevelGradeController`, `MasterManager`, `LevelGradeDao`, Sneat Views | `bos_level_grades`, `sp_Mst_level_grades*` | Implemented / Verified |
| **Master: Object Master** | `ObjectMasterController`, `ObjectMasterLogic` | `Master/ObjectMasterController`, `MasterManager`, `ObjectMasterDao`, Sneat Views | `bos_object_masters`, `sp_Mst_object_masters*` | Implemented / Verified |
| **Master: Position** | `PositionController`, `PositionLogic` | `Master/PositionController`, `MasterManager`, `PositionDao`, Sneat Views | `bos_positions`, `sp_Mst_positions*` | Implemented / Verified |
| **Master: Unit / Department** | `UnitController`, `UnitLogic`, `HierarchyLogic` | `Master/UnitController`, `MasterManager`, `UnitDao`, Sneat Views | `bos_units`, `sp_Mst_units*` | Implemented / Verified |
| **Master: Company Profile** | `CompanyController`, `CompanyLogic` | `Master/CompanyController`, `MasterManager`, `CompanyDao`, Sneat Views | `bos_companies`, `sp_Mst_companies*` | Integrated into Master Data |
| **Master: Currency & Forex** | `CurrencyController`, `CurrencyLogic` | `Master/CurrencyController`, `MasterManager`, `CurrencyDao`, Sneat Views | `bos_currencies`, `bos_currency_conversions` | Integrated into Master Data |
| **Master: Lookups & ComboGrid** | `LookupController`, `CombogridController` | `Master/LookupController`, Lookups API helper | `bos_lookups`, `bos_lookup_details` | Integrated into Master Data |
| **Master: Memberships** | `MembershipController`, `MembershipLogic` | `Master/MembershipController`, `MasterManager`, `MembershipDao` | `bos_memberships`, `bos_membership_components` | Integrated into Master Data |
| **Personal Admin: Employee Master** | `EmployeeController`, `PeopleLogic`, `MaintainPersonController` | `PersonalAdmin/EmployeeController`, `PersonalAdminManager`, `PeopleDao`, Sneat Views | `bos_people`, `sp_Psa_people*` | Implemented / Verified |
| **Personal Admin: Employee Details** | `EmployeeDetailController`, `EmployeeAddressLogic`, `FamilyLogic` | `PersonalAdmin/EmployeeController` tabs (Family, Address, Bank, Tax, Job) | `bos_employee_address`, `bos_employee_family`, `bos_bank_accounts`, `bos_employee_taxes` | Implemented / Verified |
| **Personal Admin: Employee Quota** | `EmployeeLeaveQuotaController`, `EmployeeQuotesLogic` | `TimeManagement/EmployeeLeaveQuotaController`, `EmployeeLeaveQuotaDao`, Sneat Views | `bos_employee_quota`, `sp_Psa_employee_quota*` | Implemented / Verified |
| **Personal Admin: Summary & Movement** | `SummaryEmployeeController`, `SummaryMovementController` | `PersonalAdmin/SummaryEmployeeController`, Sneat Dashboard Widgets | `bos_people`, `bos_employee_pay_attributes_histories` | Implemented / Verified |
| **Time Mgmt: Company Calendar** | `CompanyCalendarController`, `CompanyCalendarLogic` | `TimeManagement/CompanyCalendarController`, `CompanyCalendarDao`, Sneat Views | `bos_company_calendars`, `sp_Tmn_company_calendars*` | Implemented / Verified |
| **Time Mgmt: Workgroup & Shift** | `WorkGroupController`, `WorkGroupLogic` | `TimeManagement/WorkGroupController`, `WorkgroupDao`, Sneat Views | `bos_workgroups`, `sp_Tmn_workgroups*` | Implemented / Verified |
| **Time Mgmt: Schedule Variant** | `ScheduleVariantController`, `ScheduleVariantsLogic` | `TimeManagement/ScheduleVariantController`, `ScheduleVariantDao`, Sneat Views | `bos_schedule_variants`, `sp_Tmn_schedule_variants*` | Implemented / Verified |
| **Time Mgmt: Time Frame** | `TimeFrameController`, `TimeFrameLogic` | `TimeManagement/TimeFrameController`, `TimeFrameDao`, Sneat Views | `bos_time_frames`, `sp_Tmn_time_frames*` | Implemented / Verified |
| **Time Mgmt: Time Calculation** | `TimeCalculationController`, `TimeCalculation.cs` | `TimeManagement/TimeCalculationController`, `TimeCalculationManager`, Sneat Views | `bos_time_calculations`, `bos_time_calculation_details` | Implemented / Verified |
| **Time Mgmt: Timesheet & Requests** | `TimesheetsController`, `TimesheetRequestController` | `TimeManagement/TimesheetController`, `TimesheetRequestController`, DAOs, Sneat Views | `bos_timesheets`, `bos_timesheet_reqs`, `sp_Tmn_timesheet_reqs*` | Implemented / Verified |
| **Time Mgmt: Overtime Requests & Timesheets** | `EmployeeOvertimeRequestController`, `EmployeeOvertimeController` | `TimeManagement/OvertimeRequestController`, `OvertimeTimesheetController`, DAOs, Sneat Views | `bos_overtime_requests`, `bos_employee_overtimes`, `sp_Tmn_overtime_requests*` | Implemented / Verified |
| **Time Mgmt: Unattendance & Config** | `UnattendanceRequestController`, `UnattendancesConfigController` | `TimeManagement/UnattendanceRequestController`, `AttendanceConfigController`, DAOs, Sneat Views | `bos_unattendances`, `bos_unattendance_reqs`, `sp_Tmn_unattendance*` | Implemented / Verified |
| **Payroll: Payroll Area** | `PayrollAreaController`, `PayrollAreaLogic` | `Payroll/PayrollAreaController`, `IPayrollManager`, `PayrollAreaDao`, Sneat Views | `bos_payroll_areas`, `sp_Pay_payroll_areas*` | Implemented / Verified |
| **Payroll: Payroll Group** | `PayrollGroupController`, `PayrollGroupLogic` | `Payroll/PayrollGroupController`, `IPayrollManager`, `PayrollGroupDao`, Sneat Views | `bos_payroll_groups`, `bos_pay_group_comps` | Implemented / Verified |
| **Payroll: Payroll Component** | `PayrollComponentController`, `PayrollComponentLogic` | `Payroll/PayrollComponentController`, `IPayrollManager`, `PayrollComponentDao`, Sneat Views | `bos_payroll_components`, `sp_Pay_payroll_components*` | Implemented / Verified |
| **Payroll: Payroll Period** | `PayrollPeriodeController`, `PayrollPeriodeLogic` | `Payroll/PayrollPeriodController`, `IPayrollManager`, `PayrollPeriodDao`, Sneat Views | `bos_payroll_periodes`, `bos_pay_periode_dates` | Implemented / Verified |
| **Payroll: Recurring Components** | `PayrollRecurringComponentController`, `PayrollRecurringComponentLogic` | `Payroll/PayrollRecurringComponentController`, `IPayrollManager`, `PayrollRecurringComponentDao`, Sneat Views | `bos_pay_recurring_comps` | Implemented / Verified |
| **Payroll: One-Time Transactions** | `OneTimeTransactionController`, `PayrollOneTimeTransactionLogic` | `Payroll/OneTimeTransactionController`, `IPayrollManager`, `OneTimeTransactionDao`, Sneat Views | `bos_pay_onetime_trans` | Implemented / Verified |
| **Payroll: Proportional Rules** | `ProportionalController`, `ProportionalLogic` | `Payroll/ProportionalController`, `IPayrollManager`, `ProportionalDao`, Sneat Views | `bos_proportionals` | Implemented / Verified |
| **Payroll: Payroll Process** | `PayrollProcessController`, `PayrollProcessLogic` | `Payroll/PayrollProcessController`, `IPayrollManager`, `PayrollProcessDao`, Sneat Views | `bos_payroll_processes`, `bos_payroll_process_components`, `bos_pay_proc_periods` | Implemented / Verified |
| **Payroll: Calculation & Detail Engine** | `PayrollCalculationController`, `PayrollCalculationDetailController`, `Calculation.cs` | `Payroll/PayrollCalculationController`, `PayrollCalculationDetailController`, `PayrollEngine`, Sneat Views | `bos_payroll_calculations`, `bos_payroll_calc_results`, `bos_temp_pay_calcs`, `bos_pay_calc_histories` | Implemented / Verified |
| **Payroll: Salary Slip / Slip Gaji** | `SalarySlipController`, `SlipGajiController`, `SlipGajiLogic` | `Payroll/SalarySlipController`, Sneat Views & printable payslip layout | `bos_payroll_calc_results`, `bos_employee_pay_attributes` | Implemented / Verified |
| **Tax: Tax Parameters & Statuses** | `TaxParameterController`, `TaxParameterLogic` | `Tax/TaxParameterController`, `ITaxManager`, `TaxParameterDao`, Sneat Views | `bos_tax_parameters`, `bos_tax_percentages`, `bos_tax_statuses` | Implemented / Verified |
| **Tax: Tax Office (KPP)** | `TaxOfficerController`, `TaxServicesOfficeLogic` | `Tax/TaxOfficerController`, `ITaxManager`, `TaxOfficerDao`, Sneat Views | `bos_tax_service_offices` | Implemented / Verified |
| **Tax: Tax Line Components** | `TaxLineComponentController`, `TaxLineComponentLogic` | `Tax/TaxLineComponentController`, `ITaxManager`, `TaxLineComponentDao`, Sneat Views | `bos_taxline_components`, `bos_tax_component_assigments` | Implemented / Verified |
| **Tax: SPT Masa & Tax Calculations** | `SPTMasaController`, `TaxCalculationLogic`, `TaxCalculation.cs` | `Tax/SPTMasaController`, `TaxCalculationController`, Sneat Views | `bos_tax_calculations` | Implemented / Verified |
| **Reports: Jamsostek / BPJS & Statutory** | `ReportJamsostek1b/1c/2a/PerubahanUpah/SIPP`, `Jamsostek*` | `Reports/JamsostekReportController`, Sneat Report Views | Statutory employee payroll & insurance tables | Implemented / Verified |
| **ESS: Employee Self Service** | `ESSEmployeeController`, `ESSLeaveRequestController`, `ESSOvertimeRequestController`, `ESSTimesheetController` | `PersonalAdmin/ESSController` & Self-service action shortcuts in Sneat UI | Employee self-request tables (`bos_timesheet_reqs`, `bos_unattendance_reqs`, `bos_overtime_requests`) | Implemented / Verified |

---

## 4. Prioritized Implementation Roadmap

1. **Foundational & Framework (.NET 10 Compatibility):**
   - Upgrade target `.csproj` files to target `net10.0`.
   - Update NuGet packages compatible with .NET 10.
   - Clean up deprecated middleware and ensure solution compiles clean.

2. **Master Data Completion:**
   - Implement missing Sneat views for `CostCenter` and `BankSource`.
   - Implement DAOs and views for `Company`, `Currency`, `Lookup`.

3. **Payroll Management Suite (Core Priority):**
   - Model layer (`HRIS.Payroll.Model` DAOs): `PayrollAreaDao`, `PayrollGroupDao`, `PayrollComponentDao`, `PayrollPeriodDao`, `PayrollRecurringComponentDao`, `OneTimeTransactionDao`, `ProportionalDao`, `PayrollProcessDao`, `PayrollCalculationDao`.
   - Service layer: `IPayrollManager` & `PayrollManager`.
   - Web Controllers (`HRIS.Web/Controllers/Payroll/`): `PayrollAreaController`, `PayrollGroupController`, `PayrollComponentController`, `PayrollPeriodController`, `PayrollRecurringComponentController`, `OneTimeTransactionController`, `ProportionalController`, `PayrollProcessController`, `PayrollCalculationController`, `SalarySlipController`.
   - Sneat Razor Views: Index, Create, Edit, Details for all Payroll components.

4. **Tax Management Suite (PPh 21):**
   - Model layer (`HRIS.Tax.Model` DAOs): `TaxParameterDao`, `TaxOfficerDao`, `TaxLineComponentDao`, `TaxCalculationDao`.
   - Service layer: `ITaxManager` & `TaxManager`.
   - Web Controllers (`HRIS.Web/Controllers/Tax/`): `TaxParameterController`, `TaxOfficerController`, `TaxLineComponentController`, `SPTMasaController`.
   - Sneat Razor Views: Index, Create, Edit, Details.

5. **Personal Administration Enhancements & ESS Integration:**
   - Complete Employee detail sub-tabs (Family, Address, Bank Accounts).
   - Ensure clean Sneat UX for employee profiles and ESS workflows.

6. **End-to-End Build, Verification & Documentation:**
   - Comprehensive solution build on .NET 10.
   - Verify routing, navigation sidebar items, and action forms.
   - Finalize `HRIS_MIGRATION_REPORT.md`.
