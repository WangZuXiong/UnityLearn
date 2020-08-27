using System;
using System.Collections.Generic;

namespace EditorFramework
{
	// Token: 0x0200001B RID: 27
	public sealed class AssetFileNameComparer : IComparer<string>
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x00008FB0 File Offset: 0x000071B0
		public AssetFileNameComparer(int greaterResult, int smallerResult)
		{
			this._greaterResult = greaterResult;
			this._smallerResult = smallerResult;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00008FD4 File Offset: 0x000071D4
		public int Compare(string x, string y)
		{
			x = FileUtil2.GetFileName(x);
			y = FileUtil2.GetFileName(y);
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

		// Token: 0x0400009E RID: 158
		private int _greaterResult = 1;

		// Token: 0x0400009F RID: 159
		private int _smallerResult = -1;

		// Token: 0x040000A0 RID: 160
		public static readonly AssetFileNameComparer Ascending = new AssetFileNameComparer(1, -1);

		// Token: 0x040000A1 RID: 161
		public static readonly AssetFileNameComparer Descending = new AssetFileNameComparer(-1, 1);
	}
}
