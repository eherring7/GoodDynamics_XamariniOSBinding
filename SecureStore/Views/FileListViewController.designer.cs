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
	[Register ("FileListViewController")]
	partial class FileListViewController
	{
		[Outlet]
		UIKit.UITableView fileTableList { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem newDirectoryButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (fileTableList != null) {
				fileTableList.Dispose ();
				fileTableList = null;
			}

			if (newDirectoryButton != null) {
				newDirectoryButton.Dispose ();
				newDirectoryButton = null;
			}
		}
	}
}
