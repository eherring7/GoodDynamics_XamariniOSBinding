using System;
using System.Linq;
using Foundation;
using System.IO;
using GoodDynamics;

namespace SecureStore.Manager
{
    public class FileManager
    {
        public string[] SecureDocs;
        public string SyncDocFolder;

        public FileManager()
        {
        }

        public NSMutableArray FindSecureDocsAtPath(string path)
        {
            NSError error;
            var tempArray = GDFileSystem.ContentsOfDirectoryAtPath(path, out error);
            if (error == null)
            {
                SecureDocs = tempArray.Select(str => str.ToString()).OrderBy(str => str).ToArray();
            }
            else
            {
                Console.WriteLine("findSecureDocsAtPath error domain=%@ code=%ld userinfo=%@", error.Domain,
                    (long)error.Code, error.UserInfo.Description);
            }
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

