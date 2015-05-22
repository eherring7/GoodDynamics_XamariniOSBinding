using System;
using GoodDynamics;
using UIKit;
using RssReader.Delegates;
using System.Collections.Generic;
using RssReader.Models;

namespace RssReader.Helpers
{
    public class FeedDownloader
    {
        private GDHttpRequest _httpRequest;
        private string _authType;

        public FeedDownloader(IList<NewsItem> items)
        {
            _httpRequest = new GDHttpRequest();
            _httpRequest.Delegate = new CustomGDHttpRequestDelegate(items);
        }

        public void RequestData(string url)
        {
            RequestData(url, null, null);
        }

        public void RequestData(string url, string userId, string password)
        {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
            //Set Credentials
            if (userId != null && password != null && _authType != null)
            {
                //open the request
                _httpRequest.Open("GET", url, true, userId, password, _authType);
            }
            else
            {
                _httpRequest.Open("GET", url, true);
            }
        }

        public void AbortRequest()
        {
            _httpRequest.Abort();
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
        }
    }
}

