-- ============================================================================
-- HRIS Database Migration Script: Convert DATE Columns to TIMESTAMP (DATETIME)
-- Database Engine: PostgreSQL
-- Target: Fully Compatible with C# DateTime & Npgsql
-- ============================================================================

-- ----------------------------------------------------------------------------
-- 1. SETUP ALIAS DOMAIN 'datetime' DI POSTGRESQL
-- ----------------------------------------------------------------------------
-- PostgreSQL secara native tidak memiliki tipe keyword 'datetime' bawaan.
-- Padanan resmi PostgreSQL untuk C# DateTime adalah TIMESTAMP / TIMESTAMP WITHOUT TIME ZONE.
-- Blok di bawah ini mendaftarkan 'datetime' sebagai DOMAIN resmi di PostgreSQL
-- sehingga jika ada DDL/query yang memakai keyword 'datetime', PostgreSQL tidak akan error.

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'datetime') THEN
        CREATE DOMAIN datetime AS timestamp without time zone;
        RAISE NOTICE 'Domain datetime created successfully.';
    END IF;
END
$$;

-- ----------------------------------------------------------------------------
-- 2. OPSI A (SANGAT DIREKOMENDASIKAN): OTOMATIS CONVERT SEMUA TABEL bos_*
-- ----------------------------------------------------------------------------
-- Blok PL/pgSQL dinamis ini akan memindai database PostgreSQL Anda secara live,
-- mencari semua kolom bertipe 'date' pada tabel bos_*, dan mengubahnya
-- menjadi 'timestamp without time zone' secara aman tanpa kehilangan data.

DO $$
DECLARE
    r RECORD;
    cnt INT := 0;
BEGIN
    FOR r IN 
        SELECT table_name, column_name 
        FROM information_schema.columns 
        WHERE table_schema = 'public' 
          AND data_type = 'date'
          AND table_name LIKE 'bos_%'
        ORDER BY table_name, column_name
    LOOP
        EXECUTE format('ALTER TABLE %I ALTER COLUMN %I TYPE timestamp without time zone USING %I::timestamp without time zone;', 
                       r.table_name, r.column_name, r.column_name);
        cnt := cnt + 1;
        RAISE NOTICE 'Migrated: %.% to timestamp without time zone', r.table_name, r.column_name;
    END LOOP;
    RAISE NOTICE 'Selesai! Total % kolom berhasil diubah ke timestamp without time zone.', cnt;
END
$$;

-- ----------------------------------------------------------------------------
-- 3. OPSI B: EXPLICIT DDL PER TABEL & PER KOLOM (182 KOLOM)
-- ----------------------------------------------------------------------------
-- Jika Anda ingin mengeksekusi DDL per tabel secara eksplisit atau manual:

-- ============================================================================
-- HRIS Database Migration Script: Convert DATE Columns to TIMESTAMP (DATETIME)
-- Database Engine: PostgreSQL
-- Target: Compatible with C# DateTime & Npgsql
-- ============================================================================

-- Catatan Penting untuk PostgreSQL:
-- PostgreSQL tidak memiliki keyword native 'datetime' bawaan (seperti SQL Server / MySQL).
-- Padanan resmi dan 100% kompatibel di PostgreSQL untuk C# DateTime adalah:
-- 'TIMESTAMP' atau 'TIMESTAMP WITHOUT TIME ZONE'.
--
-- Untuk kemudahan, kita juga membuat alias DOMAIN 'datetime' di PostgreSQL 
-- agar skrip yang merujuk pada tipe 'datetime' dapat diterima oleh PostgreSQL.

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'datetime') THEN
        CREATE DOMAIN datetime AS timestamp without time zone;
    END IF;
END
$$;

-- ========================================================
-- Table: bos_areas
-- ========================================================
ALTER TABLE bos_areas ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_areas ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_areas ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;
ALTER TABLE bos_areas ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;

-- ========================================================
-- Table: bos_bank_sources
-- ========================================================
ALTER TABLE bos_bank_sources ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_bank_sources ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_companies
-- ========================================================
ALTER TABLE bos_companies ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_companies ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_companies ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_companies ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_company_calendars
-- ========================================================
ALTER TABLE bos_company_calendars ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_company_calendars ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;
ALTER TABLE bos_company_calendars ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_company_calendars ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;

-- ========================================================
-- Table: bos_cost_centers
-- ========================================================
ALTER TABLE bos_cost_centers ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_cost_centers ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;

-- ========================================================
-- Table: bos_educations
-- ========================================================
ALTER TABLE bos_educations ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_educations ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;
ALTER TABLE bos_educations ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_educations ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;

-- ========================================================
-- Table: bos_employee_family
-- ========================================================
ALTER TABLE bos_employee_family ALTER COLUMN birthdate TYPE timestamp without time zone USING birthdate::timestamp without time zone;

-- ========================================================
-- Table: bos_employee_grades
-- ========================================================
ALTER TABLE bos_employee_grades ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_employee_grades ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_employee_grades ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_employee_grades ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_employee_memberships
-- ========================================================
ALTER TABLE bos_employee_memberships ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_employee_memberships ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_employee_memberships ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_employee_memberships ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_employee_overtimes
-- ========================================================
ALTER TABLE bos_employee_overtimes ALTER COLUMN transaction_date TYPE timestamp without time zone USING transaction_date::timestamp without time zone;
ALTER TABLE bos_employee_overtimes ALTER COLUMN updated_date TYPE timestamp without time zone USING updated_date::timestamp without time zone;

-- ========================================================
-- Table: bos_employee_pay_attributes
-- ========================================================
ALTER TABLE bos_employee_pay_attributes ALTER COLUMN end_contract TYPE timestamp without time zone USING end_contract::timestamp without time zone;
ALTER TABLE bos_employee_pay_attributes ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_employee_pay_attributes ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;
ALTER TABLE bos_employee_pay_attributes ALTER COLUMN hire_date TYPE timestamp without time zone USING hire_date::timestamp without time zone;
ALTER TABLE bos_employee_pay_attributes ALTER COLUMN terminate_date TYPE timestamp without time zone USING terminate_date::timestamp without time zone;

-- ========================================================
-- Table: bos_employee_pay_attributes_histories
-- ========================================================
ALTER TABLE bos_employee_pay_attributes_histories ALTER COLUMN end_contract TYPE timestamp without time zone USING end_contract::timestamp without time zone;
ALTER TABLE bos_employee_pay_attributes_histories ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_employee_pay_attributes_histories ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;
ALTER TABLE bos_employee_pay_attributes_histories ALTER COLUMN hire_date TYPE timestamp without time zone USING hire_date::timestamp without time zone;
ALTER TABLE bos_employee_pay_attributes_histories ALTER COLUMN terminate_date TYPE timestamp without time zone USING terminate_date::timestamp without time zone;

-- ========================================================
-- Table: bos_employee_quota
-- ========================================================
ALTER TABLE bos_employee_quota ALTER COLUMN begdt TYPE timestamp without time zone USING begdt::timestamp without time zone;
ALTER TABLE bos_employee_quota ALTER COLUMN enddt TYPE timestamp without time zone USING enddt::timestamp without time zone;
ALTER TABLE bos_employee_quota ALTER COLUMN moddt TYPE timestamp without time zone USING moddt::timestamp without time zone;
ALTER TABLE bos_employee_quota ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;

-- ========================================================
-- Table: bos_employee_taxes
-- ========================================================
ALTER TABLE bos_employee_taxes ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_employee_taxes ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_employee_taxes ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_employee_taxes ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_employee_times
-- ========================================================
ALTER TABLE bos_employee_times ALTER COLUMN starting_date TYPE timestamp without time zone USING starting_date::timestamp without time zone;
ALTER TABLE bos_employee_times ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_employee_times ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_employee_times ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_employee_times ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_jobs
-- ========================================================
ALTER TABLE bos_jobs ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_jobs ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_jobs ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_jobs ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_level_grades
-- ========================================================
ALTER TABLE bos_level_grades ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_level_grades ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;
ALTER TABLE bos_level_grades ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_level_grades ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;

-- ========================================================
-- Table: bos_lookups
-- ========================================================
ALTER TABLE bos_lookups ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_lookups ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_membership_components
-- ========================================================
ALTER TABLE bos_membership_components ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_membership_components ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_membership_components ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_membership_components ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_memberships
-- ========================================================
ALTER TABLE bos_memberships ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_memberships ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_memberships ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_memberships ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_overtime_components
-- ========================================================
ALTER TABLE bos_overtime_components ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_overtime_components ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_overtime_components ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_overtime_requests
-- ========================================================
ALTER TABLE bos_overtime_requests ALTER COLUMN overtime_date TYPE timestamp without time zone USING overtime_date::timestamp without time zone;

-- ========================================================
-- Table: bos_pay_group_comps
-- ========================================================
ALTER TABLE bos_pay_group_comps ALTER COLUMN begdt TYPE timestamp without time zone USING begdt::timestamp without time zone;
ALTER TABLE bos_pay_group_comps ALTER COLUMN enddt TYPE timestamp without time zone USING enddt::timestamp without time zone;
ALTER TABLE bos_pay_group_comps ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_pay_group_comps ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_pay_onetime_trans
-- ========================================================
ALTER TABLE bos_pay_onetime_trans ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_pay_onetime_trans ALTER COLUMN modifed_date TYPE timestamp without time zone USING modifed_date::timestamp without time zone;
ALTER TABLE bos_pay_onetime_trans ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_pay_onetime_trans ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;

-- ========================================================
-- Table: bos_pay_periode_dates
-- ========================================================
ALTER TABLE bos_pay_periode_dates ALTER COLUMN begin_period_date TYPE timestamp without time zone USING begin_period_date::timestamp without time zone;
ALTER TABLE bos_pay_periode_dates ALTER COLUMN end_period_date TYPE timestamp without time zone USING end_period_date::timestamp without time zone;
ALTER TABLE bos_pay_periode_dates ALTER COLUMN time_period_begin TYPE timestamp without time zone USING time_period_begin::timestamp without time zone;
ALTER TABLE bos_pay_periode_dates ALTER COLUMN time_period_end TYPE timestamp without time zone USING time_period_end::timestamp without time zone;

-- ========================================================
-- Table: bos_pay_proc_periods
-- ========================================================
ALTER TABLE bos_pay_proc_periods ALTER COLUMN created TYPE timestamp without time zone USING created::timestamp without time zone;
ALTER TABLE bos_pay_proc_periods ALTER COLUMN modified TYPE timestamp without time zone USING modified::timestamp without time zone;

-- ========================================================
-- Table: bos_pay_recurring_comps
-- ========================================================
ALTER TABLE bos_pay_recurring_comps ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_pay_recurring_comps ALTER COLUMN updated_date TYPE timestamp without time zone USING updated_date::timestamp without time zone;
ALTER TABLE bos_pay_recurring_comps ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_pay_recurring_comps ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;

-- ========================================================
-- Table: bos_payroll_areas
-- ========================================================
ALTER TABLE bos_payroll_areas ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_payroll_areas ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_payroll_areas ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_payroll_areas ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_payroll_calculations
-- ========================================================
ALTER TABLE bos_payroll_calculations ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_payroll_calculations ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_payroll_calculations ALTER COLUMN running_date TYPE timestamp without time zone USING running_date::timestamp without time zone;

-- ========================================================
-- Table: bos_payroll_components
-- ========================================================
ALTER TABLE bos_payroll_components ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_payroll_components ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_payroll_components ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_payroll_components ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_payroll_groups
-- ========================================================
ALTER TABLE bos_payroll_groups ALTER COLUMN begdt TYPE timestamp without time zone USING begdt::timestamp without time zone;
ALTER TABLE bos_payroll_groups ALTER COLUMN enddt TYPE timestamp without time zone USING enddt::timestamp without time zone;
ALTER TABLE bos_payroll_groups ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_payroll_groups ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_payroll_periodes
-- ========================================================
ALTER TABLE bos_payroll_periodes ALTER COLUMN max_period TYPE timestamp without time zone USING max_period::timestamp without time zone;
ALTER TABLE bos_payroll_periodes ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_payroll_periodes ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_payroll_periodes ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_payroll_periodes ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_payroll_process_components
-- ========================================================
ALTER TABLE bos_payroll_process_components ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_payroll_process_components ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;

-- ========================================================
-- Table: bos_payroll_processes
-- ========================================================
ALTER TABLE bos_payroll_processes ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_payroll_processes ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;

-- ========================================================
-- Table: bos_people
-- ========================================================
ALTER TABLE bos_people ALTER COLUMN birth_date TYPE timestamp without time zone USING birth_date::timestamp without time zone;
ALTER TABLE bos_people ALTER COLUMN die_date TYPE timestamp without time zone USING die_date::timestamp without time zone;
ALTER TABLE bos_people ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_people ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_positions
-- ========================================================
ALTER TABLE bos_positions ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_positions ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_positions ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_positions ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_proportionals
-- ========================================================
ALTER TABLE bos_proportionals ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_proportionals ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_roles
-- ========================================================
ALTER TABLE bos_roles ALTER COLUMN created TYPE timestamp without time zone USING created::timestamp without time zone;
ALTER TABLE bos_roles ALTER COLUMN modified TYPE timestamp without time zone USING modified::timestamp without time zone;

-- ========================================================
-- Table: bos_schedule_variant_details
-- ========================================================
ALTER TABLE bos_schedule_variant_details ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_schedule_variant_details ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_schedule_variants
-- ========================================================
ALTER TABLE bos_schedule_variants ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_schedule_variants ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_setting_templates
-- ========================================================
ALTER TABLE bos_setting_templates ALTER COLUMN begdt TYPE timestamp without time zone USING begdt::timestamp without time zone;
ALTER TABLE bos_setting_templates ALTER COLUMN enddt TYPE timestamp without time zone USING enddt::timestamp without time zone;
ALTER TABLE bos_setting_templates ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_setting_templates ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_setting_time_templates
-- ========================================================
ALTER TABLE bos_setting_time_templates ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_setting_time_templates ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_setting_time_templates ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_setting_time_templates ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_status_employees
-- ========================================================
ALTER TABLE bos_status_employees ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_status_employees ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_status_employees ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_status_employees ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_tax_parameters
-- ========================================================
ALTER TABLE bos_tax_parameters ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_tax_parameters ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;
ALTER TABLE bos_tax_parameters ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_tax_parameters ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;

-- ========================================================
-- Table: bos_tax_percentages
-- ========================================================
ALTER TABLE bos_tax_percentages ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_tax_percentages ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_taxline_components
-- ========================================================
ALTER TABLE bos_taxline_components ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_taxline_components ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_temp_pay_calcs
-- ========================================================
ALTER TABLE bos_temp_pay_calcs ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_temp_pay_calcs ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;

-- ========================================================
-- Table: bos_time_calculations
-- ========================================================
ALTER TABLE bos_time_calculations ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_time_calculations ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;

-- ========================================================
-- Table: bos_time_frames
-- ========================================================
ALTER TABLE bos_time_frames ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_time_frames ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_time_unit_details
-- ========================================================
ALTER TABLE bos_time_unit_details ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_time_unit_details ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_time_units
-- ========================================================
ALTER TABLE bos_time_units ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_time_units ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_timesheet_reqs
-- ========================================================
ALTER TABLE bos_timesheet_reqs ALTER COLUMN transaction_date TYPE timestamp without time zone USING transaction_date::timestamp without time zone;
ALTER TABLE bos_timesheet_reqs ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_timesheet_reqs ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_timesheets
-- ========================================================
ALTER TABLE bos_timesheets ALTER COLUMN transaction_date TYPE timestamp without time zone USING transaction_date::timestamp without time zone;

-- ========================================================
-- Table: bos_unattendance_reqs
-- ========================================================
ALTER TABLE bos_unattendance_reqs ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_unattendance_reqs ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_unattendances
-- ========================================================
ALTER TABLE bos_unattendances ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_unattendances ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

-- ========================================================
-- Table: bos_units
-- ========================================================
ALTER TABLE bos_units ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_units ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;
ALTER TABLE bos_units ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_units ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;

-- ========================================================
-- Table: bos_users
-- ========================================================
ALTER TABLE bos_users ALTER COLUMN created TYPE timestamp without time zone USING created::timestamp without time zone;
ALTER TABLE bos_users ALTER COLUMN modified TYPE timestamp without time zone USING modified::timestamp without time zone;

-- ========================================================
-- Table: bos_workgroups
-- ========================================================
ALTER TABLE bos_workgroups ALTER COLUMN begin_date TYPE timestamp without time zone USING begin_date::timestamp without time zone;
ALTER TABLE bos_workgroups ALTER COLUMN end_date TYPE timestamp without time zone USING end_date::timestamp without time zone;
ALTER TABLE bos_workgroups ALTER COLUMN created_date TYPE timestamp without time zone USING created_date::timestamp without time zone;
ALTER TABLE bos_workgroups ALTER COLUMN modified_date TYPE timestamp without time zone USING modified_date::timestamp without time zone;

