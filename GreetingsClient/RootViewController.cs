
using System;

using Foundation;
using UIKit;
using System.Collections.Generic;

namespace GreetingsClient
{
	public partial class RootViewController : UIViewController
	{
		public class ServiceInfo : NSObject
		{
			public string ServiceID {get;set;}
			public string ApplicationID {get;set;}
			public UIButton Button {get;set;}

			public ServiceInfo(string serviceId, UIButton button)
			{
				Button = button;
				ServiceID = serviceId;
			}
		}

		private ServiceController _serviceController;
		private List<ServiceInfo> _serviceButtons;

		public RootViewController () : base ("RootViewController", null)
		{
			_serviceController = new GreetingsClient.ServiceController ();

			NSNotificationCenter.DefaultCenter.AddObserver (new NSString("kServiceConfigDidChangeNotification"), ServiceConfigDidChange);
		}

		public RootViewController(string nibName, NSBundle bundle, ServiceController serviceController)
			:base(nibName, bundle)
		{
			_serviceController = serviceController;
			NSNotificationCenter.DefaultCenter.AddObserver (new NSString("kServiceConfigDidChangeNotification"), ServiceConfigDidChange);
		}

		private void ServiceConfigDidChange(NSNotification notification)
		{
			ProcessProviderDetails ();
		}

		private string GetAppID(UIButton button)
		{
			string appId = null;

			foreach (var info in _serviceButtons) 
			{
				if (info.Button == button) 
				{
					appId = info.ApplicationID;
					break;
				}
			}

			return appId;
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			btnBringToFront.TouchUpInside += btnBringToFront_TouchUpInside;
			btnGetDateTime.TouchUpInside += btnGetDateAndTime_TouchUpInside;
			btnGreetMe.TouchUpInside += btnGreetMe_TouchUpInside;
			btnSendFiles.TouchUpInside += btnSendFiles_TouchUpInside;

			_serviceButtons = new List<ServiceInfo> () {new ServiceInfo ("com.gd.example.services.dateandtime", btnGetDateTime)};
		}
			
		void btnBringToFront_TouchUpInside (object sender, EventArgs e)
		{
			NSError goodError = null;
			bool didSendRequest = _serviceController.SendRequest(out goodError, ServiceController.ClientRequestType.BringServiceAppToFront, "com.good.gd.example.services.greetings.server");

			if(!didSendRequest)
			{
				ShowErrorAlert(goodError);
			}
		}
			
		void btnGetDateAndTime_TouchUpInside (object sender, EventArgs e)
		{
			NSError goodError = null;

			bool didSendRequest = _serviceController.SendRequest(out goodError, ServiceController.ClientRequestType.GetDateAndTime, GetAppID(btnGetDateTime));

			if(!didSendRequest)
			{
				ShowErrorAlert(goodError);
			}
		}
			
		void btnGreetMe_TouchUpInside (object sender, EventArgs e)
		{
			NSError goodError = null;
			bool didSendRequest = _serviceController.SendRequest(out goodError, ServiceController.ClientRequestType.GreetMe, "com.good.gd.example.services.greetings.server");

			if(!didSendRequest)
			{
				ShowErrorAlert(goodError);
			}
		}
			
		void btnSendFiles_TouchUpInside (object sender, EventArgs e)
		{
			NSError goodError = null;
			bool didSendRequest = _serviceController.SendRequest(out goodError, ServiceController.ClientRequestType.SendFiles, "com.good.gd.example.services.greetings.server");

			if(!didSendRequest)
			{
				ShowErrorAlert(goodError);
			}
		}

		private void ShowErrorAlert(NSError error)
		{
			UIAlertView errorAlert = new UIAlertView ("An Error Occurred.", error.LocalizedDescription, null, "OK", null);
			errorAlert.Show ();
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
			} else {
				return true;
			}
		}

		private void ProcessProviderDetails()
		{
			AppDelegate appDel = (AppDelegate)UIApplication.SharedApplication.Delegate;

			foreach(var info in _serviceButtons)
			{
				var providers = appDel.GetProvidersForService (new NSString(info.ServiceID));

				if (providers.Count > 0) 
				{
					info.Button.Enabled = true;
					info.Button.Alpha = 1.0f;

					GoodDynamics.GDServiceProvider appService = providers.GetItem<GoodDynamics.GDServiceProvider> (0);
					info.ApplicationID = appService.Identifier;
				} 
				else 
				{
					info.Button.Enabled = false;
					info.Button.Alpha = 0.4f;

					info.ApplicationID = null;
				}
			}
		}
	}
}

