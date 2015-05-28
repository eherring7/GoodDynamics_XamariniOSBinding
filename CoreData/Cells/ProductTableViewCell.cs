
using System;

using Foundation;
using UIKit;

namespace CoreData.Cells
{
    public partial class ProductTableViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("ProductTableViewCell");
        public static readonly UINib Nib;

        public String Name
        {
            get
            {
                return productName.Text;
            }
            set
            {
                productName.Text = value;
            }
        }

        public String Price
        {
            get
            {
                return ProductPrice.Text;
            }
            set
            {
                ProductPrice.Text = value;
            }
        }

        public String Category
        {
            get
            {
                return productCategory.Text;
            }
            set
            {
                productCategory.Text = value;
            }
        }

        static ProductTableViewCell()
        {
            Nib = UINib.FromName("ProductTableViewCell", NSBundle.MainBundle);
        }

        public ProductTableViewCell(IntPtr handle)
            : base(handle)
        {
        }

        public static ProductTableViewCell Create()
        {
            return (ProductTableViewCell)Nib.Instantiate(null, null)[0];
        }
    }
}

