using System;
using GoodDynamics;
using Foundation;

namespace GreetingsClient
{
	public class GreetingsClientGDServiceClientDelegate : GDServiceClientDelegate
	{
		public ServiceController ServiceController {
			get;
			private set;
		}
			
		public GreetingsClientGDServiceClientDelegate (ServiceController controller)
		{
			ServiceController = controller;
		}

		public override void DidFinishSendingTo (string application, NSObject[] attachments, Foundation.NSObject parameters, string requestID)
		{
		}

		public override void DidRecieveFrom (string application, Foundation.NSObject parameters, NSObject[] attachments, string requestID)
		{
			if(parameters != null && (parameters.GetType() == typeof(NSString) || parameters.GetType() == typeof(NSMutableString) || parameters.GetType() == typeof(NSError)))
			{
				NSString message = null;
				message = parameters as NSString;
				string title = "Success!";
				if (message == null) {
					title = "Error!";
					message = (NSString)((NSError)parameters).ValueForKey (NSError.LocalizedDescriptionKey);
				}
				ServiceController.Delegate.ShowAlert (title, message);
			}
		}
			
	}
}

