
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

            GoodService = new GDService();
            GoodService.Delegate = new GoodServiceDelegate(this);

            //doneButton.Clicked += DoneButton_Clicked;
        }

        void DoneButton_Clicked (object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_requestId))
            {
                Console.WriteLine("Service has not connected to client");
                return;
            }

            /*NSError error = null;
            var paths = NSSearchPath.GetDirectories(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User,
                            true);
            var documentPathForFile = new NSString(paths[0]).AppendPathComponent(
                new NSString("RespondDataFile.txt")).ToString();

            GDFileSystem.WriteToFile(NSData.FromString(textView.Text, NSStringEncoding.UTF8), documentPathForFile,
                out error);

            if (error != null)
            {
                
            }*/
        }

        #region IMainController implementation

        public void ShowText(string text)
        {
            //textView.Text = text;
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

