﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Buttler.Domain.Models
{
    public partial class OrderItems
    {
        public int OrderItemsId { get; set; }
        public int? OrderMasterId { get; set; }
        public int? FoodsId { get; set; }
        public int? Quantity { get; set; }

        public virtual Foods Foods { get; set; }
        public virtual OrderMasters OrderMaster { get; set; }
    }
}