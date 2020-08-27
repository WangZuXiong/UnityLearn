using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000054 RID: 84
	public class GUIToolbarRadioGroup : GUIControl
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000E584 File Offset: 0x0000C784
		// (set) Token: 0x06000206 RID: 518 RVA: 0x0000E58C File Offset: 0x0000C78C
		public GUIToolbarRadioControl CheckedControl { get; private set; }

		// Token: 0x06000207 RID: 519 RVA: 0x0000E595 File Offset: 0x0000C795
		public GUIToolbarRadioGroup(GUIToolbar toolbar) : base(toolbar.Editor, toolbar)
		{
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000E5AF File Offset: 0x0000C7AF
		public void OnCheckedControl(GUIToolbarRadioControl control)
		{
			this.CheckedControl = control;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000E5B8 File Offset: 0x0000C7B8
		public void AddControl(GUIToolbarRadioControl control, string text, string tooltip, Texture2D image, GUIControlExecuteDelegate execute, GUIControlQueryStatusDelegate querystatus)
		{
			control.Text = text;
			control.Tooltip = tooltip;
			control.Image = image;
			control.Execute = execute;
			control.QueryStatus = querystatus;
			this._controls.Add(control);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000E5EC File Offset: 0x0000C7EC
		public GUIToolbarRadioButton AddButton(string text, string tooltip, Texture2D image, GUIControlExecuteDelegate execute)
		{
			return this.AddButton(text, tooltip, image, execute, null);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000E5FC File Offset: 0x0000C7FC
		public GUIToolbarRadioButton AddButton(string text, string tooltip, Texture2D image, GUIControlExecuteDelegate execute, GUIControlQueryStatusDelegate querystatus)
		{
			GUIToolbarRadioButton guitoolbarRadioButton = new GUIToolbarRadioButton(this);
			guitoolbarRadioButton.Style = EditorStyles.toolbarButton;
			this.AddControl(guitoolbarRadioButton, text, tooltip, image, execute, querystatus);
			return guitoolbarRadioButton;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000E62C File Offset: 0x0000C82C
		protected override void DoGUI()
		{
			Vector2 iconSize = EditorGUIUtility.GetIconSize();
			EditorGUIUtility.SetIconSize(this.ImageSize);
			foreach (GUIToolbarRadioControl guitoolbarRadioControl in this._controls)
			{
				EditorGUIUtility.SetIconSize(Vector2.Min(this.ImageSize, guitoolbarRadioControl.ImageSize));
				guitoolbarRadioControl.OnGUI();
			}
			EditorGUIUtility.SetIconSize(iconSize);
		}

		// Token: 0x0400014E RID: 334
		private List<GUIToolbarRadioControl> _controls = new List<GUIToolbarRadioControl>();
	}
}
