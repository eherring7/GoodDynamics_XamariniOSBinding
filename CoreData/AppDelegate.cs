using Foundation;
using UIKit;

using GoodDynamics;
using System.Diagnostics;
using System;
using CoreData.Views;
using CoreData.Models;

namespace CoreData
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : GDiOSDelegate
	{
		// class-level declarations
        private NSPersistentStoreCoordinator _storeCoordinator;
        public NSPersistentStoreCoordinator StoreCoordinator
        {
            get
            {
                // Returns the persistent store coordinator (GDPersistentStoreCoordinator) for the application.
                // If the coordinator doesn't already exist, it is created and the application's store added to it.
                if (_storeCoordinator != null)
                {
                    return _storeCoordinator;
                }

                _storeCoordinator = new GDPersistentStoreCoordinator(ManagedObjectModel);
                NSError error;
                var path = NSBundle.PathForResourceAbsolute("initialSeedData", "sqlite", NSBundle.MainBundle.ResourcePath);
                var bundleUrl = NSUrl.CreateFileUrl(new string[]{path});

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
                return _storeCoordinator;
            }
        }

        private NSManagedObjectContext _context;

        private NSManagedObjectModel _managedObjectModel;
        public NSManagedObjectModel ManagedObjectModel
        {
            get{
                if (_managedObjectModel != null) {
                    return _managedObjectModel;
                }

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
                return _managedObjectModel;
            }
        }

		public GDiOS GDLibrary { get; private set; }

		public override UIWindow Window {
			get;
			set;
		}


		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			GDLibrary = GDiOS.SharedInstance();
			GDLibrary.Delegate = this;
			GDLibrary.Authorize();
			Window = GDLibrary.GetWindow();
			Window.MakeKeyAndVisible ();
			return true;
		}

		public override void HandleEvent (GDAppEvent anEvent)
		{
			switch (anEvent.Type)
			{
			case GDAppEventType.Authorized:
				OnAuthorized (anEvent);
				break;
			case GDAppEventType.NotAuthorized:
				OnNotAuthorized (anEvent);
				break;
			}
		}

		private void OnAuthorized(GDAppEvent anEvent)
		{
			switch (anEvent.Code) {
                case GDAppResultCode.ErrorNone:
				//Start your application
                    _context = new NSManagedObjectContext () {
                        PersistentStoreCoordinator = StoreCoordinator
                    };
                    var prodView = new ProductsView(_context);
                    var nav = new UINavigationController(prodView);
                    Window.RootViewController = nav;
                    Window.MakeKeyAndVisible();
    				break;

			    default:
			        Debug.Assert (false, "Authorized startup with an error");
				    break;
			}
		}

		private void OnNotAuthorized(GDAppEvent anEvent)
		{
			switch (anEvent.Code) {
			case GDAppResultCode.ErrorActivationFailed:
			case GDAppResultCode.ErrorProvisioningFailed:
			case GDAppResultCode.ErrorPushConnectionTimeout:
			case GDAppResultCode.ErrorSecurityError:
			case GDAppResultCode.ErrorAppDenied:
			case GDAppResultCode.ErrorBlocked:
			case GDAppResultCode.ErrorWiped:
			case GDAppResultCode.ErrorRemoteLockout:
			case GDAppResultCode.ErrorPasswordChangeRequired:
				Console.WriteLine ("OnNotAuthorized {0}", anEvent.Message);
				break;
			case GDAppResultCode.ErrorIdleLockout:
				break;
			default:
				Debug.Assert (false, "Unhandled not authorized event");
				break;
			}
		}

		public override void OnResignActivation (UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground (UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground (UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated (UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate (UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}

        public void UpdateDatabaseWithPersistantStoreCoordinator(NSPersistentStoreCoordinator persistentStoreCoordinator,
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
                persistentStoreCoordinator.MigratePersistentStore(persistentStoreCoordinator.PersistentStoreForUrl(bundleUrl),storeURL,options,storeType, out error);
                if (error != null)
                {
                    throw new Exception(error.ToString());
                }
            }
            else
            {
                error = null;
                persistentStoreCoordinator.AddPersistentStoreWithType(storeType, null, storeURL, options, out error);
                if (error != null)
                {
                    /*
                     Replace this implementation with code to handle the error appropriately.

                     Typical reasons for an error here include:
                     * The persistent store is not accessible;
                     * The schema for the persistent store is incompatible with current managed object model.
                     Check the error message to determine what the actual problem was.
                     cd
                     
                     If the persistent store is not accessible, there is typically something wrong with the file path. Often, a file URL is pointing into the application's resources directory instead of a writeable directory.
                     */
                    throw new Exception(error.ToString());
                }
            }
        }
        // Returns the URL to the application's Documents directory.
        public NSUrl ApplicationDocumentsDirectory()
        {
            return NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory,NSSearchPathDomain.User)[0];
        }


	}
}


