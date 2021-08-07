using Microsoft.AspNetCore.Mvc;
using NavtechModels;
using NavtechModels.Models;
using NavtechPrelims.Utils;
using NavtechRepository.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace NavtechPrelims.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntityController : ControllerBase
    {


        #region Property
        private readonly IEntityRepository _entityRepository;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        #endregion

        #region Constructor
        /// <summary>
        ///  Constructot
        /// </summary>
        /// <param name="_entityRepository"></param>
        public EntityController(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
        #endregion

        #region GET

        [HttpGet]
        [Route("get-weather")]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        ////GetEntity
        //[HttpGet]
        //[Route("get-entity-configuration")]
        //public object GetEntity()
        //{
        //    ResultResponse objResultResponse = new ResultResponse();

        //    try
        //    {
        //        var lstConfigurations = _entityRepository.GetEntityConfiguration();
        //        objResultResponse.ResponseCode = HttpStatusCode.OK.ToString();
        //        objResultResponse.Message = "Configuration fetched successfully.";
        //        objResultResponse.Data = lstConfigurations;
        //    }
        //    catch (Exception ex)
        //    {
        //        objResultResponse.ResponseCode = HttpStatusCode.BadRequest.ToString();
        //        objResultResponse.Message = ex.Message;
        //        objResultResponse.Data = null;
        //    }
        //    return objResultResponse;
        //}

        /// <summary>
        /// Get Mock data adn return final entity
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Route("get-entity-configuration-url")]
        public object GetEntityFromURL()
        {
            ResultResponse objResultResponse = new ResultResponse();
            try
            {
                var responseStringSource1 = string.Empty;
                var responseStringSource2 = string.Empty;
                //Get response from different sources
                try
                {
                    string Source1 = "https://localhost:44371/api/Mock/DefaultFields/Product";
                    responseStringSource1 = ApiCall.GetApi(Source1);
                }
                catch { }

                try
                {
                    string Source2 = "https://localhost:44371/api/Mock/CustomFields/ent";
                    responseStringSource2 = ApiCall.GetApi(Source2);
                }
                catch { }

                //Deserialize the responses
                var lstFieldsSource1 = JsonConvert.DeserializeObject<SourceResponseModel>(responseStringSource1);
                var lstFieldsSource2 = JsonConvert.DeserializeObject<SourceResponseModel>(responseStringSource2);

                //Append responses
                List<SourceResponseModel> lstResponse = new List<SourceResponseModel>();
                if (lstFieldsSource1 != null)
                {
                    lstResponse.Add(lstFieldsSource1);
                }
                if (lstFieldsSource2 != null)
                {
                    lstResponse.Add(lstFieldsSource2);
                }
                var lstDbCongigurations = new List<EntityConfigurationModel>();
                try
                {
                    lstDbCongigurations = _entityRepository.GetEntityConfiguration();
                }
                catch { }

                List<ResponseBodyModel> lstConfigurations = new List<ResponseBodyModel>();
                lstConfigurations = GetContextResponse(lstDbCongigurations, lstResponse);

                objResultResponse.ResponseCode = HttpStatusCode.OK.ToString();
                objResultResponse.Message = "Configuration fetched successfully.";
                objResultResponse.Data = lstConfigurations;
            }
            catch (Exception ex)
            {
                objResultResponse.ResponseCode = HttpStatusCode.BadRequest.ToString();
                objResultResponse.Message = ex.Message;
                objResultResponse.Data = null;
            }
            return objResultResponse;

        }
        #endregion

        #region POST
        /// <summary>
        ///  Add/Update an SCOrder
        /// </summary>
        /// <param name="objSCorders"></param>
        /// <returns></returns>
        [HttpPost]
        //[Route("save-entity-configuration")]
        public object SaveSCOrders([FromBody] List<EntityConfigurationRequestModel> objEntityConfigurationRequestModel)
        {
            ResultResponse objResultResponse = new ResultResponse();
            try
            {
                var objResponse = _entityRepository.SaveEntityConfiguration(objEntityConfigurationRequestModel);
                if (objResponse != null)
                {

                    objResultResponse.ResponseCode = HttpStatusCode.OK.ToString();
                    objResultResponse.Message = "Configuration saved successfully.";
                    objResultResponse.Data = objResponse;
                }
            }
            catch (Exception ex)
            {
                objResultResponse.ResponseCode = HttpStatusCode.BadRequest.ToString();
                objResultResponse.Message = ex.Message;
                objResultResponse.Data = null;
            }
            return objResultResponse;
        }
        #endregion

        #region Utility
        /// <summary>
        /// Get the final response body object
        /// </summary>
        /// <param name="lstDbCongigurations"></param>
        /// <param name="lstResponse"></param>
        /// <param name="objContextResponse"></param>
        /// <returns></returns>        
        private List<ResponseBodyModel> GetContextResponse(List<EntityConfigurationModel> lstDbCongigurations, List<SourceResponseModel> lstResponse)
        {
            List<ResponseBodyModel> ContextResponse = new List<ResponseBodyModel>();

            try
            {
                List<string> lstEntityName = new List<string>();
                List<EntityConfigurationResponseModel> lstFilteredFields = new List<EntityConfigurationResponseModel>();
                lstEntityName.AddRange(lstDbCongigurations.Select(z => z.EntityName).Distinct());
                lstEntityName.AddRange(lstResponse.Select(z => z.EntityName).Distinct());

                foreach (var entity in lstEntityName.Distinct())
                {
                    lstFilteredFields = new List<EntityConfigurationResponseModel>();
                    var tempSourceFields = new List<Fields>();
                    lstFilteredFields.AddRange(lstDbCongigurations.Where(z => z.EntityName == entity)
                        .Select(x => new EntityConfigurationResponseModel()
                        {
                            FieldName = x.FieldName,
                            IsRequired = x.IsRequired,
                            MaxLength = x.MaxLength,
                            EndPointUrl = x.EndPointUrl
                        }).ToList());

                    //filter all the fields from response
                    var lsFields = lstResponse.Where(z => z.EntityName == entity).Select(z => z.Fields).ToList();

                    var lstLookUp = lstDbCongigurations.Select(z => z.FieldName).ToList().Distinct();

                    //for each list of fileds form a final list of fields
                    foreach (var list in lsFields)
                    {
                        var lstVLookUp = new List<Fields>();
                        foreach (var lst in list)
                        {
                            if (!lstLookUp.Contains(lst.FieldName))
                            {
                                lstVLookUp.Add(lst);
                            }
                        }
                        lstFilteredFields.AddRange(lstVLookUp
                           .Select(x => new EntityConfigurationResponseModel()
                           {
                               FieldName = x.FieldName
                           }).ToList());
                    }

                    ContextResponse.Add(new ResponseBodyModel { EntityName = entity, Fields = lstFilteredFields });
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }
            return ContextResponse;
        }
        #endregion
    }
}
