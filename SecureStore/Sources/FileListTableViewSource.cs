using System;
using UIKit;
using Foundation;

namespace SecureStore.Sources
{
    public class FileListTableViewSource : UITableViewSource
    {
        private NSObject[] _directoryContents;
        private readonly string cellIdentifier = "TableCell";

        public FileListTableViewSource(NSObject[] directoryContents)
        {
            _directoryContents = directoryContents;
        }

        #region implemented abstract members of UITableViewSource

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
            // if there are no cells to reuse, create a new one
            if (cell == null)
                cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
            
            var obj = _directoryContents[indexPath.Row];
            cell.TextLabel.Text = obj.ToString();

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (_directoryContents == null)
                return 0;

            return _directoryContents.Length;
        }

        #endregion
    }
}

