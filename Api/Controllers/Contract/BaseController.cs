using Entities;
using EntitiesContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using UnitOfWork.Contract;

namespace Api.Controllers.Contract
{

    public class BaseController<T> : ControllerBase where T : Entity
    {
        /// <summary>
        /// read the unit of work
        /// </summary>
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
        public BaseController(IUnitOfWork<ApplicationDbContext> uow) => _unitOfWork = uow;

        #region READ
        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>Task<IActionResult> with StatusCodes 200 OK</returns>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<IActionResult> Get()
        {
            return Ok(await _unitOfWork.GetRepository<T>().GetAll());
        }

        /// <summary>
        /// Gets the entities based on a children inclusions.
        /// </summary>
        /// <param name="include">A function to include navigation properties</param>
        /// <returns>Task<IActionResult> with StatusCodes 200 OK</returns>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<IActionResult> GetAndInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
        {
            return Ok(await _unitOfWork.GetRepository<T>().GetMuliple(include: include));
        }

        /// <summary>
        /// Finds an entity with the given primary id.
        /// </summary>
        /// <param name="id">The values of the primary key.</param>
        /// <returns>Task<IActionResult> with StatusCodes 200 OK OR 404 NOT FOUND</returns>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Get([FromRoute] long id)
        {
            var result = await _unitOfWork.GetRepository<T>().GetFirstOrDefault(t => t.Id == id);

            if (result != null)
                return Ok(result);

            return NotFound(id);
        }

        /// <summary>
        /// Finds an entity with the given primary id.
        /// </summary>
        /// <param name="id">The values of the primary key.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <returns>Task<IActionResult> with StatusCodes 200 OK OR 404 NOT FOUND</returns>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> GetAndInclude([FromRoute] long id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
        {
            var result = await _unitOfWork.GetRepository<T>().GetFirstOrDefault(t => t.Id == id, include: include);

            if (result != null)
                return Ok(result);

            return NotFound(id);
        }

        #endregion

        #region CREATE

        /// <summary>
        /// Inserts a range of entities or a new entity.
        /// </summary>
        /// <param name="entities">entities or entity to add</param>
        /// <returns>Task<IActionResult> with StatusCodes 201 Created OR 400 BadRequest</returns>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<IActionResult> CreateMultipleOrOne([FromBody] IEnumerable<T> entities)
        {
            if (!await _unitOfWork.GetRepository<T>().Add(entities)) return BadRequest(ModelState);

            string controller = $"{typeof(T)}s";
            controller = controller.Replace("Entities.", "");

            return Created("", entities.ToList().Select(e => $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}/{controller}/{e.Id}"));
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task<IActionResult> with StatusCodes 200 OK OR 404 NOT FOUND</returns>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Update([FromBody] T entity)
        {
            var updated = await _unitOfWork.GetRepository<T>().Update(entity);
            if (updated)
                return Ok();

            return NotFound();
        }

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>Task<IActionResult> with StatusCodes 200 OK OR 404 NOT FOUND</returns>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Update([FromBody] IEnumerable<T> entities)
        {
            var updated = await _unitOfWork.GetRepository<T>().Update(entities);
            if (updated)
                return Ok();

            return NotFound();
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Deletes the entity by the specified primary key.
        /// </summary>
        /// <param name="id">The primary key value.</param>
        /// <returns>Task<IActionResult> with StatusCodes 204 NO CONTENT OR 404 NOT FOUND</returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public virtual async Task<IActionResult> Delete([FromRoute] long id)
        {
            var deleted = await _unitOfWork.GetRepository<T>().Delete(id);
            if (deleted)
                return NoContent();

            return NotFound();
        }

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        /// <returns>Task<IActionResult> with StatusCodes 204 NO CONTENT OR 404 NOT FOUND</returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public virtual async Task<IActionResult> Delete([FromBody] IEnumerable<T> entities)
        {
            var entitiesToDelete = await _unitOfWork.GetRepository<T>().GetMuliple(t => entities.ToList().Contains(t));
            if (entitiesToDelete.Count() == entities.Count())
            {
                var deleted = await _unitOfWork.GetRepository<T>().Delete(entities);
                if (deleted)
                    return NoContent();
            }

            return NotFound();
        }
        #endregion
    }
}
