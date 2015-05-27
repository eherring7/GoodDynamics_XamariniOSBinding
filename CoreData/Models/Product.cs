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

    public class Product
    {
        private NSManagedObject _managedObject;

        public nfloat Price
        { 
            get
            {
                return (nfloat)_managedObject.ValueForKey("Price");
            }
            set
            {
                _managedObject.SetValueForKey(new NSNumber(value), new NSString("Price"));
            }
        }
        public string Name
        { 
            get
            {
                return _managedObject.ValueForKey("Name").ToString();
            }
            set
            {
                _managedObject.SetValueForKey(new NSString(value), new NSString("Name"));
            }
        }
        public nint Quantity
        { 
            get
            {
                return (nint)_managedObject.ValueForKey("Quantity").ToInt32();
            }
            set
            {
                _managedObject.SetValueForKey(new NSNumber(value), new NSString("Quantity"));
            }
        }
        public ProductCategory Category
        { 
            get
            {
                return (ProductCategory)_managedObject.ValueForKey("Category").ToInt32();
            }
            set
            {
                _managedObject.SetValueForKey(new NSNumber((int)value), new NSString("Category"));
            }
        }

        public Product(NSManagedObjectContext context)
        {
            this._managedObject = CreateNewBook(context);
        }

        public static NSString EntityName()
        {
            return new NSString("Product");
        }

        public static NSManagedObject CreateNewBook(NSManagedObjectContext context)
        {
            return (NSManagedObject) NSEntityDescription.InsertNewObjectForEntityForName(EntityName(), context);
        }
    }
}

