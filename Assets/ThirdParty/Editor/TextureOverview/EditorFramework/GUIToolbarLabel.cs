using System;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000059 RID: 89
	public class GUIToolbarLabel : GUIControl
	{
		// Token: 0x06000216 RID: 534 RVA: 0x0000E894 File Offset: 0x0000CA94
		public GUIToolbarLabel(GUIToolbar toolbar) : base(toolbar.Editor, toolbar)
		{
			this.Style = new GUIStyle(GUI.skin.label);
			this.Style.alignment = TextAnchor.MiddleLeft;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000E8C4 File Offset: 0x0000CAC4
		protected override void DoGUI()
		{
			GUIContent guicontent = GUIContent2.Temp(this.Text, this.Image, this.Tooltip);
			GUILayout.Label(guicontent, this.Style, this.LayoutOptions);
		}
	}
}
