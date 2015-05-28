using System;
using Foundation;
using CoreData.Models;
using System.Collections.Generic;

namespace CoreData.Delegates
{
    public class FetchedResultsDelegate: NSFetchedResultsControllerDelegate
    {
        private IList<Product> _products;
        public FetchedResultsDelegate(IList<Product> products)
        {
            this._products = products;
        }

        public override void DidChangeObject(NSFetchedResultsController controller, NSObject anObject, NSIndexPath indexPath, NSFetchedResultsChangeType type, NSIndexPath newIndexPath)
        {
            if (indexPath.Row >= _products.Count)
            {

            }
        }
    }
}

