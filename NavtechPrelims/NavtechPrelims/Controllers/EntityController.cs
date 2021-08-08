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
        /// <summary>
        /// Get Mock data adn return final entity
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetEntityConfiguration(bool isAlternateApproach)
        {
            ResultResponse objResultResponse = new ResultResponse();
            try
            {
                //Get configuration form various sources
                List<SourceResponseModel> lstResponse = GetConfigurationsFromSouurce();

                //If alternate approach then save the source config to db and then fetch
                if (isAlternateApproach)
                {
                    List<EntityConfigurationRequestModel> objEntityConfigurationRequestModel = new List<EntityConfigurationRequestModel>();

                    objEntityConfigurationRequestModel = lstResponse.Select(z => new EntityConfigurationRequestModel
                    {
                        EntityName = z.EntityName,
                        Fields = z.Fields.Select(z => new EntityConfigurationResponseModel
                        {
                            FieldName = z.FieldName,
                            EndPointUrl = z.EndPointUrl,
                            IsRequired = false,
                            MaxLength = 0
                        }).ToList()
                    }).ToList();

                    var response = SaveEntityConfigurations(objEntityConfigurationRequestModel);
                }

                //Get configurations from database
                var lstDbCongigurations = new List<EntityConfigurationModel>();
                try
                {
                    lstDbCongigurations = _entityRepository.GetEntityConfiguration();
                }
                catch { }

                List<EntityConfigurationRequestModel> lstConfigurations = new List<EntityConfigurationRequestModel>();

                //If alternate approach then reset the source response 
                lstConfigurations = GetContextResponse(lstDbCongigurations, isAlternateApproach ? new List<SourceResponseModel>() : lstResponse);

                objResultResponse.ResponseCode = HttpStatusCode.OK.ToString();
                objResultResponse.Message = "Configurations fetched successfully.";
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
        ///  Insert/Update configurations
        /// </summary>
        /// <param name="objEntityConfigurationRequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        //[Route("save-entity-configuration")]
        public object SaveEntityConfigurations([FromBody] List<EntityConfigurationRequestModel> objEntityConfigurationRequestModel)
        {
            ResultResponse objResultResponse = new ResultResponse();
            try
            {
                var objResponse = _entityRepository.SaveEntityConfiguration(objEntityConfigurationRequestModel);
                if (objResponse != null)
                {
                    objResultResponse.ResponseCode = HttpStatusCode.OK.ToString();
                    objResultResponse.Message = "Configurations saved successfully.";
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
        private List<EntityConfigurationRequestModel> GetContextResponse(List<EntityConfigurationModel> lstDbCongigurations, List<SourceResponseModel> lstResponse)
        {
            List<EntityConfigurationRequestModel> ContextResponse = new List<EntityConfigurationRequestModel>();

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

                    ContextResponse.Add(new EntityConfigurationRequestModel { EntityName = entity, Fields = lstFilteredFields });
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

        /// <summary>
        /// Fetch and merge response from differnet sources
        /// </summary>
        /// <returns></returns>
        private List<SourceResponseModel> GetConfigurationsFromSouurce()
        {
            List<SourceResponseModel> lstResponse = new List<SourceResponseModel>();
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
                if (lstFieldsSource1 != null)
                {
                    lstResponse.Add(lstFieldsSource1);
                }
                if (lstFieldsSource2 != null)
                {
                    lstResponse.Add(lstFieldsSource2);
                }
            }
            catch
            {
                throw;
            }
            return lstResponse;
        }
        #endregion
    }
}
