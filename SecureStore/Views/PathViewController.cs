
using System;

using Foundation;
using UIKit;
using SecureStore.File;
using System.Threading.Tasks;

namespace SecureStore.Views
{
	public partial class PathViewController : UIViewController
	{
		public FileManager FileManager { get; set; }
		public string CurrentPath { get; set; }

		public PathViewController (string startingPath) : base ("PathViewController", null)
		{
			CurrentPath = startingPath;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			tableView.RowHeight = 50.0f;
			tableView.AllowsSelectionDuringEditing = true;

            NavigationController.NavigationBar.Translucent = false;
            NavigationController.NavigationBar.Opaque = true;
			NavigationItem.RightBarButtonItem = new UIBarButtonItem ("Edit", UIBarButtonItemStyle.Plain,
				new EventHandler (EditButtonPressed));

			addDirectoryButton.Clicked += AddDirectoryButton_Clicked;

			FileManager = new FileManager();
			RefreshCurrentPath();
		}

		void AddDirectoryButton_Clicked (object sender, EventArgs e)
		{
			UIAlertView alertView = new UIAlertView("New Directory", string.Empty, null,
				"Ok", new[] { "Cancel" });
			alertView.Title = "Name the directory";
			alertView.CancelButtonIndex = 1;
			alertView.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
			alertView.Clicked += NewDirectoryAlertView_Clicked;

			alertView.Show();
		}

		void NewDirectoryAlertView_Clicked (object sender, UIButtonEventArgs e)
		{
			if (e.ButtonIndex == 1)
				return;

			var directoryName = ((UIAlertView)sender).GetTextField(0).Text;
			var newPath = new NSString(CurrentPath).AppendPathComponent(new NSString(directoryName)).ToString();

			NSError error = null;
			FileManager.CreateDirectory(newPath, true, new NSDictionary(), error);

			InvokeOnMainThread(() =>
				{
					tableView.ReloadData();
				});
		}

		void EditButtonPressed(object sender, EventArgs ev)
		{
			tableView.SetEditing (true, true);

			NavigationItem.RightBarButtonItem = new UIBarButtonItem ("Done", UIBarButtonItemStyle.Done,
				new EventHandler (DoneButtonPressed));
		}

		void DoneButtonPressed(object sender, EventArgs ev)
		{
			tableView.SetEditing(false, false);

			NavigationItem.RightBarButtonItem = new UIBarButtonItem ("Edit", UIBarButtonItemStyle.Plain,
				new EventHandler (DoneButtonPressed));
		}

		void RefreshCurrentPath()
		{
			NavigationItem.Title = CurrentPath;
			var files = FileManager.FindSecureDocsAtPath (CurrentPath);

			var source = new FileListTableViewSource(files, OnFolderSelect, CurrentPath);
			tableView.Source = source;
		}

		void OnFolderSelect(string path)
		{
			var currentPath = new NSString(CurrentPath);
			var newPath = currentPath.AppendPathComponent(new NSString(path));

			NavigationController.PushViewController(new PathViewController(
					newPath.ToString()), true);
		}
	}
}

