using System;
using System.Collections.Generic;

namespace NavtechModels
{
    /// <summary>
    /// Entity config DB model
    /// </summary>
    public class EntityConfigurationModel : EntityConfigurationResponseModel
    {
        public string EntityName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    /// <summary>
    /// Response partial modal, to be inherited
    /// </summary>
    public class EntityConfigurationResponseModel
    {
        public string FieldName { get; set; }
        public bool? IsRequired { get; set; }
        public int? MaxLength { get; set; }
        public string EndPointUrl { get; set; }
    }

    /// <summary>
    /// Response and Request model
    /// </summary>
    public class EntityConfigurationRequestModel
    {
        public string EntityName { get; set; }
        public List<EntityConfigurationResponseModel> Fields { get; set; }
    }
}
