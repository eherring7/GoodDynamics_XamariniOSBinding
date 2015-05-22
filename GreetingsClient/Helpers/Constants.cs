using System;
using UIKit;

namespace GreetingsClient.Helpers
{
	public static class Constants
	{
		public static nfloat kStatusBarHeight 
		{
			get { return 20.0f; }
		}
		public static nfloat kNavBarHeight 
		{
			get { return 44.0f; }
		}
		public static nfloat kToolBarHeight
		{
			get {return 44.0f;}
		}
		public static nfloat kToolBarHeightLandscape
		{
			get {return 32.0f;}
		}

		public static nfloat kiPhoneHeight 
		{
			get { return 480.0f - kStatusBarHeight; }
		}
		public static nfloat kiPadHeight 
		{
			get { return 1024.0f - kStatusBarHeight; }
		}

		public static nfloat kiPhoneWidth
		{
			get { return 320.0f; }
		}
		public static nfloat kiPadWidth
		{
			get { return 768.0f; }
		}

		public static UIColor RGB(nfloat r, nfloat g, nfloat b)
		{
			return new UIColor (r / 255.0f, g / 255.0f, b / 255.0f, 1.0f);
		}
		public static UIColor RGBA(nfloat r, nfloat g, nfloat b, nfloat a)
		{
			return new UIColor (r / 255.0f, g / 255.0f, b / 255.0f, a);
		}
		public static UIColor MainColor
		{
			get { return RGB (232, 23, 54); }
		}

		public static string kDateAndTimeServiceId
		{
			get { return "com.gd.example.services.dateandtime"; }
		}
		public static string kGreetingsServiceId
		{
			get { return "com.good.gd.example.services.greetings"; }
		}
		public static string kGreetingsServerAppId
		{
			get { return "com.good.gd.example.services.greetings.server"; }
		}
		public static string kServiceConfigDidChangeNotification 
		{
			get { return "kServiceConfigDidChangeNotification"; }
		}
	}
}

