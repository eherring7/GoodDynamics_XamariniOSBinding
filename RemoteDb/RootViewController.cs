
using System;

using Foundation;
using UIKit;

namespace RemoteDb
{
	public partial class RootViewController : UIViewController
	{
		public RootViewController () : base ("RootViewController", null)
		{
		}

		public RootViewController (string nibName, NSBundle bundle) : base (nibName, bundle)
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
			
			// Perform any additional setup after loading the view, typically from a nib.
		}
	}
}

