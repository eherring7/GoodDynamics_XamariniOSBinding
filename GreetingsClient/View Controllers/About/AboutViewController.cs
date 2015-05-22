
using System;

using Foundation;
using UIKit;
using GreetingsClient.Helpers.ManagedSplitViewController;

namespace GreetingsClient.ViewControllers.About
{
	public partial class AboutViewController : ManagedSplitViewController
	{
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

		partial void DismissAbout (Foundation.NSObject sender)
		{
			DismissViewController(true, null);
		}
	}
}

