using System;
using UIKit;
using System.Linq;
using Foundation;

namespace GreetingsClient.Helpers.ManagedSplitViewController
{
	public class ManagedSplitViewController : UIViewController
	{
		public UIPopoverController MasterPopoverController
		{
			get;
			set;
		}

		public ManagedSplitViewController ()
		{
			
		}

		public ManagedSplitViewController(string nibName, NSBundle bundle)
			:base(nibName, bundle)
		{
		}

		public ManagedSplitViewController(string nibName, NSBundle bundle, UINavigationController navController)
			:base(nibName, bundle)
		{
			if (this) 
			{
				if (navController) 
				{
					AppDelegate appDel = UIApplication.SharedApplication.Delegate;
					appDel.SplitViewController.Delegate = new ManagedSplitViewControllerDelegate(this);

					if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.Portrait || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.PortraitUpsideDown) 
					{
						if (navController.ViewControllers.Last ().GetType () == typeof(ManagedSplitViewController)) 
						{
							this.NavigationItem.SetLeftBarButtonItem (((ManagedSplitViewController)navController.ViewControllers.Last ()).NavigationItem.LeftBarButtonItem);
							this.MasterPopoverController = ((ManagedSplitViewController)navController.ViewControllers.Last()).MasterPopoverController);
						}
					}
				}
			}
		}

		//public ManagedSplitViewController(

		public void CleanDetailNavStack()
		{
			var viewControllers = NavigationController.ViewControllers.ToList();
			while (viewControllers.Count > 1) 
			{
				viewControllers.RemoveAt (0);
			}
			NavigationController.ViewControllers = viewControllers.ToArray ();
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				return toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown;
			} 
			else 
			{
				return true;
			}
		}
	}

	public class ManagedSplitViewControllerDelegate : UISplitViewControllerDelegate
	{
		ManagedSplitViewController _controller;
		public ManagedSplitViewControllerDelegate()
		{
		}

		public ManagedSplitViewControllerDelegate(ManagedSplitViewController controller)
		{
			_controller;
		}
		public override void WillHideViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem barButtonItem, UIPopoverController pc)
		{
			barButtonItem.Title = "Send Request";
			_controller.NavigationItem.SetLeftBarButtonItem (barButtonItem, true);
			_controller.MasterPopoverController = pc;
		}

		public override void WillShowViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem button)
		{
			_controller.NavigationItem.SetLeftBarButtonItem (null, true);
			_controller.MasterPopoverController = null;
		}
	}
}

