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
   public class UnattendanceConfigDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public UnattendanceConfigDao(IConfiguration config)
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

        public IList<UnattendanceModel> GetAllUnattendanceConfig()
        {
            var data = new List<UnattendanceModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<UnattendanceModel>("sp_Tmn_unattendancesSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public UnattendanceModel GetUnattendanceConfig(UnattendanceModel model)
        {
            var data = new UnattendanceModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<UnattendanceModel>("sp_Tmn_unattendancesSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public UnattendanceModel CreateUnattendanceConfig(UnattendanceModel model)
        {
            var data = new UnattendanceModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@unattendance_id", model.unattendance_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@un_attendance_period", model.un_attendance_period);
                    param.Add("@un_attendance_unit", model.un_attendance_unit);
                    param.Add("@un_attendance_quota", model.un_attendance_quota);
                    param.Add("@amount_of_increment", model.amount_of_increment);
                    param.Add("@increment_period", model.increment_period);
                    param.Add("@created_by", model.created_by);
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@del_flag", model.del_flag);


                    data = conn.Query<UnattendanceModel>("sp_Tmn_unattendancesInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public UnattendanceModel UpdateUnattendanceConfig(UnattendanceModel model)
        {
            var data = new UnattendanceModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@unattendance_id", model.unattendance_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@un_attendance_period", model.increment_period);
                    param.Add("@un_attendance_unit", model.un_attendance_unit);
                    param.Add("@un_attendance_quota", model.un_attendance_quota);
                    param.Add("@amount_of_increment", model.amount_of_increment);
                    param.Add("@increment_period", model.increment_period);
                    param.Add("@modified_by", model.modified_by);
                    param.Add("@modified_date", DateTime.Now);
                    param.Add("@del_flag", model.del_flag);



                    data = conn.Query<UnattendanceModel>("sp_Tmn_unattendancesUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteUnattendanceConfig(int id)
        {
            var data = new UnattendanceModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tmn_unattendancesDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }



    }
}
