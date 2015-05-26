using System;

namespace CoreData.Models
{
    public enum ProductCategory{
        Food,
        Beveage,
        Clothing,
        Tools,
        Electronic,
        Other
    }

    public class Product
    {
        public nfloat Price { get; set;}
        public string Name { get; set;}
        public nint Quantity { get; set;}
        public ProductCategory Category { get; set;}

        public Product()
        {
        }
    }
}

