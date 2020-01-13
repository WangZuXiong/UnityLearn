using System;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000056 RID: 86
	public class GUIToolbarRadioButton : GUIToolbarRadioControl
	{
		// Token: 0x06000210 RID: 528 RVA: 0x0000E6D3 File Offset: 0x0000C8D3
		public GUIToolbarRadioButton(GUIToolbarRadioGroup group) : base(group)
		{
			this.Style = EditorStyles.toolbarButton;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000E6E8 File Offset: 0x0000C8E8
		protected override void DoGUI()
		{
			GUIContent guicontent = GUIContent2.Temp(this.Text, this.Image, this.Tooltip);
			bool flag = object.ReferenceEquals(base.RadioGroup.CheckedControl, this);
			bool flag2 = GUILayout.Toggle(flag, guicontent, this.Style, this.LayoutOptions);
			if (flag2 != flag && this.Execute != null)
			{
				base.RadioGroup.OnCheckedControl(this);
				this.Execute(this);
			}
		}
	}
}
