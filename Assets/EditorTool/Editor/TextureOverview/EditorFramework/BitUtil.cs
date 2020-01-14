using System;
using System.Diagnostics;

namespace EditorFramework
{
	// Token: 0x0200001F RID: 31
	public static class BitUtil
	{
		// Token: 0x06000111 RID: 273 RVA: 0x000094DE File Offset: 0x000076DE
		static BitUtil()
		{
			Debug.Assert(2018915346u == BitUtil.SwapBytes(305419896u), "SwapBytes(UInt32 value) failed.");
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000094FB File Offset: 0x000076FB
		public static uint SwapBytes(uint value)
		{
			value = ((value & 4278190080u) >> 24 | (value & 16711680u) >> 8 | (value & 65280u) << 8 | (value & 255u) << 24);
			return value;
		}
	}
}
