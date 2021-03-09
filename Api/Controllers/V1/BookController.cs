using Entities;
using EntitiesContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitOfWork.Contract;

namespace Api.Controllers.V1
{
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class BookController : BaseController<Book>
    {
        public BookController(IUnitOfWork<ApplicationDbContext> uow) : base(uow) { }

        [HttpGet("api/v1/books")]
        public override async Task<IActionResult> Get()
        {
            return await base.Get();
        }

        [HttpGet("api/v1/book/{id}")]
        public override async Task<IActionResult> Get([FromRoute] long id)
        {
            return await base.Get(id);
        }

        [HttpPost("api/v1/book")]
        public override async Task<IActionResult> Create([FromBody] Book entity)
        {
            return await base.Create(entity);
        }

        [HttpPost("api/v1/books")]
        public override async Task<IActionResult> CreateMultiple([FromBody] IEnumerable<Book> entities)
        {
            return await base.CreateMultiple(entities);
        }

        [HttpPut("api/v1/book")]
        public override async Task<IActionResult> Update([FromBody] Book entity)
        {
            return await base.Update(entity);
        }

        [HttpPut("api/v1/books")]
        public override async Task<IActionResult> Update([FromBody] IEnumerable<Book> entities)
        {
            return await base.Update(entities);
        }

        [HttpDelete("api/v1/book/{id}")]
        public override async Task<IActionResult> Delete([FromRoute] long id)
        {
            return await base.Delete(id);
        }

        [HttpDelete("api/v1/book")]
        public override async Task<IActionResult> Delete([FromBody] Book entity)
        {
            return await base.Delete(entity);
        }

        [HttpDelete("api/v1/books")]
        public override async Task<IActionResult> Delete([FromBody] IEnumerable<Book> entities)
        {
            return await base.Delete(entities);
        }
    }
}
