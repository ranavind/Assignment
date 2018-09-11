using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Interfaces
{
    /// <summary>
    /// Interface for all CRUD operations of a service
    /// </summary>
    public interface ICurdService
    {
        /// <summary>
        /// Create an Entity from a View-Model in repository (CRUD).
        /// </summary>
        /// <param name="viewModel">Valid View Model</param>
        /// <returns>
        /// A tuple of StatusCode and object.
        /// StatusCode = Created; object = instance of the created View-Model
        /// StatusCode = BadRequest, object = error message string
        /// </returns>
        (HttpStatusCode status, object obj) Create(object viewModel);

        /// <summary>
        /// Read an Entity in repository (CRUD) for a given Entity Id
        /// </summary>
        /// <param name="id">Valid Guid as string for an Entity</param>
        /// <returns>
        /// A tuple of StatusCode and object.
        /// StatusCode = OK; object = instance of the View-Model
        /// StatusCode = NotFound, object = error message string
        /// </returns>
        (HttpStatusCode status, object obj) Read(string id);

        /// <summary>
        /// Update an Entity in repository (CRUD) for a given View-Model.
        /// </summary>
        /// <param name="viewModel">Valid View Model</param>
        /// <returns>
        /// A tuple of StatusCode and object.
        /// StatusCode = OK; object = instance of the View-Model
        /// StatusCode = NotFound, object = error message string
        /// StatusCode = BadRequest, object = error message string
        /// </returns>
        (HttpStatusCode status, object obj) Update(object viewModel);


        /// <summary>
        /// Delete an Entity in repository (CRUD) for a given Entity Id
        /// </summary>
        /// <param name="id">Valid Guid as string for an Entity</param>
        /// <returns>
        /// A tuple of StatusCode and object.
        /// StatusCode = OK; object = Entity Id
        /// StatusCode = NotFound, object = error message string
        /// </returns>
        (HttpStatusCode status, object obj) Delete(string id);

    }
}
