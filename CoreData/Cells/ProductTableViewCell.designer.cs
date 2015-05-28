// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace CoreData.Cells
{
	[Register ("ProductTableViewCell")]
	partial class ProductTableViewCell
	{
		[Outlet]
		UIKit.UILabel productCategory { get; set; }

		[Outlet]
		UIKit.UILabel productName { get; set; }

		[Outlet]
		UIKit.UILabel ProductPrice { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (productName != null) {
				productName.Dispose ();
				productName = null;
			}

			if (ProductPrice != null) {
				ProductPrice.Dispose ();
				ProductPrice = null;
			}

			if (productCategory != null) {
				productCategory.Dispose ();
				productCategory = null;
			}
		}
	}
}
