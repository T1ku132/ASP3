using Microsoft.AspNetCore.Mvc;
using Pr1_ASP.Dtos;
using Pr1_ASP.Services;

namespace Pr1_ASP.Controllers
{
    // Тонкий контроллер: вызвал сервис -> перевёл результат в HTTP-статус.
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ApiControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        // GET api/categories?page=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll(
            int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var (items, total) = await _service.GetAllAsync(page, pageSize, cancellationToken);
            Response.Headers["X-Total-Count"] = total.ToString();
            return Ok(items);
        }

        // GET api/categories/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var item = await _service.GetByIdAsync(id, cancellationToken);
            if (item is null) return NotFound();
            return Ok(item);
        }

        // POST api/categories
        [HttpPost]
        public Task<IActionResult> Create(CreateCategoryDto dto, CancellationToken cancellationToken) =>
            HandleAsync(async () =>
            {
                var created = await _service.CreateAsync(dto, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = created.CategoryId }, created);
            });

        // PUT api/categories/5
        [HttpPut("{id:int}")]
        public Task<IActionResult> Update(int id, UpdateCategoryDto dto, CancellationToken cancellationToken) =>
            HandleAsync(async () =>
            {
                var updated = await _service.UpdateAsync(id, dto, cancellationToken);
                if (updated is null) return NotFound();
                return Ok(updated);
            });

        // PATCH api/categories/5
        [HttpPatch("{id:int}")]
        public Task<IActionResult> Patch(int id, PatchCategoryDto dto, CancellationToken cancellationToken) =>
            HandleAsync(async () =>
            {
                var patched = await _service.PatchAsync(id, dto, cancellationToken);
                if (patched is null) return NotFound();
                return Ok(patched);
            });

        // DELETE api/categories/5
        [HttpDelete("{id:int}")]
        public Task<IActionResult> Delete(int id, CancellationToken cancellationToken) =>
            HandleAsync(async () =>
            {
                var deleted = await _service.DeleteAsync(id, cancellationToken);
                if (!deleted) return NotFound();
                return NoContent();
            });
    }
}
