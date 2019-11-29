using System;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x0200005F RID: 95
	public class GUIToolbarSearchField : GUIControl
	{
		// Token: 0x06000226 RID: 550 RVA: 0x0000EC08 File Offset: 0x0000CE08
		public GUIToolbarSearchField(GUIToolbar toolbar) : base(toolbar.Editor, toolbar)
		{
			this.Style = EditorStyles.toolbarTextField;
			this.LayoutOptions = new GUILayoutOption[]
			{
				GUILayout.MinWidth(50f),
				GUILayout.MaxWidth(250f)
			};
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000EC6C File Offset: 0x0000CE6C
		protected override void DoGUI()
		{
			Rect rect = GUILayoutUtility.GetRect(GUIContent2.Temp(this.Text), this.Style, this.LayoutOptions);
			int searchMode = this.SearchMode;
			string text = EditorGUI2.ToolbarSearchField(rect, this.Text, this.SearchModes, ref this.SearchMode);
			Rect lastRect = GUILayoutUtility.GetLastRect();
			if (this.AcceptDrop && lastRect.Contains(Event.current.mousePosition) && DragAndDrop.paths != null && DragAndDrop.paths.Length > 0 && !string.IsNullOrEmpty(DragAndDrop.paths[0]))
			{
				if ((int)Event.current.type == 9 || (int)Event.current.type == 10)
				{
					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
				}
				if ((int)Event.current.type == 10 && DragAndDrop.paths != null)
				{
					string text2 = "";
					foreach (string text3 in DragAndDrop.paths)
					{
						if (text2.Length > 0)
						{
							text2 += " || ";
						}
						object obj = text2;
						text2 = string.Concat(new object[]
						{
							obj,
							'"',
							text3.Trim(),
							'"'
						});
					}
					text = text2;
					DragAndDrop.AcceptDrag();
					base.Editor.Repaint();
				}
			}
			if (searchMode != this.SearchMode || !string.Equals(text, this.Text, StringComparison.Ordinal))
			{
				if (this._triggerTime < 0f)
				{
					EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(this.OnEditorApplicationUpdate));
				}
				Event.current.Use();
				this.Text = text;
				this._triggerTime = Time.realtimeSinceStartup;
				if (!string.IsNullOrEmpty(this.Text) && searchMode == this.SearchMode)
				{
					this._triggerTime += this.ExecDelay;
				}
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000EE5C File Offset: 0x0000D05C
		private void OnEditorApplicationUpdate()
		{
			if (this._triggerTime > 0f && this._triggerTime < Time.realtimeSinceStartup)
			{
				this._triggerTime = -1f;
				EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(this.OnEditorApplicationUpdate));
				if (this.Execute != null)
				{
					this.Execute(this);
					base.Editor.Repaint();
				}
			}
		}

		// Token: 0x04000159 RID: 345
		private float _triggerTime = -1f;

		// Token: 0x0400015A RID: 346
		public int SearchMode;

		// Token: 0x0400015B RID: 347
		public string[] SearchModes;

		// Token: 0x0400015C RID: 348
		public float ExecDelay = 0.5f;

		// Token: 0x0400015D RID: 349
		public bool AcceptDrop;
	}
}
