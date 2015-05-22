
using System;

using Foundation;
using UIKit;

namespace SecureStore.Views
{
    public partial class MainViewController : UIViewController
    {
        public MainViewController() : base("MainViewController", null)
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
			
            // Perform any additional setup after loading the view, typically from a nib.
            fileListButton.TouchUpInside += (object sender, EventArgs e) => 
                {
                    NavigationController.PushViewController(new FileListViewController(), true);
                };
        }
    }
}

