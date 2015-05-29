
using System;

using Foundation;
using UIKit;
using GoodDynamics;

namespace AppKineticsSaveEditService
{
    public partial class MainViewController : UIViewController, IMainController
    {
        private string _application;
        private string _requestId;

        public GDService GoodService { get; set; }

        public MainViewController() : base("MainViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationController.NavigationBar.Opaque = true;
            NavigationController.NavigationBar.Translucent = false;
            Title = "Save Edit Service";

            GoodService = new GDService();
            GoodService.Delegate = new GoodServiceDelegate(this);

            doneButton.Clicked += DoneButton_Clicked;
        }

        void DoneButton_Clicked (object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_requestId))
            {
                Console.WriteLine("Service has not connected to client");
                return;
            }

            NSError error = null;
            var paths = NSSearchPath.GetDirectories(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User,
                            true);
            var documentPathForFile = new NSString(paths[0]).AppendPathComponent(
                new NSString("RespondDataFile.txt")).ToString();

            GDFileSystem.WriteToFile(NSData.FromString(textView.Text, NSStringEncoding.UTF8), documentPathForFile,
                out error);

            if (error == null)
            {
                var urlString = string.Format("{0}.sc2://", _application);
                var testUrl = new NSUrl(urlString);
                if (!UIApplication.SharedApplication.CanOpenUrl(testUrl))
                {
                    Console.WriteLine("Client is not installed");
                    return;
                }

                bool replyResult = GDService.ReplyTo(_application, null, GDTForegroundOption.EPreferPeerInForeground,
                                       new[] { new NSString(documentPathForFile) }, _requestId, out error);

                if (!replyResult)
                    Console.WriteLine("Failed to get Reply");

                if (error != null)
                {
                    Console.WriteLine("Failed to Reply: {0} {1:d} {2}", error.Domain, error.Code,
                        error.LocalizedDescription);
                }
            }
            else
            {
                Console.WriteLine("Failed to write data to secure storage: {0}", error.LocalizedDescription);
            }
        }

        #region IMainController implementation

        public void ShowText(string text)
        {
            textView.Text = text;
        }

        public void SetApplication(string application)
        {
            _application = application;
        }

        public void SetRequestId(string requestId)
        {
            _requestId = requestId;
        }

        #endregion
    }
}

