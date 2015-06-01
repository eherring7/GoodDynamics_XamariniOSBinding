using System;
using System.Runtime.InteropServices;

namespace GoodDynamics
{
	public static class Sqlite3Enc
	{
		// extern int sqlite3enc_open (const char * filename, sqlite3 ** ppDb);
		[DllImport ("__Internal", EntryPoint="sqlite3enc_open")]
		public static extern int sqlite3enc_open (IntPtr filename, out IntPtr ppDb);

		// extern int sqlite3enc_open_v2 (const char * zFilename, sqlite3 ** ppDb, int flags, const char * zVfs);
		[DllImport ("__Internal", EntryPoint="sqlite3enc_open_v2")]
		public static extern int sqlite3enc_open_v2 (IntPtr zFilename, out IntPtr ppDb, int flags, IntPtr zVfs);

		// extern int sqlite3enc_import (const char * srcFilename, const char * destFilename);
		[DllImport ("__Internal", EntryPoint="sqlite3enc_import")]
		public static extern int sqlite3enc_import (IntPtr srcFilename, IntPtr destFilename);
	}


	public enum GDAppResultCode{
		ErrorNone = 0,
		ErrorActivationFailed = -101,
		ErrorProvisioningFailed = -102,
		ErrorPushConnectionTimeout = -103,
		ErrorAppDenied = -104,
		ErrorAppVersionNotEntitled = -105,
		ErrorIdleLockout = -300,
		ErrorBlocked = -301,
		ErrorWiped = -302,
		ErrorRemoteLockout = -303,
		ErrorPasswordChangeRequired = -304,
		ErrorSecurityError = -100
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct GDFileStat{
		public int fileLen;
		public ulong lastModifiedTime;
		public bool isFolder;
	}

	public enum GDFileSystemErr : uint {
		GDFileSystemErrPathDoesntExist = 100,
		GDFileSystemErrIOError = 101,
		GDFileSystemErrPermissionError = 102,
		GDFileSystemErrDirNotEmpty = 103,
		GDFileSystemErrUnknownError = 500
	}

	public enum GDAppEventType{
		Authorized = 0,
		NotAuthorized = 1,
		RemoteSettingsUpdate = 2,
		ServicesUpdate = 3,
		PolicyUpdate = 4
	}

	public enum GDServiceProviderType{
		GDServiceProviderApplication = 0,
		GDServiceProviderServer
	}

	public enum GDUIColorTheme{
		DarkGrayTheme,
		MediumGrayTheme,
		WhiteTheme
	}

	public enum GDHttpRequest_state_t : uint {
		UNSENT = 0,
		OPENED = 1,
		SENT = 2,
		HEADERS_RECEIVED = 3,
		LOADING = 4,
		DONE = 5
	}

	public enum GDTForegroundOption{
		EPreferMeInForeground,
		EPreferPeerInForeground,
		ENoForegroundPreference
	}

	public enum GDAuthTokenErr {
		GDAuthTokenErrNotSupported = -2,
		GDAuthTokenErrRetry = -1
	}

    public enum GDServicesError {
        GDServicesErrorGeneral,
        GDServicesErrorApplicationNotFound,
        GDServicesErrorServiceNotFound,
        GDServicesErrorServiceVersionNotFound,
        GDServicesErrorMethodNotFound,
        GDServicesErrorNotAllowed,
        GDServicesErrorInvalidParams,
        GDServicesErrorCertificateNotFound
    }
}

