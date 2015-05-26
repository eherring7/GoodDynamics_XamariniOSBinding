using System;
using UIKit;
using CoreData.Cells;
using CoreData.Models;
using System.Collections.Generic;
using CoreData.Views;

namespace CoreData.Sources
{
    public class ProductTableViewSource: UITableViewSource
    {
        private readonly string _cellIdentifier = ProductTableViewCell.Key;

        public EventHandler<ProductEventArgs> OnRowSelected;

        public IList<Product> Products{ get; set; }

        public ProductTableViewSource(IList<Product> products)
        {
            this.Products = products;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Products.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = (ProductTableViewCell)tableView.DequeueReusableCell(_cellIdentifier);
            if (cell == null)
            {
                cell = ProductTableViewCell.Create();
            }
            cell.Name = Products[indexPath.Row].Name;
            cell.Price = String.Format("{0:C}", Products[indexPath.Row].Price);
            cell.Category = Enum.GetName(typeof(ProductCategory),Products[indexPath.Row].Category);
            return cell;
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            if (OnRowSelected != null)
            {
                OnRowSelected(this, new ProductEventArgs(Products[indexPath.Row])); 
            }
        }
    }

    public class ProductEventArgs : EventArgs
    {
        private readonly Product _product;

        public ProductEventArgs(Product product)
        {
            _product = product;
        }

        public Product Product
        {
            get { return _product; }
        }
    }

}

