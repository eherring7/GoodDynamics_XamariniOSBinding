
using System;
using System.Linq;
using Foundation;
using UIKit;
using GoodDynamics;
using SecureStore.Sources;
using System.IO;

namespace SecureStore.Views
{
    public partial class FileListViewController : UIViewController
    {
        public FileListViewController() : base("FileListViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            LoadPath("/");
        }

        private void LoadPath(string path)
        {
            NSError error;

            Title = path;
            var contents = GDFileSystem.ContentsOfDirectoryAtPath("/", out error);

            if (error == null)
            {
                // load the table
                fileTableList.Source = new FileListTableViewSource(
                    contents.Select(fp => {
                        GDFileStat fileStat = new GDFileStat();
                        NSError theError;

                        GDFileSystem.GetFileStat(fp.ToString(), fileStat, out theError);
                        return new Tuple<string, long>(
                            new NSString(fp.ToString()).LastPathComponent.ToString(),
                            fileStat.fileLen);
                    }).ToArray()
                );
            }
        }
    }
}

