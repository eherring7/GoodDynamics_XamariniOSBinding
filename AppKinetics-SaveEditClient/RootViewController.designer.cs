// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppKineticsSaveEditClient
{
	[Register ("RootViewController")]
	partial class RootViewController
	{
		[Outlet]
		UIKit.UIButton sendButton { get; set; }

		[Outlet]
		UIKit.UITextView theTextView { get; set; }

		[Action ("SendClick:")]
		partial void SendClick (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (sendButton != null) {
				sendButton.Dispose ();
				sendButton = null;
			}

			if (theTextView != null) {
				theTextView.Dispose ();
				theTextView = null;
			}
		}
	}
}
