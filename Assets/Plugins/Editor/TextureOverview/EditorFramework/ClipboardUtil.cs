using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;

namespace EditorFramework
{
	// Token: 0x02000025 RID: 37
	public static class ClipboardUtil
	{
		// Token: 0x0600012D RID: 301 RVA: 0x0000A04F File Offset: 0x0000824F
		public static void Copy(string text)
		{
			EditorGUIUtility.systemCopyBuffer = text;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000A058 File Offset: 0x00008258
		public static void CopyPaths(IEnumerable<string> paths)
		{
			StringBuilder stringBuilder = new StringBuilder(8192);
			foreach (string text in paths)
			{
				if (text != null)
				{
					string value = text.Trim();
					if (!string.IsNullOrEmpty(value))
					{
						stringBuilder.AppendLine(value);
					}
				}
			}
			ClipboardUtil.Copy(stringBuilder.ToString().Trim());
		}
	}
}
