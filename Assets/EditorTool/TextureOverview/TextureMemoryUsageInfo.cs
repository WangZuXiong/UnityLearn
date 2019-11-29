using System;
using System.Collections.Generic;

namespace TextureOverview
{
	// Token: 0x0200007C RID: 124
	public class TextureMemoryUsageInfo
	{
		// Token: 0x04000246 RID: 582
		public long TotalSize;

		// Token: 0x04000247 RID: 583
		public List<TextureMemoryUsageData> UsagePerType = new List<TextureMemoryUsageData>();
	}
}
