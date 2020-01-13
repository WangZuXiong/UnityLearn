using System;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x0200002B RID: 43
	public static class EditorGUILayout2
	{
		// Token: 0x0600015E RID: 350 RVA: 0x0000AC0E File Offset: 0x00008E0E
		public static Enum EnumFlagsField(Enum enumValue, params GUILayoutOption[] options)
		{
			return EditorGUILayout.EnumFlagsField(enumValue, options);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000AC18 File Offset: 0x00008E18
		public static void Separator(params GUILayoutOption[] options)
		{
			Rect rect = GUILayoutUtility.GetRect(1f, 1f, options);
			if ((int)Event.current.type == 7)
			{
				Color color = GUI.color;
				GUI.color = new Color(0f, 0f, 0f, 0.3f);
				GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture, 0, false);
				GUI.color = color;
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000AC7C File Offset: 0x00008E7C
		public static bool HorizontalSplitter(ref float value)
		{
			bool result = false;
			EditorGUILayout.BeginVertical(new GUILayoutOption[]
			{
				GUILayout.Width(2f),
				GUILayout.ExpandHeight(true)
			});
			Color color = GUI.color;
			GUI.color = new Color(EditorStyles.label.normal.textColor.r, EditorStyles.label.normal.textColor.g, EditorStyles.label.normal.textColor.b, 0.15f);
			GUILayout.Box(GUIContent.none, GUIStyles.White, new GUILayoutOption[]
			{
				GUILayout.Width(2f),
				GUILayout.ExpandHeight(true)
			});
			GUI.color = color;
			Rect lastRect = GUILayoutUtility.GetLastRect();
			lastRect.x += 1f;
			lastRect.width += 2f;
			EditorGUIUtility.AddCursorRect(lastRect, MouseCursor.ResizeHorizontal);
			int controlID = GUIUtility.GetControlID("SplitterH".GetHashCode(), FocusType.Passive);
			if ((int)Event.current.rawType == 1 && GUIUtility.hotControl == controlID)
			{
				GUIUtility.hotControl = 0;
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && lastRect.Contains(Event.current.mousePosition))
			{
				GUIUtility.hotControl = controlID;
			}
			if (GUIUtility.hotControl == controlID && (int)Event.current.type == 3)
			{
				value += Event.current.delta.x;
				Event.current.Use();
				result = true;
			}
			EditorGUILayout.EndVertical();
			return result;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000AE08 File Offset: 0x00009008
		public static bool VerticalSplitter(ref float value)
		{
			bool result = false;
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Height(2f),
				GUILayout.ExpandWidth(true)
			});
			Color color = GUI.color;
			GUI.color = new Color(EditorStyles.label.normal.textColor.r, EditorStyles.label.normal.textColor.g, EditorStyles.label.normal.textColor.b, 0.15f);
			GUILayout.Box(GUIContent.none, GUIStyles.White, new GUILayoutOption[]
			{
				GUILayout.Height(2f),
				GUILayout.ExpandWidth(true)
			});
			GUI.color = color;
			Rect lastRect = GUILayoutUtility.GetLastRect();
			lastRect.y += 2f;
			lastRect.height += 3f;
			EditorGUIUtility.AddCursorRect(lastRect, MouseCursor.ResizeVertical);
			int controlID = GUIUtility.GetControlID("SplitterV".GetHashCode(), FocusType.Passive);
			if ((int)Event.current.rawType == 1 && GUIUtility.hotControl == controlID)
			{
				GUIUtility.hotControl = 0;
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && lastRect.Contains(Event.current.mousePosition))
			{
				GUIUtility.hotControl = controlID;
			}
			if (GUIUtility.hotControl == controlID && (int)Event.current.type == 3)
			{
				value -= Event.current.delta.y;
				Event.current.Use();
				result = true;
			}
			EditorGUILayout.EndHorizontal();
			return result;
		}
	}
}
