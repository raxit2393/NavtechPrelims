using System;
using System.Collections.Generic;
using System.Text;

namespace NavtechModels
{
    public class ResultResponse
    {
        public string ResponseCode { get; set; }
        public string Message { get; set; }
        public object Data{ get; set; }
    }
}
