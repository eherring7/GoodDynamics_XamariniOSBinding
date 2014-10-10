using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using GoodDynamics;
using MonoTouch.ObjCRuntime;
using System.Runtime.InteropServices;

namespace GoodDynamicsExample
{
    public class Application
    {
        static string ApplicationDelegateName = "AppDelegate";

        [DllImport ("/usr/lib/libobjc.dylib")]
        static extern bool class_addProtocol (IntPtr cls, IntPtr protocol);

        [DllImport ("/usr/lib/libobjc.dylib")]
        static extern IntPtr objc_getProtocol (string name);

        // This is the main entry point of the application.
        static void Main (string[] args)
        {
            //We need to manually add the missing UIApplicationDelegate protocol to our AppDelegate at runtime.
            //This lets GDiOS.InitializeWithClassNameConformingToUIApplicationDelegate succeed.
            var p = objc_getProtocol ("UIApplicationDelegate");
            class_addProtocol (Class.GetHandle (ApplicationDelegateName), p);

            GDiOS.InitializeWithClassNameConformingToUIApplicationDelegate (ApplicationDelegateName);

            UIApplication.Main (args, null, ApplicationDelegateName);
        }
    }
}
