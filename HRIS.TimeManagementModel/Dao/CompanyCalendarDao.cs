using Dapper;
using HRIS.General.Model.TimeManagement;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HRIS.TimeManagement.Model.Dao
{
    public class CompanyCalendarDao
    {

        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public CompanyCalendarDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DbConnection"));
            }
        }

        public IList<CompanyCalendarModel> GetAllCompanyCalendar()
        {
            var data = new List<CompanyCalendarModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<CompanyCalendarModel>("sp_Tmn_company_calendarsSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public CompanyCalendarModel GetCompanyCalendar(CompanyCalendarModel model)
        {
            var data = new CompanyCalendarModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<CompanyCalendarModel>("sp_Tmn_company_calendarsSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public CompanyCalendarModel CreateCompanyCalendar(CompanyCalendarModel model)
        {
            var data = new CompanyCalendarModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@personal_area_id", model.personal_area_id);
                    param.Add("@workgroup_id", model.workgroup_id);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@unattendance_id", model.unattendance_id);
                    param.Add("@remark", model.remark);
                    param.Add("@created_by", model.created_by);
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@del_flag", model.del_flag);


                    data = conn.Query<CompanyCalendarModel>("sp_Tmn_company_calendarsInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public CompanyCalendarModel UpdateCompanyCalendar(CompanyCalendarModel model)
        {
            var data = new CompanyCalendarModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@personal_area_id", model.personal_area_id);
                    param.Add("@workgroup_id", model.workgroup_id);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@unattendance_id", model.unattendance_id);
                    param.Add("@remark", model.remark);
                    param.Add("@modified_by",model.modified_by);
                    param.Add("@modified_date", DateTime.Now);
                    param.Add("@del_flag", model.del_flag);



                    data = conn.Query<CompanyCalendarModel>("sp_Tmn_company_calendarsUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteCompanyCalendar(int id)
        {
            var data = new CompanyCalendarModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tmn_company_calendarsDelete", param,
                            commandType: CommandType.StoredProcedure);

            }


        }

    }
}
