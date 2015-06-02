using System;
using System.Threading.Tasks;
using Foundation;
using GoodDynamics;
using System.Text;
using UIKit;

namespace GreetingsServer
{
	public class GreetingsServerGDServiceDelegate : GDServiceDelegate
	{
		public ServiceController ServiceController {
			get;
			private set;
		}

		public GreetingsServerGDServiceDelegate (ServiceController controller)
		{
			ServiceController = controller;
		}

		public override async void DidReceiveFrom (string application, string service, string version, string method, NSObject parameters, NSObject[] attachments, string requestID)
		{
			await ProcessRequestForApplication (application, service, version, method, parameters, attachments, requestID);
		}

		async Task ProcessRequestForApplication (string application, string service, string version, string method, Foundation.NSObject parameters, NSObject[] attachments, string requestID)
		{
			bool requestProcessed = false;
			NSError goodError = null;

			if (!ServiceController.ConsumeFrontRequestService (service, application, method, version)) {
				if (String.Equals (service, "com.good.gd.example.services.greetings")) {
					requestProcessed = ProcessGreetingsService (application, service, version, method, parameters, attachments, requestID, out goodError);
				} else if (String.Equals (service, "com.gd.example.services.dateandtime")) {
					requestProcessed = ProcessDateAndTimeServiceRequest (application, service, version, method, parameters, attachments, requestID, out goodError);
				}

				if (!requestProcessed && goodError == null) {
					NSDictionary errorDetail = new NSDictionary ();
					errorDetail.SetValueForKey (new NSString (ServiceController.kServiceNotImplementedDescription), NSError.LocalizedDescriptionKey);
					NSError serviceError = new NSError (ICCErrorConstants.GDServicesErrorDomain, ICCErrorConstants.GDServicesErrorServiceNotFound, errorDetail);
					ServiceController.SendErrorTo (application, serviceError);
				}
			}
		}

		private bool ProcessGreetingsService (string application, string service, string version, string method, Foundation.NSObject parameters, NSObject[] attachments, string requestID, out NSError goodError)
		{
			goodError = null;
			bool requestProcessed = false;
			if (String.Equals (version, "1.0.0")) {
				if (String.Equals (method, "greetMe")) {
					requestProcessed = GDService.ReplyTo (application, new NSString ("G'day mate!"), GDTForegroundOption.EPreferPeerInForeground, null, requestID, out goodError);
				} else if (String.Equals (method, "sendFiles")) {
					StringBuilder fileString = new StringBuilder (20);
					GDFileStat fileStat = new GDFileStat ();
					for (int i = 0; i < attachments.Length; i++) {
						NSString file = (NSString)attachments [i];
						NSError error = null;
						bool ok = GDFileSystem.GetFileStat (file, ref fileStat, out error);
						fileString.AppendFormat ("{0}: ", file);

						if (ok && error == null) {
							goodError = null;
							fileString.AppendFormat ("{0} bytes; ", fileStat.fileLen);
							NSData fileData = GDFileSystem.ReadFromFile (file, out error);
							if (fileData != null && error != null) {
								Console.WriteLine (String.Format ("Filepath: {0}", file));
								string fileDataString = NSString.FromData (fileData, NSStringEncoding.UTF8);
								Console.WriteLine (String.Format ("File Contents: {0}", fileDataString));
							}
						} else {
							goodError = error;
							fileString.AppendFormat ("Error: {0}", error.LocalizedDescription);
						}
					}

					UIAlertView alertView = new UIAlertView ("Recieved Files", fileString.ToString (), null, "OK", null);
					alertView.Show ();
					requestProcessed = true;

				}
			}

			return requestProcessed;
		}

		private bool ProcessDateAndTimeServiceRequest (string application, string service, string version, string method, Foundation.NSObject parameters, NSObject[] attachments, string requestID, out NSError goodError)
		{
			goodError = null;
			bool didSendResponse = false;

			if (String.Equals (version, "1.0.0")) {
				String dateString = DateTime.Now.ToString ("g");
				didSendResponse = GDService.ReplyTo (application, new NSString (dateString), GDTForegroundOption.EPreferPeerInForeground, null, requestID, out goodError);
			}

			return didSendResponse;
		}
	}
}

