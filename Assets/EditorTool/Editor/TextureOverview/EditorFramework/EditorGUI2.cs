using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000029 RID: 41
	public static class EditorGUI2
	{
		// Token: 0x0600014B RID: 331 RVA: 0x0000A5BC File Offset: 0x000087BC
		static EditorGUI2()
		{
			if (EditorGUI2._searchField == null)
			{
				Debug.LogWarning("Could not find method 'UnityEditor.EditorGUI.SearchField'.");
			}
			if (EditorGUI2._toolbarSearchField == null)
			{
				Debug.LogWarning("Could not find method 'UnityEditor.EditorGUI.ToolbarSearchField(Rect, string[], ref int, string)'.");
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000A664 File Offset: 0x00008864
		public static string SearchField(Rect position, string text)
		{
			if (EditorGUI2._searchField == null)
			{
				return text;
			}
			object[] parameters = new object[]
			{
				position,
				text
			};
			return EditorGUI2._searchField.Invoke(null, parameters) as string;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000A6A4 File Offset: 0x000088A4
		public static string ToolbarSearchField(Rect position, string text, string[] searchModes, ref int searchMode)
		{
			if (EditorGUI2._toolbarSearchField == null)
			{
				return text;
			}
			object[] array = new object[]
			{
				position,
				searchModes,
				searchMode,
				text
			};
			string result = EditorGUI2._toolbarSearchField.Invoke(null, array) as string;
			searchMode = (int)array[2];
			return result;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000A6FC File Offset: 0x000088FC
		public static void Box(Rect position, GUIContent content, bool selected)
		{
			Color color = GUI.color;
			GUI.color = GUIColors.TextColor(selected);
			GUI.Box(position, content, EditorStyles.whiteLabel);
			GUI.color = color;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000A72C File Offset: 0x0000892C
		public static bool Link(Rect position, string title, bool selected)
		{
			bool result = GUILayout.Button(title, GUIStyles.Hyperlink, new GUILayoutOption[0]);
			EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);
			return result;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000A753 File Offset: 0x00008953
		public static void Label(Rect position, GUIContent content, bool selected)
		{
			EditorGUI2.Label(position, content, selected, GUIColors.SelectedText, EditorStyles.whiteLabel, GUIColors.Text, EditorStyles.whiteLabel);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000A771 File Offset: 0x00008971
		public static void Label(Rect position, string text, bool selected)
		{
			EditorGUI2.Label(position, GUIContent2.Temp(text), selected, GUIColors.SelectedText, EditorStyles.whiteLabel, GUIColors.Text, EditorStyles.whiteLabel);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000A794 File Offset: 0x00008994
		public static void Label(Rect position, string text, bool selected, Color color)
		{
			EditorGUI2.Label(position, GUIContent2.Temp(text), selected, GUIColors.SelectedText, EditorStyles.whiteLabel, color, EditorStyles.whiteLabel);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000A7B4 File Offset: 0x000089B4
		public static void Label(Rect position, GUIContent content, bool selected, Color selectedColor, GUIStyle selectedStyle, Color unselectedColor, GUIStyle unselectedStyle)
		{
			if (content == null)
			{
				content = new GUIContent("");
			}
			if (selectedStyle == null)
			{
				selectedStyle = EditorStyles.whiteLabel;
			}
			if (unselectedStyle == null)
			{
				unselectedStyle = EditorStyles.whiteLabel;
			}
			Color color = GUI.color;
			GUI.color = (selected ? selectedColor : unselectedColor);
			if (EditorStyles.label.CalcSize(content).x > position.width)
			{
				Rect rect = position;
				rect.x += rect.width - 18f;
				rect.width = 16f;
				string tooltip = string.IsNullOrEmpty(content.tooltip) ? content.text : content.text;
				position.width -= rect.width;
				GUI.Label(position, GUIContent2.Temp(content.text, content.image, tooltip), selected ? selectedStyle : unselectedStyle);
				GUI.Label(rect, GUIContent2.Temp("...", tooltip), selected ? selectedStyle : unselectedStyle);
			}
			else
			{
				GUI.Label(position, content, selected ? selectedStyle : unselectedStyle);
			}
			GUI.color = color;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000A8C2 File Offset: 0x00008AC2
		public static void PathLabel(Rect position, string text, bool selected)
		{
			EditorGUI2.PathLabel(position, text, selected, GUIColors.SelectedText, EditorStyles.whiteLabel, GUIColors.Text, EditorStyles.whiteLabel);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000A8E0 File Offset: 0x00008AE0
		public static void PathLabel(Rect position, string text, bool selected, Color selectedColor, GUIStyle selectedStyle, Color unselectedColor, GUIStyle unselectedStyle)
		{
			string text2 = text;
			Color color = GUI.color;
			GUI.color = (selected ? selectedColor : unselectedColor);
			int num = 0;
			for (;;)
			{
				num++;
				if (num >= 5 || EditorStyles.label.CalcSize(GUIContent2.Temp(text)).x <= position.width)
				{
					goto IL_158;
				}
				text = text.Replace("/.../", "/");
				int num2 = text.LastIndexOf('/');
				if (num2 == -1)
				{
					break;
				}
				int num3 = text.LastIndexOf('/', num2 - 1);
				if (num3 == -1)
				{
					break;
				}
				string str = text.Substring(num2 + 1);
				string str2 = text.Substring(0, num3);
				text = str2 + "/.../" + str;
			}
			text = ".../" + FileUtil2.GetFileName(text2);
			if (EditorStyles.label.CalcSize(GUIContent2.Temp(text)).x > position.width)
			{
				Rect rect = position;
				rect.x += rect.width - 18f;
				rect.width = 16f;
				position.width -= rect.width;
				GUI.Label(position, GUIContent2.Temp(text, text2), selected ? selectedStyle : unselectedStyle);
				GUI.Label(rect, GUIContent2.Temp("...", text2), selected ? selectedStyle : unselectedStyle);
				goto IL_179;
			}
			GUI.Label(position, GUIContent2.Temp(text), selected ? selectedStyle : unselectedStyle);
			goto IL_179;
			IL_158:
			GUI.Label(position, GUIContent2.Temp(text, (num > 1) ? text2 : ""), selected ? selectedStyle : unselectedStyle);
			IL_179:
			GUI.color = color;
		}

		// Token: 0x040000C3 RID: 195
		private static MethodInfo _searchField = typeof(EditorGUI).GetMethod("SearchField", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x040000C4 RID: 196
		private static MethodInfo _toolbarSearchField = typeof(EditorGUI).GetMethod("ToolbarSearchField", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(Rect),
			typeof(string[]),
			typeof(int).MakeByRefType(),
			typeof(string)
		}, null);

		// Token: 0x0200002A RID: 42
		public struct ModalProgressBar : IDisposable
		{
			// Token: 0x1700002F RID: 47
			// (get) Token: 0x06000156 RID: 342 RVA: 0x0000AA6C File Offset: 0x00008C6C
			public float TotalElapsedTime
			{
				get
				{
					return Time.realtimeSinceStartup - this._starttime;
				}
			}

			// Token: 0x17000030 RID: 48
			// (get) Token: 0x06000157 RID: 343 RVA: 0x0000AA7A File Offset: 0x00008C7A
			public float ElapsedTime
			{
				get
				{
					return Time.realtimeSinceStartup - this._updatetime;
				}
			}

			// Token: 0x06000158 RID: 344 RVA: 0x0000AA88 File Offset: 0x00008C88
			public ModalProgressBar(string title, bool cancable)
			{
				this = default(EditorGUI2.ModalProgressBar);
				this.Title = title;
				this.Cancable = cancable;
				this.Begin(title);
				this._titles = new string[EditorGUI2.ModalProgressBar.TextAnim.Length];
				for (int i = 0; i < this._titles.Length; i++)
				{
					this._titles[i] = title + EditorGUI2.ModalProgressBar.TextAnim[i % EditorGUI2.ModalProgressBar.TextAnim.Length];
				}
			}

			// Token: 0x06000159 RID: 345 RVA: 0x0000AAF3 File Offset: 0x00008CF3
			public void Begin(string title)
			{
				this._starttime = Time.realtimeSinceStartup;
				this._updatetime = 0f;
				this.Title = title;
			}

			// Token: 0x0600015A RID: 346 RVA: 0x0000AB12 File Offset: 0x00008D12
			public void End()
			{
				if (this._starttime > 0f)
				{
					EditorUtility.ClearProgressBar();
				}
			}

			// Token: 0x0600015B RID: 347 RVA: 0x0000AB28 File Offset: 0x00008D28
			public bool Update(string text, float progress)
			{
				if (this.Canceled)
				{
					return this.Canceled;
				}
				this._updatetime = Time.realtimeSinceStartup;
				string text2 = this._titles[this._frame % EditorGUI2.ModalProgressBar.TextAnim.Length];
				if (this._updatetime - this._frametime > 0.2f)
				{
					this._frametime = this._updatetime;
					this._frame++;
				}
				if (this.Cancable)
				{
					this.Canceled = EditorUtility.DisplayCancelableProgressBar(text2, text, progress);
					return this.Canceled;
				}
				EditorUtility.DisplayProgressBar(text2, text, progress);
				return false;
			}

			// Token: 0x0600015C RID: 348 RVA: 0x0000ABB9 File Offset: 0x00008DB9
			public void Dispose()
			{
				this.End();
			}

			// Token: 0x040000C5 RID: 197
			private static readonly string[] TextAnim = new string[]
			{
				"",
				".",
				"..",
				"...",
				"....",
				"....."
			};

			// Token: 0x040000C6 RID: 198
			private float _starttime;

			// Token: 0x040000C7 RID: 199
			private int _frame;

			// Token: 0x040000C8 RID: 200
			private float _frametime;

			// Token: 0x040000C9 RID: 201
			private string[] _titles;

			// Token: 0x040000CA RID: 202
			private float _updatetime;

			// Token: 0x040000CB RID: 203
			public string Title;

			// Token: 0x040000CC RID: 204
			public bool Cancable;

			// Token: 0x040000CD RID: 205
			public bool Canceled;
		}
	}
}
