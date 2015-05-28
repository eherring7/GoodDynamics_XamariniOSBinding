using System;
using UIKit;
using CoreData.Models;
using System.Collections.Generic;

namespace CoreData.Sources
{

    public class CategoryPickerViewModel: UIPickerViewModel
    {
        private Product _product;
        private IList<string> _enumNames;

        public CategoryPickerViewModel(Product product)
        {
            this._product = product;
            _enumNames = Enum.GetNames(typeof(ProductCategory));
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return _enumNames[(int)row];
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return _enumNames.Count;
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            _product.Category = (ProductCategory)Enum.Parse(typeof(ProductCategory), _enumNames[(int)row]);
        }

        public void Select(UIPickerView pickerView, ProductCategory category)
        {
            var indexOfItem = _enumNames.IndexOf(Enum.GetName(typeof(ProductCategory),category));
            pickerView.Select(indexOfItem, 0, true);
        }

    }
}

