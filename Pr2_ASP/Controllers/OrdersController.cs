using Microsoft.AspNetCore.Mvc;
using Pr1_ASP.Dtos;
using Pr1_ASP.Services;

namespace Pr1_ASP.Controllers
{
    // Тонкий контроллер: вызвал сервис -> перевёл результат в HTTP-статус.
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ApiControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        // GET api/orders?page=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll(
            int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var (items, total) = await _service.GetAllAsync(page, pageSize, cancellationToken);
            Response.Headers["X-Total-Count"] = total.ToString();
            return Ok(items);
        }

        // GET api/orders/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var item = await _service.GetByIdAsync(id, cancellationToken);
            if (item is null) return NotFound();
            return Ok(item);
        }

        // POST api/orders
        [HttpPost]
        public Task<IActionResult> Create(CreateOrderDto dto, CancellationToken cancellationToken) =>
            HandleAsync(async () =>
            {
                var created = await _service.CreateAsync(dto, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = created.OrderId }, created);
            });

        // PUT api/orders/5
        [HttpPut("{id:int}")]
        public Task<IActionResult> Update(int id, UpdateOrderDto dto, CancellationToken cancellationToken) =>
            HandleAsync(async () =>
            {
                var updated = await _service.UpdateAsync(id, dto, cancellationToken);
                if (updated is null) return NotFound();
                return Ok(updated);
            });

        // PATCH api/orders/5
        [HttpPatch("{id:int}")]
        public Task<IActionResult> Patch(int id, PatchOrderDto dto, CancellationToken cancellationToken) =>
            HandleAsync(async () =>
            {
                var patched = await _service.PatchAsync(id, dto, cancellationToken);
                if (patched is null) return NotFound();
                return Ok(patched);
            });

        // DELETE api/orders/5
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
