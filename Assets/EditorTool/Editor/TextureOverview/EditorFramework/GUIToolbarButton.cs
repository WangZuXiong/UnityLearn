using System;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000057 RID: 87
	public class GUIToolbarButton : GUIControl
	{
		// Token: 0x06000212 RID: 530 RVA: 0x0000E758 File Offset: 0x0000C958
		public GUIToolbarButton(GUIToolbar toolbar) : base(toolbar.Editor, toolbar)
		{
			if (GUIToolbarButton._Style == null)
			{
				GUIToolbarButton._Style = new GUIStyle(EditorStyles.toolbarButton);
				GUIToolbarButton._Style.name = "EditorFramework-GUIToolbarButton-0b4b0fde-2fb4-4165-a3f3-c6bd5b07796a";
				GUIToolbarButton._Style.alignment = TextAnchor.MiddleLeft;
				GUIToolbarButton._Style.imagePosition = 0;
			}
			this.Style = GUIToolbarButton._Style;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000E7B8 File Offset: 0x0000C9B8
		protected override void DoGUI()
		{
			GUIControlStatus guicontrolStatus = base.OnQueryStatus();
			GUIContent guicontent = GUIContent2.Temp(this.Text, this.Image, this.Tooltip);
			bool flag = (guicontrolStatus & GUIControlStatus.Checked) != GUIControlStatus.None;
			bool flag2 = GUILayout.Toggle(flag, guicontent, this.Style, this.LayoutOptions);
			if (flag2 != flag && this.Execute != null)
			{
				this.Execute(this);
			}
		}

		// Token: 0x04000151 RID: 337
		private static GUIStyle _Style;
	}
}
