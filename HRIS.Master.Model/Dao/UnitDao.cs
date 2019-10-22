using Dapper;
using HRIS.General.Model.Master;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HRIS.Master.Model.Dao
{
    public class UnitDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public UnitDao(IConfiguration config)
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

        public IList<UnitModel> GetAllUnit()
        {
            var data = new List<UnitModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<UnitModel>("sp_Mst_unitsSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public UnitModel GetUnit(UnitModel model)
        {
            var data = new UnitModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<UnitModel>("sp_Mst_unitsSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public UnitModel CreateUnit(UnitModel model)
        {
            var data = new UnitModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@unit_id", model.unit_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@created_by", model.created_by);
                    param.Add("@created_date", model.created_date);
                    param.Add("@position_id", model.position_id);
                    param.Add("@begin_date", DateTime.Now);
                    param.Add("@end_date", model.end_date);
                    param.Add("@del_flag", model.del_flag);
                    param.Add("@cost_center_id", model.cost_center_id);
                    param.Add("@head_unit_id", model.head_unit_id);
                    param.Add("@TRIAL291", "T");


                    data = conn.Query<UnitModel>("sp_Mst_unitsInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public UnitModel UpdateUnit(UnitModel model)
        {
            var data = new UnitModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@unit_id", model.unit_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@modified_by", model.modified_by);
                    param.Add("@modified_date", DateTime.Now);
                    param.Add("@position_id", model.position_id);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("del_flag", model.del_flag);
                    param.Add("@cost_center_id", model.cost_center_id);
                    param.Add("@head_unit_id", model.head_unit_id);
                    param.Add("@TRIAL291", "T");


                    data = conn.Query<UnitModel>("sp_Mst_unitsUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteUnit(int id)
        {
            var data = new UnitModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Mst_unitsDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }


    }
}
