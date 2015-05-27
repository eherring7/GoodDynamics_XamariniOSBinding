// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace SecureStore.Views
{
	[Register ("PathViewController")]
	partial class PathViewController
	{
		[Outlet]
		UIKit.UIBarButtonItem addDirectoryButton { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem addFileButton { get; set; }

		[Outlet]
		UIKit.UITableView tableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (addDirectoryButton != null) {
				addDirectoryButton.Dispose ();
				addDirectoryButton = null;
			}

			if (addFileButton != null) {
				addFileButton.Dispose ();
				addFileButton = null;
			}
		}
	}
}
