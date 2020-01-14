using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000006 RID: 6
	public class AssetBundleManifestUI
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000023AF File Offset: 0x000005AF
		private string EditorPrefsPrefix
		{
			get
			{
				return string.Format("{0}.{1}", this.EditorPrefsPath, base.GetType().Name);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000023CC File Offset: 0x000005CC
		// (set) Token: 0x0600000B RID: 11 RVA: 0x000023E8 File Offset: 0x000005E8
		public float Width
		{
			get
			{
				return EditorPrefs.GetFloat(string.Format("{0}.Width", this.EditorPrefsPrefix), 200f);
			}
			private set
			{
				EditorPrefs.SetFloat(string.Format("{0}.Width", this.EditorPrefsPrefix), value);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002400 File Offset: 0x00000600
		// (set) Token: 0x0600000D RID: 13 RVA: 0x0000241C File Offset: 0x0000061C
		private float BundleDependencySplitter
		{
			get
			{
				return EditorPrefs.GetFloat(string.Format("{0}.BundleDependencySplitter", this.EditorPrefsPrefix), 200f);
			}
			set
			{
				EditorPrefs.SetFloat(string.Format("{0}.BundleDependencySplitter", this.EditorPrefsPrefix), value);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002434 File Offset: 0x00000634
		// (set) Token: 0x0600000F RID: 15 RVA: 0x0000248A File Offset: 0x0000068A
		private string Directory
		{
			get
			{
				string @string = EditorPrefs.GetString(string.Format("{0}.Project", this.EditorPrefsPrefix), "");
				if (!string.Equals(@string, Application.dataPath, StringComparison.OrdinalIgnoreCase))
				{
					return "";
				}
				return EditorPrefs.GetString(string.Format("{0}.Directory", this.EditorPrefsPrefix), "");
			}
			set
			{
				EditorPrefs.SetString(string.Format("{0}.Directory", this.EditorPrefsPrefix), value);
				EditorPrefs.SetString(string.Format("{0}.Project", this.EditorPrefsPrefix), Application.dataPath);
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000024BC File Offset: 0x000006BC
		public void Init(EditorWindow owner)
		{
			this._owner = owner;
			this._menu = new GUIToolbar(owner, null);
			this._menu.AddButton("Open...", "Locate *.manifest directory", null, new GUIControlExecuteDelegate(this.OnClickedOpen));
			this._menu.AddButton("", "Refresh list. Loads *.manifest files from recently selected directory.", Images.Refresh16x16, new GUIControlExecuteDelegate(this.OnClickRefresh), new GUIControlQueryStatusDelegate(this.OnQueryRefresh));
			this._menu.AddSpace(8f);
			this._menu.AddFlexibleSpace();
			this._searchfield = this._menu.AddSearchField("", null, new GUIControlExecuteDelegate(this.OnSearchChange), null);
			this._listbox = new AssetBundleManifestUI.Listbox(owner, null);
			this._listbox.EditorPrefsPath = string.Format("{0}.BundleListbox", this.EditorPrefsPath);
			AssetBundleManifestUI.Listbox listbox = this._listbox;
			listbox.SelectionChange = (Action<GUIListView>)Delegate.Combine(listbox.SelectionChange, new Action<GUIListView>(this.OnListboxSelectionChanged));
			this._listbox.LoadPrefs();
			this._dependencyListbox = new AssetBundleManifestUI.DependencyListbox(owner, null);
			this._dependencyListbox.EditorPrefsPath = string.Format("{0}.DependencyListbox", this.EditorPrefsPath);
			this._dependencyListbox.LoadPrefs();
			AssetBundleManifestUI.DependencyListbox dependencyListbox = this._dependencyListbox;
			dependencyListbox.ItemDoubleClick = (Action<GUIListView, GUIListViewItemDoubleClickArgs>)Delegate.Combine(dependencyListbox.ItemDoubleClick, new Action<GUIListView, GUIListViewItemDoubleClickArgs>(this.OnDoubleClickDependency));
			if (!string.IsNullOrEmpty(this.Directory))
			{
				this.LoadFiles(this.Directory);
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002644 File Offset: 0x00000844
		public void Destroy()
		{
			this.SelectionChange = null;
			if (this._listbox != null)
			{
				this._listbox.SavePrefs();
				AssetBundleManifestUI.Listbox listbox = this._listbox;
				listbox.SelectionChange = (Action<GUIListView>)Delegate.Remove(listbox.SelectionChange, new Action<GUIListView>(this.OnListboxSelectionChanged));
				this._listbox = null;
			}
			if (this._dependencyListbox != null)
			{
				this._dependencyListbox.SavePrefs();
				AssetBundleManifestUI.DependencyListbox dependencyListbox = this._dependencyListbox;
				dependencyListbox.ItemDoubleClick = (Action<GUIListView, GUIListViewItemDoubleClickArgs>)Delegate.Remove(dependencyListbox.ItemDoubleClick, new Action<GUIListView, GUIListViewItemDoubleClickArgs>(this.OnDoubleClickDependency));
				this._dependencyListbox = null;
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000026DC File Offset: 0x000008DC
		private void OnClickRefresh(GUIControl sender)
		{
			this.LoadFiles(this.Directory);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000026EC File Offset: 0x000008EC
		private GUIControlStatus OnQueryRefresh(GUIControl sender)
		{
			GUIControlStatus guicontrolStatus = GUIControlStatus.Visible;
			if (!string.IsNullOrEmpty(this.Directory))
			{
				guicontrolStatus |= GUIControlStatus.Enable;
			}
			return guicontrolStatus;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002710 File Offset: 0x00000910
		private void OnDoubleClickDependency(GUIListView sender, GUIListViewItemDoubleClickArgs args)
		{
			AssetBundleManifestUI.DependencyListbox.Model model = args.Model as AssetBundleManifestUI.DependencyListbox.Model;
			if (model == null)
			{
				return;
			}
			this._listbox.SelectedBundlePaths = new List<string>(new string[]
			{
				model.Path
			});
			this._listbox.Focus();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000275C File Offset: 0x0000095C
		private void OnListboxSelectionChanged(GUIListView sender)
		{
			List<AssetBundleManifest2> list = new List<AssetBundleManifest2>();
			if (sender.SelectedItemsCount > 0)
			{
				foreach (object obj in sender.SelectedItems)
				{
					AssetBundleManifestUI.Listbox.Model model = obj as AssetBundleManifestUI.Listbox.Model;
					if (model == null || string.IsNullOrEmpty(model.Name))
					{
						return;
					}
					string path = string.Format("{0}/{1}.manifest", this.Directory, model.Name);
					AssetBundleManifest2 item = AssetBundleManifestParser.Load(path);
					list.Add(item);
				}
			}
			this._dependencyListbox.Clear();
			this._dependencyListbox.EmptyText = "The list is empty.";
			if (list.Count == 1)
			{
				this._dependencyListbox.EmptyText = string.Format("'{0}' has no AssetBundle dependencies.", list[0].Name);
				AssetBundleManifest2 assetBundleManifest = list[0];
				List<string> list2 = new List<string>();
				foreach (string str in assetBundleManifest.Dependencies)
				{
					string item2 = Path.Combine(Path.GetDirectoryName(assetBundleManifest.Path), str + ".manifest").Replace('\\', '/');
					list2.Add(item2);
				}
				this._dependencyListbox.SetItems(list2);
			}
			else if (list.Count > 1)
			{
				this._dependencyListbox.EmptyText = "Select only one bundle to display dependencies.";
			}
			if (this.SelectionChange != null)
			{
				Action<List<AssetBundleManifest2>> selectionChange = this.SelectionChange;
				selectionChange(list);
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000028E8 File Offset: 0x00000AE8
		private void OnClickedOpen(GUIControl sender)
		{
			string text = EditorUtility.OpenFolderPanel("Locate AssetBundle Manifest directory...", this.Directory, "");
			if (!string.IsNullOrEmpty(text))
			{
				this.LoadFiles(text);
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000291C File Offset: 0x00000B1C
		private void OnSearchChange(GUIControl sender)
		{
			GUIToolbarSearchField guitoolbarSearchField = (GUIToolbarSearchField)sender;
			this._listbox.SetTextFilter(guitoolbarSearchField.Text);
			if (this._listbox.ItemCount == 0 && !string.IsNullOrEmpty(sender.Text))
			{
				this._listbox.EmptyText = "No match found.\nYou can use search operators like: && (and) || (or) and ! (not)";
				return;
			}
			this._listbox.EmptyText = "The list is empty.";
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000297C File Offset: 0x00000B7C
		private void LoadFiles(string directory)
		{
			if (!System.IO.Directory.Exists(directory))
			{
				this._owner.ShowNotification(new GUIContent(string.Format("Directory does not exist.\n'{0}'", directory)));
				this._listbox.SelectedItems = null;
				this._listbox.Clear();
				return;
			}
			List<string> selectedBundlePaths = this._listbox.SelectedBundlePaths;
			this._listbox.Clear();
			List<AssetBundleManifestUI.Listbox.Model> list = new List<AssetBundleManifestUI.Listbox.Model>();
			string[] files = System.IO.Directory.GetFiles(directory, "*.manifest");
			foreach (string text in files)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
				long size = 0L;
				string[] array2 = new string[]
				{
					Path.Combine(directory, fileNameWithoutExtension),
					Path.Combine(directory, fileNameWithoutExtension + ".unity3d")
				};
				foreach (string text2 in array2)
				{
					if (File.Exists(text2))
					{
						size = new FileInfo(text2).Length;
						break;
					}
				}
				list.Add(new AssetBundleManifestUI.Listbox.Model
				{
					Name = fileNameWithoutExtension,
					Path = text.Replace('\\', '/'),
					Size = size
				});
			}
			this._listbox.SetItems(list);
			this.Directory = directory;
			this._listbox.SelectedBundlePaths = selectedBundlePaths;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002AD4 File Offset: 0x00000CD4
		public void OnGUI()
		{
			EditorGUILayout.BeginVertical(new GUILayoutOption[]
			{
				GUILayout.Width(this.Width)
			});
			this._menu.OnGUI();
			this._listbox.OnGUI();
			GUILayout.Space(2f);
			EditorGUILayout.BeginVertical(new GUILayoutOption[]
			{
				GUILayout.Height(this.BundleDependencySplitter)
			});
			float num = this.BundleDependencySplitter;
			EditorGUILayout2.VerticalSplitter(ref num);
			num = Mathf.Clamp(num, 30f, this._owner.position.height * 0.8f);
			if (!Mathf.Approximately(num, this.BundleDependencySplitter))
			{
				this.BundleDependencySplitter = num;
				EditorWindow2 editorWindow = this._owner as EditorWindow2;
				if (editorWindow != null)
				{
					editorWindow.TriggerResize();
				}
			}
			this._dependencyListbox.OnGUI();
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndVertical();
			float num2 = this.Width;
			EditorGUILayout2.HorizontalSplitter(ref num2);
			num2 = Mathf.Clamp(num2, 30f, this._owner.position.width * 0.8f);
			if (!Mathf.Approximately(num2, this.Width))
			{
				this.Width = num2;
				EditorWindow2 editorWindow2 = this._owner as EditorWindow2;
				if (editorWindow2 != null)
				{
					editorWindow2.TriggerResize();
				}
			}
		}

		// Token: 0x0400000E RID: 14
		private GUIToolbar _menu;

		// Token: 0x0400000F RID: 15
		private EditorWindow _owner;

		// Token: 0x04000010 RID: 16
		private AssetBundleManifestUI.Listbox _listbox;

		// Token: 0x04000011 RID: 17
		private AssetBundleManifestUI.DependencyListbox _dependencyListbox;

		// Token: 0x04000012 RID: 18
		private GUIToolbarSearchField _searchfield;

		// Token: 0x04000013 RID: 19
		private List<string> _files = new List<string>();

		// Token: 0x04000014 RID: 20
		public string EditorPrefsPath;

		// Token: 0x04000015 RID: 21
		public Action<List<AssetBundleManifest2>> SelectionChange;

		// Token: 0x0200000C RID: 12
		public class Listbox : GUIListView
		{
			// Token: 0x17000023 RID: 35
			// (get) Token: 0x060000A0 RID: 160 RVA: 0x000076C3 File Offset: 0x000058C3
			public int ItemCount
			{
				get
				{
					return this._items.Count;
				}
			}

			// Token: 0x17000024 RID: 36
			// (get) Token: 0x060000A1 RID: 161 RVA: 0x000076D0 File Offset: 0x000058D0
			// (set) Token: 0x060000A2 RID: 162 RVA: 0x0000771C File Offset: 0x0000591C
			public List<string> SelectedBundlePaths
			{
				get
				{
					List<string> list = new List<string>();
					object[] selectedItems = base.SelectedItems;
					foreach (object obj in selectedItems)
					{
						AssetBundleManifestUI.Listbox.Model model = obj as AssetBundleManifestUI.Listbox.Model;
						list.Add(model.Path);
					}
					return list;
				}
				set
				{
					if (value == null)
					{
						this.Clear();
						return;
					}
					List<object> list = new List<object>();
					foreach (string path in value)
					{
						AssetBundleManifestUI.Listbox.Model modelFromPath = this.GetModelFromPath(path);
						if (modelFromPath != null)
						{
							list.Add(modelFromPath);
						}
					}
					base.SelectedItems = list.ToArray();
				}
			}

			// Token: 0x060000A3 RID: 163 RVA: 0x00007794 File Offset: 0x00005994
			public Listbox(EditorWindow editor, GUIControl parent) : base(editor, parent)
			{
				base.HeaderStyle = GUIListViewHeaderStyle.ClickablePopup;
				base.MultiSelect = true;
				base.DragDropEnabled = false;
				base.RightClickSelect = true;
				this.Mode = GUIListViewMode.Details;
				base.ItemSize = new Vector2(0f, 22f);
				base.FullRowSelect = true;
				base.Columns.Add(new AssetBundleManifestUI.Listbox.Column("Name", "Bundle", "The asset bundle manifest file. The tool searches in this file to find assets that are stored in the bundle (assets that have been added explicitly).\nIt does NOT load the actual asset bundle.", 150f, new AssetBundleManifestUI.Listbox.Column.ColumnDrawer(this.OnDrawName), new AssetBundleManifestUI.Listbox.Column.CompareDelegate2(this.OnCompareName)));
				base.Columns.Add(new AssetBundleManifestUI.Listbox.Column("Size", "Size", "Asset bundle file size of this particular manifest file. The asset bundle must be in the same directory as the manifest file and either end with .unity3d or have no file extension at all.", 80f, new AssetBundleManifestUI.Listbox.Column.ColumnDrawer(this.OnDrawSize), new AssetBundleManifestUI.Listbox.Column.CompareDelegate2(this.OnCompareSize)));
			}

			// Token: 0x060000A4 RID: 164 RVA: 0x00007881 File Offset: 0x00005A81
			public void Clear()
			{
				this._items.Clear();
				this._allitems.Clear();
				base.DoChanged();
			}

			// Token: 0x060000A5 RID: 165 RVA: 0x0000789F File Offset: 0x00005A9F
			public void SetItems(List<AssetBundleManifestUI.Listbox.Model> items)
			{
				this._allitems = new List<AssetBundleManifestUI.Listbox.Model>(items);
				this.UpdateFilter();
				base.Sort();
				base.DoChanged();
			}

			// Token: 0x060000A6 RID: 166 RVA: 0x000078BF File Offset: 0x00005ABF
			public void SetTextFilter(string text)
			{
				this._filterResult = SearchTextParser.Parse(text);
				this.UpdateFilter();
				base.Sort();
				base.DoChanged();
			}

			// Token: 0x060000A7 RID: 167 RVA: 0x000078E0 File Offset: 0x00005AE0
			private void UpdateFilter()
			{
				this._items = new List<AssetBundleManifestUI.Listbox.Model>(this._allitems.Count);
				foreach (AssetBundleManifestUI.Listbox.Model model in this._allitems)
				{
					if (this.IsIncludedInFilter(model))
					{
						this._items.Add(model);
					}
				}
			}

			// Token: 0x060000A8 RID: 168 RVA: 0x00007958 File Offset: 0x00005B58
			private bool IsIncludedInFilter(AssetBundleManifestUI.Listbox.Model model)
			{
				return this._filterResult.NamesExpr.Count == 0 || this._filterResult.IsNameMatch(model.Name);
			}

			// Token: 0x060000A9 RID: 169 RVA: 0x00007980 File Offset: 0x00005B80
			private AssetBundleManifestUI.Listbox.Model GetModelFromPath(string path)
			{
				foreach (AssetBundleManifestUI.Listbox.Model model in this._items)
				{
					if (string.Equals(model.Path, path, StringComparison.OrdinalIgnoreCase))
					{
						return model;
					}
				}
				return null;
			}

			// Token: 0x060000AA RID: 170 RVA: 0x000079E4 File Offset: 0x00005BE4
			protected override object[] OnBeforeSortItems()
			{
				return this._items.ToArray();
			}

			// Token: 0x060000AB RID: 171 RVA: 0x000079F1 File Offset: 0x00005BF1
			protected override void OnAfterSortItems(object[] models)
			{
				this._items.Clear();
				this._items.AddRange((AssetBundleManifestUI.Listbox.Model[])models);
			}

			// Token: 0x060000AC RID: 172 RVA: 0x00007A10 File Offset: 0x00005C10
			protected override string OnGetItemKeyword(GUIListViewGetItemKeywordArgs args)
			{
				AssetBundleManifestUI.Listbox.Model model = args.Model as AssetBundleManifestUI.Listbox.Model;
				if (model != null)
				{
					return model.Name;
				}
				return null;
			}

			// Token: 0x060000AD RID: 173 RVA: 0x00007A38 File Offset: 0x00005C38
			private List<string> GetSelectedPaths()
			{
				object[] selectedItems = base.SelectedItems;
				List<string> list = new List<string>(selectedItems.Length);
				foreach (object obj in selectedItems)
				{
					AssetBundleManifestUI.Listbox.Model model = obj as AssetBundleManifestUI.Listbox.Model;
					if (!string.IsNullOrEmpty(model.Path))
					{
						list.Add(model.Path);
					}
				}
				return list;
			}

			// Token: 0x060000AE RID: 174 RVA: 0x00007A94 File Offset: 0x00005C94
			protected override void OnItemContextMenu(GUIListViewContextMenuArgs args)
			{
				base.OnItemContextMenu(args);
				if (base.SelectedItemsCount < 1)
				{
					return;
				}
				GUIUtility.hotControl = 0;
				AssetBundleManifestUI.Listbox.Model model = args.Model as AssetBundleManifestUI.Listbox.Model;
				GenericMenu genericMenu = new GenericMenu();
				genericMenu.AddItem(new GUIContent((Application.platform == null) ? "Reveal in Finder" : "Show in Explorer"), false, (base.SelectedItemsCount <= 10) ? new GenericMenu.MenuFunction2(this.OnContextMenuShowInExplorer) : null, model);
				genericMenu.AddItem(new GUIContent(string.Empty), false, null);
				genericMenu.AddItem(new GUIContent("Copy Full Path"), false, new GenericMenu.MenuFunction(this.OnContextMenuCopyFullPath));
				genericMenu.DropDown(new Rect(args.MenuLocation.x, args.MenuLocation.y, 0f, 0f));
				Event.current.Use();
				base.Editor.Repaint();
			}

			// Token: 0x060000AF RID: 175 RVA: 0x00007B78 File Offset: 0x00005D78
			private void OnContextMenuShowInExplorer(object userData)
			{
				List<string> selectedPaths = this.GetSelectedPaths();
				EditorApplication2.ShowInExplorer(selectedPaths.ToArray());
				base.Editor.Repaint();
			}

			// Token: 0x060000B0 RID: 176 RVA: 0x00007BA4 File Offset: 0x00005DA4
			private void OnContextMenuCopyFullPath()
			{
				List<string> selectedPaths = this.GetSelectedPaths();
				ClipboardUtil.CopyPaths(selectedPaths);
			}

			// Token: 0x060000B1 RID: 177 RVA: 0x00007BBE File Offset: 0x00005DBE
			protected override object OnGetItem(int index)
			{
				if (index < 0 || index >= this._items.Count)
				{
					return null;
				}
				return this._items[index];
			}

			// Token: 0x060000B2 RID: 178 RVA: 0x00007BE0 File Offset: 0x00005DE0
			protected override int OnGetItemCount()
			{
				return this._items.Count;
			}

			// Token: 0x060000B3 RID: 179 RVA: 0x00007BF0 File Offset: 0x00005DF0
			protected override void OnDrawItem(GUIListViewDrawItemArgs args)
			{
				AssetBundleManifestUI.Listbox.Model model = (AssetBundleManifestUI.Listbox.Model)args.Model;
				if (model == null)
				{
					return;
				}
				AssetBundleManifestUI.Listbox.Column column = args.Column as AssetBundleManifestUI.Listbox.Column;
				if (column == null)
				{
					return;
				}
				if (column.IsPrimaryColumn)
				{
					GUIListView.DrawItemImageHelper(ref args.ItemRect, Images.AssetBundle16x16, Vector2.one * 16f);
				}
				column.Drawer(model, args);
			}

			// Token: 0x060000B4 RID: 180 RVA: 0x00007C55 File Offset: 0x00005E55
			private void OnDrawName(AssetBundleManifestUI.Listbox.Model model, GUIListViewDrawItemArgs args)
			{
				args.ItemRect.y = args.ItemRect.y + 3f;
				EditorGUI2.PathLabel(args.ItemRect, model.Name, args.Selected);
			}

			// Token: 0x060000B5 RID: 181 RVA: 0x00007C88 File Offset: 0x00005E88
			private int OnCompareName(AssetBundleManifestUI.Listbox.Model x, AssetBundleManifestUI.Listbox.Model y)
			{
				return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x060000B6 RID: 182 RVA: 0x00007C9C File Offset: 0x00005E9C
			private void OnDrawSize(AssetBundleManifestUI.Listbox.Model model, GUIListViewDrawItemArgs args)
			{
				args.ItemRect.y = args.ItemRect.y + 3f;
				EditorGUI2.Label(args.ItemRect, EditorUtility2.FormatBytes(model.Size), args.Selected);
			}

			// Token: 0x060000B7 RID: 183 RVA: 0x00007CD4 File Offset: 0x00005ED4
			private int OnCompareSize(AssetBundleManifestUI.Listbox.Model x, AssetBundleManifestUI.Listbox.Model y)
			{
				return x.Size.CompareTo(y.Size);
			}

			// Token: 0x0400007A RID: 122
			private List<AssetBundleManifestUI.Listbox.Model> _items = new List<AssetBundleManifestUI.Listbox.Model>();

			// Token: 0x0400007B RID: 123
			private List<AssetBundleManifestUI.Listbox.Model> _allitems = new List<AssetBundleManifestUI.Listbox.Model>();

			// Token: 0x0400007C RID: 124
			private SearchTextParser.Result _filterResult = new SearchTextParser.Result();

			// Token: 0x0200000D RID: 13
			public class Model
			{
				// Token: 0x0400007D RID: 125
				public string Name;

				// Token: 0x0400007E RID: 126
				public string Path;

				// Token: 0x0400007F RID: 127
				public long Size;

				// Token: 0x04000080 RID: 128
				public AssetBundleManifest2 Manifest;
			}

			// Token: 0x02000010 RID: 16
			private class Column : GUIListViewColumn
			{
				// Token: 0x060000C2 RID: 194 RVA: 0x00008245 File Offset: 0x00006445
				public Column(string serializeName, string text, string tooltip, float width, AssetBundleManifestUI.Listbox.Column.ColumnDrawer drawer, AssetBundleManifestUI.Listbox.Column.CompareDelegate2 comparer) : base(text, tooltip, null, width, null)
				{
					this.SerializeName = serializeName;
					this.Drawer = drawer;
					this.Comparer = comparer;
					if (comparer != null)
					{
						this.CompareFunc = new GUIListViewColumn.CompareDelelgate(this.CompareFuncImpl);
					}
				}

				// Token: 0x060000C3 RID: 195 RVA: 0x00008280 File Offset: 0x00006480
				private int CompareFuncImpl(object x, object y)
				{
					return this.Comparer(x as AssetBundleManifestUI.Listbox.Model, y as AssetBundleManifestUI.Listbox.Model);
				}

				// Token: 0x04000096 RID: 150
				public AssetBundleManifestUI.Listbox.Column.ColumnDrawer Drawer;

				// Token: 0x04000097 RID: 151
				public AssetBundleManifestUI.Listbox.Column.CompareDelegate2 Comparer;

				// Token: 0x02000011 RID: 17
				// (Invoke) Token: 0x060000C5 RID: 197
				public delegate void ColumnDrawer(AssetBundleManifestUI.Listbox.Model model, GUIListViewDrawItemArgs args);

				// Token: 0x02000012 RID: 18
				// (Invoke) Token: 0x060000C9 RID: 201
				public delegate int CompareDelegate2(AssetBundleManifestUI.Listbox.Model x, AssetBundleManifestUI.Listbox.Model y);
			}
		}

		// Token: 0x02000013 RID: 19
		private class DependencyListbox : GUIListView
		{
			// Token: 0x060000CC RID: 204 RVA: 0x0000829C File Offset: 0x0000649C
			public DependencyListbox(EditorWindow editor, GUIControl parent) : base(editor, parent)
			{
				base.HeaderStyle = GUIListViewHeaderStyle.Clickable;
				base.MultiSelect = true;
				base.DragDropEnabled = false;
				base.RightClickSelect = true;
				this.Mode = GUIListViewMode.Details;
				base.ItemSize = new Vector2(0f, 22f);
				base.FullRowSelect = true;
				base.Columns.Add(new AssetBundleManifestUI.DependencyListbox.Column("Bundle", "Dependency", "", 150f, new AssetBundleManifestUI.DependencyListbox.Column.ColumnDrawer(this.OnDrawName), new AssetBundleManifestUI.DependencyListbox.Column.CompareDelegate2(this.OnCompareName)));
				base.FlexibleColumn = base.Columns[0];
			}

			// Token: 0x060000CD RID: 205 RVA: 0x00008349 File Offset: 0x00006549
			public void Clear()
			{
				this._items.Clear();
				base.DoChanged();
			}

			// Token: 0x060000CE RID: 206 RVA: 0x0000835C File Offset: 0x0000655C
			public void SetItems(List<string> paths)
			{
				this.Clear();
				foreach (string path in paths)
				{
					AssetBundleManifestUI.DependencyListbox.Model model = new AssetBundleManifestUI.DependencyListbox.Model();
					model.Path = path;
					model.Name = Path.GetFileNameWithoutExtension(path);
					this._items.Add(model);
				}
				base.Sort();
				base.DoChanged();
			}

			// Token: 0x060000CF RID: 207 RVA: 0x000083DC File Offset: 0x000065DC
			protected override object[] OnBeforeSortItems()
			{
				return this._items.ToArray();
			}

			// Token: 0x060000D0 RID: 208 RVA: 0x000083E9 File Offset: 0x000065E9
			protected override void OnAfterSortItems(object[] models)
			{
				this._items.Clear();
				this._items.AddRange((AssetBundleManifestUI.DependencyListbox.Model[])models);
			}

			// Token: 0x060000D1 RID: 209 RVA: 0x00008407 File Offset: 0x00006607
			protected override object OnGetItem(int index)
			{
				if (index < 0 || index >= this._items.Count)
				{
					return null;
				}
				return this._items[index];
			}

			// Token: 0x060000D2 RID: 210 RVA: 0x00008429 File Offset: 0x00006629
			protected override int OnGetItemCount()
			{
				return this._items.Count;
			}

			// Token: 0x060000D3 RID: 211 RVA: 0x00008438 File Offset: 0x00006638
			protected override void OnDrawItem(GUIListViewDrawItemArgs args)
			{
				AssetBundleManifestUI.DependencyListbox.Model model = (AssetBundleManifestUI.DependencyListbox.Model)args.Model;
				if (model == null)
				{
					return;
				}
				AssetBundleManifestUI.DependencyListbox.Column column = args.Column as AssetBundleManifestUI.DependencyListbox.Column;
				if (column == null)
				{
					return;
				}
				if (column.IsPrimaryColumn)
				{
					GUIListView.DrawItemImageHelper(ref args.ItemRect, Images.AssetBundle16x16, Vector2.one * 16f);
				}
				args.ItemRect.y = args.ItemRect.y + 3f;
				column.Drawer(model, args);
			}

			// Token: 0x060000D4 RID: 212 RVA: 0x000084B5 File Offset: 0x000066B5
			private void OnDrawName(AssetBundleManifestUI.DependencyListbox.Model model, GUIListViewDrawItemArgs args)
			{
				EditorGUI2.PathLabel(args.ItemRect, model.Name, args.Selected);
			}

			// Token: 0x060000D5 RID: 213 RVA: 0x000084D0 File Offset: 0x000066D0
			private int OnCompareName(AssetBundleManifestUI.DependencyListbox.Model x, AssetBundleManifestUI.DependencyListbox.Model y)
			{
				return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x04000098 RID: 152
			private List<AssetBundleManifestUI.DependencyListbox.Model> _items = new List<AssetBundleManifestUI.DependencyListbox.Model>();

			// Token: 0x02000014 RID: 20
			public class Model
			{
				// Token: 0x04000099 RID: 153
				public string Name;

				// Token: 0x0400009A RID: 154
				public string Path;
			}

			// Token: 0x02000015 RID: 21
			private class Column : GUIListViewColumn
			{
				// Token: 0x060000D7 RID: 215 RVA: 0x000084EC File Offset: 0x000066EC
				public Column(string serializeName, string text, string tooltip, float width, AssetBundleManifestUI.DependencyListbox.Column.ColumnDrawer drawer, AssetBundleManifestUI.DependencyListbox.Column.CompareDelegate2 comparer) : base(text, tooltip, null, width, null)
				{
					this.SerializeName = serializeName;
					this.Drawer = drawer;
					this.Comparer = comparer;
					if (comparer != null)
					{
						this.CompareFunc = new GUIListViewColumn.CompareDelelgate(this.CompareFuncImpl);
					}
				}

				// Token: 0x060000D8 RID: 216 RVA: 0x00008527 File Offset: 0x00006727
				private int CompareFuncImpl(object x, object y)
				{
					return this.Comparer(x as AssetBundleManifestUI.DependencyListbox.Model, y as AssetBundleManifestUI.DependencyListbox.Model);
				}

				// Token: 0x0400009B RID: 155
				public AssetBundleManifestUI.DependencyListbox.Column.ColumnDrawer Drawer;

				// Token: 0x0400009C RID: 156
				public AssetBundleManifestUI.DependencyListbox.Column.CompareDelegate2 Comparer;

				// Token: 0x02000016 RID: 22
				// (Invoke) Token: 0x060000DA RID: 218
				public delegate void ColumnDrawer(AssetBundleManifestUI.DependencyListbox.Model model, GUIListViewDrawItemArgs args);

				// Token: 0x02000017 RID: 23
				// (Invoke) Token: 0x060000DE RID: 222
				public delegate int CompareDelegate2(AssetBundleManifestUI.DependencyListbox.Model x, AssetBundleManifestUI.DependencyListbox.Model y);
			}
		}
	}
}
