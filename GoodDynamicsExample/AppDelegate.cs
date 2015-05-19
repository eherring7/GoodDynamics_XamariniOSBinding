using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using GoodDynamics;
using System.Diagnostics;

namespace GoodDynamicsExample
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : GDiOSDelegate
    {
        // class-level declarations
        public GDiOS GDLibrary { get; private set; }


        public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
        {
            GDLibrary = GDiOS.SharedInstance();
            GDLibrary.Delegate = this;
            GDLibrary.Authorize();
            Window = GDLibrary.GetWindow();
            Window.MakeKeyAndVisible ();

            return true;
        }

        #region GDiOSDelegate Support

        [Export("gdWindow")]
        public override UIWindow Window { get;  set; }

        [Export("gdApplication:didFinishLaunchingWithOptions:")]
        private bool gdFinishedLaunching(UIApplication app, NSDictionary options)
        {
            return FinishedLaunching(app, options);
        }

        [Export("gdApplication:openURL:sourceApplication:annotation:")]
        private bool gdOpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            return OpenUrl(application, url, sourceApplication, annotation);
        }

        [Export("gdApplicationWillResignActive:")]
        private void gdApplicationWillResignActive(UIApplication application)
        {
            OnResignActivation(application);
        }

        [Export("gdApplicationDidEnterBackground:")]
        private void gdApplicationDidEnterBackground(UIApplication application)
        {
            DidEnterBackground(application);
        }

        [Export("gdApplicationWillEnterForeground:")]
        private void gdApplicationWillEnterForeground(UIApplication application)
        {
            WillEnterForeground(application);
        }

        [Export("gdApplicationWillTerminate:")]
        private void gdApplicationWillTerminate(UIApplication application)
        {
            WillTerminate(application);
        }

        [Export("handleEvent")]
        public override void HandleEvent (GDAppEvent anEvent)
        {
            switch (anEvent.Type)
            {
                case GDAppEventType.Authorized:
                    OnAuthorized (anEvent);
                    break;
                case GDAppEventType.NotAuthorized:
                    OnNotAuthorized (anEvent);
                    break;
            }
        }

        private void OnAuthorized(GDAppEvent anEvent)
        {
            switch (anEvent.Code) {
                case GDAppResultCode.ErrorNone:
                    //Start your application
                    break;

                default:
                    Debug.Assert (false, "Authorized startup with an error");
                    break;
            }
        }

        private void OnNotAuthorized(GDAppEvent anEvent)
        {
            switch (anEvent.Code) {
                case GDAppResultCode.ErrorActivationFailed:
                case GDAppResultCode.ErrorProvisioningFailed:
                case GDAppResultCode.ErrorPushConnectionTimeout:
                    GDLibrary.Authorize ();
                    break;
                case GDAppResultCode.ErrorSecurityError:
                case GDAppResultCode.ErrorAppDenied:
                case GDAppResultCode.ErrorBlocked:
                case GDAppResultCode.ErrorWiped:
                case GDAppResultCode.ErrorRemoteLockout:
                case GDAppResultCode.ErrorPasswordChangeRequired:
                    Console.WriteLine ("OnNotAuthorized {0}", anEvent.Message);
                    break;
                case GDAppResultCode.ErrorIdleLockout:
                    break;
                default:
                    Debug.Assert (false, "Unhandled not authorized event");
                    break;
            }
        }

        private GDService _gdService;
        private GDServiceClient _gdServiceClient;
        private ServiceClientDelegate _gdServiceClientDelegate;
        private ServiceDelegate _gdServiceDelegate;
        private void InitGDService()
        {
            _gdService = new GDService();
            _gdServiceClientDelegate = new ServiceClientDelegate();
            _gdServiceDelegate = new ServiceDelegate();
           
            _gdService.Delegate = _gdServiceDelegate;
            _gdServiceClient = new GDServiceClient();
            _gdServiceClient.Delegate = _gdServiceClientDelegate;
        }

        #endregion



		
        // This method is invoked when the application is about to move from active to inactive state.
        // OpenGL applications should use this method to pause.
        public override void OnResignActivation(UIApplication application)
        {
        }
		
        // This method should be used to release shared resources and it should store the application state.
        // If your application supports background exection this method is called instead of WillTerminate
        // when the user quits.
        public override void DidEnterBackground(UIApplication application)
        {
        }
		
        // This method is called as part of the transiton from background to active state.
        public override void WillEnterForeground(UIApplication application)
        {
        }
		
        // This method is called when the application is about to terminate. Save data, if needed.
        public override void WillTerminate(UIApplication application)
        {
        }
    }
}

