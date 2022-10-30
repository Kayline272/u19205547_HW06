using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using u19205547_HW06.Models;

namespace u19205547_HW06.Models
{
    public class ProductOrderJoin
    {
        public product ProductDetails { get; set; }
        public order_items OrderItemDetails { get; set; }
        public order OrderDetails { get; set; }

   
    }
}