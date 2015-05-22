// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace GreetingsClient
{
	[Register ("RootViewController")]
	partial class RootViewController
	{
		[Outlet]
		UIKit.UIButton btnBringToFront { get; set; }

		[Outlet]
		UIKit.UIButton btnGetDateTime { get; set; }

		[Outlet]
		UIKit.UIButton btnGreetMe { get; set; }

		[Outlet]
		UIKit.UIButton btnSendFiles { get; set; }

		[Action ("bringToFront_TouchUpInside:")]
		partial void bringToFront_TouchUpInside (Foundation.NSObject sender);

		[Action ("getDateAndTime_TouchUpInside:")]
		partial void getDateAndTime_TouchUpInside (Foundation.NSObject sender);

		[Action ("greetMe_TouchUpInside:")]
		partial void greetMe_TouchUpInside (Foundation.NSObject sender);

		[Action ("sendFiles_TouchUpInside:")]
		partial void sendFiles_TouchUpInside (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (btnGreetMe != null) {
				btnGreetMe.Dispose ();
				btnGreetMe = null;
			}

			if (btnBringToFront != null) {
				btnBringToFront.Dispose ();
				btnBringToFront = null;
			}

			if (btnSendFiles != null) {
				btnSendFiles.Dispose ();
				btnSendFiles = null;
			}

			if (btnGetDateTime != null) {
				btnGetDateTime.Dispose ();
				btnGetDateTime = null;
			}
		}
	}
}
