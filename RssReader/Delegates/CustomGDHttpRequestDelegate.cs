using System;
using System.Xml.Linq;
using GoodDynamics;
using Foundation;
using UIKit;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using RssReader.Models;

namespace RssReader.Delegates
{
	public class CustomGDHttpRequestDelegate: GDHttpRequestDelegate
	{
		private IList<NewsItem> _items;

		public CustomGDHttpRequestDelegate (IList<NewsItem> items)
		{
			this._items = items;
		}

		public override void OnStatusChange (GDHttpRequest httpRequest)
		{
			var state = httpRequest.GetState;
			switch (state) {
			case GDHttpRequestState.GDHttpRequestOpened:
                    //Request opened so now send
				httpRequest.Send ();
				break;
			case GDHttpRequestState.GDHttpRequestDone:
                    //check status
				int status = httpRequest.GetStatus;

				if (status == 200) {
					//the request has finished so see if we have any data
					var buf = httpRequest.GetReceiveBuffer;
					var len = buf.BytesUnread;
					var rawData = new char[len];

					//Marshal space for the buffer that will be used by the Good SDK
					var dataPtr = new HandleRef (this, Marshal.StringToHGlobalAnsi (new string (rawData)));

					buf.Read (dataPtr.Handle, len);
					//rawData = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(dataPtr.Handle).ToCharArray();

					//Transform into a NSData object
					var data = NSData.FromBytes (dataPtr.Handle, (nuint)len);

					if (data.Length > 0) {
						ParseData (data);
					}
					//Must clean up memory manually for Mashalled data or it will stick around forever
					Marshal.FreeHGlobal (dataPtr.Handle);
				}
				break;
			}
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
		}

		private void ParseData (NSData data)
		{
			var doc = XDocument.Parse (data.ToString ());
			foreach (var story in doc.Descendants("item")) {
				var item = new NewsItem ();
				foreach (var element in story.Elements()) {
					var type = element.Name.LocalName;

					if (String.Compare (type, "title", true) == 0) {
						item.Title = element.Value;
					} else if (String.Compare (type, "description", true) == 0) {
						item.Description = element.Value;
					} else if (String.Compare (type, "pubDate", true) == 0) {
						item.Date = element.Value;
					} else if (String.Compare (type, "link", true) == 0) {
						item.Link = element.Value;
					}

				}
				_items.Add (item);
			}
		}

	}
}

