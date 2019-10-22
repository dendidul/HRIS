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
    public class CostCenterDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public CostCenterDao(IConfiguration config)
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

        public IList<CostCenterModel> GetAllCostCenter()
        {
            var data = new List<CostCenterModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);

                


                    data = conn.Query<CostCenterModel>("sp_Mst_cost_centersSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public CostCenterModel GetCostCenter(CostCenterModel model)
        {
            var data = new CostCenterModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<CostCenterModel>("sp_Mst_cost_centersSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public CostCenterModel CreateCostCenter(CostCenterModel model)
        {
            var data = new CostCenterModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@cost_center_code", model.cost_center_code);
                    param.Add("@description", model.description);                  
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@del_flag", model.del_flag);
                    param.Add("@TRIAL271", "T");


                    data = conn.Query<CostCenterModel>("sp_Mst_cost_centersInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public CostCenterModel UpdateCostCenter(CostCenterModel model)
        {
            var data = new CostCenterModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@cost_center_code", model.cost_center_code);
                    param.Add("@description", model.description);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@del_flag", model.del_flag);
                    param.Add("@TRIAL271", "T");
                  


                    data = conn.Query<CostCenterModel>("sp_Mst_cost_centersUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteCostCenter(int id)
        {
            var data = new CostCenterModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Mst_cost_centersDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }

    }
}
