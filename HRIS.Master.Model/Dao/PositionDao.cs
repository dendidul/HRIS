using Dapper;
using HRIS.General.Utility;
using HRIS.General.Model.Master;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HRIS.Master.Model.Dao
{
    public class PositionDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public PositionDao(IConfiguration config)
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

        public IList<PositionModel> GetAllPosition()
        {
            var data = new List<PositionModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<PositionModel>("sp_Mst_positionsSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public PositionModel GetPosition(PositionModel model)
        {
            var data = new PositionModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<PositionModel>("sp_Mst_positionsSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public PositionModel CreatePosition(PositionModel model)
        {
            var data = new PositionModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@position_id", model.position_id);
                    param.Add("@short_description", model.short_description);
                    param.Add("@long_description", model.long_description);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@created_by", model.created_by);
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@del_flag", model.del_flag);
                    param.Add("@TRIAL284", "T");


                    data = conn.Query<PositionModel>("sp_Mst_positionsInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public PositionModel UpdatePosition(PositionModel model)
        {
            var data = new PositionModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@position_id", model.position_id);
                    param.Add("@short_description", model.short_description);
                    param.Add("@long_description", model.long_description);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@modified_by", model.modified_by);
                    param.Add("@modified_date", DateTime.Now);
                    param.Add("@del_flag", model.del_flag);
                    param.Add("@TRIAL284", "T");


                    data = conn.Query<PositionModel>("sp_Mst_positionsUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeletePosition(int id)
        {
            var data = new PositionModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Mst_positionsDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }



    }
}
