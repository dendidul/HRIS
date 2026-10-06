# 🏢 Human Resource Information System (HRIS)

[![.NET Core](https://img.shields.io/badge/.NET%20Core-2.2-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Database](https://img.shields.io/badge/Database-SQL%20Server-CC292B?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![UI Theme](https://img.shields.io/badge/UI%20Theme-Sneat%20Bootstrap%205-696cff?style=for-the-badge&logo=bootstrap&logoColor=white)](https://themeselection.com/products/sneat-bootstrap-html-admin-template/)
[![License](https://img.shields.io/badge/License-Proprietary-gray?style=for-the-badge)](#)

A modular, enterprise-grade **Human Resource Information System (HRIS)** built with **ASP.NET Core MVC** and a layered multi-tier architecture. Designed to streamline end-to-end human resource operations, including personnel administration, time & attendance, payroll, tax management, master data, and role-based access control.

---

## 📑 Table of Contents

- [✨ Key Features & Modules](#-key-features--modules)
- [🏗️ System Architecture](#️-system-architecture)
- [💻 Tech Stack](#-tech-stack)
- [📁 Project Structure](#-project-structure)
- [🚀 Getting Started](#-getting-started)
  - [Prerequisites](#prerequisites)
  - [Configuration](#configuration)
  - [Installation & Execution](#installation--execution)
- [👥 Authors & Contributors](#-authors--contributors)

---

## ✨ Key Features & Modules

The system is decomposed into decoupled, domain-driven modules:

| Module | Description | Core Capabilities |
|---|---|---|
| **👤 Personal Administration** | Core employee profile & lifecycle management | Employee Master Data, Family/Dependent Data, Document Archival, Contract & Employment History, Leave Quota Allocation. |
| **⏱️ Time Management** | Attendance tracking, shift planning & leaves | Daily Timesheet Tracking, Overtime Requests & Approval Flow, Work Group & Shift Variant Schedules, Unattendance / Leave Requests, Company Holiday Calendar. |
| **💰 Payroll Management** | Automated salary calculation & disbursement | Salary Components (Allowance, Deductions, Base Pay), Automated Payroll Run, Payslip Generation, Bank Source & Transfer Integration. |
| **📊 Tax Management (PPh 21)** | Indonesian tax calculation compliance | Tax Officer Profiles, PTKP Thresholds, Progressive Tax Calculations, Withholding Tax Reporting. |
| **⚙️ Master Data Management** | Centralized organization parameters | Department, Division, Job Title, Position, Level & Grade, Cost Center, Area / Branch, Object Master & Lookups. |
| **🔐 User & Role Management** | Multi-tenant role-based access control (RBAC) | Role Management, Dynamic Menu & Action Permissions (Create/Read/Update/Delete), Application User Directory, Session & Cookie Management. |
| **📧 Notification & Logging** | System auditing and external communication | Integrated SendGrid Email Dispatcher, Custom File & Database Logging for Web and Service layers. |

---

## 🏗️ System Architecture

HRIS follows a clean **Multi-Tier Layered Architecture** with distinct separation of concerns:

```
                      ┌────────────────────────────────────────┐
                      │                HRIS.Web                │
                      │  (ASP.NET Core MVC + Sneat Admin UI)   │
                      └───────────────────┬────────────────────┘
                                          │
                                          ▼
                      ┌────────────────────────────────────────┐
                      │              HRIS.Service              │
                      │       (Business Logic & Managers)      │
                      └─────────┬────────────────────┬─────────┘
                                │                    │
            ┌───────────────────┼────────────────────┼───────────────────┐
            ▼                   ▼                    ▼                   ▼
┌──────────────────────┐ ┌──────────────┐ ┌────────────────────┐ ┌──────────────┐
│ HRIS.PersonalAdmin   │ │ HRIS.Payroll │ │ HRIS.TimeManagement│ │  HRIS.Master │
│      .Model          │ │    .Model    │ │       .Model       │ │    .Model    │
└───────────┬──────────┘ └──────┬───────┘ └─────────┬──────────┘ └──────┬───────┘
            │                   │                   │                   │
            └───────────────────┼───────────────────┼───────────────────┘
                                ▼
                      ┌────────────────────────────────────────┐
                      │              HRIS.General              │
                      │    (Utilities, Encryption, Helpers)    │
                      └───────────────────┬────────────────────┘
                                          │
                                          ▼
                      ┌────────────────────────────────────────┐
                      │         Microsoft SQL Server           │
                      └────────────────────────────────────────┘
```

---

## 💻 Tech Stack

### **Backend & Framework**
- **Framework:** ASP.NET Core 2.2 (C#)
- **Architecture:** N-Tier / Clean Modular Architecture (Data Access Objects + Domain Managers)
- **Database:** Microsoft SQL Server (MSSQL)
- **Data Access:** ADO.NET / Dapper / EF Core
- **Email Service:** SendGrid API

### **Frontend & UI**
- **Template:** Sneat Responsive Bootstrap 5 Admin Dashboard
- **Frontend Tools:** Razor Views, HTML5, SCSS / CSS3, Vanilla JS & jQuery
- **Libraries:** Select2, Quill Rich Text Editor, Typeahead.js, Perfect Scrollbar, Popper.js

---

## 📁 Project Structure

```text
├── HRIS.sln                         # Solution Configuration
├── HRIS.Web/                        # Presentation Layer (MVC Web Application)
│   ├── Controllers/                 # Module-specific MVC Controllers
│   ├── Views/                       # Razor UI Views with Sneat Dashboard
│   ├── wwwroot/                     # Static Assets (Sneat UI, Plugins, Scripts)
│   ├── appsettings.json             # Environment & Connection Configuration
│   └── Startup.cs                   # App Pipeline & Dependency Injection
├── HRIS.Service/                    # Service & Business Logic Layer
│   ├── Manager/                     # Business Logic Processors
│   └── Interface/                   # Contract Definitions & DI Interfaces
├── HRIS.PersonalAdmin.Model/        # Personnel Administration Domain & DAOs
├── HRIS.TimeManagementModel/        # Attendance, Leaves & Shift Domain & DAOs
├── HRIS.Payroll.Model/              # Payroll Processing Domain & DAOs
├── HRIS.Tax.Model/                  # Tax (PPh21) Engine Domain & DAOs
├── HRIS.Master.Model/               # Master Data Domain & DAOs
├── HRIS.UserManagement.Model/       # RBAC & User Management Domain & DAOs
└── HRIS.General/                    # Cross-cutting Utilities, Enums & Security
```

---

## 🚀 Getting Started

### Prerequisites

Ensure the following tools and SDKs are installed on your environment:
- [.NET Core 2.2 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/2.2)
- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or LocalDB / SQL Express)
- [Visual Studio 2019/2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# Extension

---

### Configuration

Update your database connection string and API keys in `HRIS.Web/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DbConnection": "Server=localhost\\sqlexpress;Initial Catalog=HRIS_DB;User ID=your_user;Password=your_password;Encrypt=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    },
    "DestinationFolder": {
      "Web": "Log\\WebLog\\",
      "Service": "Log\\ServiceLog\\"
    }
  },
  "SendGrid": {
    "ApiKey": "YOUR_SENDGRID_API_KEY"
  }
}
```

---

### Installation & Execution

1. **Clone the Repository**
   ```bash
   git clone https://github.com/dendidul/hris.git
   cd hris
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the Solution**
   ```bash
   dotnet build HRIS.sln
   ```

4. **Run the Application**
   ```bash
   cd HRIS.Web
   dotnet run
   ```

5. **Access the Portal**
   Open your browser and navigate to:
   ```text
   http://localhost:5000  or  https://localhost:5001
   ```

---

## 👥 Authors & Contributors

- **Erdendi Lesmana** ([@dendidul](https://github.com/dendidul)) — *Lead Developer & Architecture*

---

<div align="center">
  <sub>Built with ❤️ for modern human capital management.</sub>
</div>
