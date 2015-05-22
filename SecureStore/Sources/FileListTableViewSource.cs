using System;
using UIKit;

namespace SecureStore.Sources
{
    public class FileListTableViewSource : UITableViewSource
    {
        public FileListTableViewSource()
        {
        }

        #region implemented abstract members of UITableViewSource

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            throw new NotImplementedException();
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

