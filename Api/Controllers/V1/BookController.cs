using Api.Controllers.Contract;
using Entities;
using EntitiesContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitOfWork.Contract;

namespace Api.Controllers.V1
{
    /// <summary>
    /// Sample for show how use BaseController<TEntity>
    /// </summary>
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [Route("api/books")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class BookController : BaseController<Book>
    {
        public BookController(IUnitOfWork<ApplicationDbContext> uow) : base(uow) { }

        [HttpGet]
        public override async Task<IActionResult> Get()
        {
            return await base.Get();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> Get([FromRoute] long id)
        {
            return await base.Get(id);
        }

        [HttpPost]
        public override async Task<IActionResult> CreateMultipleOrOne([FromBody] IEnumerable<Book> entities)
        {
            return await base.CreateMultipleOrOne(entities);
        }

        [HttpPut]
        public override async Task<IActionResult> Update([FromBody] IEnumerable<Book> entities)
        {
            return await base.Update(entities);
        }

        [HttpDelete]
        public override async Task<IActionResult> Delete([FromBody] IEnumerable<Book> entities)
        {
            return await base.Delete(entities);
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete([FromRoute] long id)
        {
            return await base.Delete(id);
        }

        [MapToApiVersion("1.1")]
        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategoriesForAllBooks()
        {
            return await base.GetAndInclude(b => b.Include(b => b.Categories));
        }

        [MapToApiVersion("1.1")]
        [HttpGet("{id}/Categories")]
        public async Task<IActionResult> GetCategoriesForOneBooks([FromRoute] long id)
        {
            return await base.GetAndInclude(id, b => b.Include(b => b.Categories));
        }

    }
}
