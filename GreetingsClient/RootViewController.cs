
using System;

using Foundation;
using UIKit;

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
		private NSArray _serviceButtons;

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
			
			_serviceButtons = NSArray.FromNSObjects (new ServiceInfo ("com.gd.example.services.dateandtime", btnGetDateTime));
		}
			
		partial void bringToFront_TouchUpInside (Foundation.NSObject sender)
		{
			NSError goodError = null;
			bool didSendRequest = _serviceController.SendRequest(goodError, ServiceController.ClientRequestType.BringServiceAppToFront, "com.good.gd.example.services.greetings.server");

			if(!didSendRequest)
			{
				ShowErrorAlert(goodError);
			}
		}
			
		partial void getDateAndTime_TouchUpInside (Foundation.NSObject sender)
		{
			NSError goodError = null;
			bool didSendRequest = _serviceController.SendRequest(goodError, ServiceController.ClientRequestType.GetDateAndTime, "com.good.gd.example.services.greetings.server");

			if(!didSendRequest)
			{
				ShowErrorAlert(goodError);
			}
		}
			
		partial void greetMe_TouchUpInside (Foundation.NSObject sender)
		{
			NSError goodError = null;
			bool didSendRequest = _serviceController.SendRequest(goodError, ServiceController.ClientRequestType.GreetMe, "com.good.gd.example.services.greetings.server");

			if(!didSendRequest)
			{
				ShowErrorAlert(goodError);
			}
		}
			
		partial void sendFiles_TouchUpInside (Foundation.NSObject sender)
		{
			NSError goodError = null;
			bool didSendRequest = _serviceController.SendRequest(goodError, ServiceController.ClientRequestType.SendFiles, "com.good.gd.example.services.greetings.server");

			if(!didSendRequest)
			{
				ShowErrorAlert(goodError);
			}
		}

		private void ShowErrorAlert(NSError error)
		{
			UIAlertView errorAlert = new UIAlertView ("An Error Occurred.", (NSString)error.ValueForKey (NSError.LocalizedDescriptionKey), null, "OK", null);
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
		}
	}
}

