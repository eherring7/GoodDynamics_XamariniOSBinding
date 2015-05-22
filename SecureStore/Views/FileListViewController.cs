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
        private string currentPath = "/";

        public FileListViewController() : base("FileListViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            newDirectoryButton.Clicked += NewDirectoryButton_Clicked;

            LoadPath();
        }

        void NewDirectoryButton_Clicked (object sender, EventArgs e)
        {
            var alertView = new UIAlertView("Prompt", "Name for new Directory?", null, "Cancel", "Ok");
            alertView.AlertViewStyle = UIAlertViewStyle.PlainTextInput;

            alertView.Clicked += (object s, UIButtonEventArgs ev) => {
                if (ev.ButtonIndex == 1)
                {
                    //CreateDirectoryRefreshList(alertView.GetTextField(0).Text);
                }
            };

            alertView.Show();
        }

        private void LoadPath()
        {
            NSError error;

            Title = currentPath;
            var contents = GDFileSystem.ContentsOfDirectoryAtPath(currentPath, out error);

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

        private void CreateDirectoryRefreshDirectory(string newDirectoryName)
        {
            
        }
    }
}

