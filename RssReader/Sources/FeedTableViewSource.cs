using System;
using UIKit;
using System.Collections.Generic;
using RssReader.Models;
using CoreGraphics;
using Foundation;

namespace RssReader.Sources
{
    public class FeedTableViewSource: UITableViewSource
    {
        private const string _cellIdentifier = "TableCell";

        public IList<NewsItem> Items { get; set;}

        public FeedTableViewSource(IList<NewsItem> items)
        {
            this.Items = items;
        }

        //Returns the required number of sections, 1 for this app
        public override nint NumberOfSections(UITableView tableView)
        {
            //Returns the number of sections
            return 1;
        }

        //Returns the required number of cells for the table
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Items.Count;
        }

        //Called for each cell when the table loads. We compute the height depending on who much text is in the story.
        public override nfloat GetHeightForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            NSString desc = new NSString((Items[indexPath.Row].Description.Trim()));
            var maxSize = new CGSize();

            // check for orientation here as we are using the screen size to calculate height
            // needed by the cell
            if ((UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight) || (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft))
            {
                maxSize.Width = UIScreen.MainScreen.Bounds.Size.Height;
            }
            else
            {
                maxSize.Width = UIScreen.MainScreen.Bounds.Size.Width;
            }

            maxSize.Height = float.MaxValue;
            CGSize theSize = desc.StringSize(UIFont.FromName("Helvetica", 13f), maxSize, UILineBreakMode.WordWrap);

            return theSize.Height + 60; // add 60.0 for the title
        }

        //Called for each cell when the table loads. We use it to load each cell with a story from the array.
        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(_cellIdentifier);

            // if there are no cells to reuse, create a new one
            if (cell == null)
                cell = new UITableViewCell (UITableViewCellStyle.Subtitle, _cellIdentifier);
            
            cell.SelectionStyle = UITableViewCellSelectionStyle.Gray;
//
            // Set up the cell
            int index = indexPath.Row;
//
            //check the list containing our NewsItems
            var title = Items[index].Title;
            // trim here as we may have some extra whitespace
            var desc = Items[index].Description.Trim();
//
            // add the title and description
            cell.TextLabel.Text = title;
            cell.TextLabel.Lines = 2; // allows the title to wrap to 2 lines
            cell.TextLabel.TextColor = UIColor.Blue; // make the title blue
            cell.DetailTextLabel.Text = desc;
            cell.DetailTextLabel.Lines = 0; // unlimited lines for the story
            cell.DetailTextLabel.TextColor = UIColor.Black;
            cell.TextLabel.Font = UIFont.FromName("Helvetica",14f);
            cell.DetailTextLabel.Font = UIFont.FromName("Helvetica",12f);
            cell.ImageView.Image = null;
            return cell; 
        }
    }
}

