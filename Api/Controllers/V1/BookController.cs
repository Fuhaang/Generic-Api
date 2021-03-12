using Entities;
using EntitiesContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitOfWork.Contract;

namespace Api.Controllers.V1
{
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
        public override async Task<IActionResult> CreateMultiple([FromBody] IEnumerable<Book> entities)
        {
            return await base.CreateMultiple(entities);
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
        [HttpGet("{id}/categories")]
        public async Task<IActionResult> GetCategorieForOneBook([FromRoute] long id)
        {
            var book = await _unitOfWork.GetRepository<Book>().GetFirstOrDefault(predicate: b => b.Id == id, include: b => b.Include(b => b.Categories));
            return Ok(book.Categories);
        }

        [MapToApiVersion("1.1")]
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategorieForAllBook()
        {
            var books = await _unitOfWork.GetRepository<Book>().GetMuliple(include: b => b.Include(b => b.Categories));
            return Ok(books.ToList().Select(b => new { b.Categories, b.Id }));
        }
    }
}
