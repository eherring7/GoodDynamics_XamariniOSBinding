
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
        public ProductsView()
            : base("ProductsView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            goodImage.Image = new UIImage("images/GDLogo");
            goodImage.ContentMode = UIViewContentMode.ScaleAspectFit;
            var tempList = new List<Product>()
            {
                new Product(){ Name = "Bananas", Category = ProductCategory.Food, Price = 0.79f },
                new Product(){ Name = "T-Shirt", Category = ProductCategory.Clothing, Price = 10.01f },
                new Product(){ Name = "Headphones", Category = ProductCategory.Electronic, Price = 71.98f }
            };
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
            var newProductView = new EditProductView(p);
            NavigationController.PushViewController(newProductView, true);
        }


    }
}

