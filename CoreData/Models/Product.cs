using System;
using Foundation;

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

    public class Product:NSManagedObject
    {
        public nfloat Price { get; set;}
        public string Name { get; set;}
        public nint Quantity { get; set;}
        public ProductCategory Category { get; set;}

        public Product(NSEntityDescription entity, NSManagedObjectContext context):base(entity, context)
        {
        }

        public static NSString EntityName()
        {
            return new NSString("Product");
        }

        public static Product CreateNewBook(NSManagedObjectContext context)
        {
            return (Product) NSEntityDescription.InsertNewObjectForEntityForName(EntityName(), context);
        }
    }
}

