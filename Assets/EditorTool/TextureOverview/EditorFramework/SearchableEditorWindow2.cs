using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000067 RID: 103
	public static class SearchableEditorWindow2
	{
		// Token: 0x06000268 RID: 616 RVA: 0x0002C9F8 File Offset: 0x0002ABF8
		static SearchableEditorWindow2()
		{
			SearchableEditorWindow2._m_HierarchyType = typeof(SearchableEditorWindow).GetField("m_HierarchyType", BindingFlags.Instance | BindingFlags.NonPublic);
			SearchableEditorWindow2._m_HasSearchFilterFocus = typeof(SearchableEditorWindow).GetField("m_HasSearchFilterFocus", BindingFlags.Instance | BindingFlags.NonPublic);
			if (SearchableEditorWindow2._searchableWindows == null)
			{
				Debug.LogWarning("Could not find field 'UnityEditor.SearchableEditorWindow.searchableWindows'.");
			}
			if (SearchableEditorWindow2._setSearchFilter == null)
			{
				Debug.LogWarning("Could not find method 'UnityEditor.SearchableEditorWindow.SetSearchFilter(string, SearchMode, bool)'.");
			}
			if (SearchableEditorWindow2._m_HierarchyType == null)
			{
				Debug.LogWarning("Could not find field 'UnityEditor.SearchableEditorWindow.m_HierarchyType'.");
			}
			if (SearchableEditorWindow2._m_HasSearchFilterFocus == null)
			{
				Debug.LogWarning("Could not find field 'UnityEditor.SearchableEditorWindow.m_HasSearchFilterFocus'.");
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0002CAE8 File Offset: 0x0002ACE8
		public static void SetHierarchySearchFilter(string filter, SearchableEditorWindow.SearchMode mode)
		{
			if (SearchableEditorWindow2._searchableWindows == null || SearchableEditorWindow2._setSearchFilter == null || SearchableEditorWindow2._m_HierarchyType == null || SearchableEditorWindow2._m_HasSearchFilterFocus == null)
			{
				Debug.LogWarning("Could not execute 'SetHierarySearchFilter' because some of the required fields are missing.");
				return;
			}
			IEnumerable enumerable = SearchableEditorWindow2._searchableWindows.GetValue(null) as IEnumerable;
			if (enumerable == null)
			{
				return;
			}
			foreach (object obj in enumerable)
			{
				SearchableEditorWindow searchableEditorWindow = obj as SearchableEditorWindow;
				if (!(searchableEditorWindow == null))
				{
					HierarchyType hierarchyType = (HierarchyType)SearchableEditorWindow2._m_HierarchyType.GetValue(searchableEditorWindow);
					if ((int)hierarchyType == 2)
					{
						object[] parameters = new object[]
						{
							filter,
							mode,
							false
						};
						SearchableEditorWindow2._setSearchFilter.Invoke(searchableEditorWindow, parameters);
						SearchableEditorWindow2._m_HasSearchFilterFocus.SetValue(searchableEditorWindow, true);
						searchableEditorWindow.Repaint();
					}
				}
			}
		}

		// Token: 0x0400019D RID: 413
		private static FieldInfo _m_HierarchyType;

		// Token: 0x0400019E RID: 414
		private static FieldInfo _m_HasSearchFilterFocus;

		// Token: 0x0400019F RID: 415
		private static FieldInfo _searchableWindows = typeof(SearchableEditorWindow).GetField("searchableWindows", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x040001A0 RID: 416
		private static MethodInfo _setSearchFilter = typeof(SearchableEditorWindow).GetMethod("SetSearchFilter", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(string),
			typeof(SearchableEditorWindow.SearchMode),
			typeof(bool)
		}, null);
	}
}
