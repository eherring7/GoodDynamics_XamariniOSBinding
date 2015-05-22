using System;
using GoodDynamics;
using Foundation;
using UIKit;
using ObjCRuntime;
using System.Diagnostics;

namespace GreetingsClient
{
	public class ServiceControllerDelegate
	{
		public void ShowAlert(string reply)
		{
			UIAlertView view = new UIAlertView ("Error", reply, null, "OK", null);
			view.Show ();
		}
	}

	public class ServiceController
	{
		public const string kMethodNotImplementedDescription = "The requested method is not implemented.";
		public const string kServiceNotImplementedDescription = "The requested service is not implemented.";
		public const string kDateAndTimeServiceId = "com.gd.example.services.dateandtime";
		public const string kGreetingsServiceId = "com.good.gd.example.services.greetings";

		public enum ClientRequestType
		{
			GreetMe,
			BringServiceAppToFront,
			SendFiles,
			GetDateAndTime
		}

		public ServiceControllerDelegate Delegate {
			get;
			set;
		}

		public GDServiceClient GoodServiceClient
		{
			get;
			set;
		}

		public GDService GoodServiceServer {
			get;
			set;
		}

		public ServiceController ()
		{
			Delegate = new ServiceControllerDelegate ();
			GoodServiceClient = new GDServiceClient();
			GoodServiceClient.Delegate = new GreetingsClientGDServiceClientDelegate (this);
			GoodServiceServer = new GDService ();
			GoodServiceServer.Delegate = new GreetingsClientGDServiceDelegate (this);
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

		public bool SendRequest(NSError error, ClientRequestType type, string appId)
		{
			bool result = false;
			switch (type) 
			{
			case ClientRequestType.GreetMe:
				result = SendGreetMeRequest (error, appId);
				break;
			case ClientRequestType.BringServiceAppToFront:
				result = BringServiceAppToFront (error, appId);
				break;
			case ClientRequestType.SendFiles:
				result = SendFilesRequest (error, appId);
				break;
			case ClientRequestType.GetDateAndTime:
				result = SendGetDateAndTimeRequest (error, appId);
				break;
			default:
				break;
			}

			return result;
		}

		public bool SendGreetMeRequest(NSError error, string appId)
		{
			string requestId;
			return GDServiceClient.SendTo (appId, kGreetingsServiceId, "1.0.0", "greetMe", null, null, GDTForegroundOption.EPreferMeInForeground, out requestId, out error);
		}

		public bool BringServiceAppToFront(NSError error, string appId)
		{
			return GDServiceClient.BringToFront (appId, out error);
		}

		public bool SendFilesRequest(NSError error, string appId)
		{
			string filename1 = "first.txt";
			NSData data1 = new NSString ("This is first.txt, the first test file to send.").Encode (NSStringEncoding.UTF8);
			NSError file1Error = null;
			bool written = GDFileSystem.WriteToFile (data1, filename1, out file1Error);

			if (!written) 
			{
				Debug.WriteLine (String.Format ("Error writing to {0}", filename1));
				return false;
			}


			string filename2 = "second.txt";
			NSData data2 = new NSString ("This is second.txt, the second test file to send.").Encode (NSStringEncoding.UTF8);
			NSError file2Error = null;
			written = GDFileSystem.WriteToFile (data2, filename2, out file2Error);

			if (!written) 
			{
				Debug.WriteLine (String.Format ("Error writing to {0}", filename2));
				return false;
			}

			NSObject[] files = new NSObject[2]{ new NSString(filename1), new NSString(filename2) };
			string requestId;
			return GDServiceClient.SendTo (appId, kGreetingsServiceId, "1.0.0", "sendFiles", null, files, GDTForegroundOption.EPreferPeerInForeground, out requestId, out error);
		}

		public bool SendGetDateAndTimeRequest(NSError error, string appId)
		{
			string requestId;
			return GDServiceClient.SendTo (appId, kDateAndTimeServiceId, "1.0.0", "getDateAndTime", null, null, GDTForegroundOption.EPreferMeInForeground, out requestId, out error);
		}
	}
}

