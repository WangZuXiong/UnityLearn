using System;
using System.Collections.Generic;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000041 RID: 65
	public struct GUIListViewDrawItemBackgroundArgs
	{
		// Token: 0x040000F1 RID: 241
		public object Model;

		// Token: 0x040000F2 RID: 242
		public int ModelIndex;

		// Token: 0x040000F3 RID: 243
		public Rect Rect;

		// Token: 0x040000F4 RID: 244
		public GUIListViewColumn Column;

		// Token: 0x040000F5 RID: 245
		public bool Selected;

		// Token: 0x040000F6 RID: 246
		public List<object> SelectedItems;

		// Token: 0x040000F7 RID: 247
		public Color Color;

		// Token: 0x040000F8 RID: 248
		public Texture Texture;

		// Token: 0x040000F9 RID: 249
		public bool Handled;
	}
}
