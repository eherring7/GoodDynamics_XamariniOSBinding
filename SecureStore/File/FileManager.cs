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
				return files.Select (str => str.ToString ()).OrderBy (str => str).ToList ();
			}
			else
			{
				if (error != null)
				{
					Console.WriteLine("FindSecureDocsAtPath error domain={0} code={1:d} userinfo={2}", error.Domain,
						(long)error.Code, error.UserInfo.Description);
				}
			}

            return new List<string>();
		}

		public FileResult GetFileStat(string filePath)
		{
			GDFileStat fileStat = new GDFileStat();
			NSError error;

            if (GDFileSystem.GetFileStat(filePath, ref fileStat, out error))
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

        public bool CreateDirectory(string path, bool withIntermediateDirectories, NSDictionary attributes, NSError error)
		{
            return GDFileSystem.CreateDirectoryAtPath(path, withIntermediateDirectories, attributes, out error);
		}

        public bool CreateFile(string path)
        {
            NSError error = null;
            string contents = GetIpsumString();

            return GDFileSystem.WriteToFile(NSData.FromString(contents), path, out error);
        }

        private string GetIpsumString()
        {
            return @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse pharetra diam diam, at vulputate mi aliquam quis.
                Ut auctor ligula in tempus dignissim. Curabitur sit amet nisl a dui placerat mollis. Cras tempor ipsum at lacus aliquet, ac
                sodales lorem ornare. Aliquam vel ante imperdiet, dapibus augue sed, sagittis leo. Suspendisse eget pulvinar justo. Sed
                commodo nulla et leo dignissim iaculis. Vestibulum gravida leo non ex tempor, vitae tincidunt libero ullamcorper. In nulla 
                augue, rhoncus non consequat eget, lacinia in ipsum.

                Duis sollicitudin nec augue in mollis. In elementum tellus at libero bibendum, ac aliquet quam cursus. Curabitur aliquam
                venenatis mauris, at elementum lacus. Etiam posuere orci at ligula gravida consequat. Sed nec ipsum tristique, bibendum magna
                sed, suscipit risus. Nam consectetur lacus ac lorem molestie, quis egestas felis interdum. Ut fermentum consectetur lobortis.
                In ex ex, ornare quis sagittis eget, viverra eu ligula. Sed sed sem erat.";
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

        public NSData ReadFile(string fileName, NSError error)
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
