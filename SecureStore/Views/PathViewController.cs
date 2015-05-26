
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

			if (CurrentPath == "/") {
				try {
					await RefreshContentsAsync ();
				} catch (Exception ex) {
					UIAlertView alertView = new UIAlertView ("Error", ex.Message, null, "Ok", null);
					alertView.Show ();
				}
			}
		}

		async void RefreshButton_Clicked (object sender, EventArgs e)
		{
			try {
				await RefreshContentsAsync ();
			} catch (Exception ex) {
				UIAlertView alertView = new UIAlertView ("Error", ex.Message, null, "Ok", null);
				alertView.Show ();
			}
		}

		private void EditButtonPressed(object sender, EventArgs ev)
		{
			tableView.SetEditing (true, true);
		}

		private void RefreshCurrentPath()
		{
			NavigationItem.Title = CurrentPath;
			var files = FileManager.FindSecureDocsAtPath (CurrentPath);

			var source = new FileListTableViewSource(files, OnFolderSelect);
			tableView.Source = source;
		}

		private async Task RefreshContentsAsync()
		{
			/*var syncManager = new SyncManager ();
			var files = syncManager.GetFilesToSync ();

			foreach (var file in files)
			{
				await syncManager.SyncFileAsync(file);
			}*/
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

