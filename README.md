# 🏢 Human Resource Information System (HRIS)

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-13.0-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Database](https://img.shields.io/badge/Database-PostgreSQL-336791?style=for-the-badge&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![ORM](https://img.shields.io/badge/Data%20Access-Dapper%20%7C%20Npgsql-e05d44?style=for-the-badge&logo=nuget&logoColor=white)](https://github.com/DapperLib/Dapper)
[![UI Theme](https://img.shields.io/badge/UI%20Theme-Sneat%20Bootstrap%205-696cff?style=for-the-badge&logo=bootstrap&logoColor=white)](https://themeselection.com/products/sneat-bootstrap-html-admin-template/)
[![License](https://img.shields.io/badge/License-Proprietary-gray?style=for-the-badge)](#)

A modern, enterprise-grade **Human Resource Information System (HRIS)** built with **ASP.NET Core MVC .NET 10**, featuring a clean multi-tier modular architecture and a responsive **Sneat Bootstrap 5** administrative dashboard. 

Engineered for full parity with enterprise HR workflows (including personnel onboarding, attendance, shift scheduling, overtime requests & timesheets, automated payroll runs, and Indonesian PPh 21 tax compliance), powered by high-performance **PostgreSQL** queries with explicit column projections, NULL safety, and searchable **Select2** controls.

---

## 📑 Table of Contents

- [🏛️ Architectural Modules](#️-architectural-modules)
  - [1. 👤 Personal Administration Module](#1--personal-administration-module)
  - [2. ⏱️ Time Management Module](#2-️-time-management-module)
  - [3. 💰 Payroll Management Module](#3--payroll-management-module)
  - [4. 📊 Tax Management (PPh 21) Module](#4--tax-management-pph-21-module)
  - [5. ⚙️ Master Data Management Module](#5-️-master-data-management-module)
  - [6. 🔐 User Management & Security Module (RBAC)](#6--user-management--security-module-rbac)
  - [7. 🧰 Cross-Cutting & Utility Layer](#7--cross-cutting--utility-layer)
- [🏗️ System Architecture Diagram](#️-system-architecture-diagram)
- [💻 Comprehensive Tech Stack](#-comprehensive-tech-stack)
- [📁 Solution & Project Structure](#-solution--project-structure)
- [🚀 Getting Started](#-getting-started)
  - [Prerequisites](#prerequisites)
  - [Database Configuration](#database-configuration)
  - [Build & Run](#build--run)
- [👥 Authors & Contributors](#-authors--contributors)

---

## 🏛️ Architectural Modules

The solution is partitioned into decoupled domain modules, each containing dedicated Data Access Objects (DAOs), DTO models, and business service managers:

```
┌────────────────────────────────────────────────────────────────────────┐
│                              HRIS.Web                                  │
│             (Controllers, Sneat Razor Views, UI Assets)                │
└──────────────────────────────────┬─────────────────────────────────────┘
                                   │
                                   ▼
┌────────────────────────────────────────────────────────────────────────┐
│                            HRIS.Service                                │
│       (Orchestration & Business Logic: IPersonalAdminManager, etc.)    │
└──────────────┬───────────────────┼───────────────────┬─────────────────┘
               │                   │                   │
               ▼                   ▼                   ▼
┌─────────────────────────┐ ┌─────────────┐ ┌─────────────────────────┐
│ HRIS.PersonalAdmin.Model│ │HRIS.Payroll │ │ HRIS.TimeManagement.    │
│ (People & Attributes)   │ │  .Model     │ │      Model              │
└──────────────┬──────────┘ └──────┬──────┘ └──────────┬──────────────┘
               │                   │                   │
               ├───────────────────┼───────────────────┤
               ▼                   ▼                   ▼
┌─────────────────────────┐ ┌─────────────┐ ┌─────────────────────────┐
│     HRIS.Tax.Model      │ │ HRIS.Master │ │ HRIS.UserManagement     │
│  (PPh 21 Calculations)  │ │   .Model    │ │      .Model (RBAC)      │
└──────────────┬──────────┘ └──────┬──────┘ └──────────┬──────────────┘
               │                   │                   │
               └───────────────────┼───────────────────┘
                                   ▼
┌────────────────────────────────────────────────────────────────────────┐
│                              HRIS.General                              │
│       (Shared DTOs, Utilities, Encryption, Logging, Security)          │
└──────────────────────────────────┬─────────────────────────────────────┘
                                   │
                                   ▼
┌────────────────────────────────────────────────────────────────────────┐
│                        PostgreSQL Database                             │
│       (Explicit Columns, NULL-Safe Joins, Timestamp Normalization)     │
└────────────────────────────────────────────────────────────────────────┘
```

---

### 1. 👤 Personal Administration Module
*Namespace: `HRIS.PersonalAdmin.Model` | Controller: `EmployeeController`*
- **Employee Onboarding (Hire Flow):** Tabbed creation interface separating *Personal Demographics*, *Position & Organization*, *Payroll Preferences*, and *Time & Tax Configurations*.
- **Comprehensive Profile Management:** View and edit personal profiles with Sneat card summaries, employee badges (Active, Contract, Terminated), avatars, and detailed sub-tables.
- **Relational Personnel Data:** Integrates sub-entities for family members (`bos_employee_family`), residential & postal addresses (`bos_employee_address`), disbursement bank accounts (`bos_bank_accounts`), and leave quotas (`bos_employee_quota`).
- **Employment Lifecycle:** Department movements, contract expiry tracking, and structured employee termination (`TerminateEmployee`).

---

### 2. ⏱️ Time Management Module
*Namespace: `HRIS.TimeManagement.Model` | Controllers: `TimesheetController`, `OvertimeRequestController`, `OvertimeTimesheetController`, `TimeFrameController`, etc.*
- **Timesheet Tracking & Analytics:** Real-time logging of daily check-in/out, schedule deviation (`time_dev_in`, `time_dev_out`), unattendance tracking, and employee self-service (ESS) timesheet views.
- **Overtime Request Management:**
  - Auto-generated document sequence numbers (`OTR-YYYYMMDD-XXXX`).
  - Searchable **Select2** dropdowns for Units/Departments and Employee lookups.
  - Multi-tier overtime request approval workflows (Open, Approved, Rejected).
- **Overtime Timesheet Filter & Records:**
  - Date range filtering, Area, Workgroup, and Employee live search.
  - Interactive **DataTables Bootstrap 5** displaying scheduled vs. actual hours, requested periods, and approved overtime durations.
- **Workgroups & Shift Scheduling:** Configurable workgroup templates, schedule variants, shift rotations, and calendar milestone periods.
- **Leave & Unattendance Management:** Request submission, quota balance tracking, and company holiday calendars (`bos_company_calendars`).

---

### 3. 💰 Payroll Management Module
*Namespace: `HRIS.Payroll.Model` | Controllers: `PayrollCalculationController`, `PayrollPeriodController`, `SalarySlipController`, etc.*
- **Payroll Component Engine:** Configurable earnings (gaji pokok, allowances, overtime pay) and statutory deductions (BPJS, loan repayment, withholdings).
- **Payroll Processing Cycles:** Periodic calculation batches (`bos_payroll_calculations`), calculation history tracking, and proportional salary adjustments.
- **Payslips & Disbursement:** Automated payslip generation with printable breakdown and bank transfer file routing based on bank source definitions (`bos_bank_sources`).

---

### 4. 📊 Tax Management (PPh 21) Module
*Namespace: `HRIS.Tax.Model` | Controllers: `TaxCalculationController`, `TaxOfficerController`, `SPTMasaController`, etc.*
- **Indonesian Tax Engine:** Compliant PPh 21 calculation honoring PTKP categories (`TK/0`, `K/0`, `K/1`, `K/2`, `K/3`).
- **Tax Brackets & Rules:** Tiered progressive tax percentages, non-taxable income thresholds, and tax line component mapping.
- **Tax Office Administration:** Tax Service Office (KPP) assignments and automated withholding tax reporting (SPT Masa).

---

### 5. ⚙️ Master Data Management Module
*Namespace: `HRIS.Master.Model` | Controllers: `UnitController`, `PositionController`, `JobController`, `LevelGradeController`, `AreaController`, etc.*
- **Organizational Structure:** Multi-level Unit/Department hierarchy, Division mapping, and Cost Center allocations.
- **Job Architecture:** Job descriptions, Job Titles, Positions, and Level Grades.
- **Lookup & Metadata:** Centralized Object Master catalog, Bank Sources, and System Parameters.

---

### 6. 🔐 User Management & Security Module (RBAC)
*Namespace: `HRIS.UserManagement.Model` | Controllers: `UsersController`, `RoleController`, `AccessModuleController`, `MenuController`*
- **Role-Based Access Control:** Granular permission matrices per role for Create, Read, Update, and Delete actions.
- **Dynamic Menu Resolution:** Database-driven navigation tree (`bos_menu`) with client-side active state persistence and accordion expansion in the Sneat sidebar.
- **Session & Identity:** Encrypted user cookies, password hashing with salt, and profile association with employee records.

---

### 7. 🧰 Cross-Cutting & Utility Layer
*Namespace: `HRIS.General`*
- **Security & Cryptography:** AES/Rijndael encryption helpers for sensitive payloads and password verification.
- **Diagnostics & Logging:** File-based application activity logs categorized by layer (`WebLog/`, `ServiceLog/`).
- **Domain Aggregates:** ViewModels such as `EmployeeAttributeViewModel` that encapsulate multi-table transactions.

---

## 🏗️ System Architecture Diagram

```
+-------------------------------------------------------------------------------+
|                                PRESENTATION LAYER                             |
|                           HRIS.Web (ASP.NET Core MVC)                         |
|                                                                               |
|  [Sneat Admin Theme]      [DataTables Bootstrap 5]       [Select2 Searchable] |
|  Controllers: Employee, Timesheet, OvertimeRequest, OvertimeTimesheet, etc.  |
+---------------------------------------+---------------------------------------+
                                        |
                                        v
+-------------------------------------------------------------------------------+
|                            SERVICE / BUSINESS LAYER                           |
|                                  HRIS.Service                                 |
|                                                                               |
|   IPersonalAdminManager       ITimeManagementManager         IMasterManager   |
|   IPayrollManager             ITaxManager                    IUserManager     |
+---------------------------------------+---------------------------------------+
                                        |
                                        v
+-------------------------------------------------------------------------------+
|                           DATA ACCESS LAYER (DAOs)                            |
|             HRIS.*.Model Projects (Dapper Micro-ORM + Explicit SQL)           |
|                                                                               |
|  PeopleDao.cs                 OvertimeReqDao.cs         TimesheetDao.cs       |
|  EmployeeOvertimeDao.cs       PayrollCalculationDao.cs   TaxCalculationDao.cs  |
+---------------------------------------+---------------------------------------+
                                        |
                                        v
+-------------------------------------------------------------------------------+
|                           DATABASE MANAGEMENT SYSTEM                          |
|                             PostgreSQL Database                               |
|                                                                               |
|  - Explicit Column Selection (Zero "SELECT *" wildcard queries)               |
|  - ISO Timestamp Handling (timestamp without time zone)                       |
|  - Parameterized Npgsql Queries & ACID Transaction Scopes                     |
+-------------------------------------------------------------------------------+
```

---

## 💻 Comprehensive Tech Stack

### **Backend & Runtime**
| Component | Technology | Version / Details |
|---|---|---|
| **Platform** | .NET | **.NET 10.0** (`net10.0`) |
| **Language** | C# | **C# 13** |
| **Web Framework** | ASP.NET Core MVC | Target net10.0 |
| **Micro-ORM** | Dapper | High-performance object mapping |
| **DB Driver** | Npgsql | Native PostgreSQL ADO.NET Data Provider |
| **Architecture** | Layered Multi-Tier | DAO Pattern + Service Managers + MVC Controllers |
| **Query Strategy** | Explicit Projections | Strictly explicit column names; zero `SELECT *` wildcards |

### **Database & Storage**
| Component | Technology | Version / Details |
|---|---|---|
| **RDBMS** | PostgreSQL | 14+ compatible |
| **Temporal Typing** | PostgreSQL Timestamp | `timestamp without time zone` with domain alias `datetime` |
| **Transactions** | ACID DB Transactions | `IDbTransaction` scoped commit/rollback for multi-table inserts |

### **Frontend & UI Presentation**
| Component | Technology | Version / Details |
|---|---|---|
| **Theme / Template** | Sneat Admin Template | Bootstrap 5.3 HTML5/CSS3 Responsive Dashboard |
| **View Engine** | Razor Views (`.cshtml`) | Tag Helpers, Anti-Forgery Tokens, Partial Views |
| **Table Engine** | DataTables | **DataTables Bootstrap 5 Adapter** + Responsive Extension |
| **Searchable Dropdowns**| Select2 | Searchable dropdowns for Employees, Units, Areas & Workgroups |
| **Iconography** | Boxicons | Modern vector icon set |
| **Client Scripting** | Vanilla JS & jQuery 3.7+ | Client-side menu resolution, AJAX submissions |

---

## 📁 Solution & Project Structure

```text
D:\SourceCode\hris\
├── HRIS.sln                               # Visual Studio / .NET 10 Solution
│
├── HRIS.Web/                              # Presentation Layer (ASP.NET Core MVC)
│   ├── Controllers/                       # MVC Controllers grouped by domain
│   │   ├── Master/                        # Unit, Position, Job, Area, LevelGrade
│   │   ├── Payroll/                       # PayrollCalculation, SalarySlip, Periods
│   │   ├── PersonalAdmin/                 # Employee, EmployeeGroup
│   │   ├── Tax/                           # TaxCalculation, TaxOfficer, SPTMasa
│   │   ├── TimeManagement/                # Timesheet, OvertimeRequest, OvertimeTimesheet
│   │   └── UserManagement/                # Users, Role, AccessModule, Menu
│   ├── Views/                             # Sneat Bootstrap 5 Razor views
│   ├── wwwroot/                           # Static assets, Sneat CSS/JS, plugins
│   ├── Helper/                            # Cookie, session, and view helpers
│   └── appsettings.json                   # Database & logging configurations
│
├── HRIS.Service/                          # Service & Business Orchestration Layer
│   ├── Interface/                         # Service contracts (IPersonalAdminManager, etc.)
│   └── Manager/                           # Service manager implementations
│
├── HRIS.PersonalAdmin.Model/              # Personnel Data Access Layer (PeopleDao)
├── HRIS.TimeManagementModel/              # Time DAOs (TimesheetDao, OvertimeReqDao, EmployeeOvertimeDao)
├── HRIS.Payroll.Model/                    # Payroll DAOs (PayrollCalculationDao, etc.)
├── HRIS.Tax.Model/                        # Tax DAOs (TaxCalculationDao, TaxOfficerDao)
├── HRIS.Master.Model/                     # Master Data DAOs (UnitDao, PositionDao, AreaDao)
├── HRIS.UserManagement.Model/             # Security & RBAC DAOs (UserDao, RoleDao, MenuDao)
│
├── HRIS.General/                          # Shared Models, DTOs, Enums & Utilities
│   ├── Model/                             # Strongly-typed domain models & ViewModels
│   └── Utility/                           # Encryption (AES), Logger, EnumCollection
│
└── db_migrate_datetime_postgresql.sql     # PostgreSQL date-to-timestamp migration script
```

---

## 🚀 Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL 14+](https://www.postgresql.org/download/)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) / [VS Code](https://code.visualstudio.com/) with C# Dev Kit

---

### Database Configuration

Update `HRIS.Web/appsettings.json` with your PostgreSQL credentials:

```json
{
  "ConnectionStrings": {
    "DbConnection": "Host=localhost;Port=5432;Database=hris_db;Username=postgres;Password=YOUR_PASSWORD;Include Error Detail=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    },
    "DestinationFolder": {
      "Web": "Log/WebLog/",
      "Service": "Log/ServiceLog/"
    }
  }
}
```

> **Note for PostgreSQL Date/Time Columns:**
> PostgreSQL uses `timestamp without time zone`. If migrating legacy dates, apply `db_migrate_datetime_postgresql.sql` which establishes the `CREATE DOMAIN datetime AS timestamp without time zone;` alias and alters legacy date columns.

---

### Build & Run

1. **Restore Solution Dependencies**
   ```bash
   dotnet restore HRIS.sln
   ```

2. **Build the Solution**
   ```bash
   dotnet build HRIS.sln
   ```

3. **Run the Web Application**
   ```bash
   cd HRIS.Web
   dotnet run
   ```

4. **Access the Portal**
   Open your browser and navigate to:
   ```text
   http://localhost:5000  or  https://localhost:5001
   ```

---

## 👥 Authors & Contributors

- **Erdendi Lesmana** ([@dendidul](https://github.com/dendidul)) — *Lead Developer & System Architect*

---

<div align="center">
  <sub>Human Resource Information System &copy; 2026. Built with modern .NET 10 & PostgreSQL.</sub>
</div>
