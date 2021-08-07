using System;
using System.Collections.Generic;

namespace NavtechModels
{
    public class EntityConfigurationModel : EntityConfigurationResponseModel
    {
        public string EntityName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class EntityConfigurationResponseModel
    {
        public string FieldName { get; set; }
        public bool? IsRequired { get; set; }
        public int? MaxLength { get; set; }
        public string EndPointUrl { get; set; }
    }

    public class EntityConfigurationRequestModel
    {
        public string EntityName { get; set; }
        public List<EntityConfigurationResponseModel> Fields { get; set; }
    }

    public class ResponseBodyModel
    {
        public ResponseBodyModel()
        {
            Fields = new List<EntityConfigurationResponseModel>();
        }
        public string EntityName { get; set; }
        public List<EntityConfigurationResponseModel> Fields { get; set; }
    }
}
