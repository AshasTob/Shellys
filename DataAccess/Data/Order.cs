using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Data
{
    public class Order
    {
        public int Id { get; set; }
        public double TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
    }

    public enum OrderStatus
    {
        Initiated = 1,
        Finalized
    }
}
