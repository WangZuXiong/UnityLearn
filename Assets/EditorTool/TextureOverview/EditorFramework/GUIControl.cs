using System;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000007 RID: 7
	public abstract class GUIControl
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002C31 File Offset: 0x00000E31
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002C39 File Offset: 0x00000E39
		public GUIControl Parent { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002C42 File Offset: 0x00000E42
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002C4A File Offset: 0x00000E4A
		public EditorWindow Editor { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002C53 File Offset: 0x00000E53
		// (set) Token: 0x06000020 RID: 32 RVA: 0x00002C5B File Offset: 0x00000E5B
		public string ControlName { get; private set; }

		// Token: 0x06000021 RID: 33 RVA: 0x00002C64 File Offset: 0x00000E64
		public GUIControl(EditorWindow editor, GUIControl parent)
		{
			this.ControlName = string.Format("{0}_{1}", base.GetType().Name, GUIControl.s_GuidCounter++);
			this.Editor = editor;
			this.Parent = parent;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002CFC File Offset: 0x00000EFC
		public GUIControlStatus OnQueryStatus()
		{
			GUIControlStatus guicontrolStatus = GUIControlStatus.Enable | GUIControlStatus.Visible;
			if (this.QueryStatus != null)
			{
				guicontrolStatus = this.QueryStatus(this);
			}
			if (this.Parent != null)
			{
				GUIControlStatus guicontrolStatus2 = this.Parent.OnQueryStatus();
				if ((guicontrolStatus2 & GUIControlStatus.Enable) == GUIControlStatus.None)
				{
					guicontrolStatus &= ~GUIControlStatus.Enable;
				}
				if ((guicontrolStatus2 & GUIControlStatus.Visible) == GUIControlStatus.None)
				{
					guicontrolStatus &= ~GUIControlStatus.Visible;
				}
			}
			return guicontrolStatus;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002D4C File Offset: 0x00000F4C
		public void OnGUI()
		{
			if ((int)Event.current.type == 8)
			{
				this._cachedControlStatus = this.OnQueryStatus();
			}
			if ((this._cachedControlStatus & GUIControlStatus.Visible) == GUIControlStatus.None)
			{
				return;
			}
			EditorGUI.BeginDisabledGroup((this._cachedControlStatus & GUIControlStatus.Enable) == GUIControlStatus.None);
			GUI.SetNextControlName(this.ControlName);
			bool flag = this.Tint != null;
			Color color = GUI.color;
			if (flag)
			{
				GUI.color = this.Tint.Value;
			}
			this.DoGUI();
			if (flag)
			{
				GUI.color = color;
			}
			EditorGUI.EndDisabledGroup();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002DD1 File Offset: 0x00000FD1
		public void Focus()
		{
			GUI.FocusControl(this.ControlName);
		}

		// Token: 0x06000025 RID: 37
		protected abstract void DoGUI();

		// Token: 0x04000016 RID: 22
		private static int s_GuidCounter;

		// Token: 0x04000017 RID: 23
		private GUIControlStatus _cachedControlStatus;

		// Token: 0x04000018 RID: 24
		public string Text = "";

		// Token: 0x04000019 RID: 25
		public Color? Tint;

		// Token: 0x0400001A RID: 26
		public string Tooltip = "";

		// Token: 0x0400001B RID: 27
		public Texture2D Image;

		// Token: 0x0400001C RID: 28
		public Vector2 ImageSize = new Vector2(16f, 16f);

		// Token: 0x0400001D RID: 29
		public GUIStyle Style = GUI.skin.button;

		// Token: 0x0400001E RID: 30
		public GUILayoutOption[] LayoutOptions = new GUILayoutOption[0];

		// Token: 0x0400001F RID: 31
		public object Tag;

		// Token: 0x04000020 RID: 32
		public GUIControlExecuteDelegate Execute;

		// Token: 0x04000021 RID: 33
		public GUIControlQueryStatusDelegate QueryStatus;
	}
}
