using Microsoft.AspNetCore.Mvc;
using Pr1_ASP.Dtos;
using Pr1_ASP.Services;

namespace Pr1_ASP.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationsController : ApiControllerBase
    {
        private readonly IReservationService _service;

        public ReservationsController(IReservationService service)
        {
            _service = service;
        }

        // GET api/reservations?page=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll(
            int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var (items, total) = await _service.GetAllAsync(page, pageSize, cancellationToken);
            Response.Headers["X-Total-Count"] = total.ToString();
            return Ok(items);
        }

        // GET api/reservations/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var item = await _service.GetByIdAsync(id, cancellationToken);
            if (item is null) return NotFound();
            return Ok(item);
        }

        // POST api/reservations — зарезервировать товар (201 / 404 нет пользователя или товара / 409 нет остатка)
        [HttpPost]
        public Task<IActionResult> Reserve(ReserveProductDto dto, CancellationToken cancellationToken) =>
            HandleAsync(async () =>
            {
                var created = await _service.ReserveAsync(dto, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = created.ReservationId }, created);
            });

        // POST api/reservations/5/release — отменить резервирование (200 / 404 нет активной записи)
        [HttpPost("{id:int}/release")]
        public async Task<IActionResult> Release(int id, CancellationToken cancellationToken)
        {
            var released = await _service.ReleaseAsync(id, cancellationToken);
            if (released is null) return NotFound(new { error = "Активное резервирование не найдено." });
            return Ok(released);
        }

        // DELETE api/reservations/5 — удалить уже закрытую запись (204 / 404 / 409 если запись активна)
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
