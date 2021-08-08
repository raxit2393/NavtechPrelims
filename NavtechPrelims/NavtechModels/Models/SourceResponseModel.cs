using System.Collections.Generic;

namespace NavtechModels.Models
{
    public class SourceResponseModel
    {
        /// <summary>
        /// Payload response body
        /// </summary>
        public SourceResponseModel()
        {
            Fields = new List<Fields>();
        }
        public string EntityName { get; set; }
        public List<Fields> Fields { get; set; }
    }

    /// <summary>
    /// Class to get and set the fields
    /// </summary>
    public class Fields
    {
        public string FieldName { get; set; }
        public string EndPointUrl { get; set; }

    }

}
