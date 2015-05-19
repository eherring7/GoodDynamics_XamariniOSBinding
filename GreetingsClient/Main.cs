using UIKit;

using GoodDynamics;

namespace GreetingsClient
{
	public class Application
	{
		static string ApplicationDelegateName = "AppDelegate";
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			GDiOS.InitializeWithClassNameConformingToUIApplicationDelegate (ApplicationDelegateName);

			UIApplication.Main (args, null, ApplicationDelegateName);
		}
	}
}
