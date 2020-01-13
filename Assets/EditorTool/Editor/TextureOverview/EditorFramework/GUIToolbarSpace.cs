using System;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x0200005B RID: 91
	public class GUIToolbarSpace : GUIControl
	{
		// Token: 0x0600021A RID: 538 RVA: 0x0000E97D File Offset: 0x0000CB7D
		public GUIToolbarSpace(GUIToolbar toolbar) : base(toolbar.Editor, toolbar)
		{
			this.Style = EditorStyles.toolbar;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000E997 File Offset: 0x0000CB97
		protected override void DoGUI()
		{
			GUILayout.Space(this.Space);
		}

		// Token: 0x04000152 RID: 338
		public float Space;
	}
}
