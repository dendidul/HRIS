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
    public class UnattendanceReqDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public UnattendanceReqDao(IConfiguration config)
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


        public IList<UnattendanceReqModel> GetAllUnattendanceReq()
        {
            var data = new List<UnattendanceReqModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<UnattendanceReqModel>("sp_Tmn_unattendance_reqsSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public UnattendanceReqModel GetUnattendanceReq(UnattendanceReqModel model)
        {
            var data = new UnattendanceReqModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<UnattendanceReqModel>("sp_Tmn_unattendance_reqsSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public UnattendanceReqModel CreateUnattendanceReq(UnattendanceReqModel model)
        {
            var data = new UnattendanceReqModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@employee_id", model.employee_id);
                    param.Add("@unattendance_id", model.unattendance_id);
                    param.Add("@description", model.description);
                    param.Add("@start_date", model.start_date);
                    param.Add("@last_date", model.last_date);
                    param.Add("@request_status", model.request_status);
                    param.Add("@remarks", model.remarks);                   
                    param.Add("@created_by", model.created_by);
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@del_flag", model.del_flag);


                    data = conn.Query<UnattendanceReqModel>("sp_Tmn_unattendance_reqsInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public UnattendanceReqModel UpdateUnattendanceReq(UnattendanceReqModel model)
        {
            var data = new UnattendanceReqModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@employee_id", model.employee_id);
                    param.Add("@unattendance_id", model.unattendance_id);
                    param.Add("@description", model.description);
                    param.Add("@start_date", model.start_date);
                    param.Add("@last_date", model.last_date);
                    param.Add("@request_status", model.request_status);
                    param.Add("@remarks", model.remarks);
                  
                    param.Add("@modified_by", model.modified_by);
                    param.Add("@modified_date", DateTime.Now);
                    param.Add("@del_flag", model.del_flag);



                    data = conn.Query<UnattendanceReqModel>("sp_Tmn_unattendance_reqsUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteUnattendanceReq(int id)
        {
            var data = new UnattendanceReqModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tmn_unattendance_reqsDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }
    }
}
