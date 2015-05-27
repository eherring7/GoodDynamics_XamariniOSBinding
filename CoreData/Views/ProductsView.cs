
using System;

using Foundation;
using UIKit;
using CoreData.Sources;
using System.Collections.Generic;
using CoreData.Models;

namespace CoreData.Views
{
    public partial class ProductsView : UIViewController
    {
        private NSManagedObjectContext _context;

        public ProductsView(NSManagedObjectContext context): base("ProductsView", null)
        {
            this._context = context;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            goodImage.Image = new UIImage("images/GDLogo");
            goodImage.ContentMode = UIViewContentMode.ScaleAspectFit;
            var tempList = new List<Product>();

            var prod = Product.CreateNewBook(_context);
            prod.Name = "Bananas";
            prod.Category = ProductCategory.Food;
            prod.Price = 0.79f;
            tempList.Add(prod);

            prod = Product.CreateNewBook(_context);
            prod.Name = "T-Shirt";
            prod.Category = ProductCategory.Clothing;
            prod.Price = 10.01f;
            tempList.Add(prod);

            prod = Product.CreateNewBook(_context);
            prod.Name = "Headphones";
            prod.Category = ProductCategory.Electronic;
            prod.Price = 71.98f;
            tempList.Add(prod);

            var source = new ProductTableViewSource(tempList);
            source.OnRowSelected += (s, e) => ToProductEditMode(e.Product);
            productsTable.Source = source;

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add, (s,e) => ToProductEditMode(null));

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            productsTable.ReloadData();
        }

        private void ToProductEditMode(Product p)
        {
            var newProductView = new EditProductView(p, _context);
            NavigationController.PushViewController(newProductView, true);
        }


    }
}

