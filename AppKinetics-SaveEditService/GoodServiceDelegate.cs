using System;
using GoodDynamics;
using Foundation;

namespace AppKineticsSaveEditService
{
    public class GoodServiceDelegate : GDServiceDelegate
    {
        private readonly IMainController _mainController;
        private string _application;
        private string _requestId;

        public GoodServiceDelegate(IMainController mainController)
        {
            _mainController = mainController;
        }

        public override void DidReceiveFrom(string application, string service, string version, string method, NSObject parameters,
            NSObject[] attachments, string requestID)
        {
            if (string.Compare(service, "com.good.gdservice.save-edited-file", true) != 0)
            {
                ReportError(application, requestID, string.Format("Service Not Found: {0}", service),
                    (int)GDServicesError.GDServicesErrorServiceNotFound);

                return;
            }

            if (string.Compare(method, "openFileForEdit", true) != 0)
            {
                ReportError(application, requestID, string.Format("Method not found: {0}", method),
                    (int)GDServicesError.GDServicesErrorMethodNotFound);

                return;
            }

            if (attachments.Length != 1)
            {
                ReportError(application, requestID, "Should be one attachement", (int)GDServicesError.GDServicesErrorInvalidParams);
                return;
            }

            _application = application;
            _requestId = requestID;

            var text = ExtractReceivedDataFromAttachments(attachments);
            Console.WriteLine("Received Text: {0}", text);

            _mainController.ShowText(text);
        }

        void ReportError(string application, string requestID, string message, int code)
        {
            var localizedKey = NSError.LocalizedDescriptionKey;
            NSError error = null;
            NSDictionary userInfo = new NSDictionary();
            userInfo.SetValueForKey(new NSString(message), localizedKey);

            NSError replyParams = new NSError(GDService.GDServicesErrorDomain, code, userInfo);

            bool replyResult = GDService.ReplyTo(application, replyParams, GDTForegroundOption.ENoForegroundPreference,
                                   null, requestID, out error);

            if (!replyResult)
                Console.WriteLine("ReplyTo returned false");

            if (error != null)
            {
                Console.WriteLine("GDServiceReceiveFrom failed to reply: {0} {1:d} {2}",
                    error.Domain, error.Code, error.LocalizedDescription);
            }
        }

        string ExtractReceivedDataFromAttachments(NSObject[] attachments)
        {
            var localFilePath = new NSString(attachments[0].ToString());
            Console.WriteLine("Local File Path: {0}", localFilePath);

            if (!GDFileSystem.FileExistsAtPath(localFilePath, false))
            {
                ReportError(_application, _requestId, "Attachment was not found",
                    (int)GDServicesError.GDServicesErrorInvalidParams);

                return string.Empty;
            }

            NSError error = null;
            NSData attachmentData = GDFileSystem.ReadFromFile(localFilePath, out error);

            if (error != null)
            {
                var description = string.Format("Error Reading attachement: {0} {1}",
                                      localFilePath, error.LocalizedDescription);

                ReportError(_application, _requestId, description, (int)GDServicesError.GDServicesErrorInvalidParams);
                return string.Empty;
            }

            return new NSString(attachmentData.ToString(), NSStringEncoding.UTF8).ToString();
        }
    }
}

