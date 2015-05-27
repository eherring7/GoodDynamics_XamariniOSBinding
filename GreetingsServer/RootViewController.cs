
using System;

using Foundation;
using UIKit;

namespace GreetingsServer
{
	public partial class RootViewController : UIViewController
	{
		private ServiceController _serviceController;

		public RootViewController () : base ("RootViewController", null)
		{
			_serviceController = new ServiceController ();
		}

		public RootViewController(string nibName, NSBundle bundle, ServiceController serviceController)
			:base(nibName, bundle)
		{
			_serviceController = serviceController;
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
			
			// Perform any additional setup after loading the view, typically from a nib.
		}
	}
}

