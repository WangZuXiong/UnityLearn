using System;
using UnityEditor;

namespace EditorFramework
{
	// Token: 0x02000060 RID: 96
	public class GUIToolbarTextField : GUIControl
	{
		// Token: 0x06000229 RID: 553 RVA: 0x0000EECD File Offset: 0x0000D0CD
		public GUIToolbarTextField(GUIToolbar toolbar) : base(toolbar.Editor, toolbar)
		{
			this.Style = EditorStyles.toolbarTextField;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000EEE8 File Offset: 0x0000D0E8
		protected override void DoGUI()
		{
			string text = EditorGUILayout.TextField(this.Text, this.LayoutOptions);
			if (!string.Equals(text, this.Text, StringComparison.Ordinal))
			{
				this.Text = text;
				if (this.Execute != null)
				{
					this.Execute(this);
				}
			}
		}
	}
}
