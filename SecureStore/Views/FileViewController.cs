
using System;

using Foundation;
using UIKit;
using SecureStore.File;

namespace SecureStore
{
    public partial class FileViewController : UIViewController
    {
        public string FilePath { get; private set; }
        public FileManager FileManager { get; private set; }

        public FileViewController(string filePath) : base("FileViewController", null)
        {
            FilePath = filePath;

            FileManager = new FileManager();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
            // Perform any additional setup after loading the view, typically from a nib.
            NavigationController.NavigationBar.Opaque = true;
            NavigationController.NavigationBar.Translucent = false;

            NSError error = null;
            var contents = FileManager.ReadFile(FilePath, error).ToString();

            Title = new NSString(FilePath).LastPathComponent.ToString();
            fileContentsView.Text = contents;
        }
    }
}

