using System;
using GoodDynamics;
using Foundation;
using UIKit;

namespace GreetingsServer
{
	public class ServiceController
	{
		public const string kMethodNotImplementedDescription = "The requested method is not implemented.";
		public const string kServiceNotImplementedDescription = "The requested service is not implemented.";
		public const string kDateAndTimeServiceId = "com.gd.example.services.dateandtime";
		public const string kGreetingsServiceId = "com.good.gd.example.services.greetings";

		public GDService GDService {
			get;
			set;
		}

		private const int kGreetingsServiceErrorNotImplemented = -1;
		private const string kGreetingsServiceErrorNotImplementedDescription = "The requested service, version, and method is not implemented.";

		public ServiceController ()
		{
			GDService = new GDService ();
			GDService.Delegate = new GreetingsServerGDServiceDelegate (this);
		}

		public bool ConsumeFrontRequestService(string serviceID, string application, string method, string version)
		{
			if (serviceID.Equals (GoodDynamics.GDService.GDFrontRequestService) && version.Equals ("1.0.0.0")) 
			{
				if (method.Equals (GoodDynamics.GDService.GDFrontRequestMethod)) 
				{
					NSError error = null;
					GDService.BringToFront (application, out error);
				} 
				else 
				{
					NSDictionary errorDetail = new NSDictionary ();
					errorDetail.SetValueForKey (new NSString(kMethodNotImplementedDescription), NSError.LocalizedDescriptionKey);
					NSError serviceError = new NSError (GDService.GDServicesErrorDomain, GDService.GDServicesErrorMethodNotFound, errorDetail); 
					SendErrorTo (application, serviceError);
				}
				return true;
			}
			return false;
		}

		public void SendErrorTo(string application, NSError error)
		{
			NSError goodError = null;
			bool didSendErrorResponse = GDService.ReplyTo (application, error, GDTForegroundOption.EPreferPeerInForeground, null, null, out goodError);
			if (!didSendErrorResponse) 
			{
				if (goodError != null) 
				{
					UIAlertView alert = new UIAlertView ("Error", goodError.LocalizedDescription, null, "OK", null);
					alert.Show ();
				}
			}
		}
	}
}

