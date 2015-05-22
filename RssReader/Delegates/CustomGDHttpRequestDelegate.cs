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
        //private CustomNSXMLParserDelegate _xmlDelegate;

        private IList<NewsItem> _items;

        public CustomGDHttpRequestDelegate(IList<NewsItem> items)
        {
            this._items = items;
        }


        public override void OnStatusChange(GDHttpRequest httpRequest)
        {
            var state = httpRequest.GetState;
            var errorDialog = false;
            switch (state)
            {
                case GDHttpRequest_state_t.OPENED:
                    //Request opened so now send
                    httpRequest.Send();
                    break;
                case GDHttpRequest_state_t.DONE:
                    //check status
                    int status = httpRequest.GetStatus;

                    if (status == 200)
                    {
                        //the request has finished so see if we have any data
                        var buf = httpRequest.GetReceiveBuffer;
                        var len = buf.BytesUnread;
                        var rawData = new char[len];

                        //Marshal space for the buffer that will be used by the Good SDK
                        var dataPtr = new HandleRef(this, Marshal.StringToHGlobalAnsi(new string(rawData)));

                        buf.Read(dataPtr.Handle, len);
                        //rawData = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(dataPtr.Handle).ToCharArray();

                        //Transform into a NSData object
                        var data = NSData.FromBytes(dataPtr.Handle, (nuint) len);

                        if (data.Length > 0)
                        {
                            ParseData(data);
                        }
                        //Must clean up memory manually for Mashalled data or it will stick around forever
                        Marshal.FreeHGlobal(dataPtr.Handle);
                    }
                    else if ((status == 401) || (status  == 407))
                    {
//                        // assuming NTLM authentication, extract headers to determine what the server supports
//                        var headerData = httpRequest.GetResponseHeader("WWW-Authenticate");
//                        if (headerData.IndexOf(kAuthNegotiateStr, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
//                        {
//                            _authMethod = kAuthNegotiateStr;
//                            DisplayAuthQueryDialog();
//                        }
//                        else if (headerData.IndexOf(kAuthNTLMStr, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
//                        {
//                            _authMethod = kAuthNTLMStr;
//                            DisplayAuthQueryDialog();
//                        }
//                        else if (headerData.IndexOf(kAuthDigestStr, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
//                        {
//                            _authMethod = kAuthDigestStr;
//                            DisplayAuthQueryDialog();
//                        }
//                        else if (headerData.IndexOf(kAuthBasicStr, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
//                        {
//                            _authMethod = kAuthBasicStr;
//                            DisplayAuthQueryDialog();
//                        }
//                        else
//                        {
//                            // only the above methods currently supported in this application
//                            errorDialog = true;
//                        }
                    }
                    else
                    {
                        errorDialog = true;
                    }
                    break;
            }
            if(errorDialog)
            {
                var err = httpRequest.GetStatusText;
                if(err != null)
                {
                    var dialog = new UIAlertView("Error Fetching File",err, null, "Ok", null);
                    dialog.Show();
                }
            }
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
        }

        private void ParseData(NSData data){
            var doc = XDocument.Parse(data.ToString());
            foreach (var story in doc.Descendants("item"))
            {
                var item = new NewsItem();
                foreach (var element in story.Elements())
                {
                    var type = element.Name.LocalName;

                    if (String.Compare(type, "title", true) == 0)
                    {
                        item.Title = element.Value;
                    }
                    else if (String.Compare(type, "description", true) == 0)
                    {
                        item.Description = element.Value;
                    }
                    else if (String.Compare(type, "pubDate", true) == 0)
                    {
                        item.Date = element.Value;
                    }
                    else if (String.Compare(type, "link", true) == 0)
                    {
                        item.Link = element.Value;
                    }

                }
                _items.Add(item);
            }
        }

    }
}

