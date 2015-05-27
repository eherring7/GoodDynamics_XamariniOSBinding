using System;
using GoodDynamics;
using Foundation;
using System.Linq;

namespace AppKineticsSaveEditClient
{
    public class ServiceController : GDServiceClientDelegate
    {
        public GDServiceClient ServiceClient { get; private set; }

        public ServiceController()
        {
            ServiceClient = new GDServiceClient();
            ServiceClient.Delegate = this;
        }

        public override void DidRecieveFrom(string application, NSObject parameters, NSObject[] attachments, string requestID)
        {
            if (!ParamsValid(parameters))
                return;

            if (!AttachmentsValid(attachments))
                return;

            var filePath = (NSString)attachments.ElementAtOrDefault(0);
            if (string.Compare(filePath.PathExtension, ".txt", true) != 0)
            {
                Console.WriteLine("Invalid Extension - only txt supported");
                return;
            }

            ShowFile(filePath, application);
        }

        public override void DidFinishSendingTo(string application, NSObject[] attachments, NSObject parameters, string requestID)
        {
            Console.WriteLine("Send Finished");

            NSError error = null;
            if (!AttachmentsValid(attachments))
                return;

            var filePath = (NSString)attachments.ElementAtOrDefault(0);
            GDFileSystem.RemoveItemAtPath(filePath, out error);
            if (error != null)
            {
                Console.WriteLine("Cant delete old file - {0}", error.LocalizedDescription);
            }
        }

        bool ParamsValid(NSObject parameters)
        {
            if (parameters is NSError)
            {
                Console.WriteLine("Service sent an error");
                return false;
            }

            return true;
        }

        bool AttachmentsValid(NSObject[] attachments)
        {
            if (attachments != null && attachments.Length == 1)
            {
                return true;
            }

            Console.WriteLine("Error with Attachements");
            return false;
        }

        void ShowFile(string filePath, string application)
        {
            NSDictionary dictionary = new NSDictionary();
            dictionary.SetValueForKey(new NSString("kApplicationIDKey"), new NSString(application));
            dictionary.SetValueForKey(new NSString("kFilePathKey"), new NSString(filePath));

            // todo: fix this portion

            //NSNotificationCenter.DefaultCenter.PostNotification(notification);
        }
    }
}

