using System;
using Foundation;

namespace RssReader.Managers.RSSManager
{
    public class RSSFeed
    {
        public string RssName{ get; private set; }
        public NSUrl RssUrl { get; private set; }

        public RSSFeed(string rssName, NSUrl rssUrl)
        {
            RssName = rssName;
            RssUrl = rssUrl;
        }
    }
}

