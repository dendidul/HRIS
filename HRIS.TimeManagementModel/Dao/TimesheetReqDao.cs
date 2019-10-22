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
    public class TimesheetReqDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public TimesheetReqDao(IConfiguration config)
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

        public IList<TimesheetReqModel> GetAllTimesheetReq()
        {
            var data = new List<TimesheetReqModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<TimesheetReqModel>("sp_Tmn_timesheet_reqsSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public TimesheetReqModel GetTimesheetReq(TimesheetReqModel model)
        {
            var data = new TimesheetReqModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<TimesheetReqModel>("sp_Tmn_timesheet_reqsSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public TimesheetReqModel CreateTimesheetReq(TimesheetReqModel model)
        {
            var data = new TimesheetReqModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                 
                    param.Add("@employee_id", model.employee_id);
                    param.Add("@transaction_date", model.transaction_date);
                    param.Add("@time_in", model.time_in);
                    param.Add("@time_out", model.time_out);
                    param.Add("@request_note", model.request_note);
                    param.Add("@request_status", model.request_status);
                    param.Add("@created_by", model.created_by);               
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@del_flag", model.del_flag);
                    param.Add("@remarks", model.remarks);


                    data = conn.Query<TimesheetReqModel>("sp_Tmn_timesheet_reqsInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public TimesheetReqModel UpdateTimesheetReq(TimesheetReqModel model)
        {
            var data = new TimesheetReqModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@employee_id", model.employee_id);
                    param.Add("@transaction_date", model.transaction_date);
                    param.Add("@time_in", model.time_in);
                    param.Add("@time_out", model.time_out);
                    param.Add("@request_note", model.request_note);
                    param.Add("@request_status", model.request_status);
                    param.Add("@modified_by", model.modified_by);
                    param.Add("@modified_date", model.modified_date);
                    param.Add("@del_flag", model.del_flag);
                    param.Add("@remarks", DateTime.Now);
                  



                    data = conn.Query<TimesheetReqModel>("sp_Tmn_timesheet_reqsUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteTimesheetReq(int id)
        {
            var data = new UnattendanceModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tmn_timesheet_reqsDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }


    }
}
