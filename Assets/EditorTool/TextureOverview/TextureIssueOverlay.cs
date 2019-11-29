using System;
using UnityEditor;
using UnityEngine;

namespace TextureOverview
{
	// Token: 0x0200007A RID: 122
	public class TextureIssueOverlay
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0003347B File Offset: 0x0003167B
		// (set) Token: 0x060003BE RID: 958 RVA: 0x00033483 File Offset: 0x00031683
		public string TitleString { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060003BF RID: 959 RVA: 0x0003348C File Offset: 0x0003168C
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x00033494 File Offset: 0x00031694
		public string IssueString { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x000334A0 File Offset: 0x000316A0
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x0003352E File Offset: 0x0003172E
		public bool Visible
		{
			get
			{
				if (this._visible < 0)
				{
					this._visible = EditorPrefs.GetInt(string.Format("{0}.TextureIssueOverlay.Visible", Globals.ProductId), 0);
					this._position.x = EditorPrefs.GetFloat(string.Format("{0}.TextureIssueOverlay.Position.X", Globals.ProductId), this._position.x);
					this._position.y = EditorPrefs.GetFloat(string.Format("{0}.TextureIssueOverlay.Position.Y", Globals.ProductId), this._position.y);
				}
				return this._visible > 0;
			}
			set
			{
				if (this._visible != (value ? 1 : 0))
				{
					this._visible = (value ? 1 : 0);
					EditorPrefs.SetInt(string.Format("{0}.TextureIssueOverlay.Visible", Globals.ProductId), this._visible);
				}
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00033568 File Offset: 0x00031768
		public void OnGUI(EditorWindow owner)
		{
			this._Owner = owner;
			if (!this.Visible || this._Owner == null)
			{
				return;
			}
			Rect position = this._position;
			this._position = this.ClampPosition(owner, GUI.Window(0, this._position, new GUI.WindowFunction(this.DrawOverlayWindow), ""));
			if (position != this._position)
			{
				EditorPrefs.SetFloat(string.Format("{0}.TextureIssueOverlay.Position.X", Globals.ProductId), this._position.x);
				EditorPrefs.SetFloat(string.Format("{0}.TextureIssueOverlay.Position.Y", Globals.ProductId), this._position.y);
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00033610 File Offset: 0x00031810
		private void DrawOverlayWindow(int id)
		{
			EditorGUI.DrawPreviewTexture(new Rect(0f, 16f, this._position.width, this._position.height), EditorStyles.toolbar.normal.background);
			EditorGUILayout.BeginVertical(new GUILayoutOption[0]);
			EditorGUI.LabelField(new Rect(1f, 1f, this._position.width, 16f), this.TitleString, EditorStyles.boldLabel);
			EditorGUI.HelpBox(new Rect(0f, 16f, this._position.width, this._position.height - 16f), this.IssueString, MessageType.Warning);
			EditorGUILayout.EndVertical();
			GUI.DragWindow();
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x000336D4 File Offset: 0x000318D4
		private Rect ClampPosition(EditorWindow owner, Rect overlayPosition)
		{
			float num = 32f;
			if (overlayPosition.xMax <= num)
			{
				overlayPosition.x = num - overlayPosition.width;
			}
			if (overlayPosition.x >= owner.position.width - num)
			{
				overlayPosition.x = owner.position.width - num;
			}
			if (overlayPosition.yMax <= num)
			{
				overlayPosition.y = num - overlayPosition.height;
			}
			if (overlayPosition.y >= owner.position.height - num)
			{
				overlayPosition.y = owner.position.height - num;
			}
			return overlayPosition;
		}

		// Token: 0x0400023D RID: 573
		private Rect _position = new Rect(20f, 20f, 500f, 100f);

		// Token: 0x0400023E RID: 574
		private int _visible = -1;

		// Token: 0x0400023F RID: 575
		private EditorWindow _Owner;
	}
}
