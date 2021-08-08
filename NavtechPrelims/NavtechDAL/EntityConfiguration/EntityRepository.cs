using Dapper;
using NavtechModels;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace NavtechRepository.Entity
{
    public class EntityRepository : RepositoryBase, IEntityRepository
    {
        #region Properties
        private static readonly string sp_db_entity_configuration = "sp_db_entity_configuration";
        #endregion

        /// <summary>
        /// Get entity configurations form database
        /// </summary>
        /// <returns></returns>
        public List<EntityConfigurationModel> GetEntityConfiguration()
        {
            List<EntityConfigurationModel> _lstObj = new List<EntityConfigurationModel>();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "SELECT-CONIFG");
                {
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
        /// Insert/Update configurations
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
                    if (string.IsNullOrEmpty(entity.EntityName))
                    {
                        continue;
                    }
                    itemGroupEntityConfig.Append("<Fields>");
                    foreach (var item in entity.Fields)
                    {
                        if (string.IsNullOrEmpty(item.FieldName))
                        {
                            continue;
                        }
                        itemGroupEntityConfig.Append("<Field>");
                        itemGroupEntityConfig.Append($"<EntityName>{GetXMLSafeString(entity.EntityName)}</EntityName>");
                        itemGroupEntityConfig.Append($"<FieldName>{GetXMLSafeString(item.FieldName)}</FieldName>");
                        itemGroupEntityConfig.Append($"<EndPointUrl>{GetXMLSafeString(item.EndPointUrl)}</EndPointUrl>");
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
