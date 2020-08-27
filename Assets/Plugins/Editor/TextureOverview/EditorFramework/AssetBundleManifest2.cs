using System;
using System.Collections.Generic;

namespace EditorFramework
{
	// Token: 0x02000003 RID: 3
	public class AssetBundleManifest2
	{
		// Token: 0x04000004 RID: 4
		public string Name;

		// Token: 0x04000005 RID: 5
		public string Path;

		// Token: 0x04000006 RID: 6
		public List<string> Assets = new List<string>();

		// Token: 0x04000007 RID: 7
		public List<string> Dependencies = new List<string>();
	}
}
