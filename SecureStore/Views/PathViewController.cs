
using System;

using Foundation;
using UIKit;
using SecureStore.File;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            tableView.DeselectRow(tableView.IndexPathForSelectedRow, false);
        }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			tableView.RowHeight = 50.0f;
			tableView.AllowsSelectionDuringEditing = false;

            NavigationController.NavigationBar.Translucent = false;
            NavigationController.NavigationBar.Opaque = true;
			NavigationItem.RightBarButtonItem = new UIBarButtonItem ("Edit", UIBarButtonItemStyle.Plain,
				new EventHandler (EditButtonPressed));

			addDirectoryButton.Clicked += AddDirectoryButton_Clicked;
            addFileButton.Clicked += AddFileButton_Clicked;

			FileManager = new FileManager();
			RefreshCurrentPath();
		}

        void AddFileButton_Clicked (object sender, EventArgs e)
        {
            UIAlertView alertView = new UIAlertView("New File", string.Empty, null,
                                        "Ok", new[] { "Cancel" });
            
            alertView.CancelButtonIndex = 1;
            alertView.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
            alertView.Clicked += AddFileAlertView_Clicked;

            alertView.Show();
        }

        void AddFileAlertView_Clicked(object sender, UIButtonEventArgs e)
        {
            if (e.ButtonIndex == 1)
                return;

            var newFileName = ((UIAlertView)sender).GetTextField(0).Text;
            var newPath = new NSString(CurrentPath).AppendPathComponent(new NSString(newFileName)).ToString();

            if (FileManager.FileExists(newPath, false))
            {
                InvokeOnMainThread(() =>
                    {
                        new UIAlertView("Error", "File Exists", null, "Ok", null).Show();
                    });
            }

            FileManager.CreateFile(newPath);

            InvokeOnMainThread(() => {
                RefreshCurrentPath();
                tableView.ReloadData();
            });
        }

		void AddDirectoryButton_Clicked (object sender, EventArgs e)
		{
			UIAlertView alertView = new UIAlertView("New Directory", string.Empty, null,
				"Ok", new[] { "Cancel" });
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
                    RefreshCurrentPath();
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
			var source = new FileListTableViewSource(files, OnFolderSelect, OnFileSelect, CurrentPath);
			tableView.Source = source;
		}

		void OnFolderSelect(string directoryName)
		{
			var currentPath = new NSString(CurrentPath);
			var newPath = currentPath.AppendPathComponent(new NSString(directoryName));

			NavigationController.PushViewController(new PathViewController(
					newPath.ToString()), true);
		}

        void OnFileSelect(string fileName)
        {
            var currentPath = new NSString(CurrentPath);
            var newPath = currentPath.AppendPathComponent(new NSString(fileName)).ToString();

            NavigationController.PushViewController(new FileViewController(newPath), true);
        }
	}
}

