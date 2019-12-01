using System;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x0200001D RID: 29
	public class AssetFileWatcher : AssetPostprocessor
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000F7 RID: 247 RVA: 0x0000908C File Offset: 0x0000728C
		// (remove) Token: 0x060000F8 RID: 248 RVA: 0x000090C0 File Offset: 0x000072C0
		public static event Action<string[]> Imported;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000F9 RID: 249 RVA: 0x000090F4 File Offset: 0x000072F4
		// (remove) Token: 0x060000FA RID: 250 RVA: 0x00009128 File Offset: 0x00007328
		public static event Action<string[]> Deleted;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060000FB RID: 251 RVA: 0x0000915C File Offset: 0x0000735C
		// (remove) Token: 0x060000FC RID: 252 RVA: 0x00009190 File Offset: 0x00007390
		public static event Action<string[], string[]> Moved;

		// Token: 0x060000FD RID: 253 RVA: 0x000091C4 File Offset: 0x000073C4
		private static void LogIt(string title, string[] list)
		{
			Debug.Log(title);
			if (list != null)
			{
				foreach (string text in list)
				{
					Debug.Log(text);
				}
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000091F4 File Offset: 0x000073F4
		private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
		{
			if (deletedAssets != null && deletedAssets.Length > 0)
			{
				if (AssetFileWatcher.LogEnabled)
				{
					AssetFileWatcher.LogIt("AssetFileWatcher.Deleted", deletedAssets);
				}
				if (AssetFileWatcher.Deleted != null)
				{
					AssetFileWatcher.Deleted(deletedAssets);
				}
			}
			if (movedFromPath != null && movedFromPath.Length > 0)
			{
				if (AssetFileWatcher.LogEnabled)
				{
					AssetFileWatcher.LogIt("AssetFileWatcher.Moved", movedAssets);
				}
				if (AssetFileWatcher.Moved != null)
				{
					AssetFileWatcher.Moved(movedFromPath, movedAssets);
				}
			}
			if (importedAssets != null && importedAssets.Length > 0)
			{
				if (AssetFileWatcher.LogEnabled)
				{
					AssetFileWatcher.LogIt("AssetFileWatcher.Imported", importedAssets);
				}
				if (AssetFileWatcher.Imported != null)
				{
					AssetFileWatcher.Imported(importedAssets);
				}
			}
		}

		// Token: 0x040000A9 RID: 169
		public static bool LogEnabled;
	}
}
