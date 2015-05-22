using System;
using UIKit;
using Foundation;
using System.Collections.Generic;
using GreetingsClient.ViewControllers.About;
using System.Linq;

namespace GreetingsClient.Helpers.RedToolbar
{
	public class RedToolbar : UIToolbar
	{
		public RedToolbar ()
		{
		}

		public RedToolbar (NSCoder coder)
			: base (coder)
		{
			if (this) 
			{
				TintColor = Constants.MainColor;
				UIButton infoButton = new UIButton (UIButtonType.InfoLight);
				infoButton.TouchUpInside += ShowInfo;

				UIBarButtonItem infoBarButtonItem = new UIBarButtonItem (infoButton);
				UIBarButtonItem settingsButton = new UIBarButtonItem (new UIImage ("settings.png"), UIBarButtonItemStyle.Plain, ShowSettings);
				UIBarButtonItem autoSpacer = new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace);

				NSBundle assetsBundle = new NSBundle (NSBundle.MainBundle.PathForResource ("GDAssets", "bundle"));

				string path = assetsBundle.PathForResource ("SECURED_GOOD_LOGO", "png");
				UIImageView logoImageView = new UIImageView (new UIImage (path));
				UIBarButtonItem logoItem = new UIBarButtonItem (logoImageView);

				List<UIBarButtonItem> items = new List<UIBarButtonItem> () 
				{
					infoBarButtonItem,
					logoItem,
					autoSpacer,
					settingsButton
				};

				SetItems (items.ToArray (), false);
			}
		}

		void ShowInfo (object sender, EventArgs e)
		{
			AppDelegate appDel = UIApplication.SharedApplication.Delegate;
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) 
			{
				AboutViewController aboutViewController = new AboutViewController ("AboutViewController_iPhone", null);

				aboutViewController.ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal;
				aboutViewController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
				appDel.NavController.PresentViewController (aboutViewController, true, null);
			} 
			else 
			{
				if (!appDel.NavController.ViewControllers.Last().GetType() == typeof(AboutViewController)) 
				{
					AboutViewController aboutViewController = new AboutViewController ("AboutViewController_iPad", null, appDel.NavController);
					appDel.NavController.PushViewController (aboutViewController, false);
					aboutViewController.CleanDetailNavStack ();
				}
			}
		}

		void ShowSettings(object sender, EventArgs e)
		{
		}
	}
}

