using System;
using System.Collections.Generic;

namespace StockManagementSystem.Models;

public partial class Product
{
    public short PId { get; set; }

    public string PName { get; set; } = null!;

    public int PPrice { get; set; }

    public string Pimage { get; set; } = null!;

    public int SaleQuantity { get; set; }

    public int PurchaseOuantity { get; set; }

    public int Stock { get; set; }

    public short? CategoryId { get; set; }

    public short? SupplierId { get; set; }

    public virtual PodCategory? Category { get; set; }

    public virtual Supplier? Supplier { get; set; }
}
