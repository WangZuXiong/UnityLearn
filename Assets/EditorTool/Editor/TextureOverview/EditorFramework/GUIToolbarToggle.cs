using System;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000058 RID: 88
	public class GUIToolbarToggle : GUIControl
	{
		// Token: 0x06000214 RID: 532 RVA: 0x0000E81A File Offset: 0x0000CA1A
		public GUIToolbarToggle(GUIToolbar toolbar) : base(toolbar.Editor, toolbar)
		{
			this.Style = EditorStyles.toolbarButton;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000E834 File Offset: 0x0000CA34
		protected override void DoGUI()
		{
			GUIContent guicontent = GUIContent2.Temp(this.Text, this.Image, this.Tooltip);
			bool flag = (base.OnQueryStatus() & GUIControlStatus.Checked) != GUIControlStatus.None;
			bool flag2 = GUILayout.Toggle(flag, guicontent, this.Style, this.LayoutOptions);
			if (flag2 != flag && this.Execute != null)
			{
				this.Execute(this);
			}
		}
	}
}
