using System;
using System.Collections.Generic;

namespace Labb10northwind.Models
{
    public partial class OrderSubtotal
    {
        public int OrderId { get; set; }
        public decimal? Subtotal { get; set; }
    }
}
