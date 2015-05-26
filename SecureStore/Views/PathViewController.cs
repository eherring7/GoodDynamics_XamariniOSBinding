
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

		public override async void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			tableView.RowHeight = 50.0f;
			tableView.AllowsSelectionDuringEditing = true;

			NavigationItem.RightBarButtonItem = new UIBarButtonItem ("Edit", UIBarButtonItemStyle.Plain,
				new EventHandler (EditButtonPressed));

			refreshButton.Clicked += RefreshButton_Clicked;

			FileManager = new FileManager();
			RefreshCurrentPath();
		}

		void RefreshButton_Clicked (object sender, EventArgs e)
		{
			
		}

		private void EditButtonPressed(object sender, EventArgs ev)
		{
			tableView.SetEditing (true, true);

			NavigationItem.RightBarButtonItem = new UIBarButtonItem ("Done", UIBarButtonItemStyle.Done,
				new EventHandler (DoneButtonPressed));
		}

		private void DoneButtonPressed(object sender, EventArgs ev)
		{
			tableView.SetEditing(false, false);

			NavigationItem.RightBarButtonItem = new UIBarButtonItem ("Edit", UIBarButtonItemStyle.Plain,
				new EventHandler (DoneButtonPressed));
		}

		private void RefreshCurrentPath()
		{
			NavigationItem.Title = CurrentPath;
			var files = FileManager.FindSecureDocsAtPath (CurrentPath);

			var source = new FileListTableViewSource(files, OnFolderSelect, CurrentPath);
			tableView.Source = source;
		}

		private void OnFolderSelect(string path)
		{
			var currentPath = new NSString(CurrentPath);
			var newPath = currentPath.AppendPathComponent(new NSString(path));

			NavigationController.PushViewController(new PathViewController(
					newPath.ToString()), true);
		}
	}
}

