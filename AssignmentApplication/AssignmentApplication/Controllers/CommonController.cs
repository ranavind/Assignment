using CoreLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Runtime.CompilerServices;

using Newtonsoft.Json;

namespace AssignmentApplication.Controllers
{
    public class CommonController : ApiController
    {
        #region private fields

        private readonly ICurdService _crudService;
        #endregion  private fields

        /// <summary>
        /// Initializes a new instance of the CommonController class 
        /// Web API controller base class
        /// </summary>
        /// <param name="crudService">CRUD service provides access to a particular Repository</param>
       
        public CommonController(ICurdService crudService)
        {
            _crudService = crudService;
         
        }

        /// <summary>
        /// Creates an Entity from a given ViewModel
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        protected IHttpActionResult Create(object viewModel)
        {
            try
            {


                var result = _crudService.Create(viewModel);
                if ((HttpStatusCode)result.status == HttpStatusCode.OK)
                {
                    var id = result.obj.GetType().GetProperty("Id")?.GetValue(result.obj) as string;
                  
                    return Created(Request.RequestUri + "/" + id, result.obj);
                }
                if ((HttpStatusCode)result.status == HttpStatusCode.BadRequest)
                {
                   
                    return BadRequest();
                }                
             
                return InternalServerError();
            }
            catch (Exception e)
            {
           
                throw;
            }

        }

        /// <summary>
        /// Read an Entity for a given Entity ID
        /// </summary>
        /// <param name="id">Valid Guid as string for an Entity</param>
        /// <returns>
        /// HTTP 200 (OK), particular View Model in JSON
        /// HTTP 400 (Bad Request)
        /// HTTP 404 (Not Found)
        /// HTTP 500 (Internal Server Error)
        /// </returns>
        protected IHttpActionResult Read(string id)
        {
            try
            {
                //if (!IsValidId(id, EntityName))
                //{
                //    return BadRequest();
                //}
                var result = _crudService.Read(id);
                if((HttpStatusCode) result.status == HttpStatusCode.OK)
                {
                   
                    return Ok(result.obj);
                }
                if ((HttpStatusCode)result.status == HttpStatusCode.NotFound)
                {
                  
                    return NotFound();
                }
             
                return InternalServerError();
            }
            catch (Exception e)
            {
               
                throw;
            }
        }

        /// <summary>
        /// Update an Entity from a given ViewModel
        /// </summary>
        /// <param name="id">Valid Guid as string for an Entity</param>
        /// <param name="viewModel">Valid View Model</param>
        /// <returns>
        /// HTTP 200 (OK), View Model in JSON
        /// HTTP 400 (Bad Request)
        /// HTTP 404 (Not Found)
        /// HTTP 500 (Internal Server Error)
        /// </returns>
        protected IHttpActionResult Update(object viewModel)
        {
            try
            {
                if (viewModel == null)
                {
                  
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                   
                    return BadRequest();
                }
                var result = _crudService.Update(viewModel);
                if ((HttpStatusCode)result.status == HttpStatusCode.OK)
                {
                
                    return Ok(result.obj);
                }
                if ((HttpStatusCode)result.status == HttpStatusCode.BadRequest)
                {
                 
                    return BadRequest();
                }
                if ((HttpStatusCode)result.status == HttpStatusCode.NotFound)
                {
                   
                    return NotFound();
                }
                
                return InternalServerError();
            }
            catch (Exception e)
            {
               
                throw;
            }
        }

        /// <summary>
        /// Delete an Entity for a given Entity ID
        /// </summary>
        /// <param name="id">Valid Guid as string for an Entity</param>
        /// <returns>
        /// HTTP 204 (No Content)
        /// HTTP 400 (Bad Request)
        /// HTTP 404 (Not Found)
        /// HTTP 500 (Internal Server Error)
        /// </returns>
        protected IHttpActionResult DoDelete(string id)
        {
            try
            {
                //if (!IsValidId(id, EntityName))
                //{
                //    return BadRequest();
                //}
                var result = _crudService.Delete(id);
                if ((HttpStatusCode)result.status == HttpStatusCode.OK)
                {
                  
                    return StatusCode(HttpStatusCode.NoContent);
                }
                if ((HttpStatusCode)result.status == HttpStatusCode.NotFound)
                {
                    
                    return NotFound();
                }
               
                return InternalServerError();
            }
            catch (Exception e)
            {
             
                throw;
            }
        }

    }
}
