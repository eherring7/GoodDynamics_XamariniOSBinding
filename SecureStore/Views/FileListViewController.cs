
using System;

using Foundation;
using UIKit;
using GoodDynamics;

namespace SecureStore.Views
{
    public partial class FileListViewController : UIViewController
    {
        public FileListViewController()
            : base("FileListViewController", null)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        private void LoadPath(string path)
        {
            NSError error;
            var contents = GDFileSystem.ContentsOfDirectoryAtPath("/", out error);

            if (error == null)
            {
                // load the table
            }
        }
    }
}

