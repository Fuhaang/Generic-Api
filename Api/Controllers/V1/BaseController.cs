using Entities;
using EntitiesContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using UnitOfWork.Contract;

namespace Api.Controllers.V1
{
    public class BaseController<T> : ControllerBase where T : Entity
    {
        protected readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
        public BaseController(IUnitOfWork<ApplicationDbContext> uow) => _unitOfWork = uow;

        #region READ

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<IActionResult> Get()
        {
            return Ok(await _unitOfWork.GetRepository<T>().GetAll());
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Get([FromRoute] long id)
        {
            var result = await _unitOfWork.GetRepository<T>().GetById(id);
            if (result != null)
                return Ok(result);

            return NotFound(id);
        }
        #endregion

        #region CREATE
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<IActionResult> Create([FromBody] T entity)
        {
            if (!await _unitOfWork.GetRepository<T>().Add(entity)) return BadRequest(ModelState);

            var locationUri =
                $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}/{typeof(T)}";

            return Created(locationUri, entity);
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<IActionResult> CreateMultiple([FromBody] IEnumerable<T> entities)
        {
            if (!await _unitOfWork.GetRepository<T>().Add(entities)) return BadRequest(ModelState);


            var locationUri =
                $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}/{typeof(T)}";

            return Created(locationUri, entities);
        }
        #endregion

        #region UPDATE
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public virtual async Task<IActionResult> Delete([FromRoute] long id)
        {
            var deleted = await _unitOfWork.GetRepository<T>().Delete(id);
            if (deleted)
                return NoContent();

            return NotFound();
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public virtual async Task<IActionResult> Delete([FromBody] T entity)
        {
            var deleted = await _unitOfWork.GetRepository<T>().Delete(entity);
            if (deleted)
                return NoContent();

            return NotFound();
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public virtual async Task<IActionResult> Delete([FromBody] IEnumerable<T> entities)
        {
            var deleted = await _unitOfWork.GetRepository<T>().Delete(entities);
            if (deleted)
                return NoContent();

            return NotFound();
        }
        #endregion
    }
}
