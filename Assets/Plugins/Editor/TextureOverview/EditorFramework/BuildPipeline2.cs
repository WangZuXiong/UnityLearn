using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000020 RID: 32
	public static class BuildPipeline2
	{
		// Token: 0x06000113 RID: 275 RVA: 0x0000952C File Offset: 0x0000772C
		static BuildPipeline2()
		{
			if (BuildPipeline2._getBuildTargetGroup == null)
			{
				Debug.LogWarning("Could not find method 'UnityEditor.BuildPipeline.GetBuildTargetGroup(BuildTarget)'.");
			}
			if (BuildPipeline2._getBuildTargetGroupName == null)
			{
				Debug.LogWarning("Could not find method 'UnityEditor.BuildPipeline.GetBuildTargetGroupName(BuildTarget)'.");
			}
			if (BuildPipeline2._getBuildTargetGroupDisplayName == null)
			{
				Debug.LogWarning("Could not find method 'UnityEditor.BuildPipeline.GetBuildTargetGroupDisplayName(BuildTargetGroup)'.");
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00009604 File Offset: 0x00007804
		public static BuildTargetGroup GetBuildTargetGroup(BuildTarget buildtarget)
		{
			if (BuildPipeline2._getBuildTargetGroup == null)
			{
				return 0;
			}
			object[] parameters = new object[]
			{
				buildtarget
			};
			return (BuildTargetGroup)BuildPipeline2._getBuildTargetGroup.Invoke(null, parameters);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00009640 File Offset: 0x00007840
		public static string GetBuildTargetGroupName(BuildTarget buildtarget)
		{
			if (BuildPipeline2._getBuildTargetGroupName == null)
			{
				return "";
			}
			object[] parameters = new object[]
			{
				buildtarget
			};
			return (BuildPipeline2._getBuildTargetGroupName.Invoke(null, parameters) as string) ?? "";
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00009688 File Offset: 0x00007888
		public static string GetBuildTargetGroupDisplayName(BuildTargetGroup group)
		{
			if (BuildPipeline2._getBuildTargetGroupDisplayName == null)
			{
				return "";
			}
			if (group == null)
			{
				return "Default";
			}
			object[] parameters = new object[]
			{
				group
			};
			return (BuildPipeline2._getBuildTargetGroupDisplayName.Invoke(null, parameters) as string) ?? "";
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000096D8 File Offset: 0x000078D8
		public static Texture GetBuildTargetGroupImage(BuildTargetGroup group)
		{
			List<BuildPlatform2> validPlatforms = BuildPlayerWindow2.GetValidPlatforms();
			foreach (BuildPlatform2 buildPlatform in validPlatforms)
			{
				if (buildPlatform.TargetGroup == group)
				{
					return buildPlatform.Image;
				}
			}
			return null;
		}

		// Token: 0x040000AC RID: 172
		private static MethodInfo _getBuildTargetGroup = typeof(BuildPipeline).GetMethod("GetBuildTargetGroup", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(BuildTarget)
		}, null);

		// Token: 0x040000AD RID: 173
		private static MethodInfo _getBuildTargetGroupName = typeof(BuildPipeline).GetMethod("GetBuildTargetGroupName", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(BuildTarget)
		}, null);

		// Token: 0x040000AE RID: 174
		private static MethodInfo _getBuildTargetGroupDisplayName = typeof(BuildPipeline).GetMethod("GetBuildTargetGroupDisplayName", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(BuildTargetGroup)
		}, null);
	}
}
