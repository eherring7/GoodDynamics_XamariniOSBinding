using Foundation;
using UIKit;

using GoodDynamics;
using System.Diagnostics;
using System;

namespace GreetingsClient
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : GDiOSDelegate
	{
		private bool _started;
		//private ServiceController _serviceController;
		// class-level declarations
		public GDiOS GDLibrary { get; private set; }
		public NSMutableArray Providers {get; set;}

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
			case GDAppEventType.RemoteSettingsUpdate:
				//handle app config changes
				break;
			case GDAppEventType.ServicesUpdate:
				Debug.WriteLine ("Received Service Update Event");
				OnServiceUpdate (anEvent);
				break;
			default:
				Debug.WriteLine ("Event Not Handled");
				break;
			}
		}

		private void OnAuthorized(GDAppEvent anEvent)
		{
			switch (anEvent.Code) {
			case GDAppResultCode.ErrorNone:
				if (!_started) {
					_started = true;
					RootViewController rootController = null;
					ServiceController serviceController = new ServiceController ();
					if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
						rootController = new RootViewController ("RootViewController~iphone", null, serviceController);
						Window.RootViewController = rootController;
					} else {
						rootController = new RootViewController ("RootViewController~ipad", null, serviceController);
						Window.RootViewController = rootController;
					}
				}
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
			case GDAppResultCode.ErrorAppVersionNotEntitled:
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

		private void OnServiceUpdate(GDAppEvent anEvent)
		{
			switch (anEvent.Code) 
			{
			case GDAppResultCode.ErrorNone:
				NSNotificationCenter.DefaultCenter.PostNotificationName ("kServiceConfigDidChangeNotification", null);
				break;
			default:
				break;
			}
		}

		public NSArray GetProvidersForService(NSString serviceId)
		{
			return GDLibrary.GetServiceProviders (serviceId, null, GDServiceProviderType.GDServiceProviderApplication);
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
	}
}


