using System;
using UIKit;
using Foundation;
using System.IO;

namespace SecureStore.Sources
{
    public class FileListTableViewSource : UITableViewSource
    {
        private Tuple<string, long>[] _directoryContents;
        private readonly string cellIdentifier = "TableCell";

        public FileListTableViewSource(Tuple<string, long>[] directoryContents)
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
            
            var tuple = _directoryContents[indexPath.Row];
            cell.TextLabel.Text = string.Format("{0} ({1})",
                tuple.Item1, tuple.Item2);

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

