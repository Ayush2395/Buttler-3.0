﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Buttler.Domain.Models
{
    public partial class OrderMasters
    {
        public OrderMasters()
        {
            OrderItems = new HashSet<OrderItems>();
        }

        public int OrderMasterId { get; set; }
        public DateTime? DateOfOrder { get; set; }
        public int? TablesId { get; set; }
        public int? CustomerId { get; set; }
        public string StaffId { get; set; }
        public decimal? TotalBill { get; set; }
        public int OrderStatus { get; set; }

        public virtual Customers Customer { get; set; }
        public virtual AspNetUsers Staff { get; set; }
        public virtual Tables Tables { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
    }
}