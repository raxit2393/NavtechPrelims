using NavtechModels;
using System.Collections.Generic;

namespace NavtechRepository.Entity
{
    public interface IEntityRepository
    {
        /// <summary>
        /// Get entity configurations form database
        /// </summary>
        /// <returns></returns>
        List<EntityConfigurationModel> GetEntityConfiguration();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objEntityConfigurationRequestModel"></param>
        /// <returns></returns>
        object SaveEntityConfiguration(List<EntityConfigurationRequestModel> objEntityConfigurationRequestModel);
    }
}
