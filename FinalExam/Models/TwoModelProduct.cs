using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalExam.Models
{
    public class TwoModelProduct
    {
        public Product Product { get; set; }
        public List<ProductImage> ProductImage { get; set; }
    }
}