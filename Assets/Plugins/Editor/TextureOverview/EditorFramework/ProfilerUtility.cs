using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

namespace EditorFramework
{
	// Token: 0x02000066 RID: 102
	public static class ProfilerUtility
	{
		// Token: 0x06000265 RID: 613 RVA: 0x0002C9DA File Offset: 0x0002ABDA
		[Conditional("ENABLE_PROFILER")]
		public static void BeginSample(string name)
		{
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0002C9DC File Offset: 0x0002ABDC
		[Conditional("ENABLE_PROFILER")]
		public static void EndSample()
		{
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0002C9E0 File Offset: 0x0002ABE0
		public static long GetRuntimeMemorySize(Object o)
		{
			return Profiler.GetRuntimeMemorySizeLong(o);
		}
	}
}
