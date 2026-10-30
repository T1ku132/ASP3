using System;
using System.Collections.Generic;

namespace Pr1_ASP.Models;

// Факт бизнес-операции: КТО (User) зарезервировал ЧТО (Product), сколько и КОГДА.
// ReleasedAt == null — резервирование активно; иначе — отменено (остаток возвращён).
public partial class Reservation
{
    public int ReservationId { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime ReservedAt { get; set; }

    public DateTime? ReleasedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
