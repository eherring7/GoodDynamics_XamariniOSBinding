
using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using RssReader.Sources;
using RssReader.Helpers;
using RssReader.Models;
using RssReader.Delegates;

namespace RssReader.Views
{
    public partial class RssListView : UIViewController
    {
        private FeedDownloader _feedDownloader;

        private IList<NewsItem> _items;

        public RssListView(): base("RssListView", null)
        {
            _items = new List<NewsItem>();
            _feedDownloader = new FeedDownloader(_items);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            //Set Nav Title
            NavigationItem.Title = "BBC News";

            //Set the URL
            //_currentUrl = feed.RssUrl;

            //Add the refresh and save buttons
            NavigationItem.SetRightBarButtonItems(GetRightBarButtonItems(), false);
            NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Action, SaveLogs), false);

            var source = new FeedTableViewSource(_items);
            rssTable.Source = source;
        }

        public UIBarButtonItem[] GetRightBarButtonItems()
        {
            //Create the list to hold the buttons, which then gets added to the toolbar
            var buttons = new List<UIBarButtonItem>();

            //Create the stop and refresh buttons
            var b = new UIBarButtonItem(UIBarButtonSystemItem.Stop, Abort);
            buttons.Add (b);

            //create a spacer
            b = new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace, null, null);
            buttons.Add (b);

            //Create a standard "refresh" button
            b = new UIBarButtonItem(UIBarButtonSystemItem.Refresh, Refresh);
            buttons.Add (b);

            //Convert to array before returning
            return buttons.ToArray();

        }

        private void Refresh(object sender, EventArgs e)
        {
            _feedDownloader.RequestData("http://feeds.bbci.co.uk/news/world/rss.xml");
            rssTable.ReloadData();
        }

        private void Abort(object sender, EventArgs e)
        {
            _feedDownloader.AbortRequest();
        }

        private void SaveLogs(object sender, EventArgs e){
            var alert = new UIAlertView("Save Logs?", "A snapshot of your logs will be taken now and uploaded to the server for troubleshooting.\nThe upload process may take some time but it will continue in the background whenever possible until all the logs have been uploaded.\n Do you want to upload the logs now?",
                null, "Cancel", "Save");
            alert.Delegate = new LogAlertViewDelegate();
            alert.Show();
        }
    }
}

