
using System;

using Foundation;
using UIKit;
using System.Collections.Generic;

namespace RssReader.Views
{
	public partial class FeedViewController : UISplitViewController
	{
		public FeedViewController (RSSFeed feed) : base ("FeedViewController", null)
		{
			//Custom Initialization

			//Set Nav Title
			this.NavigationController.Title = feed.RssName;

			//Set the URL
			_currentUrl = feed.RssUrl;

			//Add the refresh and cancel buttons
			NavigationItem.SetRightBarButtonItems(GetRightBarButtonItems(), false);

			if (FeedDownloader) 
			{
				ResetFeedDownloader ();
			} 
			else 
			{
				//Prepare story array
				storyArray = [[NSMutableArray alloc] init];

				//See if the feed request type changes whilst stories are showing
				//Although not used in the is example - is good to know
				[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(feedModeChanged:) name:kRSSRequestModeChangedNotification object:nil];

				//Create downloader
				if([RSSManager sharedRSSManager].currentRequestMode == requestModeNSURL)
				{
					feedDownloader = [[FeedDownloadHttpNSURL alloc] init];
				}
				else
				{
					feedDownloader = [[FeedDownloadHttpGDHttp alloc] init];
				}

				feedDownloader.delegate = self;

				//Load the feed - allow UI to load first
				[self performSelector:@selector(refresh) withObject:nil afterDelay:0.1f];
			}
		}
		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			//iPhone clean up - this is coming off of the view stack
			if(UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
			{
				NSNotificationCenter.DefaultCenter.RemoveObserver(this);
				Cancel();
				feedDownloader.SetDelegate(null);
			}
		}

		public UIBarButtonItem[] GetRightBarButtonItems()
		{
			//Create the list to hold the buttons, which then gets added to the toolbar
			var buttons = new List<UIBarButtonItem>();

			//Create the stop and refresh buttons
			var b = new UIBarButtonItem(UIBarButtonSystemItem.Stop, this, Cancel);
			b.Style = UIBarButtonItemStyle.Bordered;
			buttons.Add (b);

			//create a spacer
			b = new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace, null, null);
			buttons.Add (b);

			//Create a standard "refresh" button
			b = new UIBarButtonItem(UIBarButtonSystemItem.Refresh, this, refresh);
			buttons.Add (b);

			//Convert to array before returning
			return buttons.ToArray();

		}

		private void Refresh()
		{
			StoryArray.RemoveAllObjects();
			RssStoriesTable.reloadData();// clear the table before refilling
			ParseDataFromSite(currentUrl);
		}

		private void cancel()
		{
			feedDownloader.abortRequest();
		}

		private void LoadFeed(string url, string, title)
		{
			//Check if we are loading
			if(!UIApplication.SharedApplication.NetworkActivityIndicatorVisible)
			{
				//Not loading, so update the table
				this.NavigationItem.Title = title;
				currentUrl = url;

				Refresh();
			}
			else
			{
				var dialog = new UIAlertView("Load in Progress","Please Wait",this, "Ok",null);
				dialog.Show();
			}
		}
	}
}

