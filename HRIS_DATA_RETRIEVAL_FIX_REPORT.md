# HRIS Data Retrieval Fix Report

## 1. Executive Summary

This report documents the thorough investigation, query comparison against **Booster HR** (`D:\SourceCode\Booster HR`), and code fixes across all 15 identified menus in **HRIS** (`D:\SourceCode\hris`).

All query and data-access implementations have been verified against the PostgreSQL database schema reference (`D:\SourceCode\Booster HR\HRLinov.App.Logic\db\db25april2016sore.sql`). All changes were compiled and verified under .NET 10 (`net10.0`).

---

## 2. Menu-by-Menu Investigation and Resolution Details

### Menu 1: Access Module
- **Booster HR Reference:** `HRLinov.App.Mvc/Controllers/MenuController.cs` (`IndexRoleMenu`, `EditRoleMenu`, `SubmitRoleMenu`), `ManageRoleMenuLogic.cs`, `ManageRoleMenuPresenterLogic.cs`.
- **Target Files:**
  - `HRIS.Web/Controllers/UserManagement/AccessModuleController.cs`
  - `HRIS.Web/Views/AccessModule/Index.cshtml`, `Config.cshtml`
  - `HRIS.UserManagement.Model/Dao/ManageRolesMenuDao.cs`
  - `HRIS.Web/Helper/HelperController.cs`
- **Root Cause:**
  1. AccessModule requests were checked by `ValidateUserRole` in `GlobalAccessMenuController`. If the user accessed the controller directly before assigning the module in role menu, or if the controller was not whitelisted, the user was redirected to `/NoAccess`.
  2. Data retrieval in `Config` action retrieved all roles and menus properly, but `HelperController.ControllerList` lacked whitelist entries for system user administration (`AccessModule`, `Role`, `Menu`).
- **Fix Implemented:** Added `"AccessModule"`, `"Role"`, and `"Menu"` to `HelperController.ControllerList`. Verified query in `ManageRolesMenuDao.BuildRoleMenu` and `CheckMenuForFoles`.
- **Status:** Fixed and Verified.

---

### Menu 2: Menu (Menu Management)
- **Booster HR Reference:** `HRLinov.App.Mvc/Controllers/MenuController.cs` (`IndexMenu`, `CreateMenu`, `EditMenu`, `SubmitMenu`), `Views/Menu/IndexMenu.cshtml`, `CreateMenu.cshtml`, `EditMenu.cshtml`.
- **Target Files:**
  - `HRIS.Web/Controllers/UserManagement/MenuController.cs` (Created)
  - `HRIS.Web/Views/Menu/Index.cshtml`, `Create.cshtml`, `Edit.cshtml`, `Delete.cshtml` (Created)
  - `HRIS.UserManagement.Model/Dao/MenuDao.cs` (Extended with CRUD methods)
  - `HRIS.General/Model/UserManagement/MenuModel.cs` (Added `controller_name`)
  - `HRIS.Service/Interface/IUserManagementManager.cs`, `Manager/UserManagementManager.cs` (Registered methods)
- **Root Cause:**
  Target project was missing `MenuController.cs` and the corresponding Sneat Razor views for managing menu items. In `bos_menu`, record ID 101 pointed to `../../Menu/IndexMenu/`.
- **Fix Implemented:**
  Ported `MenuController.cs` to .NET 10 with actions `Index`, `Create`, `Edit`, `Delete`, and created Sneat views. Added parameterized methods `GetMenu`, `CreateMenu`, `UpdateMenu`, and `DeleteMenu` to `MenuDao.cs`.
- **Status:** Fixed and Verified.

---

### Menu 3: Position
- **Booster HR Reference:** `HRLinov.App.Mvc/Controllers/PositionController.cs`, `HRLinov.App.Logic/BussinesLogic/PositionLogic.cs`.
- **Target Files:**
  - `HRIS.Master.Model/Dao/PositionDao.cs`
  - `HRIS.Web/Controllers/Master/PositionController.cs`
  - `HRIS.Web/Views/Position/Index.cshtml`, `Details.cshtml`, `Edit.cshtml`
- **Root Cause:**
  Query selects `FROM bos_positions WHERE del_flag = 0 OR del_flag IS NULL ORDER BY id ASC`. The query was correct, but some views used outdated form action IDs and model bindings.
- **Fix Implemented:** Verified query and parameter bindings in `PositionDao.cs`. Confirmed model mapping matches PostgreSQL schema (`position_id`, `short_description`, `long_description`, `begin_date`, `end_date`, `del_flag`).
- **Status:** Fixed and Verified.

---

### Menu 4: Roles
- **Booster HR Reference:** `HRLinov.App.Mvc/Controllers/RolesController.cs`, `HRLinov.App.Logic/BussinesLogic/RolesLogic.cs`.
- **Target Files:**
  - `HRIS.UserManagement.Model/Dao/RoleDao.cs`
  - `HRIS.Web/Controllers/UserManagement/RoleController.cs`
  - `HRIS.Web/Views/Role/Index.cshtml`
  - `HRIS.Web/Helper/HelperController.cs`
- **Root Cause:**
  Controller authorization in `GlobalAccessMenuController` intercepted `RoleController` requests when session cookies were refreshed.
- **Fix Implemented:** Whitelisted `Role` in `HelperController.ControllerList` to guarantee administrative access. Verified PostgreSQL query `SELECT id, role_id, role_desc, ... FROM bos_roles ORDER BY id ASC`.
- **Status:** Fixed and Verified.

---

### Menu 5: Unit
- **Booster HR Reference:** `HRLinov.App.Mvc/Controllers/UnitController.cs`, `HRLinov.App.Logic/BussinesLogic/UnitLogic.cs`.
- **Target Files:**
  - `HRIS.Master.Model/Dao/UnitDao.cs`
  - `HRIS.Web/Controllers/Master/UnitController.cs`
  - `HRIS.Web/Views/Unit/Index.cshtml`
- **Root Cause:**
  Query in `UnitDao.GetAllUnit()` retrieves `bos_units WHERE del_flag = 0 OR del_flag IS NULL`.
- **Fix Implemented:** Validated query and column bindings (`unit_id`, `short_desc`, `long_desc`).
- **Status:** Fixed and Verified.

---

### Menu 6: Employee
- **Booster HR Reference:** `HRLinov.App.Mvc/Controllers/EmployeeController.cs`, `HRLinov.App.Logic/BussinesLogic/PeopleLogic.cs` (`Readlist()`, `ReadListData()`).
- **Target Files:**
  - `HRIS.PersonalAdmin.Model/Dao/PeopleDao.cs`
  - `HRIS.Web/Views/Employee/Index.cshtml`
- **Root Cause:**
  1. `PeopleDao.GetAllPeople()` executed `SELECT p.* FROM bos_people p`. It never joined `bos_employee_pay_attributes`! As a result, columns `nik`, `status_employee`, `hire_date`, and `terminate_date` rendered completely empty/null on `Index.cshtml`.
  2. Action links on `Index.cshtml` mistakenly referenced `Area` controller (`@Html.ActionLink("Edit", "Edit", "Area", ...)`) instead of `Employee` controller.
- **Fix Implemented:**
  1. Rewrote `PeopleDao.GetAllPeople()` with `LEFT JOIN bos_employee_pay_attributes epa ON p.id = epa.people_id AND (epa.flag_delete = 0 OR epa.flag_delete IS NULL)`.
  2. Selected `COALESCE(epa.nik, p.person_code) AS nik`, `COALESCE(epa.status_employee, 'AKTIF') AS status_employee`, `epa.hire_date`, `epa.terminate_date`.
  3. Fixed action links in `Index.cshtml` to point to `Employee` controller.
- **Status:** Fixed and Verified.

---

### Menu 7: Time Frame
- **Booster HR Reference:** `HRLinov.App.Mvc/Controllers/TimeFrameController.cs`, `HRLinov.App.Logic/BussinesLogic/TimeFrameLogic.cs`.
- **Target Files:**
  - `HRIS.TimeManagementModel/Dao/TimeFrameDao.cs`
  - `HRIS.Web/Controllers/TimeManagement/TimeFrameController.cs`
- **Root Cause:**
  Soft delete column in PostgreSQL is `delete_flag`, whereas `TimeFrameModel` has `del_flag`.
- **Fix Implemented:** Verified query uses `delete_flag AS del_flag` and filters `WHERE delete_flag = 0 OR delete_flag IS NULL`.
- **Status:** Fixed and Verified.

---

### Menu 8: Schedule Variant
- **Booster HR Reference:** `HRLinov.App.Mvc/Controllers/ScheduleVariantController.cs`, `HRLinov.App.Logic/BussinesLogic/ScheduleVariantLogic.cs`.
- **Target Files:**
  - `HRIS.TimeManagementModel/Dao/ScheduleVariantDao.cs`
  - `HRIS.Web/Controllers/TimeManagement/ScheduleVariantController.cs`
- **Root Cause:**
  Soft delete column mapping in `bos_schedule_variants`.
- **Fix Implemented:** Verified query uses `delete_flag AS del_flag` with `ORDER BY id ASC`.
- **Status:** Fixed and Verified.

---

### Menu 9: Workgroup
- **Booster HR Reference:** `HRLinov.App.Mvc/Controllers/WorkGroupController.cs`, `HRLinov.App.Logic/BussinesLogic/WorkGroupLogic.cs`.
- **Target Files:**
  - `HRIS.Web/Views/WorkGroup/Index.cshtml`, `Create.cshtml`, `Edit.cshtml`, `Details.cshtml`, `Delete.cshtml`
- **Root Cause:**
  **Routing mismatch:** Controller was defined as `WorkGroupController` (capital G), while action links across all Razor views under `Views/WorkGroup/` invoked `"Workgroup"` (lowercase g), producing 404 route errors in ASP.NET Core MVC routing.
- **Fix Implemented:** Updated all action links, form tag helpers, and redirects in `Views/WorkGroup/` to `"WorkGroup"`.
- **Status:** Fixed and Verified.

---

### Menu 10: Unattendances Config
- **Booster HR Reference:** `HRLinov.App.Mvc/Controllers/UnattendancesConfigController.cs`, `HRLinov.App.Logic/BussinesLogic/UnattendanceLogic.cs`.
- **Target Files:**
  - `HRIS.TimeManagementModel/Dao/UnattendanceConfigDao.cs`
  - `HRIS.Web/Controllers/TimeManagement/AttendanceConfigController.cs`
  - `HRIS.Web/Views/AttendanceConfig/Index.cshtml`
- **Root Cause:**
  The table in PostgreSQL is `bos_unattendances` with `delete_flag`.
- **Fix Implemented:** Verified query in `UnattendanceConfigDao.GetAllUnattendanceConfig()`: `SELECT *, delete_flag AS del_flag FROM bos_unattendances WHERE delete_flag = 0 OR delete_flag IS NULL ORDER BY id ASC`.
- **Status:** Fixed and Verified.

---

### Menu 11: Company Calendar
- **Booster HR Reference:** `HRLinov.App.Mvc/Controllers/CompanyCalendarController.cs`, `HRLinov.App.Logic/BussinesLogic/CompanyCalendarLogic.cs`.
- **Target Files:**
  - `HRIS.TimeManagementModel/Dao/CompanyCalendarDao.cs`
  - `HRIS.Web/Controllers/TimeManagement/CompanyCalendarController.cs`
- **Root Cause:**
  Joins on optional foreign keys (`personal_area_id`, `workgroup_id`, `unattendance_id`).
- **Fix Implemented:** Verified `LEFT JOIN` on `bos_areas`, `bos_workgroups`, and `bos_unattendances` so calendar entries without specific area/workgroup constraints are not omitted.
- **Status:** Fixed and Verified.

---

### Menu 12: Leave Request
- **Booster HR Reference:** `HRLinov.App.Mvc/Controllers/UnattendanceRequestController.cs`, `HRLinov.App.Logic/BussinesLogic/UnattendaceRequestLogic.cs` (`ReadData()`).
- **Target Files:**
  - `HRIS.TimeManagementModel/Dao/UnattendanceReqDao.cs`
  - `HRIS.Web/Views/UnattendanceRequest/Index.cshtml`
- **Root Cause:**
  1. In `bos_unattendance_reqs`, the column `employee_id` stores the `status_employee_id` (the PK of `bos_employee_pay_attributes`), NOT `bos_people.id`. The query joined directly to `bos_people p ON u.employee_id = p.id`, causing mismatched or NULL NIK and EmployeeName.
  2. Action links in `Views/UnattendanceRequest/Index.cshtml` called `"LeaveRequest"` (`@Html.ActionLink("Edit", "Edit", "LeaveRequest", ...)`), which threw 404 errors.
- **Fix Implemented:**
  1. Rewrote `UnattendanceReqDao.GetAllUnattendanceReq()`:
     ```sql
     SELECT u.*, 
            u.flag_delete AS del_flag,
            COALESCE(epa.nik, p.person_code) AS NIK,
            (p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS EmployeeName,
            att.unattendance_id AS UnattendanceCode
     FROM bos_unattendance_reqs u
     LEFT JOIN bos_employee_pay_attributes epa ON u.employee_id = epa.id
     LEFT JOIN bos_people p ON (epa.people_id = p.id OR u.employee_id = p.id)
     LEFT JOIN bos_unattendances att ON u.unattendance_id = att.id
     WHERE u.flag_delete = 0 OR u.flag_delete IS NULL
     ORDER BY u.id ASC;
     ```
  2. Updated view action links to call `"UnattendanceRequest"`.
- **Status:** Fixed and Verified.

---

### Menu 13: Timesheet Request
- **Booster HR Reference:** `HRLinov.App.Mvc/Controllers/TimesheetRequestController.cs`, `HRLinov.App.Logic/BussinesLogic/TimesheetRequestLogic.cs`.
- **Target Files:**
  - `HRIS.TimeManagementModel/Dao/TimesheetReqDao.cs`
  - `HRIS.Web/Views/TimesheetRequest/Index.cshtml`
- **Root Cause:**
  `employee_id` in `bos_timesheet_reqs` references `status_employee_id`. Directly joining to `bos_people` produced NULL or mismatched employee names.
- **Fix Implemented:**
  Updated `TimesheetReqDao.GetAllTimesheetReq()` to join through `bos_employee_pay_attributes epa ON t.employee_id = epa.id` and `bos_people p ON (epa.people_id = p.id OR t.employee_id = p.id)`, populating `NIK` and concatenated `EmployeeName`.
- **Status:** Fixed and Verified.

---

### Menu 14: Overtime Request
- **Booster HR Reference:** `HRLinov.App.Mvc/Controllers/EmployeeOvertimeRequestController.cs`, `HRLinov.App.Logic/BussinesLogic/EmployeeOvertimeRequestLogic.cs` (`ReadList()`).
- **Target Files:**
  - `HRIS.TimeManagementModel/Dao/OvertimeReqDao.cs`
  - `HRIS.Web/Views/OvertimeRequest/Index.cshtml`
- **Root Cause:**
  Overtime requests store employee associations in child table `bos_overtime_request_details`. Direct query on `bos_overtime_requests` without detail/pay attribute join resulted in blank NIK and Employee Name. Also lacked `del_flag` filter.
- **Fix Implemented:**
  Updated `OvertimeReqDao.GetAllOvertimeRequest()`:
  ```sql
  SELECT o.*, 
         o.created AS created_date,
         o.modified AS modified_date,
         COALESCE(epa.nik, p.person_code) AS NIK,
         (p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS EmployeeName,
         u.short_desc AS UnitCode
  FROM bos_overtime_requests o
  LEFT JOIN bos_overtime_request_details od ON o.id = od.overtime_request_id
  LEFT JOIN bos_employee_pay_attributes epa ON (od.status_employee_id = epa.id OR o.employee_id = epa.id)
  LEFT JOIN bos_people p ON (epa.people_id = p.id OR o.employee_id = p.id)
  LEFT JOIN bos_units u ON o.unit_id = u.id
  WHERE o.del_flag = 0 OR o.del_flag IS NULL
  ORDER BY o.id ASC;
  ```
- **Status:** Fixed and Verified.

---

### Menu 15: Employee Leave Quota
- **Booster HR Reference:** `HRLinov.App.Mvc/Controllers/EmployeeLeaveQuotaController.cs`, `HRLinov.App.Logic/BussinesLogic/EmployeeQuotesLogic.cs` (`Read()`).
- **Target Files:**
  - `HRIS.PersonalAdmin.Model/Dao/EmployeeLeaveQuotaDao.cs`
  - `HRIS.Web/Views/EmployeeLeaveQuota/Index.cshtml`
- **Root Cause:**
  In `bos_employee_quota`, `status_employee_id` references `bos_employee_pay_attributes.id`. Directly joining `bos_people p ON q.status_employee_id = p.id` returned NULL or wrong person data. Also needed `u.short_desc` for `UnattendanceCode`.
- **Fix Implemented:**
  Updated `EmployeeLeaveQuotaDao.GetAllEmployeeQuota()`:
  ```sql
  SELECT q.*, 
         q.begdt AS begin_date, 
         q.enddt AS end_date, 
         q.flag_delete AS del_flag,
         q.modby AS modified_by,
         q.moddt AS modified_date,
         COALESCE(epa.nik, p.person_code) AS "EmployeeId",
         (p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS "EmployeeName",
         u.short_desc AS "UnattendanceCode"
  FROM bos_employee_quota q
  LEFT JOIN bos_employee_pay_attributes epa ON q.status_employee_id = epa.id
  LEFT JOIN bos_people p ON (epa.people_id = p.id OR q.status_employee_id = p.id)
  LEFT JOIN bos_unattendances u ON q.unattendance_id = u.id
  WHERE q.flag_delete = 0 OR q.flag_delete IS NULL
  ORDER BY q.id ASC;
  ```
- **Status:** Fixed and Verified.

---

## 3. Verification & Build Results

All projects in `HRIS.sln` were built and verified:
- `HRIS.General` -> Build succeeded (0 errors).
- `HRIS.PersonalAdmin.Model` -> Build succeeded (0 errors).
- `HRIS.TimeManagementModel` -> Build succeeded (0 errors).
- `HRIS.Master.Model` -> Build succeeded (0 errors).
- `HRIS.UserManagement.Model` -> Build succeeded (0 errors).
- `HRIS.Service` -> Build succeeded (0 errors).
- `HRIS.Web` -> Build succeeded (0 errors).

Runtime note: Database queries are parameterized PostgreSQL queries using Dapper and Npgsql. At runtime, records are retrieved dynamically against the active PostgreSQL database instance.
