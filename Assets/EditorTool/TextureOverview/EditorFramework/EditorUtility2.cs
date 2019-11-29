using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EditorFramework
{
	// Token: 0x0200002D RID: 45
	public static class EditorUtility2
	{
		// Token: 0x06000168 RID: 360 RVA: 0x0000B120 File Offset: 0x00009320
		public static List<GameObject> GetSceneRootObjects()
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				if (sceneAt.IsValid())
				{
					sceneAt.GetRootGameObjects(list);
				}
			}
			return list;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000B15C File Offset: 0x0000935C
		public static string FormatBytes(long bytes)
		{
			if (bytes < 100L)
			{
				return string.Format("{0} Bytes", bytes);
			}
			if ((float)bytes < 1048576f)
			{
				return string.Format("{0:F1} KB", (float)bytes / 1024f);
			}
			if ((double)bytes < 1073741824.0)
			{
				return string.Format("{0:F1} MB", (float)bytes / 1048576f);
			}
			if ((double)bytes < 1099511627776.0)
			{
				return string.Format("{0:F2} GB", (double)bytes / 1073741824.0);
			}
			return string.Format("{0:F2} TB", (double)bytes / 1099511627776.0);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000B20A File Offset: 0x0000940A
		public static void UnloadUnusedAssetsImmediate()
		{
			EditorUtility.UnloadUnusedAssetsImmediate();
		}
	}
}
