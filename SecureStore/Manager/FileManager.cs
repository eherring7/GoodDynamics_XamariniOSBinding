using System;
using Foundation;
using System.IO;

namespace SecureStore.Manager
{
    public class FileManager
    {
        public NSMutableArray SecureDocs;
        public string SyncDocFolder;

        public FileManager()
        {
        }

        public NSMutableArray FindSecureDocsAtPath(string path)
        {
            return new NSMutableArray(0);
        }

        public static NSData ReadFile(string filePath)
        {
            return null;
        }

        public static bool RemoveFile(string filePath)
        {
            return false;
        }

        public static bool FileExists(string filePath)
        {
            return false;
        }

        public static bool WriteToFile(NSData data, string fileName)
        {
            return false;
        }

        public static FileInfo GetFileInfo(string filePath)
        {
            return null;
        }

        public static bool CreateDirectory(string directoryName)
        {
            return false;
        }

        public static string GetSyncDocFolder()
        {
            return string.Empty;
        }

        public static bool IsFileSupported(string fileExtension)
        {
            return false;
        }
    }
}

