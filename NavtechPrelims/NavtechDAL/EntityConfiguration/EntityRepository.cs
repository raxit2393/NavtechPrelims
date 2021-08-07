using Dapper;
using NavtechModels;
using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NavtechRepository.Entity
{
    public class EntityRepository : RepositoryBase, IEntityRepository
    {
        //private static readonly string ConnectionString = "Data Source=Liquid;Initial Catalog=NavtechPrelims;Integrated Security=True";
        private static readonly string sp_db_entity_configuration = "sp_db_entity_configuration";

        /// <summary>
        /// Get entity configurations form database
        /// </summary>
        /// <returns></returns>
        public List<EntityConfigurationModel> GetEntityConfiguration()
        {
            //var resultData = new Dictionary<string, object>();
            List<EntityConfigurationModel> _lstObj = new List<EntityConfigurationModel>();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "SELECT-CONIFG");
                {
                    //using (IDbConnection db = new SqlConnection(ConnectionString))
                    //{
                    //    var response = db.QueryMultiple("sp_db_entity_configuration", parameters, commandType: CommandType.StoredProcedure);
                    //    resultData.Add("EntityConfiguration", response.Read<EntityConfigurationModel>().ToList());

                    _lstObj = ExecuteList<EntityConfigurationModel>(sp_db_entity_configuration, parameters, true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _lstObj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objEntityConfigurationRequestModel"></param>
        /// <returns></returns>
        public object SaveEntityConfiguration(List<EntityConfigurationRequestModel> objEntityConfigurationRequestModel)
        {
            try
            {
                //Prepare XMl string for Invoice multiple items
                var parameters = new DynamicParameters();
                StringBuilder itemGroupEntityConfig = new StringBuilder();
                parameters.Add("@Operation", "SAVE-CONIFG");

                foreach (var entity in objEntityConfigurationRequestModel)
                {
                    itemGroupEntityConfig.Append("<Fields>");
                    foreach (var item in entity.Fields)
                    {
                        itemGroupEntityConfig.Append("<Field>");
                        itemGroupEntityConfig.Append($"<EntityName>{GetXMLSafeString(entity.EntityName)}</EntityName>");
                        itemGroupEntityConfig.Append($"<FieldName>{GetXMLSafeString(item.FieldName)}</FieldName>");
                        itemGroupEntityConfig.Append($"<IsRequired>{item.IsRequired}</IsRequired>");
                        itemGroupEntityConfig.Append($"<MaxLength>{item.MaxLength}</MaxLength>");
                        itemGroupEntityConfig.Append("</Field>");
                    }
                    itemGroupEntityConfig.Append("</Fields>");
                }
                parameters.Add("@XMLConfig", itemGroupEntityConfig.ToString());

                var result = Execute(sp_db_entity_configuration, parameters, true);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string GetXMLSafeString(string input)
        {
            return (input ?? string.Empty).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }
    }
}
