using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalExam.Models
{
    public class OrderProduct
    {
        public List<Product> Product { get; set; }
        public List<Order> Order { get; set; }
    }
}