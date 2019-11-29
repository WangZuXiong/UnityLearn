using System;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x0200005A RID: 90
	public class GUIToolbarPathLabel : GUIControl
	{
		// Token: 0x06000218 RID: 536 RVA: 0x0000E8FB File Offset: 0x0000CAFB
		public GUIToolbarPathLabel(GUIToolbar toolbar) : base(toolbar.Editor, toolbar)
		{
			this.Style = new GUIStyle(GUI.skin.label);
			this.Style.alignment = TextAnchor.MiddleLeft;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000E92C File Offset: 0x0000CB2C
		protected override void DoGUI()
		{
			Vector2 vector = this.Style.CalcSize(GUIContent2.Temp(this.Text));
			Rect rect = GUILayoutUtility.GetRect(vector.x, vector.y, this.Style, this.LayoutOptions);
			EditorGUI2.PathLabel(rect, this.Text, false);
		}
	}
}
