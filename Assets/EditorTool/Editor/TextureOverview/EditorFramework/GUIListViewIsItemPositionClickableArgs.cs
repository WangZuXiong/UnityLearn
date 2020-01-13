using System;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000042 RID: 66
	public struct GUIListViewIsItemPositionClickableArgs
	{
		// Token: 0x040000FA RID: 250
		public object Model;

		// Token: 0x040000FB RID: 251
		public int ModelIndex;

		// Token: 0x040000FC RID: 252
		public Rect ItemRect;

		// Token: 0x040000FD RID: 253
		public GUIListViewColumn Column;

		// Token: 0x040000FE RID: 254
		public Vector2 MousePosition;
	}
}
