using System;
using System.Collections.Generic;
using Foundation;
using GoodDynamics;
using System.Linq;
using System.IO;

namespace SecureStore.File
{
	public class FileManager
	{
		public IList<string> SecureDocs { get; private set; }
		public string SyncDocFolder { get; private set; }

		public FileManager ()
		{
			SyncDocFolder = GetSyncDocFolder ();
		}

		public IList<string> FindSecureDocsAtPath (string currentPath)
		{
			NSError error;
			var files = GDFileSystem.ContentsOfDirectoryAtPath(currentPath, out error)
				.Select(r => new NSString(r.ToString())).ToList();

			if (files.Count > 0)
			{
				SecureDocs = files.Select (str => str.ToString ()).OrderBy (str => str).ToList ();
			}
			else
			{
				if (error != null)
				{
					Console.WriteLine("FindSecureDocsAtPath error domain={0} code={1:d} userinfo={2}", error.Domain,
						(long)error.Code, error.UserInfo.Description);
				}
			}

			return SecureDocs;
		}

		public FileResult GetFileStat(string filePath)
		{
			GDFileStat fileStat = new GDFileStat();
			NSError error;

			if (GDFileSystem.GetFileStat(filePath, fileStat, out error))
			{
				return new FileResult()
				{
					Filename = new NSString(filePath).LastPathComponent,
					IsFolder = fileStat.isFolder
				};
			}
			else
			{
				if (error != null)
				{
					Console.WriteLine("GetFileStat error domain={0} code={1:d} userinfo={2}",
						error.Domain, (long)error.Code,
						error.UserInfo.Description);
				}
			}

			return null;
		}

		string GetSyncDocFolder ()
		{
			var folderUrl = NSFileManager.DefaultManager.GetUrls (
				NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User) [0];

			return folderUrl.AbsoluteString;
		}

		public bool FileExists(string filePath, bool isDirectory)
		{
			return GDFileSystem.FileExistsAtPath(filePath, isDirectory);
		}

        public bool CreateDirectory(string path, bool withIntermediateDirectories, NSDictionary attributes, ref NSError error)
		{
            return GDFileSystem.CreateDirectoryAtPath(path, withIntermediateDirectories, attributes, out error);
		}

        public bool WriteToFile(NSData data, string fileName)
        {
            NSError error;
            var result = GDFileSystem.WriteToFile(data, fileName, out error);

            if (!result && error != null)
            {
                Console.WriteLine("WriteToFile error domain={0} code={1:d} userinfo={2}",
                    error.Domain, (long)error.Code,
                    error.UserInfo.Description);
            }

            return result;
        }

        public NSData ReadFile(string fileName, ref NSError error)
        {
            return GDFileSystem.ReadFromFile(fileName, out error);
        }

        public bool RemoveFile(string filePath)
        {
            NSError error;
            var result = GDFileSystem.RemoveItemAtPath(filePath, out error);
            if (!result && error != null)
            {
                Console.WriteLine("RemoveFile error domain={0} code={1:d} userinfo={2}",
                    error.Domain, (long)error.Code, error.UserInfo.Description);
            }

            return result;
        }
	}
}
