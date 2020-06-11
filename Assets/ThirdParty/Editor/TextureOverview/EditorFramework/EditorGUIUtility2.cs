using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x0200002C RID: 44
	public static class EditorGUIUtility2
	{
		// Token: 0x06000162 RID: 354 RVA: 0x0000AF94 File Offset: 0x00009194
		static EditorGUIUtility2()
		{
			EditorGUIUtility2._loadIcon = typeof(EditorGUIUtility).GetMethod("LoadIcon", BindingFlags.Static | BindingFlags.NonPublic);
			EditorGUIUtility2._labelWidth = typeof(EditorGUIUtility).GetProperty("labelWidth", BindingFlags.Static | BindingFlags.NonPublic);
			if (EditorGUIUtility2._labelWidth == null)
			{
				EditorGUIUtility2._labelWidth = typeof(EditorGUIUtility).GetProperty("labelWidth", BindingFlags.Static | BindingFlags.Public);
			}
			if (EditorGUIUtility2._loadIcon == null)
			{
				Debug.LogWarning("Could not find method 'UnityEditor.EditorGUIUtility.LoadIcon'.");
			}
			if (EditorGUIUtility2._labelWidth == null)
			{
				Debug.LogWarning("Could not find property 'UnityEditor.EditorGUIUtility.labelWidth'.");
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000163 RID: 355 RVA: 0x0000B025 File Offset: 0x00009225
		// (set) Token: 0x06000164 RID: 356 RVA: 0x0000B045 File Offset: 0x00009245
		public static float labelWidth
		{
			get
			{
				if (EditorGUIUtility2._labelWidth != null)
				{
					return (float)EditorGUIUtility2._labelWidth.GetValue(null, null);
				}
				return 150f;
			}
			set
			{
				if (EditorGUIUtility2._labelWidth != null)
				{
					EditorGUIUtility2._labelWidth.SetValue(null, value, null);
				}
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000B060 File Offset: 0x00009260
		public static void BeginLabelWidth(float width)
		{
			EditorGUIUtility2._labelWidthStack.Add(EditorGUIUtility2.labelWidth);
			EditorGUIUtility2.labelWidth = width;
			if (EditorGUIUtility2._labelWidthStack.Count == 50)
			{
				Debug.LogWarning("EditorGUIUtility2: PushLabelWidth/PopLabelWidth pairs don't match.");
				EditorGUIUtility2._labelWidthStack.Clear();
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000B09C File Offset: 0x0000929C
		public static void EndLabelWidth()
		{
			if (EditorGUIUtility2._labelWidthStack.Count > 0)
			{
				int index = EditorGUIUtility2._labelWidthStack.Count - 1;
				EditorGUIUtility2.labelWidth = EditorGUIUtility2._labelWidthStack[index];
				EditorGUIUtility2._labelWidthStack.RemoveAt(index);
				return;
			}
			Debug.LogWarning("EditorGUIUtility2: PushLabelWidth/PopLabelWidth pairs don't match.");
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000B0EC File Offset: 0x000092EC
		public static Texture2D LoadIcon(string path)
		{
			if (EditorGUIUtility2._loadIcon == null)
			{
				return null;
			}
			object[] parameters = new object[]
			{
				path
			};
			return EditorGUIUtility2._loadIcon.Invoke(null, parameters) as Texture2D;
		}

		// Token: 0x040000CE RID: 206
		private static MethodInfo _loadIcon;

		// Token: 0x040000CF RID: 207
		private static PropertyInfo _labelWidth;

		// Token: 0x040000D0 RID: 208
		private static List<float> _labelWidthStack = new List<float>();
	}
}
