using System;
using System.Collections.Generic;

namespace Pr1_ASP.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    public int StockQuantity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsAvailable { get; set; }

    public string? ImagePath { get; set; }

    public string? ImageFileName { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
