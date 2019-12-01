using System;
using System.Collections.Generic;

namespace EditorFramework
{
	// Token: 0x0200001C RID: 28
	public sealed class AssetFilePathComparer : IComparer<string>
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x00009025 File Offset: 0x00007225
		public AssetFilePathComparer(int greaterResult, int smallerResult)
		{
			this._greaterResult = greaterResult;
			this._smallerResult = smallerResult;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00009049 File Offset: 0x00007249
		public int Compare(string x, string y)
		{
			if (string.Compare(x, y, StringComparison.OrdinalIgnoreCase) > 0)
			{
				return this._greaterResult;
			}
			if (string.Compare(x, y, StringComparison.OrdinalIgnoreCase) < 0)
			{
				return this._smallerResult;
			}
			return 0;
		}

		// Token: 0x040000A2 RID: 162
		private int _greaterResult = 1;

		// Token: 0x040000A3 RID: 163
		private int _smallerResult = -1;

		// Token: 0x040000A4 RID: 164
		public static readonly AssetFilePathComparer Ascending = new AssetFilePathComparer(1, -1);

		// Token: 0x040000A5 RID: 165
		public static readonly AssetFilePathComparer Descending = new AssetFilePathComparer(-1, 1);
	}
}
