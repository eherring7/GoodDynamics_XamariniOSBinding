
using System;

using Foundation;
using UIKit;
using GoodDynamics;
using SecureStore.Sources;

namespace SecureStore.Views
{
    public partial class FileListViewController : UIViewController
    {
        public FileListViewController() : base("FileListViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            LoadPath("/");
        }

        private void LoadPath(string path)
        {
            NSError error;

            Title = path;
            var contents = GDFileSystem.ContentsOfDirectoryAtPath("/", out error);

            if (error == null)
            {
                // load the table
                fileTableList.Source = new FileListTableViewSource(contents);
            }
        }
    }
}

