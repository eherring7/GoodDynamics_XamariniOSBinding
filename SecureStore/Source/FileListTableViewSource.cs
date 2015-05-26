using System;
using UIKit;
using SecureStore.File;
using System.Collections.Generic;
using Foundation;

namespace SecureStore
{
    public class FileListTableViewSource : UITableViewSource
    {
        public FileManager FileManager { get; private set; }
        public IList<string> Datasource { get; private set; }
        public string CurrentPath { get; private set; }

        public Action<string> FolderSelectAction { get; private set; }

        public FileListTableViewSource(IList<string> files, Action<string> folderSelectAction, string currentPath)
        {
            FileManager = new FileManager();
            Datasource = files;
            CurrentPath = currentPath;

            FolderSelectAction = folderSelectAction;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (Datasource == null)
                return 0;

            return Datasource.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            const string identifier = "Portrait";
            var cell = tableView.DequeueReusableCell(identifier);
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, identifier);
            }

            cell.TextLabel.Text = Datasource[indexPath.Row];

            return cell;
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var item = Datasource[indexPath.Row];
            FolderSelectAction(item);
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        {
            if (editingStyle == UITableViewCellEditingStyle.Delete)
            {
                string filename = FileManager.FindSecureDocsAtPath(CurrentPath)[indexPath.Row];
                string filepath = (new NSString(CurrentPath).AppendPathComponent(new NSString(filename))).ToString();

                if (FileManager.RemoveFile(filepath))
                {
                    Datasource.RemoveAt(indexPath.Row);
                    tableView.DeleteRows(new[] { indexPath }, UITableViewRowAnimation.Fade);
                }
            }
        }
    }
}

