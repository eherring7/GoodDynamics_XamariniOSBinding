using System;
using System.Runtime.InteropServices;
using Foundation;

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

	/** \struct GDFileStat
	 * Information about a file or directory in the secure store.
	 * This structure is used to return information about a file or
	 * directory in the secure store.
	 * \see \link GDFileSystem.GetFileStat GetFileStat\endlink
	 */
	[StructLayout (LayoutKind.Sequential)]
	public struct GDFileStat{
		public int fileLen;
		/**< File size in bytes */

		public ulong lastModifiedTime;
		/**< Timestamp for when file was last modified */

		public bool isFolder;
		/**< <TT>true</TT> for directories, <TT>false</TT> for plain files */
	}


	/**
	 * \defgroup gdappevent GDAppEvent property constants
	 * Use these enumerated constants in the application code for the
	 * Good Dynamics Runtime event-handler.
	 *
	 * \{
	 */

	/** Constants for GDAppEvent type.
	* This enumeration represents the type of a GDAppEvent that is being
	* notified. The \ref GDAppEvent.Type property will always take one of these
	* values.
	*/
	public enum GDAppEventType{
		Authorized = 0,
		/**< Either the user has been authorized to access the application and its
	     * data, following authorization processing, or a condition that caused
	     * authorization to be withdrawn has been cleared.
	     * In either case, the user can be given access to the application data, and
	     * the application can make full use of the Good Dynamics API.
	     * 
	     * The event result code will be <TT>GDErrorNone</TT>.
	     * 
	     * See  \link GDiOS.Authorize Authorize (GDiOS)\endlink for authorization processing initiation.
	     */
		NotAuthorized = 1,
		/**< Either the user has <EM>not </EM>been authorized to access the
	     * application and its data, following authorization processing, or a
	     * condition has arisen that caused authorization to be withdrawn.
	     * In either case, the application must deny the user access to any
	     * application data. This includes not displaying any data in the
	     * application user interface.
	     * 
	     * In the case that the user is found not to be authorized following
	     * authorization processing, the application cannot make use of the Good
	     * Dynamics APIs, except to initiate authorization processing again.
	     * Otherwise, if authorization has only been withdrawn, the application can
	     * make use of the Good Dynamics APIs.
	     * 
	     * The event result code will indicate the condition that has arisen.
	     * See \ref GDAppResultCode.
	     * 
	     * See  \link GDiOS.Authorize: Authorize (GDiOS)\endlink for authorization processing initiation.
	     */
		RemoteSettingsUpdate = 2,
		/**< A change to application configuration or other settings from the
	     * enterprise has been received.\ An event of this type is despatched
	     * whenever there is a change in any value that is returned by
	     * \link GDiOS.GetApplicationConfig GetApplicationConfig (GDiOS)\endlink.
	     */
		ServicesUpdate = 3,
		/**< A change to services-related configuration of one or more applications
	     * has been received.\ See under  \link GDiOS.GetServiceProvidersFor GetServiceProvidersFor:  (GDiOS)\endlink.
	     */
		PolicyUpdate = 4,
		/**< A change to one or more application-specific policy settings has been
	     * received.\ See under \link GDiOS.GetApplicationPolicy GetApplicationPolicy (GDiOS)\endlink.
	     */
		DataPlanUpdate = 5
		/**< A change to the data plan state of the running application has been
	     * received.\ See also \link GDiOS.IsUsingDataPlan IsUsingDataPlan (GDiOS)\endlink.
	     */
	}
	/** \}
 	*/

	/** Constants for GD service provider type.
	 * This enumeration represents the type of service for which a service discovery
	 * query is being issued. The <TT>serviceProviderType</TT> parameter of the
	 *  \link GDiOS.GetServiceProvidersFor GetServiceProvidersFor:  (GDiOS)\endlink function always takes one of these values.
	 */
	public enum GDServiceProviderType{
		GDServiceProviderApplication = 0,
		/**< Application-based service. */
		GDServiceProviderServer
		/**< Server-based service. */
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


	/**
	 * \addtogroup gdfilesystemerrordomain GDFileSystem Error Domain
	 * These constants can be used when handling errors returned by
	 * \ref GDFileSystem, \ref GDCReadStream, and \ref GDCWriteStream functions.
	 *
	 * \{
	 */
	public enum GDFileSystemErr : uint {
		GDFileSystemErrPathDoesntExist = 100,
		GDFileSystemErrIOError = 101,
		GDFileSystemErrPermissionError = 102,
		GDFileSystemErrDirNotEmpty = 103,
		GDFileSystemErrUnknownError = 500
	}

	/** \}
	 */ 

	/** \addtogroup iccconstants Good Inter-Container Communication Miscellaneous Constants
	* These miscellaneous constants can be used with the Good Inter-Container
	* Communication (ICC) sytem.
	*
	* For an overall description of ICC see the   \link GDService GDService class reference\endlink.
	* 
	* \{
	*/
	public enum GDTForegroundOption{
		EPreferMeInForeground,
		EPreferPeerInForeground,
		ENoForegroundPreference
	}
	/** \}
	 */

	
	/** Constants for GDSocket errors.
	* This enumeration represents the type of a GDSocket error that is being
	* notified. The <TT>error</TT> parameter of the
	* \link GDSocketDelegate::onErr:inSocket: GDSocketDelegate::onErr:\endlink
	* callback always takes one of these values.
	*/
	public enum GDSocketErrorType{
		GDSocketErrorNone=0,
		/**< No error.
		 * This value is a placeholder for when the socket operation succeeded. The
		 * error parameter never takes this value.
		 */

		GDSocketErrorNetworkUnvailable,
		/**< Destination network not available.
		 * This value indicates that the socket operation failed because the
		 * destination network could not be reached.
		 */

		GDSocketErrorServiceTimeOut
		/**< Socket operation timed out.
		 * This value indicates that a socket operation timed out and did not
		 * complete.
		 */
	}

	/**
	 * \addtogroup gdauthtokendomain Good Dynamics Authentication Token Error Domain
	 * These constants can be used when handling Good Dynamics Authentication Token
	 * request errors, in a \link GDAuthTokenDelegate GDAuthTokenDelegate\endlink implementation.
	 *
	 * \{
	 */

	public enum GDAuthTokenErr {
		GDAuthTokenErrNotSupported = -2,
		/**< The version of the Good Dynamics servers installed at the enterprise
	     * does not support the Good Dynamics Authentication Token mechanism.
	     */
		GDAuthTokenErrRetry = -1
		/**< An error occurred during token generation or communication.
	     * Sending the same request later may not encounter the same condition, and
	     * could succeed.
	     */
	}
	/** \}
 	*/

	/** Constants for HTTP Request ready states.
	 * This enumeration represents the possible states of an HTTP request.
	 *
	 * \see http://www.w3.org/TR/XMLHttpRequest/#states
	 *
	 * Compare the value returned by \ref GDHttpRequest::getState "getState" to
	 * these constants to check the ready state of the GDHttpRequest object.
	 * (The XHR state names have been prefixed with <TT>GDHttpRequest_</TT> and
	 * the standard values used.)
	 */
	public enum GDHttpRequestState{
		GDHttpRequestUnsent = 0,
		/**< Prior to the request being opened. */
		GDHttpRequestOpened = 1,
		/**< Ready to have headers added, and be sent. */
		GDHttpRequestSent = 2,
		/**< The request has been sent. */
		GDHttpRequestHeadersReceived = 3,
		/**< Sent, and response headers have been received. */
		GDHttpRequestLoading = 4,
		/**< Response headers and some data have been received. */
		GDHttpRequestDone = 5
		/**< All data has been received, or a permanent error has been encountered. */
	}
}

