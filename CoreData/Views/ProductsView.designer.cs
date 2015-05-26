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
	[Register ("ProductsView")]
	partial class ProductsView
	{
		[Outlet]
		UIKit.UIImageView goodImage { get; set; }

		[Outlet]
		UIKit.UITableView productsTable { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (productsTable != null) {
				productsTable.Dispose ();
				productsTable = null;
			}

			if (goodImage != null) {
				goodImage.Dispose ();
				goodImage = null;
			}
		}
	}
}
