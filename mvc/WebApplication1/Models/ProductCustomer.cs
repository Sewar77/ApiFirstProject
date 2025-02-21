using WebApplication1.Models;
using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class ProductCustomers
{
    public decimal ProductCustomerId { get; set; }

    public decimal? ProductId { get; set; }

    public decimal? CustomerId { get; set; }

    public decimal? Quantity { get; set; }

    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Product? Product { get; set; }
}
