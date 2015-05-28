using System;
using GoodDynamics;
using Foundation;

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
        }

        bool ParamsValid(NSObject parameters)
        {
            throw new NotImplementedException();
        }

        bool AttachmentsValid(NSObject[] attachments)
        {
            throw new NotImplementedException();
        }
    }
}

