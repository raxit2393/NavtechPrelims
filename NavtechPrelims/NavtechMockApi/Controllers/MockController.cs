using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NavtechModels.Models;
using Newtonsoft.Json;

namespace NavtechMockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockController : ControllerBase
    {
        /// <summary>
        /// Mock API, source 1
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>string array</returns>
        [HttpGet]
        [Route("DefaultFields/{entity}")]
        public object GetDefaultFields(string entity)
        {
            SourceResponseModel payload = new SourceResponseModel();
            try
            {
                List<Fields> lstFields = new List<Fields>();
                for (int i = 1; i < 5; i++)
                {
                    lstFields.Add(new Fields { FieldName = "Field" + i });
                }

                payload.Fields = lstFields;
                payload.EntityName = entity;
            }
            catch (Exception)
            {
                throw;
            }
            return JsonConvert.SerializeObject(payload);
        }

        /// <summary>
        /// Mock API, source 2
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>string array</returns>
        [HttpGet]
        [Route("CustomFields/{entity}")]
        public object GetCustomFields(string entity)
        {
            SourceResponseModel payload = new SourceResponseModel();
            try
            {
                List<Fields> lstFields = new List<Fields>();
                for (int i = 1; i < 5; i++)
                {
                    lstFields.Add(new Fields { FieldName = "CField" + i });
                }

                payload.Fields = lstFields;
                payload.EntityName = entity;
            }
            catch (Exception)
            {
                throw;
            }
            return JsonConvert.SerializeObject(payload);
        }

    }
}
