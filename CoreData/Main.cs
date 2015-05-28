using UIKit;
using GoodDynamics;
using System;

namespace CoreData
{
	public class Application
	{
		// This is the main entry point of the application.
		static string ApplicationDelegateName = "AppDelegate";
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			GDiOS.InitializeWithClassNameConformingToUIApplicationDelegate (ApplicationDelegateName);

			UIApplication.Main (args, null, ApplicationDelegateName);
		}
	}
}
