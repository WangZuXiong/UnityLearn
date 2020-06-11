using System;
using System.Collections.Generic;
using EditorFramework;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TextureOverview
{
	// Token: 0x02000077 RID: 119
	public class MainWindow : EditorWindow2
	{
		// Token: 0x0600038D RID: 909 RVA: 0x00031F08 File Offset: 0x00030108
		public static EditorWindow CreateWindow()
		{
			EditorWindow window = EditorWindow.GetWindow(typeof(MainWindow));
			window.titleContent = new GUIContent(Globals.ProductTitle);
			window.minSize = new Vector2(500f, 150f);
			return window;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00031F4B File Offset: 0x0003014B
		protected override bool __OnCheckCompatibility()
		{
			return InstallerWindow.ValidateVersion(Globals.MinimumMajorVersion, Globals.MinimumMinorVersion, Globals.ProductTitle, Globals.ProductName, Globals.ProductFeedbackUrl) && base.__OnCheckCompatibility();
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00031F78 File Offset: 0x00030178
		protected override void __OnCreate()
		{
			BuildTargetGroup @int = (BuildTargetGroup)EditorPrefs.GetInt(string.Format("{0}.Platform", Globals.ProductId), 0);
			this._listview = new Listbox(this, null);
			this._listview.EditorPrefsPath = string.Format("{0}.ListView", Globals.ProductId);
			this._listview.LoadPrefs();
			this._listview.EmptyText = "Loading texture importer settings. Please wait...";
			this._listview.SetPlatform(@int);
			this._menu = new GUIToolbar(this, null);
			GUIToolbarMenu guitoolbarMenu = this._menu.AddMenu("Tools", "", null);
			guitoolbarMenu.Add(new GUIToolbarMenuItem("Export as CSV...", new Action<GUIToolbarMenuItem>(this.OnToolsExportCsvExecute)));
			guitoolbarMenu.Add(new GUIToolbarMenuItem("Memory Usage", new Action<GUIToolbarMenuItem>(this.OnToolsMemoryUsageExecute), new Func<GUIToolbarMenuItem, GUIControlStatus>(this.OnToolsMemoryUsageQuery)));
			guitoolbarMenu.Add(new GUIToolbarMenuItem("Issue Detection/Warn Compression Fail", new Action<GUIToolbarMenuItem>(this.OnWarningsCompressionFailExecute), new Func<GUIToolbarMenuItem, GUIControlStatus>(this.OnWarningsCompressionFailQuery)));
			guitoolbarMenu.Add(new GUIToolbarMenuItem("Issue Detection/Warn Lossy Compressed Source Texture", new Action<GUIToolbarMenuItem>(this.OnWarningsLossyCompressedSourceTextureExecute), new Func<GUIToolbarMenuItem, GUIControlStatus>(this.OnWarningsLossyCompressedSourceTextureQuery)));
			guitoolbarMenu.Add(new GUIToolbarMenuItem("Issue Detection/Warn Legacy Cubemap", new Action<GUIToolbarMenuItem>(this.OnWarningsLegacyCubemapExecute), new Func<GUIToolbarMenuItem, GUIControlStatus>(this.OnWarningsLegacyCubemapQuery)));
			guitoolbarMenu.Add(GUIToolbarMenuItem.Separator);
			guitoolbarMenu.Add(new GUIToolbarMenuItem("Advanced/Show assets from Editor directories", new Action<GUIToolbarMenuItem>(this.OnToolsShowEditorAssetsExecute), new Func<GUIToolbarMenuItem, GUIControlStatus>(this.OnToolsShowEditorAssetsQuery)));
			guitoolbarMenu.Add(new GUIToolbarMenuItem("Advanced/Show assets from Packages", new Action<GUIToolbarMenuItem>(this.OnToolsShowPackageAssetsExecute), new Func<GUIToolbarMenuItem, GUIControlStatus>(this.OnToolsShowPackageAssetsQuery)));
			guitoolbarMenu.Add(new GUIToolbarMenuItem("Advanced/Count Renderer (Scene mode only, EXPERIMENTAL)", new Action<GUIToolbarMenuItem>(this.OnToolsCountRendererExecute), new Func<GUIToolbarMenuItem, GUIControlStatus>(this.OnToolsCountRendererQuery)));
			guitoolbarMenu.Add(new GUIToolbarMenuItem("Advanced/Handle RGB24 as RGBA32 on GPU", new Action<GUIToolbarMenuItem>(this.OnToolsGpuExpandRgb24ToRgba32Execute), new Func<GUIToolbarMenuItem, GUIControlStatus>(this.OnToolsGpuExpandRgb24ToRgba32Query)));
			guitoolbarMenu.Add(new GUIToolbarMenuItem("Advanced/Rebuild Texture Overview Cache...", new Action<GUIToolbarMenuItem>(this.OnToolsRebuildCacheExecute)));
			guitoolbarMenu.Add(GUIToolbarMenuItem.Separator);
			guitoolbarMenu.Add(new GUIToolbarMenuItem("About", new Action<GUIToolbarMenuItem>(this.OnToolsAbout)));
			this._pickmode = (MainWindow.PickMode)EditorPrefs.GetInt(string.Format("{0}.PickMode", Globals.ProductId), (int)this._pickmode);
			GUIToolbarPopup guitoolbarPopup = this._menu.AddPopup("", "Select from where to list textures.", new GUIControlExecuteDelegate(this.OnPickModeChange));
			guitoolbarPopup.Items = new GUIToolbarPopupItem[]
			{
				new GUIToolbarPopupItem("Project", MainWindow.PickMode.Project),
				new GUIToolbarPopupItem("Scene", MainWindow.PickMode.Scene),
				new GUIToolbarPopupItem("Selection", MainWindow.PickMode.Selection),
				new GUIToolbarPopupItem("AssetBundle Manifest", MainWindow.PickMode.AssetBundleManifest)
			};
			guitoolbarPopup.SelectTag(this._pickmode);
			this._lockbutton = this._menu.AddButton("", "Selection changes within the Unity Project or Hierachy window have no impact on the list when it is locked.", null, new GUIControlExecuteDelegate(this.OnLockToggle), new GUIControlQueryStatusDelegate(this.OnQueryLockToggle));
			this._lockbutton.ImageSize = new Vector2(13f, 13f);
			this.SetLock(false);
			GUIToolbarButton guitoolbarButton = this._menu.AddButton("Refresh", "Get new snapshot of all textures used by GameObjects at this moment.", EditorFramework.Images.Refresh16x16, new GUIControlExecuteDelegate(this.OnRefreshScene), new GUIControlQueryStatusDelegate(this.OnQueryRefreshScene));
			guitoolbarButton.ImageSize = new Vector2(13f, 13f);
			this._menu.AddSpace(8f);
			this._menu.AddFlexibleSpace();
			this._searchfield = this._menu.AddSearchField("", null, new GUIControlExecuteDelegate(this.OnSearchChange), null);
			this._searchfield.SearchModes = new string[]
			{
				"Search in All",
				"Search in Name",
				"Search in Path",
				"Search in Sprite Packing Tag"
			};
			this._searchfield.SearchMode = EditorPrefs.GetInt(string.Format("{0}.TextFilterMode", Globals.ProductId), 0);
			this._searchfield.LayoutOptions = new GUILayoutOption[]
			{
				GUILayout.MinWidth(50f),
				GUILayout.MaxWidth(5000f),
				GUILayout.ExpandWidth(true)
			};
			this._searchfield.AcceptDrop = true;
			this._typeMenu = this._menu.AddMenu("", "Filter by Type", null);
			foreach (Listbox.TypeFilter typeFilter in new Listbox.TypeFilter[]
			{
				new Listbox.TypeFilter("Default", (TextureImporterType)0, (TextureImporterShape)1),
				new Listbox.TypeFilter("Normal map", (TextureImporterType)1, (TextureImporterShape)1),
				new Listbox.TypeFilter("Editor GUI & Legacy UI", (TextureImporterType)2, (TextureImporterShape)1),
				new Listbox.TypeFilter("Sprite (2D & UI)", (TextureImporterType)8, (TextureImporterShape)1),
				new Listbox.TypeFilter("Cubemap", 0, (TextureImporterShape)2),
				new Listbox.TypeFilter("Cursor", (TextureImporterType)7, (TextureImporterShape)1),
				new Listbox.TypeFilter("Cookie", (TextureImporterType)4, (TextureImporterShape)1),
				new Listbox.TypeFilter("Lightmap", (TextureImporterType)6, (TextureImporterShape)1),
				new Listbox.TypeFilter("Single Channel", (TextureImporterType)10, (TextureImporterShape)1)
			})
			{
				GUIToolbarMenuItem guitoolbarMenuItem = new GUIToolbarMenuItem(typeFilter.Name.Replace("&", "and"), new Action<GUIToolbarMenuItem>(this.OnTypeFilterExecute), new Func<GUIToolbarMenuItem, GUIControlStatus>(this.OnQueryTypeFilter));
				guitoolbarMenuItem.Tag = typeFilter;
				this._typeMenu.Add(guitoolbarMenuItem);
			}
			this._typeMenu.Add(GUIToolbarMenuItem.Separator);
			this._typeMenu.Add(new GUIToolbarMenuItem("Reset Type Filter", new Action<GUIToolbarMenuItem>(this.OnTypeFilterResetExecute))
			{
				Tag = 1
			});
			this._typeMenu.Image = EditorGUIUtility.FindTexture("FilterByType");
			if (this._typeMenu.Image != null)
			{
				this._typeMenu.ImageSize = new Vector2((float)this._typeMenu.Image.width, (float)this._typeMenu.Image.height);
			}
			this._menu.AddSpace(8f);
			GUIToolbarRadioGroup guitoolbarRadioGroup = this._menu.AddRadioGroup();
			GUIToolbarRadioButton guitoolbarRadioButton = guitoolbarRadioGroup.AddButton("Default", "Default settings", null, new GUIControlExecuteDelegate(this.OnPlatformChange));
			guitoolbarRadioButton.Tag = 0;
			GUIToolbarRadioButton control = guitoolbarRadioButton;
			List<BuildPlatform2> validPlatforms = BuildPlayerWindow2.GetValidPlatforms();
			foreach (BuildPlatform2 buildPlatform in validPlatforms)
			{
				GUIToolbarRadioButton guitoolbarRadioButton2 = guitoolbarRadioGroup.AddButton("", buildPlatform.DisplayName + " settings", buildPlatform.SmallIcon as Texture2D, new GUIControlExecuteDelegate(this.OnPlatformChange));
				guitoolbarRadioButton2.Tag = buildPlatform.TargetGroup;
				if (buildPlatform.TargetGroup == @int)
				{
					control = guitoolbarRadioButton2;
				}
			}
			guitoolbarRadioGroup.OnCheckedControl(control);
			this._bottommenu = new GUIToolbar(this, null);
			this._statslabel = this._bottommenu.AddLabel();
			this._bottommenu.AddFlexibleSpace();
			this._searchlabel = this._bottommenu.AddLabel();
			this._bottommenu.AddSpace(9f);
			this._memoryUsage = new TextureMemoryUsageOverlay();
			this._issueOverlay = new TextureIssueOverlay();
			this._manifestGUI = new AssetBundleManifestUI();
			this._manifestGUI.EditorPrefsPath = string.Format("{0}.AssetBundleManifest", Globals.ProductId);
			this._manifestGUI.Init(this);
			this._manifestGUI.SelectionChange = new Action<List<AssetBundleManifest2>>(this.OnShowAssetBundleManifestContent);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00032744 File Offset: 0x00030944
		protected override void __OnDestroy()
		{
			if (this._manifestGUI != null)
			{
				AssetBundleManifestUI manifestGUI = this._manifestGUI;
				manifestGUI.SelectionChange = (Action<List<AssetBundleManifest2>>)Delegate.Remove(manifestGUI.SelectionChange, new Action<List<AssetBundleManifest2>>(this.OnShowAssetBundleManifestContent));
				this._manifestGUI.Destroy();
				this._manifestGUI = null;
			}
			if (this._listview != null)
			{
				Listbox listview = this._listview;
				listview.Changed = (Action<GUIListView>)Delegate.Remove(listview.Changed, new Action<GUIListView>(this.OnListboxChanged));
				Listbox listview2 = this._listview;
				listview2.TextureChanged = (Action<Listbox>)Delegate.Remove(listview2.TextureChanged, new Action<Listbox>(this.OnListboxTextureChanged));
				this._listview.SavePrefs();
				this._listview.Dispose();
				this._listview = null;
			}
			EditorFramework.Images.OnDestroy();
			InternalEditorUtility.RepaintAllViews();
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00032810 File Offset: 0x00030A10
		protected override void __OnInit()
		{
			this._listview.ReadCacheFile();
			this._listview.EmptyText = "The list is empty.";
			Listbox listview = this._listview;
			listview.Changed = (Action<GUIListView>)Delegate.Combine(listview.Changed, new Action<GUIListView>(this.OnListboxChanged));
			Listbox listview2 = this._listview;
			listview2.TextureChanged = (Action<Listbox>)Delegate.Combine(listview2.TextureChanged, new Action<Listbox>(this.OnListboxTextureChanged));
			this.DoPickModeChange(this._pickmode, true);
			if (this._memoryUsage.Visible)
			{
				this.UpdateMemoryUsage();
			}
		}

		// Token: 0x06000392 RID: 914 RVA: 0x000328A8 File Offset: 0x00030AA8
		protected override void __OnGUI()
		{
			base.BeginWindows();
			EditorGUILayout.BeginVertical(new GUILayoutOption[0]);
			this._menu.OnGUI();
			this._typeMenu.Text = "";
			this._typeMenu.Tint = null;
			if (this._listview.GetTypeFilterCount() > 0)
			{
				this._typeMenu.Text = "!";
				this._typeMenu.Tint = new Color?(new Color32(204, 194, 16, 128));
			}
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (this._manifestGUI != null && this._pickmode == MainWindow.PickMode.AssetBundleManifest)
			{
				this._manifestGUI.OnGUI();
			}
			EditorGUILayout.BeginVertical(new GUILayoutOption[0]);
			this._listview.OnGUI();
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			this._bottommenu.OnGUI();
			EditorGUILayout.EndVertical();
			if (this._memoryUsage != null && this._listview.Platform != null)
			{
				this._memoryUsage.OnGUI(this);
			}
			if (this._issueOverlay != null)
			{
				this._issueOverlay.OnGUI(this);
			}
			base.EndWindows();
		}

		// Token: 0x06000393 RID: 915 RVA: 0x000329D0 File Offset: 0x00030BD0
		protected override void __OnFindCommand()
		{
			this._searchfield.Focus();
		}

		// Token: 0x06000394 RID: 916 RVA: 0x000329DD File Offset: 0x00030BDD
		private void OnLockToggle(GUIControl sender)
		{
			this.SetLock(!this._locked);
			base.Repaint();
		}

		// Token: 0x06000395 RID: 917 RVA: 0x000329F4 File Offset: 0x00030BF4
		private GUIControlStatus OnQueryLockToggle(GUIControl sender)
		{
			GUIControlStatus guicontrolStatus = GUIControlStatus.None;
			if (this._pickmode == MainWindow.PickMode.Selection)
			{
				guicontrolStatus |= (GUIControlStatus.Enable | GUIControlStatus.Visible);
			}
			return guicontrolStatus;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00032A10 File Offset: 0x00030C10
		private void SetLock(bool locked)
		{
			if (locked)
			{
				this._lockbutton.Image = EditorFramework.Images.Lock16x16;
				this._lockbutton.Text = "Unlock";
			}
			else
			{
				this._lockbutton.Image = EditorFramework.Images.Unlock16x16;
				this._lockbutton.Text = "Lock";
			}
			this._locked = locked;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00032A69 File Offset: 0x00030C69
		private void OnRefreshScene(GUIControl sender)
		{
			this.DoPickModeChange(MainWindow.PickMode.Scene, true);
			base.Repaint();
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00032A7C File Offset: 0x00030C7C
		private GUIControlStatus OnQueryRefreshScene(GUIControl sender)
		{
			GUIControlStatus guicontrolStatus = GUIControlStatus.None;
			if (this._pickmode == MainWindow.PickMode.Scene)
			{
				guicontrolStatus |= (GUIControlStatus.Enable | GUIControlStatus.Visible);
			}
			return guicontrolStatus;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00032A9C File Offset: 0x00030C9C
		private void OnPlatformChange(GUIControl sender)
		{
			BuildTargetGroup buildTargetGroup = (BuildTargetGroup)sender.Tag;
			this._listview.SetPlatform(buildTargetGroup);
			EditorPrefs.SetInt(string.Format("{0}.Platform", Globals.ProductId), (int)buildTargetGroup);
			base.Repaint();
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00032ADC File Offset: 0x00030CDC
		private void OnToolsExportCsvExecute(GUIToolbarMenuItem sender)
		{
			this._listview.SaveCsv();
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00032AE9 File Offset: 0x00030CE9
		private void OnToolsMemoryUsageExecute(GUIToolbarMenuItem sender)
		{
			this._memoryUsage.Visible = !this._memoryUsage.Visible;
			if (this._memoryUsage.Visible)
			{
				this.UpdateMemoryUsage();
			}
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00032B18 File Offset: 0x00030D18
		private GUIControlStatus OnToolsMemoryUsageQuery(GUIToolbarMenuItem sender)
		{
			GUIControlStatus guicontrolStatus = GUIControlStatus.Visible;
			if (this._listview.Platform != null)
			{
				guicontrolStatus |= GUIControlStatus.Enable;
			}
			if (this._memoryUsage.Visible)
			{
				guicontrolStatus |= GUIControlStatus.Checked;
			}
			return guicontrolStatus;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00032B4A File Offset: 0x00030D4A
		private void OnWarningsCompressionFailExecute(GUIToolbarMenuItem sender)
		{
			Globals.WarnCompressionFail = !Globals.WarnCompressionFail;
			this.DoPickModeChange(this._pickmode, false);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00032B68 File Offset: 0x00030D68
		private GUIControlStatus OnWarningsCompressionFailQuery(GUIToolbarMenuItem sender)
		{
			GUIControlStatus guicontrolStatus = GUIControlStatus.Enable | GUIControlStatus.Visible;
			if (Globals.WarnCompressionFail)
			{
				guicontrolStatus |= GUIControlStatus.Checked;
			}
			return guicontrolStatus;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00032B83 File Offset: 0x00030D83
		private void OnWarningsLossyCompressedSourceTextureExecute(GUIToolbarMenuItem sender)
		{
			Globals.WarnLossyCompressedSourceTexture = !Globals.WarnLossyCompressedSourceTexture;
			this.DoPickModeChange(this._pickmode, false);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00032BA0 File Offset: 0x00030DA0
		private GUIControlStatus OnWarningsLossyCompressedSourceTextureQuery(GUIToolbarMenuItem sender)
		{
			GUIControlStatus guicontrolStatus = GUIControlStatus.Enable | GUIControlStatus.Visible;
			if (Globals.WarnLossyCompressedSourceTexture)
			{
				guicontrolStatus |= GUIControlStatus.Checked;
			}
			return guicontrolStatus;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00032BBB File Offset: 0x00030DBB
		private void OnWarningsLegacyCubemapExecute(GUIToolbarMenuItem sender)
		{
			Globals.WarnLegacyCubemap = !Globals.WarnLegacyCubemap;
			this.DoPickModeChange(this._pickmode, false);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00032BD8 File Offset: 0x00030DD8
		private GUIControlStatus OnWarningsLegacyCubemapQuery(GUIToolbarMenuItem sender)
		{
			GUIControlStatus guicontrolStatus = GUIControlStatus.Enable | GUIControlStatus.Visible;
			if (Globals.WarnLegacyCubemap)
			{
				guicontrolStatus |= GUIControlStatus.Checked;
			}
			return guicontrolStatus;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00032BF4 File Offset: 0x00030DF4
		private void OnToolsRebuildCacheExecute(GUIToolbarMenuItem sender)
		{
			string text = "Rebuilding the Texture Overview Cache often helps if Texture Overview displays invalid/non-sense data, that could occur when switching between different Unity versions or installing/uninstalling Platform modules.\n\nRebuilding the Cache might take a while to complete, but can be canceled and resumed at any time.\n\nDo you want to continue?";
			if (!EditorUtility.DisplayDialog(Globals.ProductTitle, text, "Rebuild", "Cancel"))
			{
				return;
			}
			this._listview.DeleteCacheFile();
			this._listview.ReadCacheFile();
			this.DoPickModeChange(this._pickmode, true);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00032C44 File Offset: 0x00030E44
		private void OnToolsAbout(GUIToolbarMenuItem sender)
		{
			AboutWindow window = EditorWindow.GetWindow<AboutWindow>(true, "About " + Globals.ProductTitle, true);
			window.ProductName = Globals.ProductName;
			window.FeedbackUrl = Globals.ProductFeedbackUrl;
			window.AssetStoreUrl = Globals.ProductAssetStoreUrl;
			window.Show();
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00032C8F File Offset: 0x00030E8F
		private void OnToolsGpuExpandRgb24ToRgba32Execute(GUIToolbarMenuItem sender)
		{
			Globals.GpuExpandRgb24ToRgba32 = !Globals.GpuExpandRgb24ToRgba32;
			this._listview.RefreshModels();
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00032CAC File Offset: 0x00030EAC
		private GUIControlStatus OnToolsGpuExpandRgb24ToRgba32Query(GUIToolbarMenuItem sender)
		{
			GUIControlStatus guicontrolStatus = GUIControlStatus.Enable | GUIControlStatus.Visible;
			if (Globals.GpuExpandRgb24ToRgba32)
			{
				guicontrolStatus |= GUIControlStatus.Checked;
			}
			return guicontrolStatus;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00032CC7 File Offset: 0x00030EC7
		private void OnToolsCountRendererExecute(GUIToolbarMenuItem sender)
		{
			Globals.CountRendererInSceneMode = !Globals.CountRendererInSceneMode;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00032CD8 File Offset: 0x00030ED8
		private GUIControlStatus OnToolsCountRendererQuery(GUIToolbarMenuItem sender)
		{
			GUIControlStatus guicontrolStatus = GUIControlStatus.Enable | GUIControlStatus.Visible;
			if (Globals.CountRendererInSceneMode)
			{
				guicontrolStatus |= GUIControlStatus.Checked;
			}
			return guicontrolStatus;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00032CF3 File Offset: 0x00030EF3
		private void OnToolsShowEditorAssetsExecute(GUIToolbarMenuItem sender)
		{
			Globals.ShowEditorAssets = !Globals.ShowEditorAssets;
			this.DoPickModeChange(this._pickmode, false);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00032D10 File Offset: 0x00030F10
		private GUIControlStatus OnToolsShowEditorAssetsQuery(GUIToolbarMenuItem sender)
		{
			GUIControlStatus guicontrolStatus = GUIControlStatus.Enable | GUIControlStatus.Visible;
			if (Globals.ShowEditorAssets)
			{
				guicontrolStatus |= GUIControlStatus.Checked;
			}
			return guicontrolStatus;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00032D2B File Offset: 0x00030F2B
		private void OnToolsShowPackageAssetsExecute(GUIToolbarMenuItem sender)
		{
			Globals.ShowPackageAssets = !Globals.ShowPackageAssets;
			this.DoPickModeChange(this._pickmode, false);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00032D48 File Offset: 0x00030F48
		private GUIControlStatus OnToolsShowPackageAssetsQuery(GUIToolbarMenuItem sender)
		{
			GUIControlStatus guicontrolStatus = GUIControlStatus.Enable | GUIControlStatus.Visible;
			if (Globals.ShowPackageAssets)
			{
				guicontrolStatus |= GUIControlStatus.Checked;
			}
			return guicontrolStatus;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00032D64 File Offset: 0x00030F64
		private void OnListboxChanged(GUIListView sender)
		{
			this._searchlabel.Text = this._listview.ItemCountString;
			this._statslabel.Text = this._listview.SelectionString;
			this._issueOverlay.TitleString = "Issue detected";
			this._issueOverlay.IssueString = this._listview.IssueString;
			this._issueOverlay.Visible = !string.IsNullOrEmpty(this._listview.IssueString);
			this.UpdateMemoryUsage();
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00032DE7 File Offset: 0x00030FE7
		private void OnListboxTextureChanged(GUIListView sender)
		{
			this.UpdateMemoryUsage();
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00032DF0 File Offset: 0x00030FF0
		private void UpdateMemoryUsage()
		{
			if (!this._memoryUsage.Visible)
			{
				return;
			}
			this._listview.GetMemoryUsage(out this._memoryUsage.CpuUsage, out this._memoryUsage.GpuUsage, out this._memoryUsage.RuntimeUsage, out this._memoryUsage.StorageUsage);
			base.Repaint();
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00032E48 File Offset: 0x00031048
		private void OnSearchChange(GUIControl sender)
		{
			GUIToolbarSearchField guitoolbarSearchField = (GUIToolbarSearchField)sender;
			EditorPrefs.SetInt(string.Format("{0}.TextFilterMode", Globals.ProductId), guitoolbarSearchField.SearchMode);
			this._listview.SetTextFilter(guitoolbarSearchField.Text, (Listbox.TextFilterMode)guitoolbarSearchField.SearchMode);
			if (this._listview.ItemCount == 0 && !string.IsNullOrEmpty(sender.Text))
			{
				this._listview.EmptyText = "No match found. Proper search mode used?\n\nYou can choose the search mode using the magnifying glass next to the search field.\nYou can drop files and folders on the search field.\nYou can use search operators like: && (and) || (or) and ! (not)";
				return;
			}
			this._listview.EmptyText = "The list is empty.";
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00032EC8 File Offset: 0x000310C8
		private void OnSelectionChange()
		{
			if (this._listview == null)
			{
				return;
			}
			if (this._pickmode != MainWindow.PickMode.Selection || this._locked || this._listview.ChangeUnitySelectionTime + 0.5 > DateTime.Now.TimeOfDay.TotalSeconds)
			{
				return;
			}
			Object[] array = Selection.objects;
			List<string> list = new List<string>();
			array = EditorUtility.CollectDependencies(array);
			new List<string>();
			foreach (Object @object in array)
			{
				if (@object is Texture)
				{
					string assetPath = AssetDatabase.GetAssetPath(@object);
					if (!string.IsNullOrEmpty(assetPath))
					{
						list.Add(assetPath);
					}
				}
			}
			this._listview.SetItems(list);
			this.UpdateMemoryUsage();
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00032F84 File Offset: 0x00031184
		private void OnShowAssetBundleManifestContent(List<AssetBundleManifest2> manifests)
		{
			List<string> list = new List<string>();
			foreach (AssetBundleManifest2 assetBundleManifest in manifests)
			{
				list.AddRange(assetBundleManifest.Assets);
			}
			this._listview.SetItems(list);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00032FEC File Offset: 0x000311EC
		private void OnPickModeChange(GUIControl sender)
		{
			GUIToolbarPopup guitoolbarPopup = (GUIToolbarPopup)sender;
			this.SetLock(false);
			this._pickmode = (MainWindow.PickMode)guitoolbarPopup.SelectedItem.Tag;
			EditorPrefs.SetInt(string.Format("{0}.PickMode", Globals.ProductId), (int)this._pickmode);
			this.DoPickModeChange(this._pickmode, false);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00033044 File Offset: 0x00031244
		private void DoPickModeChange(MainWindow.PickMode newpickmode, bool checkTimeStamp)
		{
			this._listview.SetSceneOccurrenceLookup(null);
			switch (newpickmode)
			{
			case MainWindow.PickMode.Selection:
				this._listview.SetItems(new List<string>());
				this.OnSelectionChange();
				break;
			case MainWindow.PickMode.Project:
				this._listview.SetItems(new List<string>(AssetDatabase.GetAllAssetPaths()));
				break;
			case MainWindow.PickMode.Scene:
			{
				List<Object> list = new List<Object>(EditorUtility.CollectDependencies(EditorUtility2.GetSceneRootObjects().ToArray()));
				if (RenderSettings.skybox != null)
				{
					list.AddRange(EditorUtility.CollectDependencies(new Material[]
					{
						RenderSettings.skybox
					}));
				}
				if (Globals.CountRendererInSceneMode)
				{
					Dictionary<string, int> sceneOccurrenceLookup = this.CountRendererInScene();
					this._listview.SetSceneOccurrenceLookup(sceneOccurrenceLookup);
				}
				List<string> list2 = new List<string>(128);
				foreach (Object @object in list)
				{
					if (@object is Texture)
					{
						string assetPath = AssetDatabase.GetAssetPath(@object);
						if (!string.IsNullOrEmpty(assetPath))
						{
							list2.Add(assetPath);
						}
					}
				}
				this._listview.SetItems(list2);
				break;
			}
			case MainWindow.PickMode.AssetBundleManifest:
				this._listview.SetItems(new List<string>());
				break;
			}
			this.UpdateMemoryUsage();
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00033198 File Offset: 0x00031398
		private Dictionary<string, int> CountRendererInScene()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			foreach (GameObject gameObject in EditorUtility2.GetSceneRootObjects())
			{
				this.CountRendererInSceneRecursive(gameObject.transform, dictionary);
			}
			return dictionary;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x000331F8 File Offset: 0x000313F8
		private void CountRendererInSceneRecursive(Transform parent, Dictionary<string, int> lookup)
		{
			if (parent == null)
			{
				return;
			}
			Renderer component = parent.GetComponent<Renderer>();
			if (component != null)
			{
				Object[] array = EditorUtility.CollectDependencies(component.sharedMaterials);
				foreach (Object @object in array)
				{
					Texture texture = @object as Texture;
					if (!(texture == null))
					{
						string assetPath = AssetDatabase.GetAssetPath(@object);
						if (!string.IsNullOrEmpty(assetPath))
						{
							string text = AssetDatabase.AssetPathToGUID(assetPath);
							if (!string.IsNullOrEmpty(text))
							{
								int num;
								if (!lookup.TryGetValue(text, out num))
								{
									num = (lookup[text] = 0);
								}
								num++;
								lookup[text] = num;
							}
						}
					}
				}
			}
			int j = 0;
			int childCount = parent.childCount;
			while (j < childCount)
			{
				Transform child = parent.GetChild(j);
				if (child != null)
				{
					this.CountRendererInSceneRecursive(child, lookup);
				}
				j++;
			}
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x000332DC File Offset: 0x000314DC
		private void OnTypeFilterExecute(GUIToolbarMenuItem sender)
		{
			this._listview.BeginChangeFilter();
			try
			{
				Listbox.TypeFilter filter = sender.Tag as Listbox.TypeFilter;
				if (this._listview.HasTypeFilter(filter))
				{
					this._listview.RemoveTypeFilter(filter);
				}
				else
				{
					this._listview.AddTypeFilter(filter);
				}
				base.Repaint();
			}
			finally
			{
				this._listview.EndChangeFilter();
			}
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0003334C File Offset: 0x0003154C
		private GUIControlStatus OnQueryTypeFilter(GUIToolbarMenuItem sender)
		{
			GUIControlStatus guicontrolStatus = GUIControlStatus.Enable | GUIControlStatus.Visible;
			if (this._listview.HasTypeFilter(sender.Tag as Listbox.TypeFilter))
			{
				guicontrolStatus |= GUIControlStatus.Checked;
			}
			return guicontrolStatus;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00033378 File Offset: 0x00031578
		private void OnTypeFilterResetExecute(GUIToolbarMenuItem sender)
		{
			this._listview.ClearTypeFilter();
		}

		// Token: 0x04000229 RID: 553
		private GUIToolbar _menu;

		// Token: 0x0400022A RID: 554
		private GUIToolbar _bottommenu;

		// Token: 0x0400022B RID: 555
		private Listbox _listview;

		// Token: 0x0400022C RID: 556
		private MainWindow.PickMode _pickmode = MainWindow.PickMode.Project;

		// Token: 0x0400022D RID: 557
		private GUIToolbarLabel _searchlabel;

		// Token: 0x0400022E RID: 558
		private GUIToolbarLabel _statslabel;

		// Token: 0x0400022F RID: 559
		private GUIToolbarMenu _typeMenu;

		// Token: 0x04000230 RID: 560
		private GUIToolbarButton _lockbutton;

		// Token: 0x04000231 RID: 561
		private GUIToolbarSearchField _searchfield;

		// Token: 0x04000232 RID: 562
		private bool _locked;

		// Token: 0x04000233 RID: 563
		private TextureMemoryUsageOverlay _memoryUsage;

		// Token: 0x04000234 RID: 564
		private AssetBundleManifestUI _manifestGUI;

		// Token: 0x04000235 RID: 565
		private TextureIssueOverlay _issueOverlay;

		// Token: 0x02000078 RID: 120
		private enum PickMode
		{
			// Token: 0x04000237 RID: 567
			Selection,
			// Token: 0x04000238 RID: 568
			Project,
			// Token: 0x04000239 RID: 569
			Scene,
			// Token: 0x0400023A RID: 570
			AssetBundleManifest
		}
	}
}
