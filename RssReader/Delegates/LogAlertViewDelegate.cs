using System;
using UIKit;
using GoodDynamics;
using RssReader.Models;
using System.Collections.Generic;
using Foundation;

namespace RssReader.Delegates
{
    public class LogAlertViewDelegate: UIAlertViewDelegate
    {
        public LogAlertViewDelegate()
        {
        }

        public override void WillDismiss(UIAlertView alertView, nint buttonIndex)
        {
            if (buttonIndex == 1)
            {
                //Cannot pass null in to an out parameter due to the architecture of C#. Pass dummy object instead.
                var err = new NSError();

                GDFileSystem.UploadLogs(out err);
            }
        }
    }
}

