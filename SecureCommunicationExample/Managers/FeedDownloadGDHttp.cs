using System;
using GoodDynamics;
using UIKit;
using Foundation;

namespace SecureCommunicationExample.Managers
{
    public class FeedDownloadGDHttp: GDHttpRequestDelegate, UIAlertViewDelegate
    {
        private const nint KAuthDialog = 1;
        private const nint KSSLDialog = 2;
        private const nint KErrorAlertDialog = 101;

        private const string kAuthBasicStr = "Basic";
        private const string kAuthDigestStr = "Digest";
        private const string kAuthNTLMStr = "NTLM";
        private const string kAuthNegotiateStr = "Negotiate";

        private GDHttpRequest _httpRequest;
        private string _currentUrl;
        private string _authMethod;

        public FeedDownloadGDHttp()
        {
            _httpRequest = new GDHttpRequest();
            _httpRequest.Delegate = this;
        }

        public void RequestData(string url)
        {
            _currentUrl = url;
            _authMethod = null;

            RequestData(url, null, null);
        }

        public void RequestData(string url, string userId, string password)
        {
            //Set Credentials
            if (userId && password && _authMethod)
            {
                //open the request
                _httpRequest.Open("GET", url, true, userId, password, _authMethod);
            }
            else
            {
                _httpRequest.Open("GET", url, true);
            }
        }

        public void AbortRequest()
        {
            _httpRequest.Abort;
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
        }

        #region GDHttpRequestDelegate implementation

        public void OnStatusChange(GDHttpRequest httpRequest)
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

                    // check to see if we have a SSL certificate issue
                    // and if so give the user the chance to relax the
                    // acceptance criteria
                    if (status == 0)
                    {
                        var err = httpRequest.GetStatusText;

                        if (err.IndexOf("SSL", 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            DisplaySSLQueryDialog();
                        }
                        else
                        {
                            errorDialog = true;
                        }
                    }
                    else if (status == 200)
                    {
                        //the request has finished so see if we have any data
                        var buf = httpRequest.GetReceiveBuffer;
                        var len = buf.BytesUnread;
                        var rawData = new char[len];
                        buf.Read(rawData, len);
                        var data = NSData.FromBytes(rawData, (nuint) len);
                        if (data.Length > 0)
                        {
                            this.Delegate.DownloadDone(data);
                        }
                    }
                    else if ((status == 401) || (status  == 407))
                    {
                        // assuming NTLM authentication, extract headers to determine what the server supports
                        var headerData = httpRequest.GetResponseHeader("WWW-Authenticate");
                        if (headerData.IndexOf(kAuthNegotiateStr, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            _authMethod = kAuthNegotiateStr;
                            DisplayAuthQueryDialog();
                        }
                        else if (headerData.IndexOf(kAuthNTLMStr, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            _authMethod = kAuthNTLMStr;
                            DisplayAuthQueryDialog();
                        }
                        else if (headerData.IndexOf(kAuthDigestStr, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            _authMethod = kAuthDigestStr;
                            DisplayAuthQueryDialog();
                        }
                        else if (headerData.IndexOf(kAuthBasicStr, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            _authMethod = kAuthBasicStr;
                            DisplayAuthQueryDialog();
                        }
                        else
                        {
                            // only the above methods currently supported in this application
                            errorDialog = true;
                        }
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
                    var dialog = new UIAlertView("Error Fetching File",err, this, "Ok", null);
                    dialog.Tag = KErrorAlertDialog;
                    dialog.Show();
                }
            }
        }

        public void DisplayAuthQueryDialog()
        {
            var dialog = new UIAlertView("Enter ID and Password", "", this, "Cancel", "Ok", null);
            dialog.AlertViewStyle = UIAlertViewStyle.LoginAndPasswordInput;
            dialog.Tag = KAuthDialog;
            dialog.Show();
        }

        #endregion

        #region UIAlertViewDelegate

        public override void Clicked(UIAlertView alertview, nint buttonIndex)
        {
            switch (alertview.Tag)
            {
                case KAuthDialog:
                    if (buttonIndex == 1)
                    {
                        var username = alertview.GetTextField(0).Text;
                        var password = alertview.GetTextField(1).Text;
                        RequestData(_currentUrl, username, password);
                    }
                    else
                    {
                        AbortRequest();
                    }                 
                    break;
                case KErrorAlertDialog:
                    this.Delegate.DownloadDone(null);
                    break;
            }
        }


        #endregion
    }
}

