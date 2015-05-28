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
                var num = (NSNumber)ObjCRuntime.Runtime.GetNSObject(_managedObject.ValueForKey("Price"));
                return num.NFloatValue;
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
                return (NSString)ObjCRuntime.Runtime.GetNSObject(_managedObject.ValueForKey("Name"));
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
                var num = (NSNumber)ObjCRuntime.Runtime.GetNSObject(_managedObject.ValueForKey("Quantity"));
                return num.NIntValue;
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
                var num = (NSNumber)ObjCRuntime.Runtime.GetNSObject(_managedObject.ValueForKey("Category"));
                return (ProductCategory) num.Int32Value;
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

