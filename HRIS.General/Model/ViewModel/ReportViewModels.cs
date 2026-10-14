using System;
using System.Collections.Generic;
using HRIS.General.Model.Payroll;

namespace HRIS.General.Model.ViewModel
{
    public class PayrollCalculationDetailHeaderModel
    {
        public int? PayrollCalculationId { get; set; }
        public int? StatusEmployeeId { get; set; }
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {MiddleName} {LastName}".Replace("  ", " ").Trim();
        public string Unit { get; set; }
        public DateTime? JoinDate { get; set; }
        public string WorkGroup { get; set; }
        public DateTime? TerminateDate { get; set; }

        public IEnumerable<PayrollCalculationDetailContainModel> PayrollCalculationDetailContain { get; set; }
        public IEnumerable<PayComponentModel> PayrollComponent { get; set; }
    }

    public class PayrollCalculationDetailContainModel
    {
        public int PayrollCalculationId { get; set; }
        public int? StatusEmployeeId { get; set; }
        public int? PayrollComponentId { get; set; }
        public string ComponentName { get; set; }
        public string Transaction { get; set; }
        public decimal? PayrollComponentAmount { get; set; }
        public decimal? PayrollComponent { get; set; }
    }

    public class PayrollCalculationDetailViewModel
    {
        public IEnumerable<PayrollCalculationDetailHeaderModel> PayrollCalculationDetailHeader { get; set; }
        public IEnumerable<PayrollComponentColumnModel> PayrollComponent { get; set; }
        public IEnumerable<PayrollComponentTotalModel> payrollComponentTotal { get; set; }
    }

    public class PayrollComponentColumnModel
    {
        public int? ComponentId { get; set; }
        public string ComponentName { get; set; }
    }

    public class PayrollComponentTotalModel
    {
        public int? ComponentId { get; set; }
        public decimal? ComponentTotalAmount { get; set; }
    }

    public class ReportJamsostekPerubahanUpahViewModel
    {
        public int rn { get; set; }
        public string nomor_induk_karyawan { get; set; }
        public string nomor_peserta { get; set; }
        public string membership_no { get; set; }
        public string nama { get; set; }
        public string company { get; set; }
        public DateTime? begindate { get; set; }
        public string tanggal_lahir { get; set; }
        public string jenis_kelamin { get; set; }
        public decimal gaji_bulan_ini { get; set; }
        public decimal gaji_bulan_kemarin { get; set; }
        public decimal selisih_gaji { get; set; }
    }

    public class ReportJamsostek2aViewModel
    {
        public string no_kpj { get; set; }
        public string nik { get; set; }
        public string name { get; set; }
        public string birth_date { get; set; }
        public decimal data_upah { get; set; }
        public decimal jkk { get; set; }
        public decimal jkm { get; set; }
        public decimal jpk { get; set; }
        public decimal jht_tk { get; set; }
        public decimal jht_per { get; set; }
        public decimal total_iuran => jkk + jkm + jpk + jht_tk + jht_per;
    }
}
