// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace CoreData.Views
{
	[Register ("EditProductView")]
	partial class EditProductView
	{
		[Outlet]
		UIKit.UIView categoryBoarder { get; set; }

		[Outlet]
		UIKit.UILabel categoryName { get; set; }

		[Outlet]
		UIKit.UIPickerView CategoryPicker { get; set; }

		[Outlet]
		UIKit.UITextField nameField { get; set; }

		[Outlet]
		UIKit.UITextField priceField { get; set; }

		[Outlet]
		UIKit.UITextField quantityField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CategoryPicker != null) {
				CategoryPicker.Dispose ();
				CategoryPicker = null;
			}

			if (nameField != null) {
				nameField.Dispose ();
				nameField = null;
			}

			if (priceField != null) {
				priceField.Dispose ();
				priceField = null;
			}

			if (quantityField != null) {
				quantityField.Dispose ();
				quantityField = null;
			}

			if (categoryName != null) {
				categoryName.Dispose ();
				categoryName = null;
			}

			if (categoryBoarder != null) {
				categoryBoarder.Dispose ();
				categoryBoarder = null;
			}
		}
	}
}
