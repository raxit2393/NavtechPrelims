using System;
using System.Collections.Generic;
using System.Text;

namespace NavtechModels.Models
{
    public class SourceResponseModel
    {
        public SourceResponseModel()
        {
            Fields = new List<Fields>();
        }
        public string EntityName { get; set; }
        public List<Fields> Fields { get; set; }
    }

    public class Fields
    {
        public string FieldName { get; set; }
    }

}
