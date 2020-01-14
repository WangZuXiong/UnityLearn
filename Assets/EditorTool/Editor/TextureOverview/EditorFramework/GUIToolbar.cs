using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000053 RID: 83
	public class GUIToolbar : GUIControl
	{
		// Token: 0x060001ED RID: 493 RVA: 0x0000E220 File Offset: 0x0000C420
		public GUIToolbar(EditorWindow editor, GUIControl parent) : base(editor, parent)
		{
			this.Style = EditorStyles.toolbar;
			this.LayoutOptions = new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(true)
			};
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000E262 File Offset: 0x0000C462
		public void AddControl(GUIControl control)
		{
			this._controls.Add(control);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000E270 File Offset: 0x0000C470
		public T AddControl<T>(T control, string text, string tooltip, Texture2D image, GUIControlExecuteDelegate execute) where T : GUIControl
		{
			return this.AddControl<T>(control, text, tooltip, image, execute, null);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000E280 File Offset: 0x0000C480
		public T AddControl<T>(T control, string text, string tooltip, Texture2D image, GUIControlExecuteDelegate execute, GUIControlQueryStatusDelegate querystatus) where T : GUIControl
		{
			control.Text = text;
			control.Tooltip = tooltip;
			control.Image = image;
			control.Execute = execute;
			control.QueryStatus = querystatus;
			this.AddControl(control);
			return control;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000E2D9 File Offset: 0x0000C4D9
		public GUIToolbarButton AddButton(string text, string tooltip, Texture2D image, GUIControlExecuteDelegate execute)
		{
			return this.AddButton(text, tooltip, image, execute, null);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000E2E8 File Offset: 0x0000C4E8
		public GUIToolbarButton AddButton(string text, string tooltip, Texture2D image, GUIControlExecuteDelegate execute, GUIControlQueryStatusDelegate querystatus)
		{
			GUIToolbarButton guitoolbarButton = new GUIToolbarButton(this);
			this.AddControl<GUIToolbarButton>(guitoolbarButton, text, tooltip, image, execute, querystatus);
			return guitoolbarButton;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000E30C File Offset: 0x0000C50C
		public GUIToolbarToggle AddToggle(string text, string tooltip, Texture2D image, GUIControlExecuteDelegate execute)
		{
			return this.AddToggle(text, tooltip, image, execute, null);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000E31C File Offset: 0x0000C51C
		public GUIToolbarToggle AddToggle(string text, string tooltip, Texture2D image, GUIControlExecuteDelegate execute, GUIControlQueryStatusDelegate querystatus)
		{
			GUIToolbarToggle guitoolbarToggle = new GUIToolbarToggle(this);
			guitoolbarToggle.Style = EditorStyles.toolbarButton;
			this.AddControl<GUIToolbarToggle>(guitoolbarToggle, text, tooltip, image, execute, querystatus);
			return guitoolbarToggle;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000E34B File Offset: 0x0000C54B
		public GUIToolbarSearchField AddSearchField(string text, string[] searchModes, GUIControlExecuteDelegate execute)
		{
			return this.AddSearchField(text, searchModes, execute, null);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000E358 File Offset: 0x0000C558
		public GUIToolbarSearchField AddSearchField(string text, string[] searchModes, GUIControlExecuteDelegate execute, GUIControlQueryStatusDelegate querystatus)
		{
			GUIToolbarSearchField guitoolbarSearchField = new GUIToolbarSearchField(this);
			guitoolbarSearchField.SearchModes = searchModes;
			this.AddControl<GUIToolbarSearchField>(guitoolbarSearchField, text, "", null, execute, querystatus);
			return guitoolbarSearchField;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000E386 File Offset: 0x0000C586
		public GUIToolbarTextField AddTextField(string text, string tooltip, GUIControlExecuteDelegate execute)
		{
			return this.AddTextField(text, tooltip, execute, null);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000E394 File Offset: 0x0000C594
		public GUIToolbarTextField AddTextField(string text, string tooltip, GUIControlExecuteDelegate execute, GUIControlQueryStatusDelegate querystatus)
		{
			GUIToolbarTextField guitoolbarTextField = new GUIToolbarTextField(this);
			this.AddControl<GUIToolbarTextField>(guitoolbarTextField, text, tooltip, null, execute, querystatus);
			return guitoolbarTextField;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000E3B8 File Offset: 0x0000C5B8
		public GUIToolbarSpace AddSpace(float space)
		{
			GUIToolbarSpace guitoolbarSpace = new GUIToolbarSpace(this);
			guitoolbarSpace.Space = space;
			this.AddControl<GUIToolbarSpace>(guitoolbarSpace, "", "", null, null, null);
			return guitoolbarSpace;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000E3EC File Offset: 0x0000C5EC
		public GUIToolbarFlexibleSpace AddFlexibleSpace()
		{
			GUIToolbarFlexibleSpace guitoolbarFlexibleSpace = new GUIToolbarFlexibleSpace(this);
			this.AddControl<GUIToolbarFlexibleSpace>(guitoolbarFlexibleSpace, "", "", null, null, null);
			return guitoolbarFlexibleSpace;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000E416 File Offset: 0x0000C616
		public GUIToolbarLabel AddLabel()
		{
			return this.AddLabel("", "", null);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000E429 File Offset: 0x0000C629
		public GUIToolbarLabel AddLabel(string text)
		{
			return this.AddLabel(text, "", null);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000E438 File Offset: 0x0000C638
		public GUIToolbarLabel AddLabel(string text, string tooltip)
		{
			return this.AddLabel(text, tooltip, null);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000E444 File Offset: 0x0000C644
		public GUIToolbarLabel AddLabel(string text, string tooltip, GUIControlQueryStatusDelegate querystatus)
		{
			GUIToolbarLabel guitoolbarLabel = new GUIToolbarLabel(this);
			this.AddControl<GUIToolbarLabel>(guitoolbarLabel, text, tooltip, null, null, querystatus);
			return guitoolbarLabel;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000E466 File Offset: 0x0000C666
		public GUIToolbarPopup AddPopup(string text, string tooltip, GUIControlExecuteDelegate execute)
		{
			return this.AddPopup(text, tooltip, execute, null);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000E474 File Offset: 0x0000C674
		public GUIToolbarPopup AddPopup(string text, string tooltip, GUIControlExecuteDelegate execute, GUIControlQueryStatusDelegate querystatus)
		{
			GUIToolbarPopup guitoolbarPopup = new GUIToolbarPopup(this);
			this.AddControl<GUIToolbarPopup>(guitoolbarPopup, text, tooltip, null, execute, querystatus);
			return guitoolbarPopup;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000E497 File Offset: 0x0000C697
		public GUIToolbarMenu AddMenu(string text, string tooltip, GUIControlExecuteDelegate execute)
		{
			return this.AddMenu(text, tooltip, execute, null);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000E4A4 File Offset: 0x0000C6A4
		public GUIToolbarMenu AddMenu(string text, string tooltip, GUIControlExecuteDelegate execute, GUIControlQueryStatusDelegate querystatus)
		{
			GUIToolbarMenu guitoolbarMenu = new GUIToolbarMenu(this);
			this.AddControl<GUIToolbarMenu>(guitoolbarMenu, text, tooltip, null, execute, querystatus);
			return guitoolbarMenu;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000E4C8 File Offset: 0x0000C6C8
		public GUIToolbarRadioGroup AddRadioGroup()
		{
			GUIToolbarRadioGroup guitoolbarRadioGroup = new GUIToolbarRadioGroup(this);
			this.AddControl(guitoolbarRadioGroup);
			return guitoolbarRadioGroup;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000E4E4 File Offset: 0x0000C6E4
		protected override void DoGUI()
		{
			Vector2 iconSize = EditorGUIUtility.GetIconSize();
			EditorGUIUtility.SetIconSize(this.ImageSize);
			GUILayout.BeginHorizontal(this.Style, this.LayoutOptions);
			foreach (GUIControl guicontrol in this._controls)
			{
				EditorGUIUtility.SetIconSize(Vector2.Min(this.ImageSize, guicontrol.ImageSize));
				guicontrol.OnGUI();
			}
			GUILayout.Space(1f);
			GUILayout.EndHorizontal();
			EditorGUIUtility.SetIconSize(iconSize);
		}

		// Token: 0x0400014D RID: 333
		private List<GUIControl> _controls = new List<GUIControl>();
	}
}
