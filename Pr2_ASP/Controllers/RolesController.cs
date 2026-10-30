using Microsoft.AspNetCore.Mvc;
using Pr1_ASP.Dtos;
using Pr1_ASP.Services;

namespace Pr1_ASP.Controllers
{
    // Тонкий контроллер: вызвал сервис -> перевёл результат в HTTP-статус.
    [ApiController]
    [Route("api/roles")]
    public class RolesController : ApiControllerBase
    {
        private readonly IRoleService _service;

        public RolesController(IRoleService service)
        {
            _service = service;
        }

        // GET api/roles?page=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll(
            int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var (items, total) = await _service.GetAllAsync(page, pageSize, cancellationToken);
            Response.Headers["X-Total-Count"] = total.ToString();
            return Ok(items);
        }

        // GET api/roles/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var item = await _service.GetByIdAsync(id, cancellationToken);
            if (item is null) return NotFound();
            return Ok(item);
        }

        // POST api/roles
        [HttpPost]
        public Task<IActionResult> Create(CreateRoleDto dto, CancellationToken cancellationToken) =>
            HandleAsync(async () =>
            {
                var created = await _service.CreateAsync(dto, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = created.RoleId }, created);
            });

        // PUT api/roles/5
        [HttpPut("{id:int}")]
        public Task<IActionResult> Update(int id, UpdateRoleDto dto, CancellationToken cancellationToken) =>
            HandleAsync(async () =>
            {
                var updated = await _service.UpdateAsync(id, dto, cancellationToken);
                if (updated is null) return NotFound();
                return Ok(updated);
            });

        // PATCH api/roles/5
        [HttpPatch("{id:int}")]
        public Task<IActionResult> Patch(int id, PatchRoleDto dto, CancellationToken cancellationToken) =>
            HandleAsync(async () =>
            {
                var patched = await _service.PatchAsync(id, dto, cancellationToken);
                if (patched is null) return NotFound();
                return Ok(patched);
            });

        // DELETE api/roles/5
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
