using Foundation;
using UIKit;

using GoodDynamics;
using System.Diagnostics;
using System;
using System.Text;

namespace RemoteDb
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : GDiOSDelegate
	{
		// class-level declarations
		public GDiOS GDLibrary { get; private set; }
		private RemoteDBSettings _dbSettings;
		private bool _started;

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
				OnRemoteSettingsUpdated ();
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
					if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
						rootController = new RootViewController ("RootViewController~iphone", null);
						Window.RootViewController = rootController;
					} else {
						rootController = new RootViewController ("RootViewController~ipad", null);
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

		private void ShowAlert(GDAppEvent e)
		{
			UIAlertView alertView = new UIAlertView ("An error has occurred", e.Message, null, "OK", null);
			alertView.Show ();
		}

		private void OnRemoteSettingsUpdated()
		{
			NSDictionary settings = GDLibrary.GetApplicationConfig ();

			NSString appConfig = (NSString)settings.ValueForKey (GDiOS.GDAppConfigKeyConfig);
			NSNumber copyPasteOn = (NSNumber)settings.ValueForKey (GDiOS.GDAppConfigKeyCopyPasteOn);
			NSNumber detailedLogsOn = (NSNumber)settings.ValueForKey (GDiOS.GDAppConfigKeyDetailedLogsOn);
			NSString userEmail = (NSString)settings.ValueForKey (GDiOS.GDAppConfigKeyUserId);

			GDAppServer firstServer = null;
			NSArray allServers = (NSArray)settings.ValueForKey (GDiOS.GDAppConfigKeyServers);
			if (allServers != null && allServers.Count > 0) 
			{
				firstServer = allServers.GetItem<GDAppServer> (0);
			}

			NSString server = null;
			NSNumber port = null;
			NSNumber priority = null;

			if (firstServer != null) 
			{
				if (firstServer.Server != null) 
				{
					server = new NSString(firstServer.Server);
					Console.Write (String.Format ("Server Host: {0}", server));
				}
				if (firstServer.Port != null) 
				{
					port = firstServer.Port;
					Console.WriteLine (String.Format ("Server Port: {0}", port.NIntValue));
				}
				if (firstServer.Priority != null) 
				{
					priority = firstServer.Priority;
					Console.WriteLine (String.Format ("Server Priority: {0}", priority.NIntValue));
				}
			}

			bool result = StoreRemoteSettings (server, port, priority, appConfig, copyPasteOn, detailedLogsOn, userEmail);

			if (!result) 
			{
				Console.WriteLine ("Error storing settings");
			}

			ShowCurrentSettings ();
		}

		private bool StoreRemoteSettings(NSString serverHost, NSNumber port, NSNumber priority, NSString config, NSNumber cpOn, NSNumber dlOn, NSString email)
		{
			if (_dbSettings == null) 
			{
				_dbSettings = new RemoteDBSettings ();
			}

			int portInt = port == null ? 0 : port.Int32Value;
			int priorityInt = priority == null ? 0 : priority.Int32Value;
			bool result = _dbSettings.UpdateRemoteSettings (serverHost, portInt, priorityInt, config, cpOn.Int32Value, dlOn.Int32Value, email);

			return result;
		}

		private void ShowCurrentSettings()
		{
			if (_dbSettings == null) 
			{
				_dbSettings = new RemoteDBSettings ();
			}

			NSDictionary settings = _dbSettings.GetConfigSettings ();

			NSString config = (NSString)settings.ValueForKey (GDiOS.GDAppConfigKeyConfig);
			NSNumber copyPasteOn = (NSNumber)settings.ValueForKey (GDiOS.GDAppConfigKeyCopyPasteOn);
			NSNumber detailedLogsOn = (NSNumber)settings.ValueForKey (GDiOS.GDAppConfigKeyDetailedLogsOn);
			NSString userEmail = (NSString)settings.ValueForKey (GDiOS.GDAppConfigKeyUserId);

			GDAppServer firstServer = (GDAppServer)settings.ValueForKey (GDiOS.GDAppConfigKeyServers);

			StringBuilder builder = new StringBuilder ();
			if (firstServer != null) 
			{
				if (!string.IsNullOrWhiteSpace (firstServer.Server)) 
				{
					builder.AppendLine (String.Format ("Server Host: {0}", firstServer.Server));
				}
				if (firstServer.Port != null) 
				{
					builder.AppendLine (String.Format ("Server Port: {0}", firstServer.Port.Int32Value));
				}
				if (firstServer.Priority != null) 
				{
					builder.AppendLine (String.Format ("Server Priority: {0}", firstServer.Priority.Int32Value));
				}
				if (config != null) 
				{
					builder.AppendLine (String.Format ("Config: {0}", config));
				}
				if (copyPasteOn != null) 
				{
					builder.AppendLine (String.Format ("Data Leakage Security Policy: {0}", copyPasteOn.Int32Value));
				}
				if (detailedLogsOn != null) 
				{
					builder.AppendLine (String.Format ("Logging Level: {0}", detailedLogsOn.Int32Value));
				}
				if (userEmail != null) 
				{
					builder.AppendLine (String.Format ("User Email: {0}", userEmail));
				}
			}

			UIAlertView alertView = new UIAlertView ("App Config Change", builder.ToString (), null, "OK", null);
			alertView.Show ();
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


