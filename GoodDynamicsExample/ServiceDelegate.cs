using System;
using GoodDynamics;
using MonoTouch.Foundation;

namespace GoodDynamicsExample
{
    public class ServiceDelegate : GDServiceDelegate
    {
        public ServiceDelegate()
        {
        }

        public override void WithAttachments(string application, NSObject[] attachments, MonoTouch.Foundation.NSObject parameters, string requestID)
        {

        }
         
        //Use this method to initiate the App Kinetics service system. This example uses the tranfer-file service.
        public override void ForService(string application, string service, string version, string method, MonoTouch.Foundation.NSObject parameters, NSObject[] attachments, string requestID)
        {
            if (service != "com.good.gdservice.transfer-file")
            {
                ReplyWithError(application, service, requestID, "Unsupported Service", 0);
            }
            else if (method != "transferFile")
            {
                ReplyWithError(application, service, requestID, "Unsupported Method", 0);
            }
            else if (parameters != null ||  attachments.Length != 1)
            {
                ReplyWithError(application, service, requestID, "Invalid Number of Parameters/Attachments", 0);
            }
            else
            {
                String filePath = attachments[0].ToString();

                NSUrl url = new NSUrl(filePath);

                NSError replyErr;

                //Import file from URL
            }     
        }

        private void ReplyWithError(string application, string service, string requestId, string errorDescription, int code)
        {
            NSError error = new NSError(new NSString(service), code, NSDictionary.FromObjectAndKey(new NSString(errorDescription), NSError.LocalizedDescriptionKey));
            NSError replyErr;
            GDService.ReplyTo(application, error, GDTForegroundOption.EPreferPeerInForeground, null, requestId, out replyErr);

            if (replyErr != null)
            {
                //DO SOMETHING
            }
        }
    }
}

