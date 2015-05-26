using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecureStore
{
	public class SyncManager
	{
		public SyncManager ()
		{
		}

		public IList<string> GetFilesToSync ()
		{
			throw new NotImplementedException ();
		}

		public Task SyncFileAsync (string file)
		{
			throw new NotImplementedException ();
		}
	}
}

