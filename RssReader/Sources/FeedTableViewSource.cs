using System;
using UIKit;

namespace RssReader.Sources
{
    public class FeedTableViewSource: UITableViewSource
    {
        public FeedTableViewSource(){}


        //Returns the required number of sections, 1 for this app
        public override nint NumberOfSections(UITableView tableView)
        {
            //Returns the number of sections
            return 1;
        }

        //Returns the required number of cells for the table
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return [storyArray count];
        }

        //Called for each cell when the table loads. We compute the height depending on who much text is in the story.
        public override nfloat GetHeightForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            NSString* desc = [[[storyArray objectAtIndex:indexPath.row] objectForKey:@"description"] stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
            CGSize maxSize;

            // check for orientation here as we are using the screen size to calculate height
            // needed by the cell
            if (([UIApplication sharedApplication].statusBarOrientation == UIInterfaceOrientationLandscapeRight) || ([UIApplication sharedApplication].statusBarOrientation == UIInterfaceOrientationLandscapeLeft))
            {
                maxSize.width = [[UIScreen mainScreen] bounds].size.height;
            }
            else
            {
                maxSize.width = [[UIScreen mainScreen] bounds].size.width;
            }

            maxSize.height = MAXFLOAT;
            CGSize theSize = [desc sizeWithFont:[UIFont systemFontOfSize:13.0f] constrainedToSize:maxSize lineBreakMode:NSLineBreakByWordWrapping];

            return theSize.height + 60.0; // add 60.0 for the title
        }

        //Called for each cell when the table loads. We use it to load each cell with a story from the array.
        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            static NSString* CellIdentifier = @"Cell";
            UITableViewCell* cell = [tableView dequeueReusableCellWithIdentifier:CellIdentifier];
            if (cell == nil)
            {
                #if !(__has_feature(objc_arc))
                cell = [[[UITableViewCell alloc] initWithStyle:UITableViewCellStyleSubtitle reuseIdentifier:CellIdentifier] autorelease];
                #else
                cell = [[UITableViewCell alloc] initWithStyle:UITableViewCellStyleSubtitle reuseIdentifier:CellIdentifier];
                #endif

                [cell setSelectionStyle:UITableViewCellSelectionStyleGray];
            }

            // Set up the cell
            int index = indexPath.row;

            // check the dictionary containing our parsed elements
            NSString* title = [[storyArray objectAtIndex:index] objectForKey:@"title"];
            // trim here as we may have some redundant preceding CR, LF or whitespaces
            NSString* desc = [[[storyArray objectAtIndex:index] objectForKey:@"description"] stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];

            // add the title and description
            cell.textLabel.text = title;
            cell.textLabel.numberOfLines = 2; // allows the title to wrap to 2 lines
            cell.textLabel.textColor = [UIColor blueColor]; // make the title blue
            cell.detailTextLabel.text = desc;
            cell.detailTextLabel.numberOfLines = 0; // unlimited lines for the story
            cell.detailTextLabel.textColor = [UIColor blackColor];
            cell.textLabel.font = [UIFont fontWithName:@"Helvetica" size:14.0];
            cell.detailTextLabel.font = [UIFont fontWithName:@"Helvetica" size:12.0];
            cell.imageView.image  = nil;
            return cell;        
        }
    }
}

