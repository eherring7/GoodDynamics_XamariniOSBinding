
using System;

using Foundation;
using UIKit;
using CoreData.Sources;
using System.Collections.Generic;
using CoreData.Models;
using CoreData.Delegates;
using GoodDynamics;

namespace CoreData.Views
{
    public partial class ProductsView : UIViewController
    {
        #region Accessors
        private NSPersistentStoreCoordinator _storeCoordinator;
        private NSManagedObjectModel _managedObjectModel;
        private NSManagedObjectContext _context;

        IList<Product> Products { get; set;}


        NSPersistentStoreCoordinator StoreCoordinator
        {
            get
            {
                // Returns the persistent store coordinator (GDPersistentStoreCoordinator) for the application.
                // If the coordinator doesn't already exist, it is created and the application's store added to it.
                if (_storeCoordinator == null)
                {
                    _storeCoordinator = new GDPersistentStoreCoordinator(ManagedObjectModel);
                    NSError error;
                    var path = NSBundle.PathForResourceAbsolute("initialSeedData", "sqlite", NSBundle.MainBundle.ResourcePath);
                    var bundleUrl = NSUrl.CreateFileUrl(new string[]{ path });

                    NSString storeType = null;
                    var storeURL = ApplicationDocumentsDirectory().Append("CoreDataProduct.sqlite", false);
                    var exists = NSFileManager.DefaultManager.FileExists(storeURL.Path);
                    if (!exists)
                    {
                        storeURL = NSUrl.FromString("CoreDataProduct.sqlite");
                        exists = GDFileSystem.FileExistsAtPath(storeURL.Path, false);
                        storeType = GDPersistentStoreCoordinator.GDEncryptedIncrementalStoreType;
                    }
                    _storeCoordinator.PerformAndWait(() => UpdateDatabaseWithPersistantStoreCoordinator(_storeCoordinator,
                            storeType, storeURL, bundleUrl, exists));
                }
                return _storeCoordinator;
            }
        }

        NSManagedObjectModel ManagedObjectModel
        {
            get{
                if (_managedObjectModel == null)
                {

                    var name = new NSAttributeDescription()
                    {
                        Name = "Name",
                        AttributeType = NSAttributeType.String,
                        Optional = false
                    };

                    var price = new NSAttributeDescription()
                    {
                        Name = "Price",
                        AttributeType = NSAttributeType.Double,
                        Optional = false
                    };

                    var quantity = new NSAttributeDescription()
                    {
                        Name = "Quantity",
                        AttributeType = NSAttributeType.Integer32,
                        Optional = false
                    };
                    var category = new NSAttributeDescription()
                    {
                        Name = "Category",
                        AttributeType = NSAttributeType.Integer32,
                        Optional = false
                    };

                    var product = new NSEntityDescription()
                    {
                        Name = "Product",
                        Properties = new NSPropertyDescription[] { name, price, quantity, category }
                    };
                    product.ManagedObjectClassName = Product.EntityName();

                    _managedObjectModel = new NSManagedObjectModel()
                    {
                        Entities = new NSEntityDescription[] { product }
                    };
                }
                return _managedObjectModel;
            }
        }

        NSManagedObjectContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new NSManagedObjectContext()
                    {
                        PersistentStoreCoordinator = StoreCoordinator
                    };
                }
                return _context;
            }
        }
            
        #endregion

        public ProductsView(): base("ProductsView", null)
        {
            Products = new List<Product>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            goodImage.Image = new UIImage("images/GDLogo");
            goodImage.ContentMode = UIViewContentMode.ScaleAspectFit;
            FetchCoreData();
            var source = new ProductTableViewSource(Products);
            source.OnRowSelected += (s, e) => ToProductEditMode(e.Product, false);
            productsTable.Source = source;

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add, (s,e) => {
                var p = new Product(Context);
                Products.Add(p);
                ToProductEditMode(p, true);
            });

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            productsTable.ReloadData();
        }

        private void ToProductEditMode(Product p, bool isNew)
        {
            var newProductView = new EditProductView(p, Context, isNew);
            NavigationController.PushViewController(newProductView, true);
        }

        #region CoreData Methods

        private void FetchCoreData()
        {
            NSError error = null;
            NSFetchRequest request = new NSFetchRequest("Product");
            NSObject[] results = Context.ExecuteFetchRequest(request, out error);
            Products = new List<Product>();
            foreach (var r in results)
            {
                Products.Add(new Product((NSManagedObject)r));
            }
            productsTable.ReloadData();
            //Context.ExecuteFetchRequest(FetchController.FetchRequest, out error);
        }

        private void UpdateDatabaseWithPersistantStoreCoordinator(NSPersistentStoreCoordinator persistentStoreCoordinator,
            NSString storeType, NSUrl storeURL, NSUrl bundleUrl, bool DatabaseExists)
        {
            var keys = new NSObject[]{NSPersistentStoreCoordinator.MigratePersistentStoresAutomaticallyOption, NSPersistentStoreCoordinator.InferMappingModelAutomaticallyOption};
            var objects = new NSObject[]{ NSNumber.FromBoolean(true), NSNumber.FromBoolean(true) };
            NSDictionary options = NSDictionary.FromObjectsAndKeys(objects, keys);

            NSError error = null;
            if (!DatabaseExists)
            {
                persistentStoreCoordinator.AddPersistentStoreWithType(NSPersistentStoreCoordinator.SQLiteStoreType, null, bundleUrl, null, out error);

                if (error != null)
                {
                    throw new Exception(error.ToString());
                }
                error = null;
                persistentStoreCoordinator.MigratePersistentStore(persistentStoreCoordinator.PersistentStoreForUrl(bundleUrl),storeURL,null,storeType, out error);
                if (error != null)
                {
                    throw new Exception(error.ToString());
                }
            }
            else
            {
                error = null;
                persistentStoreCoordinator.AddPersistentStoreWithType(storeType, null, storeURL, null, out error);
                if (error != null)
                {
                    throw new Exception(error.ToString());
                }
            }
        }

        // Returns the URL to the application's Documents directory.
        public NSUrl ApplicationDocumentsDirectory()
        {
            return NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory,NSSearchPathDomain.User)[0];
        }

        #endregion
    }
}

