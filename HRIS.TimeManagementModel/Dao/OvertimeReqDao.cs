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
    public class OvertimeReqDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public OvertimeReqDao(IConfiguration config)
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

        public IList<OvertimeRequestModel> GetAllOvertimeRequest()
        {
            var data = new List<OvertimeRequestModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<OvertimeRequestModel>("sp_Tmn_overtime_requestsSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public OvertimeRequestModel GetOvertimeRequest(OvertimeRequestModel model)
        {
            var data = new OvertimeRequestModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<OvertimeRequestModel>("sp_Tmn_overtime_requestsSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public OvertimeRequestModel CreateOvertimeRequest(OvertimeRequestModel model)
        {
            var data = new OvertimeRequestModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@employee_id", model.employee_id);
                    param.Add("@unit_id", model.unit_id);
                    param.Add("@description", model.description);
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@created_by", model.created_by);
                    param.Add("@request_status", model.request_status);
              
                    param.Add("@del_flag", model.del_flag);


                    data = conn.Query<OvertimeRequestModel>("sp_Tmn_overtime_requestsInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public OvertimeRequestModel UpdateOvertimeRequest(OvertimeRequestModel model)
        {
            var data = new OvertimeRequestModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@doc_number", model.doc_number);
                    param.Add("@employee_id", model.employee_id);
                    param.Add("@unit_id", model.unit_id);
                    param.Add("@description", model.description);
                    param.Add("@request_status", model.request_status);                
                    param.Add("@modified_date", DateTime.Now);
                    param.Add("@modified_by", model.modified_by);
                    param.Add("@del_flag", model.del_flag);



                    data = conn.Query<OvertimeRequestModel>("sp_Tmn_overtime_requestsUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteOvertimeRequest(int id)
        {
            var data = new OvertimeRequestModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tmn_overtime_requestsDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }

    }
}
