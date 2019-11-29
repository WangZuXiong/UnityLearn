using System;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x0200005D RID: 93
	public class GUIToolbarPopupItem
	{
		// Token: 0x0600021E RID: 542 RVA: 0x0000E9C5 File Offset: 0x0000CBC5
		public GUIToolbarPopupItem(string text)
		{
			this.Text = new GUIContent(text);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000E9D9 File Offset: 0x0000CBD9
		public GUIToolbarPopupItem(string text, object tag)
		{
			this.Text = new GUIContent(text);
			this.Tag = tag;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000E9F4 File Offset: 0x0000CBF4
		public override string ToString()
		{
			return this.Text.text;
		}

		// Token: 0x04000153 RID: 339
		public GUIContent Text;

		// Token: 0x04000154 RID: 340
		public object Tag;
	}
}
