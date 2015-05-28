
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using GoodDynamics;

namespace AppKineticsSaveEditService
{
    public partial class ViewController : UIViewController, IMainController
	{
        private string _application;
        private string _requestId;

        public GDService GoodService { get; set; }

		public ViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            GoodService = new GDService();
            GoodService.Delegate = new GoodServiceDelegate(this);
        }

        #region IMainController implementation

        public void ShowText(string text)
        {
            textView.Text = text;
        }

        #endregion
	}
}
