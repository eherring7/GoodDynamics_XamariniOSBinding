using System;
using UIKit;
using Foundation;

namespace RssReader.Helpers
{
	public static class Constants
	{
		//Dimisions
		public const nfloat KStatusBarHeight = 20f;
		public const nfloat KNavBarHeight = 44f;

		public const nfloat KToolbarHeight = 44.0f;
		public const nfloat KToolbarHeightLandscape = 32.0f;

		public const nfloat KiPhoneHeight = 480.0f - KStatusBarHeight;
		public const nfloat KiPadHeight = 1024.0f - KStatusBarHeight;

		public const nfloat kiPhoneWidth = 320.0f;
		public const nfloat KiPadWidth = 768.0f;

		//Colors
		public const UIColor maincolor =  new UIColor(232,23,54,1);

		//RSS
		public const String RSSTitle = "BBC World";
		public const NSUrl RSSUrl = new NSUrl("http://feeds.bbci.co.uk/news/world/rss.xml");
	}
}

