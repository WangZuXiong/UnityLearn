using System;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x0200005C RID: 92
	public class GUIToolbarFlexibleSpace : GUIControl
	{
		// Token: 0x0600021C RID: 540 RVA: 0x0000E9A4 File Offset: 0x0000CBA4
		public GUIToolbarFlexibleSpace(GUIToolbar toolbar) : base(toolbar.Editor, toolbar)
		{
			this.Style = EditorStyles.toolbar;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000E9BE File Offset: 0x0000CBBE
		protected override void DoGUI()
		{
			GUILayout.FlexibleSpace();
		}
	}
}
