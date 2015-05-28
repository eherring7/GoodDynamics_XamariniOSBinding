using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using GoodDynamics;
using System.Linq;

namespace AppKineticsSaveEditClient
{
	partial class RootViewController : UIViewController
	{
		public RootViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NSBundle bundle = NSBundle.MainBundle;
            var filePath = bundle.PathForResource("DataFile", "txt");
            Console.WriteLine("FilePath: {0}", filePath);

            NSData fileData = NSData.FromFile(filePath);
            var contents = fileData.ToString(NSStringEncoding.UTF8);
            Console.WriteLine("Contents: {0}", contents);

            theTextView.Text = contents;

            NSNotificationCenter.DefaultCenter.AddObserver(new NSString("openFileForEdit"), HandleFileForEditNotification, (NSObject)this);
            NSNotificationCenter.DefaultCenter.AddObserver(new NSString("kShowServiceAlert"), HandleShowServiceAlertError, (NSObject)this);
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();

            NSNotificationCenter.DefaultCenter.RemoveObserver(this);
        }

        partial void SendClick(UIButton sender)
        {
            var text = theTextView.Text;
            Console.WriteLine("TextView Text: {0}", text);

            var appDelegate = ((AppDelegate)UIApplication.SharedApplication.Delegate);
            var appDetail = (NSMutableArray)appDelegate.GDLibrary.GetServiceProviders(
                "com.good.gdservice.save-edited-file", "1.0.0.0", GoodDynamics.GDServiceProviderType.GDServiceProviderApplication
            );

            for (nuint i = 0; i<appDetail.Count; i++)
            {
                var details = (GDServiceProvider)appDetail.GetItem<GDServiceProvider>(i);
                if (details.Identifier == NSBundle.MainBundle.BundleIdentifier)
                {
                    appDetail.RemoveObject((nint)i);
                    break;
                }
            }

            if (!AppDetailIsValid(appDetail))
                return;

            var documentPathForFile = SaveTextViewToFile();
            SendFile(documentPathForFile, appDetail, appDelegate.ServiceController);
            Console.WriteLine("Sent File to {0}", appDetail.GetItem<GDServiceProvider>(0).Identifier);
        }

        void HandleFileForEditNotification(NSNotification notification)
        {
        }

        void HandleShowServiceAlertError(NSNotification notification)
        {
        }

        bool AppDetailIsValid(NSMutableArray appDetail)
        {
            if (appDetail.Count == 0)
            {
                UIAlertView alertView = new UIAlertView("Error", "Cant find service application", null, "Ok", null);
                alertView.Show();

                return false;
            }

            return true;
        }

        string SaveTextViewToFile()
        {
            NSError error = null;

            var fileData = NSData.FromString(theTextView.Text);
            var paths = NSSearchPath.GetDirectories(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User, true);
            var documentDirectory = paths.ElementAt(0);
            var documentPathForFile = new NSString(documentDirectory).AppendPathComponent(new NSString("DataFile.txt")).ToString();
            GDFileSystem.WriteToFile(fileData, documentPathForFile, out error);

            return documentPathForFile;
        }

        void SendFile(string documentPathForFile, NSMutableArray appDetail, ServiceController svcController)
        {
            NSError error = null;

            var attachments = new[] { documentPathForFile };
            GDServiceProvider serviceProvider = null;

            for (nuint i = 0; i < appDetail.Count; i++)
            {
                var detail = appDetail.GetItem<GDServiceProvider>(0);
                if (detail.Identifier == "com.good.gd.example.appkinetics.saveeditservice")
                {
                    serviceProvider = detail;
                    break;
                }
            }

            var isRequested = svcController.SendSaveEditFileRequest(out error, serviceProvider.Identifier,
                                  null, attachments, "openFileForEdit");

            if (!isRequested)
                Console.WriteLine("Request was not accepted");

            if (error != null)
            {
                UIAlertView alertView = new UIAlertView("Error", error.LocalizedDescription, null, "Ok", null);
                alertView.Show();
            }
                
        }
	}
}
