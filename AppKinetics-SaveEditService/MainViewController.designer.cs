// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppKineticsSaveEditService
{
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		UIKit.UIBarButtonItem doneButton { get; set; }

		[Outlet]
		UIKit.UITextView textView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (textView != null) {
				textView.Dispose ();
				textView = null;
			}

			if (doneButton != null) {
				doneButton.Dispose ();
				doneButton = null;
			}
		}
	}
}
