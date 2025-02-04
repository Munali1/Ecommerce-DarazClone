using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
    {
        public class Order
        {
            [Key]
            public int Id { get; set; }

            public int Product_ID { get; set; }
            public int Customer_ID { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice { get; set; }
            public string ShippingAddress { get; set; }
            public DateTime OrderDate { get; set; }

            [ForeignKey("Product_ID")]
            public Product Products { get; set; }

            [ForeignKey("Customer_ID")]
            public Customer Customers { get; set; }
        }
    }


