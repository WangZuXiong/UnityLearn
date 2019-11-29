using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EditorFramework
{
	// Token: 0x02000034 RID: 52
	[Serializable]
	public class FindAssetUsageWindow : EditorWindow
	{
		// Token: 0x06000190 RID: 400 RVA: 0x0000C0B0 File Offset: 0x0000A2B0
		public static void FindUsageInProject(string productid, string producttitle, IEnumerable<string> findpaths, IEnumerable<Type> findtypes)
		{
			FindAssetUsage.Result result = FindAssetUsage.InProject(findpaths, findtypes);
			FindAssetUsageWindow.Show(productid, producttitle, result);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000C0D0 File Offset: 0x0000A2D0
		public static void FindUsageInProject(string productid, string producttitle, IEnumerable<Object> findobjs, IEnumerable<Type> findtypes)
		{
			FindAssetUsage.Result result = FindAssetUsage.InProject(findobjs, findtypes);
			FindAssetUsageWindow.Show(productid, producttitle, result);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000C0F0 File Offset: 0x0000A2F0
		public static void FindUsageInProject(string productid, string producttitle, IEnumerable<Type> findobjs, IEnumerable<Type> findtypes)
		{
			FindAssetUsage.Result result = FindAssetUsage.InProject(findobjs, findtypes);
			FindAssetUsageWindow.Show(productid, producttitle, result);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000C110 File Offset: 0x0000A310
		public static void Show(string productid, string producttitle, FindAssetUsage.Result result)
		{
			FindAssetUsageWindow window = EditorWindow.GetWindow<FindAssetUsageWindow>(true, producttitle + " - Find Usage Results", true);
			window.ShowUtility();
			window.RebuildUI();
			window._result = result;
			window._productId = productid;
			window._productTitle = producttitle;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000C151 File Offset: 0x0000A351
		private void RebuildUI()
		{
			this._searchresult = null;
			this._searchfor = null;
			base.Repaint();
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000C270 File Offset: 0x0000A470
		private void Init()
		{
			if (this._result == null)
			{
				this._result = new FindAssetUsage.Result();
			}
			this._searchfor = new FindAssetUsageWindow.LeftListbox(this, null);
			if (!string.IsNullOrEmpty(this._productId))
			{
				this._searchfor.EditorPrefsPath = string.Format("{0}.FindAssetUsage.ListView1", this._productId);
				this._searchfor.LoadPrefs();
			}
			List<FindAssetUsage.AssetProxy> list = new List<FindAssetUsage.AssetProxy>();
			foreach (FindAssetUsage.ResultEntry item in this._result.Entries)
			{
				list.Add(item);
			}
			this._searchfor.SetItems(list);
			this._searchfor.SelectionChange = delegate(GUIListView sender)
			{
				Dictionary<FindAssetUsage.AssetProxy, FindAssetUsage.AssetProxy> dictionary = new Dictionary<FindAssetUsage.AssetProxy, FindAssetUsage.AssetProxy>();
				foreach (object objB in this._searchfor.SelectedItems)
				{
					foreach (FindAssetUsage.ResultEntry resultEntry in this._result.Entries)
					{
						if (object.ReferenceEquals(resultEntry, objB))
						{
							foreach (FindAssetUsage.AssetProxy assetProxy in resultEntry.Findings)
							{
								dictionary[assetProxy] = assetProxy;
							}
						}
					}
				}
				FindAssetUsage.AssetProxy[] array = new FindAssetUsage.AssetProxy[dictionary.Keys.Count];
				int count = dictionary.Keys.Count;
				dictionary.Keys.CopyTo(array, 0);
				this._searchresult.SetItems(array);
			};
			this._searchresult = new FindAssetUsageWindow.RightListbox(this, null);
			if (!string.IsNullOrEmpty(this._productId))
			{
				this._searchresult.EditorPrefsPath = string.Format("{0}.FindAssetUsage.ListView2", this._productId);
				this._searchresult.LoadPrefs();
			}
			this._splitterWidth = EditorPrefs.GetFloat(string.Format("{0}.FindAssetUsage.SplitterWidth", this._productId), 300f);
			if (this._result.Entries.Count > 0)
			{
				this._searchfor.SelectItem(0);
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000C3C8 File Offset: 0x0000A5C8
		private void OnDestroy()
		{
			if (this._searchfor != null)
			{
				this._searchfor.SavePrefs();
			}
			if (this._searchresult != null)
			{
				this._searchresult.SavePrefs();
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000C3F4 File Offset: 0x0000A5F4
		private void OnGUI()
		{
			if (this._result == null || this._searchresult == null || this._searchfor == null)
			{
				this.Init();
			}
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Width(this._splitterWidth)
			});
			this._searchfor.OnGUI();
			EditorGUILayout.EndHorizontal();
			if (EditorGUILayout2.HorizontalSplitter(ref this._splitterWidth))
			{
				EditorPrefs.SetFloat(string.Format("{0}.FindAssetUsage.SplitterWidth", this._productId), this._splitterWidth);
			}
			this._splitterWidth = Mathf.Clamp(this._splitterWidth, Mathf.Max(64f, base.position.width * 0.05f), Mathf.Min(base.position.width - 64f, base.position.width * 0.95f));
			this._searchresult.OnGUI();
			EditorGUILayout.EndHorizontal();
		}

		// Token: 0x040000DC RID: 220
		[SerializeField]
		private FindAssetUsage.Result _result;

		// Token: 0x040000DD RID: 221
		[SerializeField]
		private string _productId;

		// Token: 0x040000DE RID: 222
		[SerializeField]
		private string _productTitle;

		// Token: 0x040000DF RID: 223
		private FindAssetUsageWindow.Listbox _searchfor;

		// Token: 0x040000E0 RID: 224
		private FindAssetUsageWindow.Listbox _searchresult;

		// Token: 0x040000E1 RID: 225
		private float _splitterWidth;

		// Token: 0x02000035 RID: 53
		private class Listbox : GUIListView
		{
			// Token: 0x0600019A RID: 410 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
			public Listbox(EditorWindow editor, GUIControl parent) : base(editor, parent)
			{
				this._items = new List<FindAssetUsage.AssetProxy>();
				this.Mode = GUIListViewMode.Details;
				base.FullRowSelect = true;
				base.HeaderStyle = GUIListViewHeaderStyle.ClickablePopup;
				base.MultiSelect = true;
			}

			// Token: 0x0600019B RID: 411 RVA: 0x0000C524 File Offset: 0x0000A724
			protected void OnDrawAssetName(FindAssetUsage.AssetProxy model, GUIListViewDrawItemArgs args)
			{
				EditorGUI2.PathLabel(args.ItemRect, model.Name, args.Selected);
			}

			// Token: 0x0600019C RID: 412 RVA: 0x0000C540 File Offset: 0x0000A740
			protected int OnCompareAssetName(object x, object y)
			{
				FindAssetUsage.AssetProxy assetProxy = x as FindAssetUsage.AssetProxy;
				FindAssetUsage.AssetProxy assetProxy2 = y as FindAssetUsage.AssetProxy;
				return string.Compare(assetProxy.Name, assetProxy2.Name, StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x0600019D RID: 413 RVA: 0x0000C56D File Offset: 0x0000A76D
			protected void OnDrawAssetPath(FindAssetUsage.AssetProxy model, GUIListViewDrawItemArgs args)
			{
				EditorGUI2.PathLabel(args.ItemRect, model.AssetPath ?? "<no path available>", args.Selected);
			}

			// Token: 0x0600019E RID: 414 RVA: 0x0000C594 File Offset: 0x0000A794
			protected int OnCompareAssetPath(object x, object y)
			{
				FindAssetUsage.AssetProxy assetProxy = x as FindAssetUsage.AssetProxy;
				FindAssetUsage.AssetProxy assetProxy2 = y as FindAssetUsage.AssetProxy;
				return string.Compare(assetProxy.AssetPath, assetProxy2.AssetPath, StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x0600019F RID: 415 RVA: 0x0000C5C4 File Offset: 0x0000A7C4
			public void SelectItem(int index)
			{
				FindAssetUsage.AssetProxy assetProxy = this.OnGetItem(index) as FindAssetUsage.AssetProxy;
				base.SelectedItems = new FindAssetUsage.AssetProxy[]
				{
					assetProxy
				};
			}

			// Token: 0x060001A0 RID: 416 RVA: 0x0000C5F0 File Offset: 0x0000A7F0
			public void SetItems(IEnumerable<FindAssetUsage.AssetProxy> items)
			{
				this._items = new List<FindAssetUsage.AssetProxy>();
				foreach (FindAssetUsage.AssetProxy item in items)
				{
					this._items.Add(item);
				}
				base.Sort();
				base.Editor.Repaint();
			}

			// Token: 0x060001A1 RID: 417 RVA: 0x0000C65C File Offset: 0x0000A85C
			protected override object[] OnBeforeSortItems()
			{
				return this._items.ToArray();
			}

			// Token: 0x060001A2 RID: 418 RVA: 0x0000C669 File Offset: 0x0000A869
			protected override void OnAfterSortItems(object[] models)
			{
				this._items = new List<FindAssetUsage.AssetProxy>((FindAssetUsage.AssetProxy[])models);
			}

			// Token: 0x060001A3 RID: 419 RVA: 0x0000C67C File Offset: 0x0000A87C
			protected override void OnItemDoubleClick(GUIListViewItemDoubleClickArgs args)
			{
				this.OnContextMenuSelect();
			}

			// Token: 0x060001A4 RID: 420 RVA: 0x0000C684 File Offset: 0x0000A884
			protected override void OnItemKeyDown(ref GUIListViewItemKeyDownArgs args)
			{
				if (base.SelectedItemsCount < 1)
				{
					return;
				}
				KeyCode keyCode = Event.current.keyCode;
				if ((int)keyCode != 13)
				{
					return;
				}
				if (Event.current.control)
				{
					this.OnContextMenuOpenWithDefaultApp();
					args.Handled = true;
					GUIUtility.ExitGUI();
					return;
				}
				this.OnContextMenuSelect();
				args.Handled = true;
			}

			// Token: 0x060001A5 RID: 421 RVA: 0x0000C6D8 File Offset: 0x0000A8D8
			protected override object OnGetItem(int index)
			{
				if (index < 0 || index >= this._items.Count)
				{
					return null;
				}
				return this._items[index];
			}

			// Token: 0x060001A6 RID: 422 RVA: 0x0000C6FA File Offset: 0x0000A8FA
			protected override int OnGetItemCount()
			{
				return this._items.Count;
			}

			// Token: 0x060001A7 RID: 423 RVA: 0x0000C708 File Offset: 0x0000A908
			protected override void OnDrawItem(GUIListViewDrawItemArgs args)
			{
				FindAssetUsage.AssetProxy assetProxy = args.Model as FindAssetUsage.AssetProxy;
				if (assetProxy == null)
				{
					return;
				}
				if (args.Column.IsPrimaryColumn)
				{
					Texture texture = null;
					if (assetProxy.Asset != null)
					{
						texture = AssetPreview.GetMiniTypeThumbnail(assetProxy.Asset.GetType());
					}
					else if (assetProxy.AssetType != null)
					{
						texture = AssetPreview.GetMiniTypeThumbnail(assetProxy.AssetType);
					}
					else if (!string.IsNullOrEmpty(assetProxy.AssetPath))
					{
						texture = AssetPreview.GetMiniTypeThumbnail(AssetDatabase2.GetAssetType(assetProxy.AssetPath));
					}
					if (texture == null)
					{
						texture = AssetPreview.GetMiniTypeThumbnail(typeof(TextAsset));
					}
					GUIListView.DrawItemImageHelper(ref args.ItemRect, texture, new Vector2(16f, 16f));
				}
				args.ItemRect.y = args.ItemRect.y + 3f;
				args.ItemRect.height = args.ItemRect.height - 3f;
				FindAssetUsageWindow.Listbox.Column column = args.Column as FindAssetUsageWindow.Listbox.Column;
				column.DrawFunc(assetProxy, args);
			}

			// Token: 0x060001A8 RID: 424 RVA: 0x0000C810 File Offset: 0x0000AA10
			protected override void OnItemContextMenu(GUIListViewContextMenuArgs args)
			{
				base.OnItemContextMenu(args);
				if (base.SelectedItemsCount < 1)
				{
					return;
				}
				FindAssetUsage.AssetProxy assetProxy = args.Model as FindAssetUsage.AssetProxy;
				bool flag = FileUtil2.Exists(assetProxy.AssetPath);
				GenericMenu genericMenu = new GenericMenu();
				genericMenu.AddItem(new GUIContent((Application.platform == null) ? "Reveal in Finder" : "Show in Explorer"), false, flag ? new GenericMenu.MenuFunction(this.OnContextMenuShowInExplorer) : null);
				genericMenu.AddItem(new GUIContent("Open %enter"), false, FileUtil2.Exists(assetProxy.AssetPath) ? new GenericMenu.MenuFunction(this.OnContextMenuOpenWithDefaultApp) : null);
				genericMenu.AddItem(new GUIContent(string.Empty), false, null);
				genericMenu.AddItem(new GUIContent("Select in Project _enter"), false, flag ? new GenericMenu.MenuFunction(this.OnContextMenuSelect) : null);
				genericMenu.AddItem(new GUIContent("Find References in Scene"), false, (base.SelectedItemsCount == 1) ? new GenericMenu.MenuFunction(this.OnContextMenuFindReferencesInScene) : null);
				genericMenu.AddItem(new GUIContent(string.Empty), false, null);
				genericMenu.AddItem(new GUIContent("Copy Full Path"), false, flag ? new GenericMenu.MenuFunction(this.OnContextMenuCopyFullPath) : null);
				genericMenu.DropDown(new Rect(args.MenuLocation.x, args.MenuLocation.y, 0f, 0f));
				Event.current.Use();
				base.Editor.Repaint();
			}

			// Token: 0x060001A9 RID: 425 RVA: 0x0000C980 File Offset: 0x0000AB80
			private void OnContextMenuShowInExplorer()
			{
				List<string> selectedPaths = this.GetSelectedPaths();
				EditorApplication2.ShowInExplorer(selectedPaths.ToArray());
			}

			// Token: 0x060001AA RID: 426 RVA: 0x0000C9A0 File Offset: 0x0000ABA0
			private void OnContextMenuOpenWithDefaultApp()
			{
				List<string> selectedPaths = this.GetSelectedPaths();
				EditorApplication2.OpenAssets(selectedPaths.ToArray());
			}

			// Token: 0x060001AB RID: 427 RVA: 0x0000C9C0 File Offset: 0x0000ABC0
			private void OnContextMenuFindReferencesInScene()
			{
				List<Object> selectedAssets = this.GetSelectedAssets();
				if (selectedAssets.Count > 0)
				{
					EditorApplication2.FindReferencesInScene(selectedAssets[0]);
				}
			}

			// Token: 0x060001AC RID: 428 RVA: 0x0000C9EC File Offset: 0x0000ABEC
			private void OnContextMenuCopyFullPath()
			{
				List<string> selectedPaths = this.GetSelectedPaths();
				ClipboardUtil.CopyPaths(selectedPaths);
			}

			// Token: 0x060001AD RID: 429 RVA: 0x0000CA08 File Offset: 0x0000AC08
			private void OnContextMenuSelect()
			{
				List<Object> selectedAssets = this.GetSelectedAssets();
				Selection.objects = selectedAssets.ToArray();
			}

			// Token: 0x060001AE RID: 430 RVA: 0x0000CA28 File Offset: 0x0000AC28
			private List<string> GetSelectedPaths()
			{
				List<string> list = new List<string>();
				object[] selectedItems = base.SelectedItems;
				for (int i = 0; i < selectedItems.Length; i++)
				{
					FindAssetUsage.AssetProxy assetProxy = selectedItems[i] as FindAssetUsage.AssetProxy;
					if (assetProxy != null)
					{
						string assetPath = assetProxy.AssetPath;
						if (assetProxy.Asset != null)
						{
							assetPath = AssetDatabase.GetAssetPath(assetProxy.Asset);
						}
						if (!string.IsNullOrEmpty(assetPath))
						{
							list.Add(assetPath);
						}
					}
				}
				return list;
			}

			// Token: 0x060001AF RID: 431 RVA: 0x0000CA94 File Offset: 0x0000AC94
			private List<Object> GetSelectedAssets()
			{
				List<Object> list = new List<Object>();
				object[] selectedItems = base.SelectedItems;
				List<Object> result;
				using (EditorGUI2.ModalProgressBar modalProgressBar = new EditorGUI2.ModalProgressBar("Loading assets...", true))
				{
					for (int i = 0; i < selectedItems.Length; i++)
					{
						FindAssetUsage.AssetProxy assetProxy = selectedItems[i] as FindAssetUsage.AssetProxy;
						if (assetProxy != null)
						{
							if (modalProgressBar.TotalElapsedTime > 1f && modalProgressBar.ElapsedTime > 0.1f)
							{
								float progress = (float)i / (float)selectedItems.Length;
								string text = string.Format("[{1} remaining] {0}", assetProxy.Name, selectedItems.Length - i - 1);
								if (modalProgressBar.Update(text, progress))
								{
									break;
								}
							}
							Object @object = assetProxy.LoadAsset();
							if (@object != null)
							{
								list.Add(@object);
							}
						}
					}
					result = list;
				}
				return result;
			}

			// Token: 0x040000E2 RID: 226
			private List<FindAssetUsage.AssetProxy> _items;

			// Token: 0x02000036 RID: 54
			protected class Column : GUIListViewColumn
			{
				// Token: 0x060001B0 RID: 432 RVA: 0x0000CB6C File Offset: 0x0000AD6C
				public Column(string id, string title, int width, GUIListViewColumn.CompareDelelgate comparer, FindAssetUsageWindow.Listbox.Column.ColumnDrawer drawer) : base(title, "", null, (float)width, comparer)
				{
					this.SerializeName = id;
					this.DrawFunc = drawer;
				}

				// Token: 0x040000E3 RID: 227
				public FindAssetUsageWindow.Listbox.Column.ColumnDrawer DrawFunc;

				// Token: 0x02000037 RID: 55
				// (Invoke) Token: 0x060001B2 RID: 434
				public delegate void ColumnDrawer(FindAssetUsage.AssetProxy model, GUIListViewDrawItemArgs args);
			}
		}

		// Token: 0x02000038 RID: 56
		private class LeftListbox : FindAssetUsageWindow.Listbox
		{
			// Token: 0x060001B5 RID: 437 RVA: 0x0000CB90 File Offset: 0x0000AD90
			public LeftListbox(EditorWindow editor, GUIControl parent) : base(editor, parent)
			{
				FindAssetUsageWindow.Listbox.Column item = new FindAssetUsageWindow.Listbox.Column("Name", "Name", 200, new GUIListViewColumn.CompareDelelgate(base.OnCompareAssetName), new FindAssetUsageWindow.Listbox.Column.ColumnDrawer(base.OnDrawAssetName));
				base.Columns.Add(item);
				FindAssetUsageWindow.Listbox.Column item2 = new FindAssetUsageWindow.Listbox.Column("Results", "Results", 60, new GUIListViewColumn.CompareDelelgate(this.OnCompareFindingsCount), new FindAssetUsageWindow.Listbox.Column.ColumnDrawer(this.OnDrawFindingsCount));
				base.Columns.Add(item2);
				base.MultiSelect = false;
			}

			// Token: 0x060001B6 RID: 438 RVA: 0x0000CC1C File Offset: 0x0000AE1C
			protected void OnDrawFindingsCount(FindAssetUsage.AssetProxy m, GUIListViewDrawItemArgs args)
			{
				FindAssetUsage.ResultEntry resultEntry = m as FindAssetUsage.ResultEntry;
				GUI.Label(args.ItemRect, resultEntry.Findings.Count.ToString(), args.Selected ? EditorStyles.whiteLabel : EditorStyles.label);
			}

			// Token: 0x060001B7 RID: 439 RVA: 0x0000CC64 File Offset: 0x0000AE64
			protected int OnCompareFindingsCount(object x, object y)
			{
				FindAssetUsage.ResultEntry resultEntry = x as FindAssetUsage.ResultEntry;
				FindAssetUsage.ResultEntry resultEntry2 = y as FindAssetUsage.ResultEntry;
				return resultEntry.Findings.Count.CompareTo(resultEntry2.Findings.Count);
			}
		}

		// Token: 0x02000039 RID: 57
		private class RightListbox : FindAssetUsageWindow.Listbox
		{
			// Token: 0x060001B8 RID: 440 RVA: 0x0000CCA0 File Offset: 0x0000AEA0
			public RightListbox(EditorWindow editor, GUIControl parent) : base(editor, parent)
			{
				base.EmptyText = "No assets found";
				base.Columns.Add(new FindAssetUsageWindow.Listbox.Column("Name", "Name", 200, new GUIListViewColumn.CompareDelelgate(base.OnCompareAssetName), new FindAssetUsageWindow.Listbox.Column.ColumnDrawer(base.OnDrawAssetName)));
				base.Columns.Add(new FindAssetUsageWindow.Listbox.Column("Path", "Path", 400, new GUIListViewColumn.CompareDelelgate(base.OnCompareAssetPath), new FindAssetUsageWindow.Listbox.Column.ColumnDrawer(base.OnDrawAssetPath)));
			}
		}
	}
}
