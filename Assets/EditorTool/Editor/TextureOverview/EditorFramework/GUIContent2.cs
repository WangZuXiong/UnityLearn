using System;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x0200003B RID: 59
	public static class GUIContent2
	{
		// Token: 0x060001C4 RID: 452 RVA: 0x0000CFBC File Offset: 0x0000B1BC
		public static GUIContent Temp(string text)
		{
			GUIContent2._content.text = text;
			GUIContent2._content.image = null;
			GUIContent2._content.tooltip = null;
			return GUIContent2._content;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000CFE4 File Offset: 0x0000B1E4
		public static GUIContent Temp(string text, Texture image)
		{
			GUIContent2._content.text = text;
			GUIContent2._content.image = image;
			GUIContent2._content.tooltip = null;
			return GUIContent2._content;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000D00C File Offset: 0x0000B20C
		public static GUIContent Temp(string text, Texture image, string tooltip)
		{
			GUIContent2._content.text = text;
			GUIContent2._content.image = image;
			GUIContent2._content.tooltip = tooltip;
			return GUIContent2._content;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000D034 File Offset: 0x0000B234
		public static GUIContent Temp(string text, string tooltip)
		{
			GUIContent2._content.text = text;
			GUIContent2._content.image = null;
			GUIContent2._content.tooltip = tooltip;
			return GUIContent2._content;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000D05C File Offset: 0x0000B25C
		public static GUIContent Temp(Texture image)
		{
			GUIContent2._content.text = null;
			GUIContent2._content.image = image;
			GUIContent2._content.tooltip = null;
			return GUIContent2._content;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000D084 File Offset: 0x0000B284
		public static GUIContent Temp(Texture image, string tooltip)
		{
			GUIContent2._content.text = null;
			GUIContent2._content.image = image;
			GUIContent2._content.tooltip = tooltip;
			return GUIContent2._content;
		}

		// Token: 0x040000E6 RID: 230
		private static GUIContent _content = new GUIContent();
	}
}
