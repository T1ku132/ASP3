using Microsoft.AspNetCore.Mvc;
using Pr1_ASP.Services;

namespace Pr1_ASP.Controllers
{
    // Единое место, где исключения сервисного слоя переводятся в HTTP-статусы.
    // Благодаря этому контроллеры остаются тонкими, а сервисы — свободными от HTTP.
    public abstract class ApiControllerBase : ControllerBase
    {
        protected async Task<IActionResult> HandleAsync(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (KeyNotFoundException ex)        // связанная сущность не найдена
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ConflictException ex)           // нарушено бизнес-условие (нет остатка, дубликат и т.п.)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)   // некорректные входные данные (например, неверный FK)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
