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

		public override void WithAttachments (string application, NSObject[] attachments, Foundation.NSObject parameters, string requestID)
		{
		}

		public override void WithParams (string application, Foundation.NSObject parameters, NSObject[] attachments, string requestID)
		{
			if(parameters != null && (parameters.GetType() == typeof(string) || parameters.GetType() == typeof(NSError)))
			{
				string message = String.Empty;
				if (parameters.GetType () == typeof(string)) {
					message = (NSString)parameters;
				} else {
					message = (NSString)((NSError)parameters).ValueForKey (NSError.LocalizedDescriptionKey);
				}
				ServiceController.Delegate.ShowAlert (message);
			}
		}
			
	}
}

