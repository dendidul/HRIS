# HRIS Migration & Implementation Final Report

## 1. Executive Summary

This report documents the completed migration and implementation from the legacy HRIS system (**Booster HR**) to the new target web application (**HRIS**) targeting **.NET 10** with the **Sneat Bootstrap 5 Admin Template**.

- **Source Codebase:** `D:\SourceCode\Booster HR` (C# ASP.NET MVC 5, .NET Framework 4.5, PostgreSQL `db25april2016sore.sql`).
- **Target Codebase:** `D:\SourceCode\hris` (C# ASP.NET Core MVC on **.NET 10**, PostgreSQL `booster_hr`).
- **Lead Developer & Primary Architect:** Erdendi Lesmana (`@dendidul`).

---

## 2. Architecture Comparison

### A. Legacy Architecture (`Booster HR`)
- **Web Layer:** `HRLinov.App.Mvc` (ASP.NET MVC 5 on .NET Framework 4.5).
- **Business Logic Layer:** `HRLinov.App.Logic` containing BusinessLogic and PresenterLogic classes.
- **Data Access Layer:** Entity Framework 6 / 5 EDMX (`hris_linovEntities`), `Npgsql` for PostgreSQL, raw SQL queries.
- **UI & Presentation:** Bootstrap 3, GridMvc, jQuery EasyUI with ComboGrid components.
- **Security:** FormsAuthentication with database-driven menu access checks (`MenuController`).

### B. Target Architecture (`HRIS`)
- **Web Layer:** `HRIS.Web` (ASP.NET Core MVC on **.NET 10**).
- **Service Layer:** `HRIS.Service` with clean interface abstractions (`IMasterManager`, `IPayrollManager`, `ITaxManager`, `IPersonalAdminManager`, `ITimeManagementManager`, `IUserManagementManager`).
- **Data Access & DAO Layer:** Modular domain DAO libraries (`HRIS.Master.Model`, `HRIS.Payroll.Model`, `HRIS.Tax.Model`, `HRIS.TimeManagementModel`, `HRIS.PersonalAdmin.Model`, `HRIS.UserManagement.Model`).
- **Database Access:** Dapper micro-ORM with high-performance stored procedure conventions and fallback SQL queries.
- **UI & Design System:** Modern Sneat Bootstrap 5 Admin Template, Boxicons, DataTables, responsive forms, and cards.
- **Security & RBAC:** ASP.NET Core Cookie Authentication and dynamic role-based access module checks (`GlobalAccessMenuController`).

---

## 3. Modules and Features Migrated

### 1. Modern Framework Upgrade (.NET 10 Compatibility)
- All projects (`HRIS.General`, `HRIS.Master.Model`, `HRIS.PersonalAdmin.Model`, `HRIS.TimeManagement.Model`, `HRIS.Payroll.Model`, `HRIS.Tax.Model`, `HRIS.UserManagement.Model`, `HRIS.Service`, and `HRIS.Web`) upgraded to **`net10.0`**.
- Updated NuGet dependencies to modern .NET versions (Dapper 2.1.35, Microsoft.AspNetCore.Identity.EntityFrameworkCore 9.0.0, Microsoft.EntityFrameworkCore.SqlServer 9.0.0, System.Data.SqlClient 4.8.6).
- Resolved endpoint routing and middleware configuration in `Startup.cs` (`IWebHostEnvironment`, `AddControllersWithViews`, `FormOptions`).

### 2. Master Data Management (MST)
- Complete CRUD and Sneat views implemented for:
  - **Area** (`AreaController`, `AreaDao`, Sneat Views: Index, Create, Edit, Details)
  - **Bank Source** (`BankSourceController`, `BankSourceDao`, Sneat Views: Index, Create, Edit, Details)
  - **Cost Center** (`CostCenterController`, `CostCenterDao`, Sneat Views: Index, Create, Edit, Details)
  - **Employee Group** (`EmployeeGroupController`, `EmployeeGroupDao`)
  - **Job Title** (`JobController`, `JobDao`)
  - **Level & Grade** (`LevelGradeController`, `LevelGradeDao`)
  - **Object Master** (`ObjectMasterController`, `ObjectMasterDao`)
  - **Position** (`PositionController`, `PositionDao`)
  - **Unit / Department** (`UnitController`, `UnitDao`)

### 3. Personal Administration (PSA) & Self-Service
- **Employee Master & People:** Comprehensive employee profiles, NIK, personal data, biodata.
- **Employee Sub-Tabs:** Family members, address details, bank accounts, employee tax identifiers, and job classifications.
- **Employee Quotas:** Leave balance tracking and quota entitlements (`bos_employee_quota`).

### 4. Time Management (TMN)
- **Company Calendar:** Working days, weekends, and public holidays management.
- **Workgroups & Shift Scheduling:** Shift patterns, work hours, and group assignments.
- **Schedule Variants & Time Frames:** Variable shifts, break times, and milestones.
- **Time Calculation Engine:** Attendance tracking, lateness, early leaving, working hours calculation.
- **Timesheets & Requests:** In/Out attendance logging, attendance regularization requests.
- **Overtime Management:** Overtime requests, approvals, and overtime timesheet calculation.
- **Unattendance / Leave Management:** Annual leave, sick leave, unpaid leave requests, and configurations.

### 5. Payroll Management Suite (PAY)
- Complete new implementation across Model, DAO, Service, and Web layers:
  - **Payroll Area:** Area definition and boundaries (`bos_payroll_areas`).
  - **Payroll Group:** Group configuration linked to periods (`bos_payroll_groups`).
  - **Payroll Component:** Component catalog, allowance (+), deduction (-), fixed/variable/formula types (`bos_payroll_components`).
  - **Payroll Period:** Monthly and periodic payroll calendar ranges (`bos_payroll_periodes`).
  - **Payroll Process:** Calculation schedules, cycle definitions, and working days (`bos_payroll_processes`).
  - **Recurring Components:** Employee recurring allowances/deductions (`bos_pay_recurring_comps`).
  - **One-Time Transactions:** Bonuses, thr, manual adjustments per period (`bos_pay_onetime_trans`).
  - **Proportional Rules:** Prorated salary calculations for joiners/leavers (`bos_proportionals`).
  - **Payroll Calculation Batch & Engine:** Batch creation, execution engine, employee wage evaluation, recurring/one-time component processing, and batch closing (`bos_payroll_calculations`, `bos_payroll_calc_results`).
  - **Salary Slip / Slip Gaji:** Visual employee payslips with breakdown of earnings, deductions, and net take-home pay, plus dedicated print/PDF layout.

### 6. Tax Management Suite (PPh 21)
- Complete new implementation across Model, DAO, Service, and Web layers:
  - **Tax Parameters:** Maximum position cost (Biaya Jabatan 5% up to Rp 500,000/month or custom limit), PTKP rates (`bos_tax_parameters`).
  - **Tax Service Office (KPP):** Kantor Pelayanan Pajak master data (`bos_tax_service_offices`).
  - **Tax Line Components:** Mapping payroll components to tax calculation lines (`bos_taxline_components`).
  - **SPT Masa PPh 21 Engine:** Progressive Indonesian PPh 21 tax bracket calculation engine (5%, 15%, 25%, 35%) and monthly tax liabilities reporting (`bos_tax_calculations`).

### 7. User Management & Security (USM)
- User accounts (`UsersController`), Roles (`RoleController`), Access Modules (`AccessModuleController`).
- Responsive Sneat navigation sidebar in `Menu.cshtml` with dynamic database RBAC menu support and static full-catalog fallback navigation.

---

## 4. Business Rules Preserved

1. **Transaction Types in Payroll:**
   - Components marked `PLUS` add to gross/earnings.
   - Components marked `MINUS` subtract from gross as deductions.
   - Net take-home pay equals $\sum \text{Earnings} - \sum \text{Deductions}$.
2. **Payroll Execution Lifecycle:**
   - Calculation batches initiate in `Open` state.
   - Processing executes synchronously over active employees, populating `bos_payroll_calc_results`.
   - Upon finish, timing and processed counts are recorded, transitioning the batch to `Closed`.
3. **Statutory PPh 21 Tax Logic:**
   - Position cost (Biaya Jabatan) capped at 5% of gross earnings up to Rp 500,000/month.
   - Annualized net income reduced by PTKP (Penghasilan Tidak Kena Pajak).
   - Progressive bracket rates applied (UU HPP):
     - $\le$ Rp 60M: 5%
     - Rp 60M – Rp 250M: 15%
     - Rp 250M – Rp 500M: 25%
     - $>$ Rp 500M: 35%
   - Monthly tax liability calculated and stored for SPT Masa reporting.

---

## 5. Build and Verification Results

- **Compiler / SDK:** .NET SDK 10.0.401 on Windows 11.
- **Solution File:** `D:\SourceCode\hris\HRIS.sln`
- **Build Status:** **BUILD SUCCEEDED** (0 Errors across all 9 projects).
  - `HRIS.General` -> `bin\Debug\net10.0\HRIS.General.dll`
  - `HRIS.Master.Model` -> `bin\Debug\net10.0\HRIS.Master.Model.dll`
  - `HRIS.Payroll.Model` -> `bin\Debug\net10.0\HRIS.Payroll.Model.dll`
  - `HRIS.PersonalAdmin.Model` -> `bin\Debug\net10.0\HRIS.PersonalAdmin.Model.dll`
  - `HRIS.Tax.Model` -> `bin\Debug\net10.0\HRIS.Tax.Model.dll`
  - `HRIS.TimeManagementModel` -> `bin\Debug\net10.0\HRIS.TimeManagement.Model.dll`
  - `HRIS.UserManagement.Model` -> `bin\Debug\net10.0\HRIS.UserManagement.Model.dll`
  - `HRIS.Service` -> `bin\Debug\net10.0\HRIS.Service.dll`
  - `HRIS.Web` -> `bin\Debug\net10.0\HRIS.Web.dll`

---

## 6. Remaining Manual Verification Steps

1. Configure local SQL Server connection string in `appsettings.json` if connecting to a live SQL Server instance.
2. Launch web application using `dotnet run --project HRIS.Web` and access `https://localhost:5001`.
3. Log in with administrative account and verify sidebar navigation across Master Data, Personnel, Time Management, Payroll, and Tax.
4. Execute sample payroll run in `PayrollCalculation` and inspect generated payslip in `SalarySlip`.
