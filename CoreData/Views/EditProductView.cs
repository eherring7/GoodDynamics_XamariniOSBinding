
using System;

using Foundation;
using UIKit;
using CoreData.Models;
using CoreData.Sources;
using CoreGraphics;

namespace CoreData.Views
{
    public partial class EditProductView : UIViewController
    {
        private Product _product;
        private bool isNewProduct = false;
        private NSManagedObjectContext _context;

        public EditProductView(Product product, NSManagedObjectContext context): base("EditProductView", null)
        {
            this._context = context;
            this._product = product;
            if (_product == null)
            {
                //This is a new Product
                this._product = Product.CreateNewBook(this._context);
                this.isNewProduct = true;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationItem.Title = "Product";

            var model = new CategoryPickerViewModel(_product);
            CategoryPicker.Model = model;

            if (isNewProduct)
            {
                //Will start in edit mode
                NavigationItem.SetHidesBackButton(true, true);
                NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done, HandleDoneButtonPressed);
                categoryBoarder.Hidden = true;
            }
            else
            {
                NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Edit, HandleEditButtonPressed);
                nameField.Enabled = false;
                priceField.Enabled = false;
                quantityField.Enabled = false;
                CategoryPicker.Hidden = true;
                categoryBoarder.Hidden = false;

                nameField.Text = _product.Name;
                priceField.Text = _product.Price.ToString();
                quantityField.Text = _product.Quantity.ToString();
                categoryName.Text = Enum.GetName(typeof(ProductCategory), _product.Category);
            }
        }

        private void HandleDoneButtonPressed(object sender, EventArgs e)
        {
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Edit, HandleEditButtonPressed);

            categoryName.Text = Enum.GetName(typeof(ProductCategory), _product.Category);

            //Hide and show UI elements
            NavigationItem.SetHidesBackButton(false, true);
            nameField.Enabled = false;
            priceField.Enabled = false;
            quantityField.Enabled = false;
            CategoryPicker.Hidden = true;
            categoryBoarder.Hidden = false;

            //Assign changed fields back into the product
            _product.Name = nameField.Text;
            _product.Price = (nfloat) Double.Parse(priceField.Text);
            _product.Quantity = (nint) Int32.Parse(quantityField.Text);
        }

        private void HandleEditButtonPressed(object sender, EventArgs e)
        {
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done, HandleDoneButtonPressed);

            //Hide and show UI elements
            NavigationItem.SetHidesBackButton(true, true);
            nameField.Enabled = true;
            priceField.Enabled = true;
            quantityField.Enabled = true;
            CategoryPicker.Hidden = false;
            categoryBoarder.Hidden = true;
            ((CategoryPickerViewModel)CategoryPicker.Model).Select(CategoryPicker, _product.Category);

        }
    }
}

