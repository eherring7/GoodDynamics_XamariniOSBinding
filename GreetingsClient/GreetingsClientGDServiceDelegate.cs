using System;
using GoodDynamics;
using System.Threading.Tasks;
using Foundation;

namespace GreetingsClient
{
	public class GreetingsClientGDServiceDelegate : GDServiceDelegate
	{
		public ServiceController ServiceController {
			get;
			private set;
		}

		public GreetingsClientGDServiceDelegate (ServiceController serviceController)
		{
			ServiceController = serviceController;
		}

		public override async void ForService (string application, string service, string version, string method, Foundation.NSObject parameters, NSObject[] attachments, string requestID)
		{
			await ProcessRequestForApplication(application, service, version, method, parameters, attachments, requestID);
		}

		async Task ProcessRequestForApplication (string application, string service, string version, string method, Foundation.NSObject parameters, NSObject[] attachments, string requestID)
		{
			InvokeOnMainThread (() => {
				if(!ServiceController.ConsumeFrontRequestService(service, application, method, version))
				{
					NSDictionary errorDetail = new NSDictionary();
					errorDetail.SetValueForKey(new NSString(ServiceController.kServiceNotImplementedDescription), NSError.LocalizedDescriptionKey);
					NSError serviceError = new NSError(GDService.GDServicesErrorDomain, GDService.GDServicesErrorServiceNotFound, errorDetail);
					ServiceController.SendErrorTo(application, serviceError);
				}
			});
		}
	}
}

