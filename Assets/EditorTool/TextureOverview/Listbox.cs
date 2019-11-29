using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using EditorFramework;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TextureOverview
{
	// Token: 0x0200006D RID: 109
	public class Listbox : GUIListView, IDisposable
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0002D675 File Offset: 0x0002B875
		public int ItemCount
		{
			get
			{
				return this._items.Count;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0002D684 File Offset: 0x0002B884
		public string ItemCountString
		{
			get
			{
				string text = string.Format("{0} {1}", this._warningCount, (this._warningCount == 1) ? "issue" : "issues");
				if (this._items.Count != this._allitems.Count)
				{
					return string.Format("{0}/{1} textures | {2}", this._items.Count, this._allitems.Count, text);
				}
				return string.Format("{0} textures | {1}", this._items.Count, text);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0002D71B File Offset: 0x0002B91B
		// (set) Token: 0x0600028E RID: 654 RVA: 0x0002D723 File Offset: 0x0002B923
		public double ChangeUnitySelectionTime { get; private set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600028F RID: 655 RVA: 0x0002D72C File Offset: 0x0002B92C
		public string SelectionString
		{
			get
			{
				if (base.SelectedItemsCount == 0)
				{
					return "";
				}
				List<Listbox.Model> selectedModels = this.GetSelectedModels();
				if (base.SelectedItemsCount == 1)
				{
					return selectedModels[0].AssetPath;
				}
				long num = 0L;
				long num2 = 0L;
				foreach (Listbox.Model model in selectedModels)
				{
					num += (long)model.PlatformStorageSize;
					num2 += (long)model.PlatformRuntimeSize;
				}
				return string.Format("{0} selected | Storage: {1} | Runtime: {2}", selectedModels.Count, EditorUtility2.FormatBytes(num), EditorUtility2.FormatBytes(num2));
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0002D7DC File Offset: 0x0002B9DC
		public string IssueString
		{
			get
			{
				if (base.SelectedItemsCount != 1)
				{
					return "";
				}
				List<Listbox.Model> selectedModels = this.GetSelectedModels();
				return selectedModels[0].PlatformIssueString;
			}
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0002D80C File Offset: 0x0002BA0C
		public Listbox(EditorWindow editor, GUIControl parent) : base(editor, parent)
		{
			base.HeaderStyle = GUIListViewHeaderStyle.ClickablePopup;
			base.MultiSelect = true;
			base.DragDropEnabled = true;
			base.RightClickSelect = true;
			this.Mode = GUIListViewMode.Details;
			base.ItemSize = new Vector2(0f, 22f);
			base.FullRowSelect = true;
			base.Columns.Add(new Listbox.Column("Name", "Name", "Asset Name", 220f, new Listbox.Column.ColumnDrawer(this.OnDrawAssetName), new Listbox.Column.CompareDelegate2(this.OnCompareAssetName), new Listbox.Column.ExportDelegate(this.OnExportAssetName)));
			base.Columns.Add(new Listbox.Column("Path", "Path", "Asset Path", 220f, new Listbox.Column.ColumnDrawer(this.OnDrawAssetPath), new Listbox.Column.CompareDelegate2(this.OnCompareAssetPath), new Listbox.Column.ExportDelegate(this.OnExportAssetPath))
			{
				Visible = false
			});
			base.Columns.Add(new Listbox.Column("Issue", "Issue", "Indicates whether an issue was found with the texture and the selected platform.", 40f, new Listbox.Column.ColumnDrawer(this.OnDrawHasIssue), new Listbox.Column.CompareDelegate2(this.OnCompareHasIssue), new Listbox.Column.ExportDelegate(this.OnExportHasIssue))
			{
				Visible = false
			});
			base.Columns.Add(new Listbox.Column("Type", "Type", "Texture Type is used to define what your Texture is to be used for.", 110f, new Listbox.Column.ColumnDrawer(this.OnDrawImportType), new Listbox.Column.CompareDelegate2(this.OnCompareImportType), new Listbox.Column.ExportDelegate(this.OnExportImportType)));
			base.Columns.Add(new Listbox.Column("Shape", "Shape", "Texture Shape setting.", 60f, new Listbox.Column.ColumnDrawer(this.OnDrawImportShape), new Listbox.Column.CompareDelegate2(this.OnCompareImportShape), new Listbox.Column.ExportDelegate(this.OnExportImportShape)));
			base.Columns.Add(new Listbox.Column("RendererCount", "Renderer Count", "How often a texture is referenced by Renderer Components, through their sharedMaterials property, in the open scene. UI elements are not included. This is valid in 'Scene Mode' only.\n(EXPERIMENTAL)", 60f, new Listbox.Column.ColumnDrawer(this.OnDrawRendererCount), new Listbox.Column.CompareDelegate2(this.OnCompareRendererCount), new Listbox.Column.ExportDelegate(this.OnExportRendererCount))
			{
				Visible = false
			});
			base.Columns.Add(new Listbox.Column("sRGB", "sRGB (Color Texture)", "Texture content is stored in gamme space. Non-HDR color textures should enable this flag (except if used for IMGUI).", 85f, new Listbox.Column.ColumnDrawer(this.OnDrawsRGBTexture), new Listbox.Column.CompareDelegate2(this.OnComparesRGBTexture), new Listbox.Column.ExportDelegate(this.OnExportsRGBTexture))
			{
				Visible = false
			});
			base.Columns.Add(new Listbox.Column("Alpha", "Alpha", "Indicates whether the texture contains an alpha channel.", 40f, new Listbox.Column.ColumnDrawer(this.OnDrawHasAlpha), new Listbox.Column.CompareDelegate2(this.OnCompareHasAlpha), new Listbox.Column.ExportDelegate(this.OnExportHasAlpha))
			{
				Visible = false
			});
			base.Columns.Add(new Listbox.Column("AlphaSource", "Alpha Source", "How is the alpha channel generated for the texture.", 100f, new Listbox.Column.ColumnDrawer(this.OnDrawAlphaSource), new Listbox.Column.CompareDelegate2(this.OnCompareAlphaSource), new Listbox.Column.ExportDelegate(this.OnExportAlphaSource))
			{
				Visible = false
			});
			base.Columns.Add(new Listbox.Column("AlphaIsTransparency", "Alpha is Transparency", "Alpha Is Transparency", 40f, new Listbox.Column.ColumnDrawer(this.OnDrawAlphaIsTransparency), new Listbox.Column.CompareDelegate2(this.OnCompareAlphaIsTransparency), new Listbox.Column.ExportDelegate(this.OnExportAlphaIsTransparency))
			{
				Visible = false
			});
			base.Columns.Add(new Listbox.Column("StorageSize", "Storage Size", "Estimate how many space the asset consumes on the storage system (disk). 'Crunched' compressed assets always assumed to have the worst/biggest size, rather than the actual compressed file size.", 80f, new Listbox.Column.ColumnDrawer(this.OnDrawStorageSize), new Listbox.Column.CompareDelegate2(this.OnCompareStorageSize), new Listbox.Column.ExportDelegate(this.OnExportStorageSize))
			{
				HideIfDefault = true
			});
			base.Columns.Add(new Listbox.Column("RuntimeSize", "Runtime Size", "How many space the asset consumes in memory (graphics + main memory).", 85f, new Listbox.Column.ColumnDrawer(this.OnDrawRuntimeSize), new Listbox.Column.CompareDelegate2(this.OnCompareRuntimeSize), new Listbox.Column.ExportDelegate(this.OnExportRuntimeSize))
			{
				HideIfDefault = true
			});
			base.Columns.Add(new Listbox.Column("CpuSize", "Main Memory", "How many space the texture consumes in main memory. Only applies to textures that use Read/Write Enabled.", 85f, new Listbox.Column.ColumnDrawer(this.OnDrawCpuSize), new Listbox.Column.CompareDelegate2(this.OnCompareCpuSize), new Listbox.Column.ExportDelegate(this.OnExportCpuSize))
			{
				Visible = false,
				HideIfDefault = true
			});
			base.Columns.Add(new Listbox.Column("GpuSize", "Graphics Memory", "How many space the texture consumes in graphics memory.", 85f, new Listbox.Column.ColumnDrawer(this.OnDrawGpuSize), new Listbox.Column.CompareDelegate2(this.OnCompareGpuSize), new Listbox.Column.ExportDelegate(this.OnExportGpuSize))
			{
				Visible = false,
				HideIfDefault = true
			});
			base.Columns.Add(new Listbox.Column("RW", "Read\\Write Enabled", "Read/Write enables access to the texture data from scripts (GetPixels, SetPixels, etc).\n\nNote however that a copy of the texture data will be made, doubling the amount of memory required for the texture. This is only valid for uncompressed and DTX compressed textures, other types of compressed textures cannot be read from.", 35f, new Listbox.Column.ColumnDrawer(this.OnDrawReadWriteEnabled), new Listbox.Column.CompareDelegate2(this.OnCompareReadWriteEnabled), new Listbox.Column.ExportDelegate(this.OnExportReadWriteEnabled)));
			base.Columns.Add(new Listbox.Column("NPOTScale", "Non Power of 2 Scaling", "How non power of two textures are scaled on import.", 100f, new Listbox.Column.ColumnDrawer(this.OnDrawNPOTScale), new Listbox.Column.CompareDelegate2(this.OnCompareNPOTScale), new Listbox.Column.ExportDelegate(this.OnExportNPOTScale))
			{
				Visible = false
			});
			base.Columns.Add(new Listbox.Column("Mips", "Mips", "Number of mipmaps in texture.", 40f, new Listbox.Column.ColumnDrawer(this.OnDrawMips), new Listbox.Column.CompareDelegate2(this.OnCompareMips), new Listbox.Column.ExportDelegate(this.OnExportMips))
			{
				Visible = false,
				HideIfDefault = true
			});
			base.Columns.Add(new Listbox.Column("Mips Enabled", "Mips Enabled", "Generate Mip Maps.", 40f, new Listbox.Column.ColumnDrawer(this.OnDrawMipsEnabled), new Listbox.Column.CompareDelegate2(this.OnCompareMipsEnabled), new Listbox.Column.ExportDelegate(this.OnExportMipsEnabled))
			{
				Visible = false
			});
			base.Columns.Add(new Listbox.Column("StreamingMips", "Streaming Mips", "", 115f, new Listbox.Column.ColumnDrawer(this.OnDrawStreamingMips), new Listbox.Column.CompareDelegate2(this.OnCompareStreamingMips), new Listbox.Column.ExportDelegate(this.OnExportStreamingMips))
			{
				Visible = false
			});
			base.Columns.Add(new Listbox.Column("StreamingMipsPrio", "Streaming Mips Prio", "", 115f, new Listbox.Column.ColumnDrawer(this.OnDrawStreamingMipsPrio), new Listbox.Column.CompareDelegate2(this.OnCompareStreamingMipsPrio), new Listbox.Column.ExportDelegate(this.OnExportStreamingMipsPrio))
			{
				Visible = false
			});
			base.Columns.Add(new Listbox.Column("Width", "Width", "Texture Width in pixels.", 45f, new Listbox.Column.ColumnDrawer(this.OnDrawWidth), new Listbox.Column.CompareDelegate2(this.OnCompareWidth), new Listbox.Column.ExportDelegate(this.OnExportWidth))
			{
				HideIfDefault = true
			});
			base.Columns.Add(new Listbox.Column("Height", "Height", "Texture Height in pixels.", 45f, new Listbox.Column.ColumnDrawer(this.OnDrawHeight), new Listbox.Column.CompareDelegate2(this.OnCompareHeight), new Listbox.Column.ExportDelegate(this.OnExportHeight))
			{
				HideIfDefault = true
			});
			base.Columns.Add(new Listbox.Column("Override", "Override", "Whether texture settings have been overridden for the selected platform.", 60f, new Listbox.Column.ColumnDrawer(this.OnDrawOverride), new Listbox.Column.CompareDelegate2(this.OnCompareOverride), new Listbox.Column.ExportDelegate(this.OnExportOverride))
			{
				Mode = Listbox.Column.ColumnMode.PlatformSpecific,
				HideIfDefault = true
			});
			base.Columns.Add(new Listbox.Column("MaxSize", "Max Size", "Maximum Texture Size in pixels.", 60f, new Listbox.Column.ColumnDrawer(this.OnDrawMaxSize), new Listbox.Column.CompareDelegate2(this.OnCompareMaxSize), new Listbox.Column.ExportDelegate(this.OnExportMaxSize))
			{
				Mode = Listbox.Column.ColumnMode.PlatformSpecific
			});
			base.Columns.Add(new Listbox.Column("TextureCompression", "Compression", "How will the texture be compressed?", 85f, new Listbox.Column.ColumnDrawer(this.OnDrawCompression), new Listbox.Column.CompareDelegate2(this.OnCompareCompression), new Listbox.Column.ExportDelegate(this.OnExportCompression))
			{
				Mode = Listbox.Column.ColumnMode.PlatformSpecific
			});
			base.Columns.Add(new Listbox.Column("FormatShort", "Format", "Internal representation used for the texture.", 115f, new Listbox.Column.ColumnDrawer(this.OnDrawFormatShort), new Listbox.Column.CompareDelegate2(this.OnCompareFormatShort), new Listbox.Column.ExportDelegate(this.OnExportFormatShort))
			{
				Mode = Listbox.Column.ColumnMode.PlatformSpecific
			});
			base.Columns.Add(new Listbox.Column("Format", "Format (full)", "Internal representation used for the texture.", 200f, new Listbox.Column.ColumnDrawer(this.OnDrawFormat), new Listbox.Column.CompareDelegate2(this.OnCompareFormat), new Listbox.Column.ExportDelegate(this.OnExportFormat))
			{
				Visible = false,
				Mode = Listbox.Column.ColumnMode.PlatformSpecific
			});
			base.Columns.Add(new Listbox.Column("CompressionQuality", "Compressor Quality", "Quality of texture compression (0=Fastest, 100=Best).", 85f, new Listbox.Column.ColumnDrawer(this.OnDrawCompressionQuality), new Listbox.Column.CompareDelegate2(this.OnCompareCompressionQuality), new Listbox.Column.ExportDelegate(this.OnExportCompressionQuality))
			{
				Visible = false,
				Mode = Listbox.Column.ColumnMode.PlatformSpecific
			});
			base.Columns.Add(new Listbox.Column("UseCrunchCompression", "Crunch", "Whether the texture is crunch-compressed to save space on disk.", 85f, new Listbox.Column.ColumnDrawer(this.OnDrawUseCrunchCompression), new Listbox.Column.CompareDelegate2(this.OnCompareUseCrunchCompression), new Listbox.Column.ExportDelegate(this.OnExportUseCrunchCompression))
			{
				Visible = false,
				Mode = Listbox.Column.ColumnMode.PlatformSpecific
			});
			base.Columns.Add(new Listbox.Column("TextureResizeAlgorithm", "Resize Algorithm", "", 115f, new Listbox.Column.ColumnDrawer(this.OnDrawResizeAlgorithm), new Listbox.Column.CompareDelegate2(this.OnCompareResizeAlgorithm), new Listbox.Column.ExportDelegate(this.OnExportResizeAlgorithm))
			{
				Visible = false,
				Mode = Listbox.Column.ColumnMode.PlatformSpecific
			});
			base.Columns.Add(new Listbox.Column("SourceWidth", "Source Width", "Width in pixels of source texture.", 40f, new Listbox.Column.ColumnDrawer(this.OnDrawOriginalWidth), new Listbox.Column.CompareDelegate2(this.OnCompareOriginalWidth), new Listbox.Column.ExportDelegate(this.OnExportOriginalWidth))
			{
				Visible = false
			});
			base.Columns.Add(new Listbox.Column("SourceHeight", "Source Height", "Height in pixels of source texture.", 40f, new Listbox.Column.ColumnDrawer(this.OnDrawOriginalHeight), new Listbox.Column.CompareDelegate2(this.OnCompareOriginalHeight), new Listbox.Column.ExportDelegate(this.OnExportOriginalHeight))
			{
				Visible = false
			});
			base.Columns.Add(new Listbox.Column("Wrap", "Wrap", "Texture Wrap Mode (U, V, W).", 60f, new Listbox.Column.ColumnDrawer(this.OnDrawWrapMode), new Listbox.Column.CompareDelegate2(this.OnCompareWrapMode), new Listbox.Column.ExportDelegate(this.OnExportWrapMode)));
			base.Columns.Add(new Listbox.Column("Filter", "Filter", "Texture Filter Mode.", 60f, new Listbox.Column.ColumnDrawer(this.OnDrawFilterMode), new Listbox.Column.CompareDelegate2(this.OnCompareFilterMode), new Listbox.Column.ExportDelegate(this.OnExportFilterMode)));
			base.Columns.Add(new Listbox.Column("Aniso", "Aniso", "Anisotropic Texture Filtering Level.", 40f, new Listbox.Column.ColumnDrawer(this.OnDrawAnisoLevel), new Listbox.Column.CompareDelegate2(this.OnCompareAnisoLevel), new Listbox.Column.ExportDelegate(this.OnExportAnisoLevel))
			{
				Visible = false
			});
			base.Columns.Add(new Listbox.Column("PoT", "Power of 2", "Indicates whether the texture size is power of two.", 35f, new Listbox.Column.ColumnDrawer(this.OnDrawIsPowerOfTwo), new Listbox.Column.CompareDelegate2(this.OnCompareIsPowerOfTwo), new Listbox.Column.ExportDelegate(this.OnExportIsPowerOfTwo))
			{
				Visible = false,
				HideIfDefault = true
			});
			base.Columns.Add(new Listbox.Column("Sqr", "Square", "Indicates whether the texture size is square (width and height are of equal size).", 35f, new Listbox.Column.ColumnDrawer(this.OnDrawIsSquare), new Listbox.Column.CompareDelegate2(this.OnCompareIsSquare), new Listbox.Column.ExportDelegate(this.OnExportIsSquare))
			{
				Visible = false,
				HideIfDefault = true
			});
			base.Columns.Add(new Listbox.Column("ResizeRatio", "Resize Ratio", "The ratio between the number of pixels in the original texture and the imported texture.", 80f, new Listbox.Column.ColumnDrawer(this.OnDrawPixelRatioPercentage), new Listbox.Column.CompareDelegate2(this.OnComparePixelRatioPercentage), new Listbox.Column.ExportDelegate(this.OnExportPixelRatioPercentage))
			{
				Visible = false,
				HideIfDefault = true
			});
			base.Columns.Add(new Listbox.Column("FileExtension", "Extension", "File extension of source texture.", 70f, new Listbox.Column.ColumnDrawer(this.OnDrawFileExtension), new Listbox.Column.CompareDelegate2(this.OnCompareFileExtension), new Listbox.Column.ExportDelegate(this.OnExportFileExtension))
			{
				Visible = false
			});
			base.Columns.Add(new Listbox.Column("SpriteImportMode", "Sprite Mode", "Indicates how the the sprite graphic will be extracted from the image.", 90f, new Listbox.Column.ColumnDrawer(this.OnDrawSpriteImportMode), new Listbox.Column.CompareDelegate2(this.OnCompareSpriteImportMode), new Listbox.Column.ExportDelegate(this.OnExportSpriteImportMode))
			{
				Visible = false
			});
			base.Columns.Add(new Listbox.Column("SpritePackingTag", "Sprite Packing Tag", "The name of an optional sprite atlas into which this texture should be packed.", 90f, new Listbox.Column.ColumnDrawer(this.OnDrawSpritePackingTag), new Listbox.Column.CompareDelegate2(this.OnCompareSpritePackingTag), new Listbox.Column.ExportDelegate(this.OnExportSpritePackingTag)));
			base.Columns.Add(new Listbox.Column("SpritePixelPerUnit", "Sprite Pixels per Unit", "The number of pixels of width/height in the sprite image that will correspond to one distance unit in world space.", 80f, new Listbox.Column.ColumnDrawer(this.OnDrawSpritePixelPerUnit), new Listbox.Column.CompareDelegate2(this.OnCompareSpritePixelPerUnit), new Listbox.Column.ExportDelegate(this.OnExportSpritePixelPerUnit))
			{
				Visible = false
			});
			AssetFileWatcher.Imported += this.AssetFileWatcher_Imported;
			AssetFileWatcher.Deleted += this.AssetFileWatcher_Deleted;
			AssetFileWatcher.Moved += this.AssetFileWatcher_Moved;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0002E6A4 File Offset: 0x0002C8A4
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			if (!this._cache.IsEmpty)
			{
				this._cache.Write();
			}
			AssetFileWatcher.Imported -= this.AssetFileWatcher_Imported;
			AssetFileWatcher.Deleted -= this.AssetFileWatcher_Deleted;
			AssetFileWatcher.Moved -= this.AssetFileWatcher_Moved;
			this._cache = null;
			this._items = null;
			this._allitems = null;
			this._disposed = true;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0002E728 File Offset: 0x0002C928
		public void RefreshModels()
		{
			BuildTargetGroup buildTargetGroup = this._platform;
			if (buildTargetGroup == null)
			{
				buildTargetGroup = BuildPipeline2.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
			}
			this._warningCount = 0;
			foreach (Listbox.Model model in this._allitems)
			{
				model.SetPlatform(buildTargetGroup);
				if (model.PlatformHasIssue || model.HasError)
				{
					this._warningCount++;
				}
			}
			base.DoChanged();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0002E7BC File Offset: 0x0002C9BC
		public void ReadCacheFile()
		{
			this._cache.Read();
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0002E7C9 File Offset: 0x0002C9C9
		public bool DeleteCacheFile()
		{
			return this._cache.Delete();
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0002E7D8 File Offset: 0x0002C9D8
		private void DoTextureChanged()
		{
			if (this.TextureChanged != null)
			{
				Action<Listbox> textureChanged = this.TextureChanged;
				textureChanged(this);
			}
			base.Editor.Repaint();
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0002E806 File Offset: 0x0002CA06
		public void BeginChangeFilter()
		{
			this._filterUpdateCount++;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0002E816 File Offset: 0x0002CA16
		public void EndChangeFilter()
		{
			this._filterUpdateCount--;
			this.UpdateFilter();
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0002E82C File Offset: 0x0002CA2C
		public void SetTextFilter(string text, Listbox.TextFilterMode mode)
		{
			SearchTextParser.Result parserresult = SearchTextParser.Parse(text);
			this.SetTextFilter(parserresult, mode);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0002E848 File Offset: 0x0002CA48
		private void SetTextFilter(SearchTextParser.Result parserresult, Listbox.TextFilterMode mode)
		{
			this._filterResult = parserresult;
			this._filterMode = mode;
			this.UpdateFilter();
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0002E85E File Offset: 0x0002CA5E
		public void AddTypeFilter(Listbox.TypeFilter filter)
		{
			if (!this.HasTypeFilter(filter))
			{
				this._typeFilter.Add(filter);
				this.UpdateFilter();
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0002E87B File Offset: 0x0002CA7B
		public void RemoveTypeFilter(Listbox.TypeFilter filter)
		{
			if (this._typeFilter.Remove(filter))
			{
				this.UpdateFilter();
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0002E891 File Offset: 0x0002CA91
		public void ClearTypeFilter()
		{
			this._typeFilter.Clear();
			this.UpdateFilter();
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0002E8A4 File Offset: 0x0002CAA4
		public bool HasTypeFilter(Listbox.TypeFilter filter)
		{
			return this._typeFilter.Contains(filter);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0002E8B7 File Offset: 0x0002CAB7
		public int GetTypeFilterCount()
		{
			return this._typeFilter.Count;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0002E8C4 File Offset: 0x0002CAC4
		private void UpdateFilter()
		{
			if (this._filterUpdateCount > 0)
			{
				return;
			}
			this._typeFilterLookup = new Dictionary<int, Listbox.TypeFilter>();
			foreach (Listbox.TypeFilter typeFilter in this._typeFilter)
			{
				this._typeFilterLookup[typeFilter.GetHashCode()] = typeFilter;
			}
			this._items = new List<Listbox.Model>(this._allitems.Count);
			for (int i = 0; i < this._allitems.Count; i++)
			{
				Listbox.Model model = this._allitems[i];
				if (this.IsIncludedInFilter(model))
				{
					this._items.Add(model);
				}
			}
			base.DoChanged();
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0002E98C File Offset: 0x0002CB8C
		private bool IsIncludedInFilter(Listbox.Model model)
		{
			if (this._typeFilterLookup.Count > 0 && !this._typeFilterLookup.ContainsKey(Listbox.TypeFilter.ComputeHashCode(model.TextureType, model.TextureShape)))
			{
				return false;
			}
			if (this._filterResult.NamesExpr.Count > 0)
			{
				switch (this._filterMode)
				{
				case Listbox.TextFilterMode.All:
					if (this._filterResult.IsNameMatch(model.AssetName))
					{
						return true;
					}
					if (this._filterResult.IsNameMatch(model.AssetPath))
					{
						return true;
					}
					if (this._filterResult.IsNameMatch(model.SpritePackingTag))
					{
						return true;
					}
					break;
				case Listbox.TextFilterMode.Name:
					if (this._filterResult.IsNameMatch(model.AssetName))
					{
						return true;
					}
					break;
				case Listbox.TextFilterMode.Path:
					if (this._filterResult.IsNameMatch(model.AssetPath))
					{
						return true;
					}
					break;
				case Listbox.TextFilterMode.SpritePackingTag:
					if (this._filterResult.IsNameMatch(model.SpritePackingTag))
					{
						return true;
					}
					break;
				}
				return false;
			}
			return true;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0002EA80 File Offset: 0x0002CC80
		public void SetPlatform(BuildTargetGroup platform)
		{
			this._platform = platform;
			List<string> allPaths = this.GetAllPaths();
			this.SetItems(allPaths);
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0002EAA2 File Offset: 0x0002CCA2
		public BuildTargetGroup Platform
		{
			get
			{
				return this._platform;
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0002EAAA File Offset: 0x0002CCAA
		public void SetSceneOccurrenceLookup(Dictionary<string, int> lookup)
		{
			this._RendererCountLookup = lookup;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0002EAB4 File Offset: 0x0002CCB4
		public void SetItems(List<string> paths)
		{
			long num = 536870912L;
			long val = (long)(SystemInfo.systemMemorySize * 1024) * 1024L - num;
			long availableMemory = Math.Max(0L, Math.Min(num, val));
			List<string> selectedPaths = this.GetSelectedPaths();
			this._allitems = new List<Listbox.Model>();
			List<string> list = new List<string>(paths.Count);
			int i = 0;
			int count = paths.Count;
			while (i < count)
			{
				string text = paths[i];
				if (this.IsSupportedFile(text))
				{
					list.Add(text);
				}
				i++;
			}
			using (EditorGUI2.ModalProgressBar modalProgressBar = new EditorGUI2.ModalProgressBar(string.Format("{0}: Loading", Globals.ProductTitle), true))
			{
				long num2 = 0L;
				for (int j = 0; j < list.Count; j++)
				{
					string text2 = list[j];
					if (modalProgressBar.TotalElapsedTime > 1f && modalProgressBar.ElapsedTime > 0.1f)
					{
						float progress = (float)j / (float)list.Count;
						string text3 = string.Format("[{1} remaining] {0}", FileUtil2.GetFileName(text2), list.Count - j - 1);
						if (modalProgressBar.Update(text3, progress))
						{
							break;
						}
					}
					Listbox.Model model = null;
					num2 += this.InitModel(ref model, text2);
					model.RendererCount = 0;
					if (this._RendererCountLookup != null && !model.HasError)
					{
						this._RendererCountLookup.TryGetValue(model.AssetGuid, out model.RendererCount);
					}
					this.CheckUnloadUnused(ref num2, availableMemory);
					this._allitems.Add(model);
				}
				if (num2 > 0L)
				{
					this._cache.Write();
				}
			}
			this._items = new List<Listbox.Model>(this._allitems);
			this.RefreshModels();
			base.Sort();
			List<Listbox.Model> list2 = new List<Listbox.Model>(selectedPaths.Count);
			int k = 0;
			int count2 = selectedPaths.Count;
			while (k < count2)
			{
				Listbox.Model model2 = this.FindModel(selectedPaths[k]);
				if (model2 != null)
				{
					list2.Add(model2);
				}
				k++;
			}
			base.SelectedItems = list2.ToArray();
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0002ECDC File Offset: 0x0002CEDC
		private bool IsSupportedFile(string path)
		{
			Type assetType = AssetDatabase2.GetAssetType(path);
			if (assetType == null)
			{
				return false;
			}
			if (!typeof(Texture).IsAssignableFrom(assetType))
			{
				return false;
			}
			if (path.IndexOf("/StreamingAssets/", StringComparison.OrdinalIgnoreCase) != -1)
			{
				return false;
			}
			if (!Globals.ShowEditorAssets)
			{
				if (path.IndexOf("/Editor Resources/", StringComparison.OrdinalIgnoreCase) != -1)
				{
					return false;
				}
				if (path.IndexOf("/Editor/", StringComparison.OrdinalIgnoreCase) != -1)
				{
					return false;
				}
				if (path.IndexOf("/Editor Default Resources/", StringComparison.OrdinalIgnoreCase) != -1)
				{
					return false;
				}
				if (path.IndexOf("/Gizmos/", StringComparison.OrdinalIgnoreCase) != -1)
				{
					return false;
				}
			}
			return Globals.ShowPackageAssets || path.IndexOf("Packages/", StringComparison.OrdinalIgnoreCase) == -1;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0002ED7E File Offset: 0x0002CF7E
		private void CheckUnloadUnused(ref long loadedsize, long availableMemory)
		{
			if (loadedsize <= 0L)
			{
				return;
			}
			if (loadedsize >= availableMemory)
			{
				this._cache.Write();
				EditorUtility2.UnloadUnusedAssetsImmediate();
				loadedsize = 0L;
				Thread.Sleep(1);
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0002EDA8 File Offset: 0x0002CFA8
		private long InitModel(ref Listbox.Model model, string path)
		{
			if (model == null)
			{
				string assetguid = AssetDatabase.AssetPathToGUID(path);
				if (this._cache.TryGetEntry(assetguid, out model, true) && model.GetImporterSettingsAvailable(this._platform))
				{
					return 0L;
				}
				model = new Listbox.Model();
			}
			long result = model.Init(path);
			this._cache.UpdateEntry(model);
			return result;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0002EE00 File Offset: 0x0002D000
		private Listbox.Model FindModel(string assetPath)
		{
			int i = 0;
			int count = this._allitems.Count;
			while (i < count)
			{
				Listbox.Model model = this._allitems[i];
				if (string.Equals(assetPath, model.AssetPath, StringComparison.OrdinalIgnoreCase))
				{
					return model;
				}
				i++;
			}
			return null;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0002EE44 File Offset: 0x0002D044
		private List<Listbox.Model> GetSelectedModels()
		{
			object[] selectedItems = base.SelectedItems;
			List<Listbox.Model> list = new List<Listbox.Model>(selectedItems.Length);
			foreach (object obj in selectedItems)
			{
				Listbox.Model item = obj as Listbox.Model;
				list.Add(item);
			}
			return list;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0002EE8C File Offset: 0x0002D08C
		private List<string> GetSelectedPaths()
		{
			object[] selectedItems = base.SelectedItems;
			List<string> list = new List<string>(selectedItems.Length);
			foreach (object obj in selectedItems)
			{
				Listbox.Model model = obj as Listbox.Model;
				if (!string.IsNullOrEmpty(model.AssetPath))
				{
					list.Add(model.AssetPath);
				}
			}
			return list;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0002EEE8 File Offset: 0x0002D0E8
		private List<string> GetAllPaths()
		{
			List<string> list = new List<string>(this._allitems.Count);
			int i = 0;
			int count = this._allitems.Count;
			while (i < count)
			{
				list.Add(this._allitems[i].AssetPath);
				i++;
			}
			return list;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0002EF38 File Offset: 0x0002D138
		private List<Object> GetSelectedAssets(bool showprogress, int maxcount)
		{
			List<Object> list = new List<Object>(64);
			using (EditorGUI2.ModalProgressBar modalProgressBar = new EditorGUI2.ModalProgressBar(string.Format("{0}: Loading", Globals.ProductTitle), true))
			{
				object[] selectedItems = base.SelectedItems;
				for (int i = 0; i < Mathf.Min(maxcount, selectedItems.Length); i++)
				{
					Listbox.Model model = selectedItems[i] as Listbox.Model;
					if (!string.IsNullOrEmpty(model.AssetGuid))
					{
						string text = AssetDatabase.GUIDToAssetPath(model.AssetGuid);
						if (!string.IsNullOrEmpty(text))
						{
							if (showprogress && modalProgressBar.TotalElapsedTime > 1f && modalProgressBar.ElapsedTime > 0.1f)
							{
								float progress = (float)i / (float)selectedItems.Length;
								string text2 = string.Format("[{1} remaining] {0}", FileUtil2.GetFileNameWithoutExtension(text), selectedItems.Length - i - 1);
								if (modalProgressBar.Update(text2, progress))
								{
									break;
								}
							}
							Object @object = AssetDatabase.LoadMainAssetAtPath(text);
							if (null != @object)
							{
								list.Add(@object);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0002F048 File Offset: 0x0002D248
		protected override void OnBeginDrag(int index, out Object[] objects, out string[] paths)
		{
			paths = this.GetSelectedPaths().ToArray();
			objects = this.GetSelectedAssets(base.SelectedItemsCount > 20, int.MaxValue).ToArray();
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0002F073 File Offset: 0x0002D273
		protected override object[] OnBeforeSortItems()
		{
			return this._allitems.ToArray();
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0002F080 File Offset: 0x0002D280
		protected override void OnAfterSortItems(object[] models)
		{
			base.OnAfterSortItems(models);
			this._allitems = new List<Listbox.Model>((Listbox.Model[])models);
			this.SetTextFilter(this._filterResult, this._filterMode);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0002F0AC File Offset: 0x0002D2AC
		protected override void OnSelectionChange()
		{
			base.OnSelectionChange();
			base.DoChanged();
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0002F0BC File Offset: 0x0002D2BC
		protected override void OnItemContextMenu(GUIListViewContextMenuArgs args)
		{
			base.OnItemContextMenu(args);
			if (base.SelectedItemsCount < 1)
			{
				return;
			}
			GUIUtility.hotControl = 0;
			Listbox.Model model = args.Model as Listbox.Model;
			GenericMenu genericMenu = new GenericMenu();
			genericMenu.AddItem(new GUIContent((Application.platform == null) ? "Reveal in Finder" : "Show in Explorer"), false, (base.SelectedItemsCount <= 10) ? new GenericMenu.MenuFunction2(this.OnContextMenuShowInExplorer) : null, model);
			genericMenu.AddItem(new GUIContent("Open %enter"), false, new GenericMenu.MenuFunction2(this.OnContextMenuOpenWithDefaultApp), model);
			genericMenu.AddItem(new GUIContent("Delete... _delete"), false, new GenericMenu.MenuFunction(this.OnContextMenuDelete));
			genericMenu.AddItem(new GUIContent(string.Empty), false, null);
			genericMenu.AddItem(new GUIContent("Select in Project _enter"), false, new GenericMenu.MenuFunction(this.OnContextMenuSelect));
			genericMenu.AddItem(new GUIContent("Find References In Scene"), false, (base.SelectedItemsCount == 1) ? new GenericMenu.MenuFunction(this.OnContextMenuFindReferencesInScene) : null);
			genericMenu.AddItem(new GUIContent("Find Materials in Project"), false, new GenericMenu.MenuFunction(this.OnContextMenuFindMaterialsInProject));
			genericMenu.AddItem(new GUIContent("Find Prefabs in Project"), false, new GenericMenu.MenuFunction(this.OnContextMenuFindPrefabsInProject));
			genericMenu.AddItem(new GUIContent("Find Prefabs and Scenes in Project"), false, new GenericMenu.MenuFunction(this.OnContextMenuFindPrefabsAndScenesInProject));
			genericMenu.AddItem(new GUIContent(string.Empty), false, null);
			genericMenu.AddItem(new GUIContent("Reimport"), false, new GenericMenu.MenuFunction(this.OnContextMenuReimport));
			genericMenu.AddItem(new GUIContent(string.Empty), false, null);
			genericMenu.AddItem(new GUIContent("Copy Full Path"), false, new GenericMenu.MenuFunction(this.OnContextMenuCopyFullPath));
			genericMenu.DropDown(new Rect(args.MenuLocation.x, args.MenuLocation.y, 0f, 0f));
			Event.current.Use();
			base.Editor.Repaint();
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0002F2B8 File Offset: 0x0002D4B8
		private void OnContextMenuShowInExplorer(object userData)
		{
			List<string> selectedPaths = this.GetSelectedPaths();
			EditorApplication2.ShowInExplorer(selectedPaths.ToArray());
			base.Editor.Repaint();
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0002F2E4 File Offset: 0x0002D4E4
		private void OnContextMenuOpenWithDefaultApp(object userData)
		{
			List<string> selectedPaths = this.GetSelectedPaths();
			EditorApplication2.OpenAssets(selectedPaths.ToArray());
			base.Editor.Repaint();
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0002F310 File Offset: 0x0002D510
		private void OnContextMenuDelete()
		{
			List<string> selectedPaths = this.GetSelectedPaths();
			AssetDatabase2.Delete(selectedPaths);
			base.Editor.Focus();
			base.Editor.Repaint();
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0002F340 File Offset: 0x0002D540
		private void OnContextMenuSelect()
		{
			List<Object> selectedAssets = this.GetSelectedAssets(base.SelectedItemsCount > 20, int.MaxValue);
			Selection.objects = selectedAssets.ToArray();
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0002F370 File Offset: 0x0002D570
		private void OnContextMenuFindMaterialsInProject()
		{
			FindAssetUsageWindow.FindUsageInProject(Globals.ProductId, Globals.ProductTitle, this.GetSelectedPaths(), new Type[]
			{
				typeof(Material)
			});
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0002F3A8 File Offset: 0x0002D5A8
		private void OnContextMenuFindPrefabsInProject()
		{
			FindAssetUsageWindow.FindUsageInProject(Globals.ProductId, Globals.ProductTitle, this.GetSelectedPaths(), new Type[]
			{
				typeof(GameObject)
			});
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0002F3E0 File Offset: 0x0002D5E0
		private void OnContextMenuFindPrefabsAndScenesInProject()
		{
			FindAssetUsageWindow.FindUsageInProject(Globals.ProductId, Globals.ProductTitle, this.GetSelectedPaths(), new Type[]
			{
				typeof(GameObject),
				typeof(AssetDatabase2.UnityScene)
			});
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0002F424 File Offset: 0x0002D624
		private void OnContextMenuFindReferencesInScene()
		{
			List<Object> selectedAssets = this.GetSelectedAssets(false, 1);
			if (selectedAssets.Count > 0)
			{
				EditorApplication2.FindReferencesInScene(selectedAssets[0]);
				EditorUtility2.UnloadUnusedAssetsImmediate();
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0002F454 File Offset: 0x0002D654
		private void OnContextMenuReimport()
		{
			List<string> selectedPaths = this.GetSelectedPaths();
			AssetDatabase2.Reimport(selectedPaths, 10);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0002F474 File Offset: 0x0002D674
		private void OnContextMenuCopyFullPath()
		{
			List<string> selectedPaths = this.GetSelectedPaths();
			ClipboardUtil.CopyPaths(selectedPaths);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0002F490 File Offset: 0x0002D690
		protected override void OnItemKeyDown(ref GUIListViewItemKeyDownArgs args)
		{
			KeyCode keyCode = Event.current.keyCode;
			if ((int)keyCode != 13)
			{
				if ((int)keyCode != 127)
				{
					return;
				}
				EditorApplication.delayCall = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.delayCall, new EditorApplication.CallbackFunction(this.OnContextMenuDelete));
				args.Handled = true;
				return;
			}
			else
			{
				if (Event.current.control)
				{
					this.OnContextMenuOpenWithDefaultApp(args.Model);
					args.Handled = true;
					return;
				}
				EditorApplication.delayCall = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.delayCall, new EditorApplication.CallbackFunction(this.OnContextMenuSelect));
				args.Handled = true;
				return;
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0002F524 File Offset: 0x0002D724
		protected override void OnItemDoubleClick(GUIListViewItemDoubleClickArgs args)
		{
			base.OnItemDoubleClick(args);
			List<Object> selectedAssets = this.GetSelectedAssets(false, 1);
			if (selectedAssets.Count > 0)
			{
				this.ChangeUnitySelectionTime = DateTime.Now.TimeOfDay.TotalSeconds;
				Selection.activeObject = selectedAssets[0];
				EditorGUIUtility.PingObject(selectedAssets[0]);
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0002F580 File Offset: 0x0002D780
		protected override string OnGetItemKeyword(GUIListViewGetItemKeywordArgs args)
		{
			Listbox.Model model = args.Model as Listbox.Model;
			if (model != null)
			{
				return model.AssetName;
			}
			return null;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0002F5A5 File Offset: 0x0002D7A5
		protected override object OnGetItem(int index)
		{
			if (index < 0 || index >= this._items.Count)
			{
				return null;
			}
			return this._items[index];
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0002F5C7 File Offset: 0x0002D7C7
		protected override int OnGetItemCount()
		{
			return this._items.Count;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0002F5D4 File Offset: 0x0002D7D4
		protected override string OnGetItemText(GUIListViewGetItemTextArgs args)
		{
			Listbox.Column column = args.Column as Listbox.Column;
			return column.Exporter(args.Model as Listbox.Model);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0002F608 File Offset: 0x0002D808
		protected override void OnDrawItem(GUIListViewDrawItemArgs args)
		{
			Listbox.Model model = args.Model as Listbox.Model;
			if (model == null)
			{
				return;
			}
			if (model.HasError)
			{
				if (args.Column == base.FirstVisibleColumn)
				{
					GUIListView.DrawItemImageHelper(ref args.ItemRect, GUIContent2.Temp(EditorFramework.Images.Error16x16, "The texture could not be loaded."), new Vector2(16f, 16f));
					args.ItemRect.y = args.ItemRect.y + 4f;
					args.ItemRect.height = args.ItemRect.height - 4f;
					EditorGUI2.Label(args.ItemRect, "ERROR: " + model.AssetPath, args.Selected);
				}
				return;
			}
			if (args.Column.IsPrimaryColumn)
			{
				GUIListView.DrawItemImageHelper(ref args.ItemRect, model.AllocAssetIcon(), new Vector2(16f, 16f));
				if (model.PlatformHasIssue)
				{
					GUIListView.DrawItemImageHelper(ref args.ItemRect, new GUIContent("", EditorFramework.Images.Warning16x16, model.PlatformIssueString), new Vector2(16f, 16f));
				}
			}
			args.ItemRect.width = args.ItemRect.width - 5f;
			args.ItemRect.y = args.ItemRect.y + 4f;
			args.ItemRect.height = args.ItemRect.height - 4f;
			Listbox.Column column = args.Column as Listbox.Column;
			if (column != null && column.Drawer != null)
			{
				if (column.HideIfDefault && this._platform == null)
				{
					return;
				}
				column.Drawer(model, args);
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0002F7A8 File Offset: 0x0002D9A8
		protected override void OnItemHide(object item)
		{
			base.OnItemHide(item);
			Listbox.Model model = item as Listbox.Model;
			if (model == null)
			{
				return;
			}
			model.ReleaseAssetIcon();
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0002F7CD File Offset: 0x0002D9CD
		private void OnDrawAssetPath(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.PathLabel(args.ItemRect, model.AssetPath, args.Selected);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0002F7E8 File Offset: 0x0002D9E8
		private int OnCompareAssetPath(Listbox.Model x, Listbox.Model y)
		{
			return string.Compare(x.AssetPath, y.AssetPath, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0002F7FC File Offset: 0x0002D9FC
		private string OnExportAssetPath(Listbox.Model model)
		{
			return model.AssetPath;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0002F804 File Offset: 0x0002DA04
		private void OnDrawAssetName(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, model.AssetName, args.Selected);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0002F81F File Offset: 0x0002DA1F
		private int OnCompareAssetName(Listbox.Model x, Listbox.Model y)
		{
			return string.Compare(x.AssetName, y.AssetName, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0002F833 File Offset: 0x0002DA33
		private string OnExportAssetName(Listbox.Model model)
		{
			return model.AssetName;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0002F83B File Offset: 0x0002DA3B
		private void OnDrawRendererCount(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			if (this._RendererCountLookup == null)
			{
				return;
			}
			EditorGUI2.Label(args.ItemRect, model.RendererCount.ToString(), args.Selected);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0002F864 File Offset: 0x0002DA64
		private int OnCompareRendererCount(Listbox.Model x, Listbox.Model y)
		{
			return x.RendererCount.CompareTo(y.RendererCount);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0002F877 File Offset: 0x0002DA77
		private string OnExportRendererCount(Listbox.Model model)
		{
			return model.RendererCount.ToString();
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0002F884 File Offset: 0x0002DA84
		private void OnDrawImportType(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, model.TextureTypeString, args.Selected);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0002F89F File Offset: 0x0002DA9F
		private int OnCompareImportType(Listbox.Model x, Listbox.Model y)
		{
			return string.Compare(x.TextureTypeString, y.TextureTypeString, StringComparison.CurrentCultureIgnoreCase);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0002F8B3 File Offset: 0x0002DAB3
		private string OnExportImportType(Listbox.Model model)
		{
			return model.TextureTypeString;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0002F8BB File Offset: 0x0002DABB
		private void OnDrawImportShape(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, model.TextureShapeString, args.Selected);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0002F8D6 File Offset: 0x0002DAD6
		private int OnCompareImportShape(Listbox.Model x, Listbox.Model y)
		{
			return string.Compare(x.TextureShapeString, y.TextureShapeString, StringComparison.CurrentCultureIgnoreCase);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0002F8EA File Offset: 0x0002DAEA
		private string OnExportImportShape(Listbox.Model model)
		{
			return model.TextureShapeString;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0002F8F4 File Offset: 0x0002DAF4
		private void OnDrawCompression(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			TextureImporterCompression importerTextureCompression = model.GetImporterTextureCompression(this._platform);
			string compressionString = TextureUtil2.GetCompressionString(importerTextureCompression);
			EditorGUI2.Label(args.ItemRect, compressionString, args.Selected);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0002F92C File Offset: 0x0002DB2C
		private int OnCompareCompression(Listbox.Model x, Listbox.Model y)
		{
			string compressionString = TextureUtil2.GetCompressionString(x.GetImporterTextureCompression(this._platform));
			string compressionString2 = TextureUtil2.GetCompressionString(y.GetImporterTextureCompression(this._platform));
			return compressionString.CompareTo(compressionString2);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0002F964 File Offset: 0x0002DB64
		private string OnExportCompression(Listbox.Model model)
		{
			return TextureUtil2.GetCompressionString(model.GetImporterTextureCompression(this._platform));
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0002F984 File Offset: 0x0002DB84
		private void OnDrawMaxSize(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			int importerMaxSize = model.GetImporterMaxSize(this._platform);
			EditorGUI2.Label(args.ItemRect, importerMaxSize.ToString(), args.Selected);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0002F9B8 File Offset: 0x0002DBB8
		private int OnCompareMaxSize(Listbox.Model x, Listbox.Model y)
		{
			int importerMaxSize = x.GetImporterMaxSize(this._platform);
			int importerMaxSize2 = y.GetImporterMaxSize(this._platform);
			return importerMaxSize.CompareTo(importerMaxSize2);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0002F9E8 File Offset: 0x0002DBE8
		private string OnExportMaxSize(Listbox.Model model)
		{
			return model.GetImporterMaxSize(this._platform).ToString();
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0002FA0C File Offset: 0x0002DC0C
		private void OnDrawFormat(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			if (this._platform == null)
			{
				EditorGUI2.Label(args.ItemRect, "Auto", args.Selected);
				return;
			}
			string textureImporterFormatLongString = TextureUtil2.GetTextureImporterFormatLongString(model.PlatformFormat);
			EditorGUI2.Label(args.ItemRect, textureImporterFormatLongString, args.Selected);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0002FA5C File Offset: 0x0002DC5C
		private int OnCompareFormat(Listbox.Model x, Listbox.Model y)
		{
			if (this._platform == null)
			{
				return 0;
			}
			string textureImporterFormatLongString = TextureUtil2.GetTextureImporterFormatLongString(x.PlatformFormat);
			string textureImporterFormatLongString2 = TextureUtil2.GetTextureImporterFormatLongString(y.PlatformFormat);
			return string.Compare(textureImporterFormatLongString, textureImporterFormatLongString2, StringComparison.CurrentCultureIgnoreCase);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0002FA94 File Offset: 0x0002DC94
		private string OnExportFormat(Listbox.Model model)
		{
			if (this._platform == null)
			{
				return "Auto";
			}
			return TextureUtil2.GetTextureImporterFormatLongString(model.PlatformFormat);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0002FABC File Offset: 0x0002DCBC
		private void OnDrawFormatShort(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			if (this._platform == null)
			{
				EditorGUI2.Label(args.ItemRect, "Auto", args.Selected);
				return;
			}
			string textureImporterFormatString = TextureUtil2.GetTextureImporterFormatString(model.PlatformFormat);
			if (textureImporterFormatString == null)
			{
				textureImporterFormatString = TextureUtil2.GetTextureImporterFormatString(model.PlatformFormat);
			}
			EditorGUI2.Label(args.ItemRect, textureImporterFormatString.ToString(), args.Selected);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0002FB20 File Offset: 0x0002DD20
		private int OnCompareFormatShort(Listbox.Model x, Listbox.Model y)
		{
			if (this._platform == null)
			{
				return 0;
			}
			string textureImporterFormatString = TextureUtil2.GetTextureImporterFormatString(x.PlatformFormat);
			string textureImporterFormatString2 = TextureUtil2.GetTextureImporterFormatString(y.PlatformFormat);
			return string.Compare(textureImporterFormatString.ToString(), textureImporterFormatString2.ToString(), StringComparison.CurrentCultureIgnoreCase);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0002FB64 File Offset: 0x0002DD64
		private string OnExportFormatShort(Listbox.Model model)
		{
			if (this._platform == null)
			{
				return "Auto";
			}
			string textureImporterFormatString = TextureUtil2.GetTextureImporterFormatString(model.PlatformFormat);
			return textureImporterFormatString.ToString();
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0002FB94 File Offset: 0x0002DD94
		private void OnDrawResizeAlgorithm(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			string resizeAlgorithmString = TextureUtil2.GetResizeAlgorithmString(model.GetImporterResizeAlgorithm(this._platform));
			EditorGUI2.Label(args.ItemRect, resizeAlgorithmString, args.Selected);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0002FBC8 File Offset: 0x0002DDC8
		private int OnCompareResizeAlgorithm(Listbox.Model x, Listbox.Model y)
		{
			string resizeAlgorithmString = TextureUtil2.GetResizeAlgorithmString(x.GetImporterResizeAlgorithm(this._platform));
			string resizeAlgorithmString2 = TextureUtil2.GetResizeAlgorithmString(y.GetImporterResizeAlgorithm(this._platform));
			return resizeAlgorithmString.CompareTo(resizeAlgorithmString2);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0002FC00 File Offset: 0x0002DE00
		private string OnExportResizeAlgorithm(Listbox.Model model)
		{
			return TextureUtil2.GetResizeAlgorithmString(model.GetImporterResizeAlgorithm(this._platform));
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0002FC20 File Offset: 0x0002DE20
		private void OnDrawNPOTScale(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			string npotscaleString = TextureUtil2.GetNPOTScaleString(model.NPOTScale);
			EditorGUI2.Label(args.ItemRect, npotscaleString, args.Selected);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0002FC50 File Offset: 0x0002DE50
		private int OnCompareNPOTScale(Listbox.Model x, Listbox.Model y)
		{
			string npotscaleString = TextureUtil2.GetNPOTScaleString(x.NPOTScale);
			string npotscaleString2 = TextureUtil2.GetNPOTScaleString(y.NPOTScale);
			return npotscaleString.CompareTo(npotscaleString2);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0002FC7C File Offset: 0x0002DE7C
		private string OnExportNPOTScale(Listbox.Model model)
		{
			return TextureUtil2.GetNPOTScaleString(model.NPOTScale);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0002FC98 File Offset: 0x0002DE98
		private void OnDrawCompressionQuality(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			int importerCompressionQuality = model.GetImporterCompressionQuality(this._platform);
			string compressionQualityString = TextureUtil2.GetCompressionQualityString(importerCompressionQuality);
			EditorGUI2.Label(args.ItemRect, compressionQualityString, args.Selected);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0002FCD0 File Offset: 0x0002DED0
		private int OnCompareCompressionQuality(Listbox.Model x, Listbox.Model y)
		{
			int importerCompressionQuality = x.GetImporterCompressionQuality(this._platform);
			int importerCompressionQuality2 = y.GetImporterCompressionQuality(this._platform);
			return importerCompressionQuality.CompareTo(importerCompressionQuality2);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0002FD00 File Offset: 0x0002DF00
		private string OnExportCompressionQuality(Listbox.Model model)
		{
			int importerCompressionQuality = model.GetImporterCompressionQuality(this._platform);
			return TextureUtil2.GetCompressionQualityString(importerCompressionQuality);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0002FD24 File Offset: 0x0002DF24
		private void OnDrawUseCrunchCompression(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			bool importerUseCrunchCompression = model.GetImporterUseCrunchCompression(this._platform);
			this.DrawToggle(args, importerUseCrunchCompression);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0002FD48 File Offset: 0x0002DF48
		private int OnCompareUseCrunchCompression(Listbox.Model x, Listbox.Model y)
		{
			bool importerUseCrunchCompression = x.GetImporterUseCrunchCompression(this._platform);
			bool importerUseCrunchCompression2 = y.GetImporterUseCrunchCompression(this._platform);
			return importerUseCrunchCompression.CompareTo(importerUseCrunchCompression2);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0002FD78 File Offset: 0x0002DF78
		private string OnExportUseCrunchCompression(Listbox.Model model)
		{
			return model.GetImporterUseCrunchCompression(this._platform).ToString();
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0002FD9C File Offset: 0x0002DF9C
		private void OnDrawFilterMode(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			string filterModeString = TextureUtil2.GetFilterModeString(model.FilterMode);
			EditorGUI2.Label(args.ItemRect, filterModeString, args.Selected);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0002FDCC File Offset: 0x0002DFCC
		private int OnCompareFilterMode(Listbox.Model x, Listbox.Model y)
		{
			string filterModeString = TextureUtil2.GetFilterModeString(x.FilterMode);
			string filterModeString2 = TextureUtil2.GetFilterModeString(y.FilterMode);
			return string.Compare(filterModeString, filterModeString2, StringComparison.CurrentCultureIgnoreCase);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0002FDFC File Offset: 0x0002DFFC
		private string OnExportFilterMode(Listbox.Model model)
		{
			return TextureUtil2.GetFilterModeString(model.FilterMode);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0002FE18 File Offset: 0x0002E018
		private void OnDrawWrapMode(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			string wrapModeString = TextureUtil2.GetWrapModeString(model.TextureShape, model.WrapModeU, model.WrapModeV, model.WrapModeW);
			EditorGUI2.Label(args.ItemRect, wrapModeString, args.Selected);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0002FE58 File Offset: 0x0002E058
		private int OnCompareWrapMode(Listbox.Model x, Listbox.Model y)
		{
			string wrapModeString = TextureUtil2.GetWrapModeString(x.TextureShape, x.WrapModeU, x.WrapModeV, x.WrapModeW);
			string wrapModeString2 = TextureUtil2.GetWrapModeString(y.TextureShape, y.WrapModeU, y.WrapModeV, y.WrapModeW);
			return string.Compare(wrapModeString, wrapModeString2, StringComparison.CurrentCultureIgnoreCase);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0002FEAC File Offset: 0x0002E0AC
		private string OnExportWrapMode(Listbox.Model model)
		{
			return TextureUtil2.GetWrapModeString(model.TextureShape, model.WrapModeU, model.WrapModeV, model.WrapModeW);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0002FED8 File Offset: 0x0002E0D8
		private void OnDrawMips(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, model.PlatformMipmapCount.ToString(), args.Selected);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0002FF08 File Offset: 0x0002E108
		private int OnCompareMips(Listbox.Model x, Listbox.Model y)
		{
			return x.PlatformMipmapCount.CompareTo(y.PlatformMipmapCount);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0002FF2C File Offset: 0x0002E12C
		private string OnExportMips(Listbox.Model model)
		{
			return model.PlatformMipmapCount.ToString();
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0002FF47 File Offset: 0x0002E147
		private void OnDrawMipsEnabled(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			this.DrawToggle(args, model.MipmapEnabled);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0002FF56 File Offset: 0x0002E156
		private int OnCompareMipsEnabled(Listbox.Model x, Listbox.Model y)
		{
			return x.MipmapEnabled.CompareTo(y.MipmapEnabled);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0002FF69 File Offset: 0x0002E169
		private string OnExportMipsEnabled(Listbox.Model model)
		{
			return model.MipmapEnabled.ToString();
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0002FF76 File Offset: 0x0002E176
		private void OnDrawStreamingMips(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			this.DrawToggle(args, model.StreamingMipmaps);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0002FF85 File Offset: 0x0002E185
		private int OnCompareStreamingMips(Listbox.Model x, Listbox.Model y)
		{
			return x.StreamingMipmaps.CompareTo(y.StreamingMipmaps);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0002FF98 File Offset: 0x0002E198
		private string OnExportStreamingMips(Listbox.Model model)
		{
			return model.StreamingMipmaps.ToString();
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0002FFA5 File Offset: 0x0002E1A5
		private void OnDrawStreamingMipsPrio(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, model.StreamingMipmapsPriority.ToString(), args.Selected);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0002FFC5 File Offset: 0x0002E1C5
		private int OnCompareStreamingMipsPrio(Listbox.Model x, Listbox.Model y)
		{
			return x.StreamingMipmapsPriority.CompareTo(y.StreamingMipmapsPriority);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0002FFD8 File Offset: 0x0002E1D8
		private string OnExportStreamingMipsPrio(Listbox.Model model)
		{
			return model.StreamingMipmapsPriority.ToString();
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0002FFE5 File Offset: 0x0002E1E5
		private void OnDrawsRGBTexture(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			this.DrawToggle(args, model.sRGBTexture);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0002FFF4 File Offset: 0x0002E1F4
		private int OnComparesRGBTexture(Listbox.Model x, Listbox.Model y)
		{
			return x.sRGBTexture.CompareTo(y.sRGBTexture);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00030007 File Offset: 0x0002E207
		private string OnExportsRGBTexture(Listbox.Model model)
		{
			return model.sRGBTexture.ToString();
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00030014 File Offset: 0x0002E214
		private void OnDrawFileExtension(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, model.FileExtension, args.Selected);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0003002F File Offset: 0x0002E22F
		private int OnCompareFileExtension(Listbox.Model x, Listbox.Model y)
		{
			return string.Compare(x.FileExtension, y.FileExtension, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00030043 File Offset: 0x0002E243
		private string OnExportFileExtension(Listbox.Model model)
		{
			return model.FileExtension;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0003004B File Offset: 0x0002E24B
		private void OnDrawStorageSize(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, model.StorageSizeString, args.Selected);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00030066 File Offset: 0x0002E266
		private int OnCompareStorageSize(Listbox.Model x, Listbox.Model y)
		{
			return x.PlatformStorageSize.CompareTo(y.PlatformStorageSize);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00030079 File Offset: 0x0002E279
		private string OnExportStorageSize(Listbox.Model model)
		{
			return model.PlatformStorageSize.ToString();
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00030086 File Offset: 0x0002E286
		private void OnDrawRuntimeSize(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, model.RuntimeSizeString, args.Selected);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x000300A1 File Offset: 0x0002E2A1
		private int OnCompareRuntimeSize(Listbox.Model x, Listbox.Model y)
		{
			return x.PlatformRuntimeSize.CompareTo(y.PlatformRuntimeSize);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x000300B4 File Offset: 0x0002E2B4
		private string OnExportRuntimeSize(Listbox.Model model)
		{
			return model.PlatformRuntimeSize.ToString();
		}

		// Token: 0x0600030A RID: 778 RVA: 0x000300C1 File Offset: 0x0002E2C1
		private void OnDrawCpuSize(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, model.CpuSizeString, args.Selected);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x000300DC File Offset: 0x0002E2DC
		private int OnCompareCpuSize(Listbox.Model x, Listbox.Model y)
		{
			return x.PlatformCpuSize.CompareTo(y.PlatformCpuSize);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x000300EF File Offset: 0x0002E2EF
		private string OnExportCpuSize(Listbox.Model model)
		{
			return model.PlatformCpuSize.ToString();
		}

		// Token: 0x0600030D RID: 781 RVA: 0x000300FC File Offset: 0x0002E2FC
		private void OnDrawGpuSize(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, model.GpuSizeString, args.Selected);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00030117 File Offset: 0x0002E317
		private int OnCompareGpuSize(Listbox.Model x, Listbox.Model y)
		{
			return x.PlatformGpuSize.CompareTo(y.PlatformGpuSize);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0003012A File Offset: 0x0002E32A
		private string OnExportGpuSize(Listbox.Model model)
		{
			return model.PlatformGpuSize.ToString();
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00030137 File Offset: 0x0002E337
		private void OnDrawPixelRatioPercentage(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, GUIContent2.Temp(model.PlatformPixelRatioPercentageString, model.PlatformPixelRatioTooltip), args.Selected);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00030160 File Offset: 0x0002E360
		private int OnComparePixelRatioPercentage(Listbox.Model x, Listbox.Model y)
		{
			return x.PlatformPixelRatioPercentage.CompareTo(y.PlatformPixelRatioPercentage);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00030181 File Offset: 0x0002E381
		private string OnExportPixelRatioPercentage(Listbox.Model model)
		{
			return model.PlatformPixelRatioPercentageString;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00030189 File Offset: 0x0002E389
		private void OnDrawWidth(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, model.WidthString, args.Selected);
		}

		// Token: 0x06000314 RID: 788 RVA: 0x000301A4 File Offset: 0x0002E3A4
		private int OnCompareWidth(Listbox.Model x, Listbox.Model y)
		{
			return x.PlatformWidth.CompareTo(y.PlatformWidth);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x000301B7 File Offset: 0x0002E3B7
		private string OnExportWidth(Listbox.Model model)
		{
			return model.PlatformWidth.ToString();
		}

		// Token: 0x06000316 RID: 790 RVA: 0x000301C4 File Offset: 0x0002E3C4
		private void OnDrawHeight(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, model.HeightString, args.Selected);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x000301DF File Offset: 0x0002E3DF
		private int OnCompareHeight(Listbox.Model x, Listbox.Model y)
		{
			return x.PlatformHeight.CompareTo(y.PlatformHeight);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x000301F2 File Offset: 0x0002E3F2
		private string OnExportHeight(Listbox.Model model)
		{
			return model.PlatformHeight.ToString();
		}

		// Token: 0x06000319 RID: 793 RVA: 0x000301FF File Offset: 0x0002E3FF
		private void OnDrawOriginalWidth(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, model.OrgWidthString, args.Selected);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0003021A File Offset: 0x0002E41A
		private int OnCompareOriginalWidth(Listbox.Model x, Listbox.Model y)
		{
			return x.OrgWidth.CompareTo(y.OrgWidth);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0003022D File Offset: 0x0002E42D
		private string OnExportOriginalWidth(Listbox.Model model)
		{
			return model.OrgWidth.ToString();
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0003023A File Offset: 0x0002E43A
		private void OnDrawOriginalHeight(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, model.OrgHeightString, args.Selected);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00030255 File Offset: 0x0002E455
		private int OnCompareOriginalHeight(Listbox.Model x, Listbox.Model y)
		{
			return x.OrgHeight.CompareTo(y.OrgHeight);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00030268 File Offset: 0x0002E468
		private string OnExportOriginalHeight(Listbox.Model model)
		{
			return model.OrgHeight.ToString();
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00030275 File Offset: 0x0002E475
		private void OnDrawAnisoLevel(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, model.AnisoLevelString, args.Selected);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00030290 File Offset: 0x0002E490
		private int OnCompareAnisoLevel(Listbox.Model x, Listbox.Model y)
		{
			return x.AbsAnisoLevel.CompareTo(y.AbsAnisoLevel);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x000302A3 File Offset: 0x0002E4A3
		private string OnExportAnisoLevel(Listbox.Model model)
		{
			return model.AbsAnisoLevel.ToString();
		}

		// Token: 0x06000322 RID: 802 RVA: 0x000302B0 File Offset: 0x0002E4B0
		private void DrawToggle(GUIListViewDrawItemArgs args, bool value)
		{
			EditorGUI.Toggle(args.ItemRect, value);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x000302C0 File Offset: 0x0002E4C0
		private void OnDrawHasAlpha(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			this.DrawToggle(args, model.HasAlpha);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x000302D0 File Offset: 0x0002E4D0
		private int OnCompareHasAlpha(Listbox.Model x, Listbox.Model y)
		{
			int num = x.HasAlpha ? 1 : 0;
			int num2 = y.HasAlpha ? 1 : 0;
			return num - num2;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x000302FA File Offset: 0x0002E4FA
		private string OnExportHasAlpha(Listbox.Model model)
		{
			return model.HasAlpha.ToString();
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00030307 File Offset: 0x0002E507
		private void OnDrawHasIssue(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			this.DrawToggle(args, model.PlatformHasIssue);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00030318 File Offset: 0x0002E518
		private int OnCompareHasIssue(Listbox.Model x, Listbox.Model y)
		{
			int num = x.PlatformHasIssue ? 1 : 0;
			int num2 = y.PlatformHasIssue ? 1 : 0;
			return num - num2;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00030342 File Offset: 0x0002E542
		private string OnExportHasIssue(Listbox.Model model)
		{
			return model.PlatformHasIssue.ToString();
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0003034F File Offset: 0x0002E54F
		private void OnDrawAlphaSource(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, model.AlphaSourceString, args.Selected);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0003036A File Offset: 0x0002E56A
		private int OnCompareAlphaSource(Listbox.Model x, Listbox.Model y)
		{
			return string.Compare(x.AlphaSourceString, y.AlphaSourceString);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0003037D File Offset: 0x0002E57D
		private string OnExportAlphaSource(Listbox.Model model)
		{
			return model.AlphaSourceString;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00030385 File Offset: 0x0002E585
		private void OnDrawAlphaIsTransparency(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			this.DrawToggle(args, model.AlphaIsTransparency);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00030394 File Offset: 0x0002E594
		private int OnCompareAlphaIsTransparency(Listbox.Model x, Listbox.Model y)
		{
			return x.AlphaIsTransparency.CompareTo(y.AlphaIsTransparency);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x000303A7 File Offset: 0x0002E5A7
		private string OnExportAlphaIsTransparency(Listbox.Model model)
		{
			return model.AlphaIsTransparency.ToString();
		}

		// Token: 0x0600032F RID: 815 RVA: 0x000303B4 File Offset: 0x0002E5B4
		private void OnDrawIsPowerOfTwo(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			this.DrawToggle(args, model.PlatformIsPowerOfTwo);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x000303C4 File Offset: 0x0002E5C4
		private int OnCompareIsPowerOfTwo(Listbox.Model x, Listbox.Model y)
		{
			int num = x.PlatformIsPowerOfTwo ? 1 : 0;
			int num2 = y.PlatformIsPowerOfTwo ? 1 : 0;
			return num - num2;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x000303EE File Offset: 0x0002E5EE
		private string OnExportIsPowerOfTwo(Listbox.Model model)
		{
			return model.PlatformIsPowerOfTwo.ToString();
		}

		// Token: 0x06000332 RID: 818 RVA: 0x000303FB File Offset: 0x0002E5FB
		private void OnDrawIsSquare(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			this.DrawToggle(args, model.PlatformIsSquare);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0003040C File Offset: 0x0002E60C
		private int OnCompareIsSquare(Listbox.Model x, Listbox.Model y)
		{
			int num = x.PlatformIsSquare ? 1 : 0;
			int num2 = y.PlatformIsSquare ? 1 : 0;
			return num - num2;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00030436 File Offset: 0x0002E636
		private string OnExportIsSquare(Listbox.Model model)
		{
			return model.PlatformIsSquare.ToString();
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00030443 File Offset: 0x0002E643
		private void OnDrawReadWriteEnabled(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			this.DrawToggle(args, model.IsReadable);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00030454 File Offset: 0x0002E654
		private int OnCompareReadWriteEnabled(Listbox.Model x, Listbox.Model y)
		{
			int num = x.IsReadable ? 1 : 0;
			int num2 = y.IsReadable ? 1 : 0;
			return num - num2;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0003047E File Offset: 0x0002E67E
		private string OnExportReadWriteEnabled(Listbox.Model model)
		{
			return model.IsReadable.ToString();
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0003048B File Offset: 0x0002E68B
		private void OnDrawSpriteImportMode(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, (model.SpriteImportMode != null) ? model.SpriteImportModeString : "", args.Selected);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x000304B5 File Offset: 0x0002E6B5
		private int OnCompareSpriteImportMode(Listbox.Model x, Listbox.Model y)
		{
			return string.Compare(x.SpriteImportModeString, y.SpriteImportModeString, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x000304C9 File Offset: 0x0002E6C9
		private string OnExportSpriteImportMode(Listbox.Model model)
		{
			return model.SpriteImportModeString;
		}

		// Token: 0x0600033B RID: 827 RVA: 0x000304D1 File Offset: 0x0002E6D1
		private void OnDrawSpritePackingTag(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, (model.SpriteImportMode != null) ? model.SpritePackingTag : "", args.Selected);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x000304FB File Offset: 0x0002E6FB
		private int OnCompareSpritePackingTag(Listbox.Model x, Listbox.Model y)
		{
			return string.Compare(x.SpritePackingTag, y.SpritePackingTag, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0003050F File Offset: 0x0002E70F
		private string OnExportSpritePackingTag(Listbox.Model model)
		{
			return model.SpritePackingTag;
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00030517 File Offset: 0x0002E717
		private void OnDrawSpritePixelPerUnit(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			EditorGUI2.Label(args.ItemRect, (model.SpriteImportMode != null) ? model.SpritePixelPerUnitString : "", args.Selected);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00030541 File Offset: 0x0002E741
		private int OnCompareSpritePixelPerUnit(Listbox.Model x, Listbox.Model y)
		{
			return string.Compare(x.SpritePixelPerUnitString, y.SpritePixelPerUnitString, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00030555 File Offset: 0x0002E755
		private string OnExportSpritePixelPerUnit(Listbox.Model model)
		{
			return model.SpritePixelPerUnitString;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00030560 File Offset: 0x0002E760
		private void OnDrawOverride(Listbox.Model model, GUIListViewDrawItemArgs args)
		{
			bool importerOverridden = model.GetImporterOverridden(this._platform);
			this.DrawToggle(args, importerOverridden);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00030584 File Offset: 0x0002E784
		private int OnCompareOverride(Listbox.Model x, Listbox.Model y)
		{
			bool importerOverridden = x.GetImporterOverridden(this._platform);
			bool importerOverridden2 = y.GetImporterOverridden(this._platform);
			return importerOverridden.CompareTo(importerOverridden2);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x000305B4 File Offset: 0x0002E7B4
		private string OnExportOverride(Listbox.Model model)
		{
			return model.GetImporterOverridden(this._platform).ToString();
		}

		// Token: 0x06000344 RID: 836 RVA: 0x000305D8 File Offset: 0x0002E7D8
		private void AssetFileWatcher_Moved(string[] oldPaths, string[] newPaths)
		{
			int num = 0;
			long num2 = 0L;
			for (int i = 0; i < oldPaths.Length; i++)
			{
				if (this.IsSupportedFile(oldPaths[i]))
				{
					num++;
					Listbox.Model model = this.FindModel(oldPaths[i]);
					if (model != null)
					{
						num2 += this.InitModel(ref model, newPaths[i]);
						this.CheckUnloadUnused(ref num2, 134217728L);
					}
				}
			}
			if (num > 0)
			{
				this.RefreshModels();
				base.Sort();
				this.DoTextureChanged();
				if (!this._cache.IsEmpty)
				{
					this._cache.Write();
				}
			}
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00030660 File Offset: 0x0002E860
		private void AssetFileWatcher_Deleted(string[] paths)
		{
			int num = 0;
			foreach (string text in paths)
			{
				if (this.IsSupportedFile(text))
				{
					num++;
					Listbox.Model model = this.FindModel(text);
					if (model != null)
					{
						this._allitems.Remove(model);
						this._items.Remove(model);
					}
				}
			}
			if (num > 0)
			{
				this.RefreshModels();
				base.DoChanged();
				this.DoTextureChanged();
				if (!this._cache.IsEmpty)
				{
					this._cache.Write();
				}
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x000306E8 File Offset: 0x0002E8E8
		private void AssetFileWatcher_Imported(string[] paths)
		{
			int num = 0;
			bool flag = false;
			long num2 = 0L;
			foreach (string text in paths)
			{
				if (this.IsSupportedFile(text))
				{
					num++;
					Listbox.Model model = this.FindModel(text);
					if (model != null)
					{
						num2 += this.InitModel(ref model, text);
					}
					else
					{
						num2 += this.InitModel(ref model, text);
						this._allitems.Add(model);
						if (this.IsIncludedInFilter(model))
						{
							this._items.Add(model);
						}
						flag = true;
						this.CheckUnloadUnused(ref num2, 134217728L);
					}
				}
			}
			if (num > 0)
			{
				this.RefreshModels();
				if (flag)
				{
					base.Sort();
				}
				base.DoChanged();
				this.DoTextureChanged();
				if (!this._cache.IsEmpty)
				{
					this._cache.Write();
				}
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x000307D8 File Offset: 0x0002E9D8
		public void GetMemoryUsage(out TextureMemoryUsageInfo cpuUsage, out TextureMemoryUsageInfo gpuUsage, out TextureMemoryUsageInfo runtimeUsage, out TextureMemoryUsageInfo storageUsage)
		{
			this.GetMemoryUsage(out cpuUsage, (Listbox.Model model) => model.PlatformCpuSize);
			this.GetMemoryUsage(out gpuUsage, (Listbox.Model model) => model.PlatformGpuSize);
			this.GetMemoryUsage(out runtimeUsage, (Listbox.Model model) => model.PlatformRuntimeSize);
			this.GetMemoryUsage(out storageUsage, (Listbox.Model model) => model.PlatformStorageSize);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0003088C File Offset: 0x0002EA8C
		private void GetMemoryUsage(out TextureMemoryUsageInfo usageInfo, Func<Listbox.Model, int> getSize)
		{
			usageInfo = new TextureMemoryUsageInfo();
			Dictionary<int, TextureMemoryUsageData> dictionary = new Dictionary<int, TextureMemoryUsageData>();
			int i = 0;
			int count = this._items.Count;
			while (i < count)
			{
				Listbox.Model model = this._items[i];
				if (model != null)
				{
					int num = getSize(model);
					usageInfo.TotalSize += (long)num;
					int key = Listbox.TypeFilter.ComputeHashCode(model.TextureType, model.TextureShape);
					TextureMemoryUsageData textureMemoryUsageData;
					if (!dictionary.TryGetValue(key, out textureMemoryUsageData))
					{
						dictionary.Add(key, textureMemoryUsageData = new TextureMemoryUsageData());
					}
					textureMemoryUsageData.TextureType = model.TextureType;
					textureMemoryUsageData.TextureShape = model.TextureShape;
					textureMemoryUsageData.Size += (long)num;
				}
				i++;
			}
			usageInfo.UsagePerType.AddRange(dictionary.Values);
			for (int j = 0; j < usageInfo.UsagePerType.Count; j++)
			{
				TextureMemoryUsageData textureMemoryUsageData2 = usageInfo.UsagePerType[j];
				textureMemoryUsageData2.Percentage = (float)textureMemoryUsageData2.Size / (float)usageInfo.TotalSize;
			}
			usageInfo.UsagePerType.Sort((TextureMemoryUsageData a, TextureMemoryUsageData b) => b.Size.CompareTo(a.Size));
		}

		// Token: 0x040001B5 RID: 437
		private CacheFile<Listbox.Model> _cache = new CacheFile<Listbox.Model>(20180201u, Globals.ProductTitle, "TextureOverview");

		// Token: 0x040001B6 RID: 438
		private List<Listbox.Model> _items = new List<Listbox.Model>();

		// Token: 0x040001B7 RID: 439
		private List<Listbox.Model> _allitems = new List<Listbox.Model>();

		// Token: 0x040001B8 RID: 440
		private bool _disposed;

		// Token: 0x040001B9 RID: 441
		private List<Listbox.TypeFilter> _typeFilter = new List<Listbox.TypeFilter>();

		// Token: 0x040001BA RID: 442
		private Dictionary<int, Listbox.TypeFilter> _typeFilterLookup = new Dictionary<int, Listbox.TypeFilter>();

		// Token: 0x040001BB RID: 443
		private SearchTextParser.Result _filterResult = new SearchTextParser.Result();

		// Token: 0x040001BC RID: 444
		private Listbox.TextFilterMode _filterMode;

		// Token: 0x040001BD RID: 445
		private int _filterUpdateCount;

		// Token: 0x040001BE RID: 446
		private BuildTargetGroup _platform;

		// Token: 0x040001BF RID: 447
		private int _warningCount;

		// Token: 0x040001C0 RID: 448
		public Action<Listbox> TextureChanged;

		// Token: 0x040001C1 RID: 449
		private Dictionary<string, int> _RendererCountLookup;

		// Token: 0x0200006E RID: 110
		private class Column : GUIListViewColumn
		{
			// Token: 0x0600034E RID: 846 RVA: 0x000309C8 File Offset: 0x0002EBC8
			public Column(string serializeName, string text, string tooltip, float width, Listbox.Column.ColumnDrawer drawer, Listbox.Column.CompareDelegate2 comparer, Listbox.Column.ExportDelegate exporter) : base(text, tooltip, null, width, null)
			{
				this.SerializeName = serializeName;
				this.Drawer = drawer;
				this.Comparer = comparer;
				this.Exporter = exporter;
				if (comparer != null)
				{
					this.CompareFunc = new GUIListViewColumn.CompareDelelgate(this.CompareFuncImpl);
				}
			}

			// Token: 0x0600034F RID: 847 RVA: 0x00030A18 File Offset: 0x0002EC18
			private int CompareFuncImpl(object x, object y)
			{
				Listbox.Model x2 = x as Listbox.Model;
				Listbox.Model y2 = y as Listbox.Model;
				return this.Comparer(x2, y2);
			}

			// Token: 0x040001C8 RID: 456
			public Listbox.Column.ColumnDrawer Drawer;

			// Token: 0x040001C9 RID: 457
			public Listbox.Column.CompareDelegate2 Comparer;

			// Token: 0x040001CA RID: 458
			public Listbox.Column.ExportDelegate Exporter;

			// Token: 0x040001CB RID: 459
			public Listbox.Column.ColumnMode Mode;

			// Token: 0x040001CC RID: 460
			public bool HideIfDefault;

			// Token: 0x0200006F RID: 111
			public enum ColumnMode
			{
				// Token: 0x040001CE RID: 462
				None,
				// Token: 0x040001CF RID: 463
				PlatformSpecific
			}

			// Token: 0x02000070 RID: 112
			// (Invoke) Token: 0x06000351 RID: 849
			public delegate void ColumnDrawer(Listbox.Model model, GUIListViewDrawItemArgs args);

			// Token: 0x02000071 RID: 113
			// (Invoke) Token: 0x06000355 RID: 853
			public delegate int CompareDelegate2(Listbox.Model x, Listbox.Model y);

			// Token: 0x02000072 RID: 114
			// (Invoke) Token: 0x06000359 RID: 857
			public delegate string ExportDelegate(Listbox.Model model);
		}

		// Token: 0x02000073 RID: 115
		private class Model : ICacheFileEntry
		{
			// Token: 0x1700007E RID: 126
			// (get) Token: 0x0600035C RID: 860 RVA: 0x00030A44 File Offset: 0x0002EC44
			public string AssetPath
			{
				get
				{
					if (this._assetPath != null)
					{
						return this._assetPath;
					}
					this._assetPath = (AssetDatabase.GUIDToAssetPath(this.AssetGuid) ?? this.AssetGuid);
					if (string.IsNullOrEmpty(this._assetPath))
					{
						this._assetPath = "<Missing asset in project>";
					}
					return this._assetPath;
				}
			}

			// Token: 0x1700007F RID: 127
			// (get) Token: 0x0600035D RID: 861 RVA: 0x00030A99 File Offset: 0x0002EC99
			public string FileExtension
			{
				get
				{
					if (this._fileExtension == null)
					{
						this._fileExtension = FileUtil2.GetFileExtension(this.AssetPath).ToLower();
					}
					return this._fileExtension;
				}
			}

			// Token: 0x17000080 RID: 128
			// (get) Token: 0x0600035E RID: 862 RVA: 0x00030ABF File Offset: 0x0002ECBF
			public string AssetName
			{
				get
				{
					if (this._assetName == null)
					{
						this._assetName = FileUtil2.GetFileNameWithoutExtension(this.AssetPath);
					}
					return this._assetName;
				}
			}

			// Token: 0x17000081 RID: 129
			// (get) Token: 0x0600035F RID: 863 RVA: 0x00030AE0 File Offset: 0x0002ECE0
			public string OrgWidthString
			{
				get
				{
					if (this._orgWidthString == null)
					{
						this._orgWidthString = this.OrgWidth.ToString();
					}
					return this._orgWidthString;
				}
			}

			// Token: 0x17000082 RID: 130
			// (get) Token: 0x06000360 RID: 864 RVA: 0x00030B01 File Offset: 0x0002ED01
			public string OrgHeightString
			{
				get
				{
					if (this._orgHeightString == null)
					{
						this._orgHeightString = this.OrgHeight.ToString();
					}
					return this._orgHeightString;
				}
			}

			// Token: 0x17000083 RID: 131
			// (get) Token: 0x06000361 RID: 865 RVA: 0x00030B22 File Offset: 0x0002ED22
			public string AnisoLevelString
			{
				get
				{
					if (this._anisoLevelString == null)
					{
						this._anisoLevelString = this.AbsAnisoLevel.ToString();
					}
					return this._anisoLevelString;
				}
			}

			// Token: 0x17000084 RID: 132
			// (get) Token: 0x06000362 RID: 866 RVA: 0x00030B43 File Offset: 0x0002ED43
			public string TextureTypeString
			{
				get
				{
					if (this.IsLegacyCubemap)
					{
						return "Cubemap (Legacy)";
					}
					return TextureUtil2.GetTextureTypeString(this.TextureType, this.TextureShape);
				}
			}

			// Token: 0x17000085 RID: 133
			// (get) Token: 0x06000363 RID: 867 RVA: 0x00030B64 File Offset: 0x0002ED64
			public string TextureShapeString
			{
				get
				{
					return TextureUtil2.GetTextureShapeString(this.TextureShape);
				}
			}

			// Token: 0x17000086 RID: 134
			// (get) Token: 0x06000364 RID: 868 RVA: 0x00030B71 File Offset: 0x0002ED71
			public string AlphaSourceString
			{
				get
				{
					return TextureUtil2.GetAlphaSourceString(this.AlphaSource);
				}
			}

			// Token: 0x17000087 RID: 135
			// (get) Token: 0x06000365 RID: 869 RVA: 0x00030B7E File Offset: 0x0002ED7E
			public string SpriteImportModeString
			{
				get
				{
					if (this._spriteImportModeString == null)
					{
						this._spriteImportModeString = this.SpriteImportMode.ToString();
					}
					return this._spriteImportModeString;
				}
			}

			// Token: 0x17000088 RID: 136
			// (get) Token: 0x06000366 RID: 870 RVA: 0x00030BA4 File Offset: 0x0002EDA4
			public string SpritePixelPerUnitString
			{
				get
				{
					if (this._spritePixelToUnitString == null)
					{
						this._spritePixelToUnitString = string.Format("{0:F2}", this.SpritePixelPerUnit);
					}
					return this._spritePixelToUnitString;
				}
			}

			// Token: 0x17000089 RID: 137
			// (get) Token: 0x06000367 RID: 871 RVA: 0x00030BCF File Offset: 0x0002EDCF
			public string MaxTextureSizeString
			{
				get
				{
					if (this._maxTextureSizeString == null)
					{
						this._maxTextureSizeString = this.MaxTextureSize.ToString();
					}
					return this._maxTextureSizeString;
				}
			}

			// Token: 0x1700008A RID: 138
			// (get) Token: 0x06000368 RID: 872 RVA: 0x00030BF0 File Offset: 0x0002EDF0
			public string WidthString
			{
				get
				{
					if (this._widthString == null)
					{
						this._widthString = this.PlatformWidth.ToString();
					}
					return this._widthString;
				}
			}

			// Token: 0x1700008B RID: 139
			// (get) Token: 0x06000369 RID: 873 RVA: 0x00030C11 File Offset: 0x0002EE11
			public string HeightString
			{
				get
				{
					if (this._heightString == null)
					{
						this._heightString = this.PlatformHeight.ToString();
					}
					return this._heightString;
				}
			}

			// Token: 0x1700008C RID: 140
			// (get) Token: 0x0600036A RID: 874 RVA: 0x00030C32 File Offset: 0x0002EE32
			public string RuntimeSizeString
			{
				get
				{
					if (this._runtimeSizeString == null)
					{
						this._runtimeSizeString = EditorUtility2.FormatBytes((long)this.PlatformRuntimeSize);
					}
					return this._runtimeSizeString;
				}
			}

			// Token: 0x1700008D RID: 141
			// (get) Token: 0x0600036B RID: 875 RVA: 0x00030C54 File Offset: 0x0002EE54
			public string CpuSizeString
			{
				get
				{
					if (this._cpuSizeString == null)
					{
						this._cpuSizeString = EditorUtility2.FormatBytes((long)this.PlatformCpuSize);
					}
					return this._cpuSizeString;
				}
			}

			// Token: 0x1700008E RID: 142
			// (get) Token: 0x0600036C RID: 876 RVA: 0x00030C76 File Offset: 0x0002EE76
			public string GpuSizeString
			{
				get
				{
					if (this._gpuSizeString == null)
					{
						this._gpuSizeString = EditorUtility2.FormatBytes((long)this.PlatformGpuSize);
					}
					return this._gpuSizeString;
				}
			}

			// Token: 0x1700008F RID: 143
			// (get) Token: 0x0600036D RID: 877 RVA: 0x00030C98 File Offset: 0x0002EE98
			public string StorageSizeString
			{
				get
				{
					if (this._storageSizeString == null)
					{
						this._storageSizeString = EditorUtility2.FormatBytes((long)this.PlatformStorageSize);
					}
					return this._storageSizeString;
				}
			}

			// Token: 0x17000090 RID: 144
			// (get) Token: 0x0600036E RID: 878 RVA: 0x00030CBC File Offset: 0x0002EEBC
			public int PlatformMipmapCount
			{
				get
				{
					if (!this.MipmapEnabled)
					{
						return 1;
					}
					return 1 + Mathf.FloorToInt(Mathf.Log((float)Mathf.Max(this.PlatformWidth, this.PlatformHeight), 2f));
				}
			}

			// Token: 0x17000091 RID: 145
			// (get) Token: 0x0600036F RID: 879 RVA: 0x00030CF8 File Offset: 0x0002EEF8
			public float PlatformPixelRatio
			{
				get
				{
					if (this.HasError)
					{
						return 0f;
					}
					return 1f / ((float)(this.PlatformWidth * this.PlatformHeight) / (float)(this.OrgWidth * this.OrgHeight));
				}
			}

			// Token: 0x17000092 RID: 146
			// (get) Token: 0x06000370 RID: 880 RVA: 0x00030D2B File Offset: 0x0002EF2B
			public string PlatformPixelRatioString
			{
				get
				{
					if (this._pixelRatioString == null)
					{
						this._pixelRatioString = string.Format("1/{0:F0}", this.PlatformPixelRatio);
					}
					return this._pixelRatioString;
				}
			}

			// Token: 0x17000093 RID: 147
			// (get) Token: 0x06000371 RID: 881 RVA: 0x00030D58 File Offset: 0x0002EF58
			public string PlatformPixelRatioTooltip
			{
				get
				{
					if (this._pixelRatioTooltip == null)
					{
						if (Mathf.Abs(this.PlatformPixelRatioPercentage - 100f) > 0.0001f)
						{
							this._pixelRatioTooltip = string.Format("'{4}' resized from {0}x{1} to {2}x{3}", new object[]
							{
								this.OrgWidth,
								this.OrgHeight,
								this.PlatformWidth,
								this.PlatformHeight,
								this.AssetName
							});
						}
						else
						{
							this._pixelRatioTooltip = "";
						}
					}
					return this._pixelRatioTooltip;
				}
			}

			// Token: 0x17000094 RID: 148
			// (get) Token: 0x06000372 RID: 882 RVA: 0x00030DF1 File Offset: 0x0002EFF1
			public float PlatformPixelRatioPercentage
			{
				get
				{
					if (this.HasError)
					{
						return 0f;
					}
					return 100f * ((float)(this.PlatformWidth * this.PlatformHeight) / (float)(this.OrgWidth * this.OrgHeight));
				}
			}

			// Token: 0x17000095 RID: 149
			// (get) Token: 0x06000373 RID: 883 RVA: 0x00030E24 File Offset: 0x0002F024
			public string PlatformPixelRatioPercentageString
			{
				get
				{
					if (this._pixelRatioPercentageString == null)
					{
						this._pixelRatioPercentageString = string.Format(Listbox.Model._enusCulture, "{0:F1}%", new object[]
						{
							this.PlatformPixelRatioPercentage
						});
					}
					return this._pixelRatioPercentageString;
				}
			}

			// Token: 0x17000096 RID: 150
			// (get) Token: 0x06000374 RID: 884 RVA: 0x00030E6A File Offset: 0x0002F06A
			private Listbox.Model.ImporterPlatformSettings DefaultImporterSettings
			{
				get
				{
					return this.ImporterSettings[0];
				}
			}

			// Token: 0x06000375 RID: 885 RVA: 0x00030E7D File Offset: 0x0002F07D
			public string GetAssetGuid()
			{
				return this.AssetGuid;
			}

			// Token: 0x06000376 RID: 886 RVA: 0x00030E85 File Offset: 0x0002F085
			public string GetAssetHash()
			{
				return this.AssetHash;
			}

			// Token: 0x06000377 RID: 887 RVA: 0x00030E90 File Offset: 0x0002F090
			public void Serialize(BinarySerializer data)
			{
				data.Serialize(ref this.AssetGuid);
				data.Serialize(ref this.AssetHash);
				data.Serialize(ref this.HasError);
				data.Serialize(ref this.MaxTextureSize);
				data.Serialize(ref this.OrgWidth);
				data.Serialize(ref this.OrgHeight);
				data.Serialize(ref this.AnisoLevel);
				data.Serialize(ref this.MipmapEnabled);
				data.Serialize(ref this.MipmapBorder);
				data.Serialize(ref this.MipmapFadeout);
				data.Serialize(ref this.MipmapBias);
				data.Serialize(ref this.MipmapPreserveCoverage);
				this.MipmapFilter = (TextureImporterMipFilter)data.SerializeInt32((int)this.MipmapFilter);
				data.Serialize(ref this.StreamingMipmaps);
				data.Serialize(ref this.StreamingMipmapsPriority);
				this.WrapModeU = (TextureWrapMode)data.SerializeInt32((int)this.WrapModeU);
				this.WrapModeV = (TextureWrapMode)data.SerializeInt32((int)this.WrapModeV);
				this.WrapModeW = (TextureWrapMode)data.SerializeInt32((int)this.WrapModeW);
				this.FilterMode = (FilterMode)data.SerializeInt32((int)this.FilterMode);
				this.TextureType = (TextureImporterType)data.SerializeInt32((int)this.TextureType);
				this.TextureShape = (TextureImporterShape)data.SerializeInt32((int)this.TextureShape);
				data.Serialize(ref this.HasAlpha);
				data.Serialize(ref this.IsReadable);
				data.Serialize(ref this.sRGBTexture);
				this.SpriteImportMode = (SpriteImportMode)data.SerializeInt32((int)this.SpriteImportMode);
				data.Serialize(ref this.SpritePackingTag);
				data.Serialize(ref this.SpritePixelPerUnit);
				this.AlphaSource = (TextureImporterAlphaSource)data.SerializeInt32((int)this.AlphaSource);
				data.Serialize(ref this.AlphaIsTransparency);
				data.Serialize(ref this.AlphaAllowSplitting);
				this.GenerateCubemap = (TextureImporterGenerateCubemap)data.SerializeInt32((int)this.GenerateCubemap);
				data.Serialize(ref this.HeightmapToNormalmap);
				this.NPOTScale = (TextureImporterNPOTScale)data.SerializeInt32((int)this.NPOTScale);
				this.NormalFilter = (TextureImporterNormalFilter)data.SerializeInt32((int)this.NormalFilter);
				data.Serialize(ref this.HeightmapScale);
				if (data.IsReader)
				{
					this.ImporterSettings = new Listbox.Model.ImporterPlatformSettings[32];
				}
				int i = 0;
				int num = this.ImporterSettings.Length;
				while (i < num)
				{
					Listbox.Model.ImporterPlatformSettings importerPlatformSettings = this.ImporterSettings[i];
					data.Serialize(ref importerPlatformSettings.available);
					data.Serialize(ref importerPlatformSettings.overridden);
					data.Serialize(ref importerPlatformSettings.allowsAlphaSplitting);
					data.Serialize(ref importerPlatformSettings.compressionQuality);
					data.Serialize(ref importerPlatformSettings.crunchedCompression);
					importerPlatformSettings.textureCompression = (TextureImporterCompression)data.SerializeInt32((int)importerPlatformSettings.textureCompression);
					importerPlatformSettings.format = (TextureImporterFormat)data.SerializeInt32((int)importerPlatformSettings.format);
					data.Serialize(ref importerPlatformSettings.maxTextureSize);
					importerPlatformSettings.platformFormat = (TextureImporterFormat)data.SerializeInt32((int)importerPlatformSettings.platformFormat);
					importerPlatformSettings.resizeAlgorithm = (TextureResizeAlgorithm)data.SerializeInt32((int)importerPlatformSettings.resizeAlgorithm);
					this.ImporterSettings[i] = importerPlatformSettings;
					i++;
				}
			}

			// Token: 0x06000378 RID: 888 RVA: 0x0003117C File Offset: 0x0002F37C
			public Texture AllocAssetIcon()
			{
				if (this._assetIcon != null)
				{
					return this._assetIcon;
				}
				if ((int)this.TextureShape == 2)
				{
					this._assetIcon = EditorFramework.Images.Cubemap16x16;
				}
				if ((int)this.TextureShape == 1)
				{
					this._assetIcon = AssetDatabase.GetCachedIcon(this.AssetPath);
				}
				if (this._assetIcon == null)
				{
					this._assetIcon = EditorFramework.Images.Texture2D16x16;
				}
				return this._assetIcon;
			}

			// Token: 0x06000379 RID: 889 RVA: 0x000311EB File Offset: 0x0002F3EB
			public void ReleaseAssetIcon()
			{
				this._assetIcon = null;
			}

			// Token: 0x0600037A RID: 890 RVA: 0x000311F4 File Offset: 0x0002F3F4
			private void ResetCachedString()
			{
				this.PlatformIssueString = "";
				this._assetPath = null;
				this._fileExtension = null;
				this._assetName = null;
				this._maxTextureSizeString = null;
				this._widthString = null;
				this._heightString = null;
				this._orgWidthString = null;
				this._orgHeightString = null;
				this._anisoLevelString = null;
				this._runtimeSizeString = null;
				this._cpuSizeString = null;
				this._gpuSizeString = null;
				this._storageSizeString = null;
				this._pixelRatioString = null;
				this._pixelRatioTooltip = null;
				this._pixelRatioPercentageString = null;
				this._assetIcon = null;
				this._spriteImportModeString = null;
				this._spritePixelToUnitString = null;
				this.ImporterSettings = new Listbox.Model.ImporterPlatformSettings[32];
			}

			// Token: 0x0600037B RID: 891 RVA: 0x000312A0 File Offset: 0x0002F4A0
			public long Init(string path)
			{
				this.ResetCachedString();
				this.HasError = false;
				this.AssetGuid = AssetDatabase.AssetPathToGUID(path);
				this.AssetHash = AssetDatabase.GetAssetDependencyHash(path).ToString();
				AssetImporter atPath = AssetImporter.GetAtPath(path);
				if (atPath == null)
				{
					this.HasError = true;
					return 0L;
				}
				if (!string.IsNullOrEmpty(path) && path.EndsWith(".cubemap"))
				{
					return this.InitLegacyCubemap(atPath);
				}
				TextureImporter textureImporter = atPath as TextureImporter;
				if (textureImporter == null)
				{
					this.HasError = true;
				}
				Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(path);
				if (null == texture)
				{
					this.HasError = true;
				}
				long result = 0L;
				if (texture != null)
				{
					result = ProfilerUtility.GetRuntimeMemorySize(texture);
				}
				if (this.HasError)
				{
					return result;
				}
				this.HasAlpha = textureImporter.DoesSourceTextureHaveAlpha();
				this.AlphaIsTransparency = textureImporter.alphaIsTransparency;
				this.AlphaSource = textureImporter.alphaSource;
				this.AlphaAllowSplitting = textureImporter.allowAlphaSplitting;
				this.AnisoLevel = textureImporter.anisoLevel;
				this.FilterMode = textureImporter.filterMode;
				this.IsReadable = textureImporter.isReadable;
				this.WrapModeU = textureImporter.wrapModeU;
				this.WrapModeV = textureImporter.wrapModeV;
				this.WrapModeW = textureImporter.wrapModeW;
				this.TextureType = textureImporter.textureType;
				this.TextureShape = textureImporter.textureShape;
				this.sRGBTexture = textureImporter.sRGBTexture;
				this.MipmapEnabled = textureImporter.mipmapEnabled;
				this.MipmapBorder = textureImporter.borderMipmap;
				this.MipmapFadeout = textureImporter.fadeout;
				this.MipmapFilter = textureImporter.mipmapFilter;
				this.MipmapBias = textureImporter.mipMapBias;
				this.MipmapPreserveCoverage = textureImporter.mipMapsPreserveCoverage;
				this.StreamingMipmaps = textureImporter.streamingMipmaps;
				this.StreamingMipmapsPriority = textureImporter.streamingMipmapsPriority;
				this.GenerateCubemap = textureImporter.generateCubemap;
				this.HeightmapScale = textureImporter.heightmapScale;
				this.HeightmapToNormalmap = textureImporter.convertToNormalmap;
				this.NPOTScale = textureImporter.npotScale;
				this.NormalFilter = textureImporter.normalmapFilter;
				this.SpriteImportMode = 0;
				this.SpritePackingTag = "";
				this.SpritePixelPerUnit = -1f;
				if (textureImporter.spriteImportMode != null)
				{
					this.SpriteImportMode = textureImporter.spriteImportMode;
					this.SpritePackingTag = textureImporter.spritePackingTag;
					this.SpritePixelPerUnit = textureImporter.spritePixelsPerUnit;
				}
				TextureUtil2.GetOriginalWidthAndHeight(texture, textureImporter, out this.OrgWidth, out this.OrgHeight);
				TextureImporterPlatformSettings defaultPlatformTextureSettings = textureImporter.GetDefaultPlatformTextureSettings();
				int num = 0;
				this.ImporterSettings[num].maxTextureSize = defaultPlatformTextureSettings.maxTextureSize;
				this.ImporterSettings[num].compressionQuality = defaultPlatformTextureSettings.compressionQuality;
				this.ImporterSettings[num].crunchedCompression = defaultPlatformTextureSettings.crunchedCompression;
				this.ImporterSettings[num].allowsAlphaSplitting = defaultPlatformTextureSettings.allowsAlphaSplitting;
				this.ImporterSettings[num].format = 0;
				this.ImporterSettings[num].textureCompression = defaultPlatformTextureSettings.textureCompression;
				this.ImporterSettings[num].overridden = true;
				this.ImporterSettings[num].available = true;
				this.ImporterSettings[num].resizeAlgorithm = defaultPlatformTextureSettings.resizeAlgorithm;
				List<BuildPlatform2> validPlatforms = BuildPlayerWindow2.GetValidPlatforms();
				int i = 0;
				int count = validPlatforms.Count;
				while (i < count)
				{
					BuildPlatform2 buildPlatform = validPlatforms[i];
					if (buildPlatform.TargetGroup != null)
					{
						int targetGroup = (int)buildPlatform.TargetGroup;
						string name = buildPlatform.Name;
						TextureImporterPlatformSettings textureImporterPlatformSettings = textureImporter.GetPlatformTextureSettings(name);
						if (!textureImporterPlatformSettings.overridden)
						{
							textureImporterPlatformSettings = textureImporter.GetDefaultPlatformTextureSettings();
						}
						Listbox.Model.ImporterPlatformSettings importerPlatformSettings = default(Listbox.Model.ImporterPlatformSettings);
						importerPlatformSettings.available = true;
						importerPlatformSettings.overridden = textureImporterPlatformSettings.overridden;
						importerPlatformSettings.allowsAlphaSplitting = textureImporterPlatformSettings.allowsAlphaSplitting;
						importerPlatformSettings.compressionQuality = textureImporterPlatformSettings.compressionQuality;
						importerPlatformSettings.crunchedCompression = textureImporterPlatformSettings.crunchedCompression;
						importerPlatformSettings.format = textureImporterPlatformSettings.format;
						importerPlatformSettings.maxTextureSize = textureImporterPlatformSettings.maxTextureSize;
						importerPlatformSettings.textureCompression = textureImporterPlatformSettings.textureCompression;
						importerPlatformSettings.platformFormat = textureImporter.GetAutomaticFormat(name);
						importerPlatformSettings.resizeAlgorithm = textureImporterPlatformSettings.resizeAlgorithm;
						this.ImporterSettings[targetGroup] = importerPlatformSettings;
					}
					i++;
				}
				return result;
			}

			// Token: 0x0600037C RID: 892 RVA: 0x000316E0 File Offset: 0x0002F8E0
			private long InitLegacyCubemap(AssetImporter importer)
			{
				Cubemap cubemap = AssetDatabase.LoadAssetAtPath<Cubemap>(importer.assetPath);
				if (null == cubemap)
				{
					this.HasError = true;
					return 0L;
				}
				this.HasAlpha = false;
				this.AlphaIsTransparency = false;
				this.AlphaSource = 0;
				this.AlphaAllowSplitting = false;
				this.AnisoLevel = cubemap.anisoLevel;
				this.FilterMode = cubemap.filterMode;
				this.IsReadable = true;
				this.WrapModeU = cubemap.wrapModeU;
				this.WrapModeV = cubemap.wrapModeV;
				this.WrapModeW = cubemap.wrapModeW;
				this.TextureType = 0;
				this.TextureShape = (TextureImporterShape)2;
				this.sRGBTexture = true;
				this.MipmapEnabled = (cubemap.mipmapCount > 1);
				this.MipmapBorder = false;
				this.MipmapFadeout = false;
				this.MipmapFilter = 0;
				this.MipmapBias = cubemap.mipMapBias;
				this.HeightmapScale = 0f;
				this.HeightmapToNormalmap = false;
				this.NPOTScale = 0;
				this.NormalFilter = 0;
				this.SpriteImportMode = 0;
				this.SpritePackingTag = "";
				this.SpritePixelPerUnit = 0f;
				this.OrgWidth = cubemap.width;
				this.OrgHeight = cubemap.height;
				for (int i = 0; i < this.ImporterSettings.Length; i++)
				{
					this.ImporterSettings[i] = default(Listbox.Model.ImporterPlatformSettings);
					this.ImporterSettings[i].available = true;
					this.ImporterSettings[i].maxTextureSize = cubemap.width;
					this.ImporterSettings[i].format = TextureUtil2.GetTextureImporterFormatFromTextureFormat(cubemap.format);
					this.ImporterSettings[i].platformFormat = this.ImporterSettings[i].format;
					this.ImporterSettings[i].textureCompression = 0;
					this.ImporterSettings[i].resizeAlgorithm = 0;
				}
				return ProfilerUtility.GetRuntimeMemorySize(cubemap);
			}

			// Token: 0x0600037D RID: 893 RVA: 0x000318C4 File Offset: 0x0002FAC4
			public bool GetImporterSettingsAvailable(BuildTargetGroup platform)
			{
				Listbox.Model.ImporterPlatformSettings importerPlatformSettings = this.ImporterSettings[(int)platform];
				return importerPlatformSettings.available;
			}

			// Token: 0x0600037E RID: 894 RVA: 0x000318EA File Offset: 0x0002FAEA
			public bool GetImporterOverridden(BuildTargetGroup platform)
			{
				return this.ImporterSettings[(int)platform].overridden;
			}

			// Token: 0x0600037F RID: 895 RVA: 0x000318FD File Offset: 0x0002FAFD
			public TextureImporterFormat GetImporterFormat(BuildTargetGroup platform)
			{
				return this.ImporterSettings[(int)platform].format;
			}

			// Token: 0x06000380 RID: 896 RVA: 0x00031910 File Offset: 0x0002FB10
			public TextureImporterFormat GetImporterPlatformFormat(BuildTargetGroup platform)
			{
				return this.ImporterSettings[(int)platform].platformFormat;
			}

			// Token: 0x06000381 RID: 897 RVA: 0x00031923 File Offset: 0x0002FB23
			public int GetImporterCompressionQuality(BuildTargetGroup platform)
			{
				return this.ImporterSettings[(int)platform].compressionQuality;
			}

			// Token: 0x06000382 RID: 898 RVA: 0x00031936 File Offset: 0x0002FB36
			public int GetImporterMaxSize(BuildTargetGroup platform)
			{
				return this.ImporterSettings[(int)platform].maxTextureSize;
			}

			// Token: 0x06000383 RID: 899 RVA: 0x00031949 File Offset: 0x0002FB49
			public bool GetImporterUseCrunchCompression(BuildTargetGroup platform)
			{
				return this.ImporterSettings[(int)platform].crunchedCompression;
			}

			// Token: 0x06000384 RID: 900 RVA: 0x0003195C File Offset: 0x0002FB5C
			public TextureImporterCompression GetImporterTextureCompression(BuildTargetGroup platform)
			{
				return this.ImporterSettings[(int)platform].textureCompression;
			}

			// Token: 0x06000385 RID: 901 RVA: 0x00031970 File Offset: 0x0002FB70
			private int GetImporterWidthAndHeight(BuildTargetGroup platform, out int width, out int height)
			{
				width = this.OrgWidth;
				height = this.OrgHeight;
				Listbox.Model.ImporterPlatformSettings importerPlatformSettings = this.ImporterSettings[(int)platform];
				if (!importerPlatformSettings.overridden)
				{
					importerPlatformSettings = this.DefaultImporterSettings;
				}
				if (height <= width)
				{
					float num = (float)height / (float)width;
					width = Mathf.Min(width, importerPlatformSettings.maxTextureSize);
					height = Mathf.RoundToInt((float)width * num);
				}
				else
				{
					float num2 = (float)width / (float)height;
					height = Mathf.Min(height, importerPlatformSettings.maxTextureSize);
					width = Mathf.RoundToInt((float)height * num2);
				}
				return importerPlatformSettings.maxTextureSize;
			}

			// Token: 0x06000386 RID: 902 RVA: 0x00031A06 File Offset: 0x0002FC06
			public TextureResizeAlgorithm GetImporterResizeAlgorithm(BuildTargetGroup platform)
			{
				return this.ImporterSettings[(int)platform].resizeAlgorithm;
			}

			// Token: 0x06000387 RID: 903 RVA: 0x00031A1C File Offset: 0x0002FC1C
			public void SetPlatform(BuildTargetGroup platform)
			{
				if (this.HasError)
				{
					this.PlatformHasIssue = true;
					return;
				}
				this.PlatformIssueString = "";
				this._gpuSizeString = null;
				this._cpuSizeString = null;
				this._runtimeSizeString = null;
				this._storageSizeString = null;
				this._widthString = null;
				this._heightString = null;
				this.PlatformFormat = this.GetImporterPlatformFormat(platform);
				TextureImporterFormat platformFormat = this.PlatformFormat;
				string textureImporterFormatString = TextureUtil2.GetTextureImporterFormatString(platformFormat);
				this.GetImporterFormat(platform);
				this.AbsAnisoLevel = Mathf.Abs(this.AnisoLevel);
				this.IsLegacyCubemap = ((int)this.TextureShape == 2 && this.FileExtension == "cubemap");
				int importerWidthAndHeight = this.GetImporterWidthAndHeight(platform, out this.PlatformWidth, out this.PlatformHeight);
				this.PlatformWidth = TextureUtil2.GetPOTSize(this.PlatformWidth, this.NPOTScale);
				this.PlatformHeight = TextureUtil2.GetPOTSize(this.PlatformHeight, this.NPOTScale);
				if (TextureUtil2.IsSquareRequired(platformFormat))
				{
					if ((int)this.NPOTScale == 3)
					{
						this.PlatformWidth = Mathf.Min(this.PlatformWidth, this.PlatformHeight);
					}
					if ((int)this.NPOTScale == 2)
					{
						this.PlatformWidth = Mathf.Max(this.PlatformWidth, this.PlatformHeight);
					}
					if ((int)this.NPOTScale == 1)
					{
						if (importerWidthAndHeight - this.PlatformWidth < importerWidthAndHeight - this.PlatformHeight)
						{
							this.PlatformHeight = this.PlatformWidth;
						}
						else
						{
							this.PlatformWidth = this.PlatformHeight;
						}
					}
				}
				this.PlatformIsPowerOfTwo = (Mathf.IsPowerOfTwo(this.PlatformWidth) && Mathf.IsPowerOfTwo(this.PlatformHeight));
				this.PlatformIsSquare = (this.PlatformWidth == this.PlatformHeight);
				if (Globals.WarnCompressionFail && TextureUtil2.IsCompressedFormat(platformFormat))
				{
					bool flag = (int)this.TextureType == 8 && !string.IsNullOrEmpty(this.SpritePackingTag);
					if (!this.PlatformIsPowerOfTwo && this.MipmapEnabled && TextureUtil2.IsPOTRequiredForMipMaps(platformFormat))
					{
						this.PlatformIssueString += "Only POT textures can be compressed if mip-maps are enabled.\n\n";
						this.PlatformFormat = TextureUtil2.GetUncompressedFormat(this.PlatformFormat);
					}
					if (!flag && TextureUtil2.GetMultipleOf(platformFormat) != 0)
					{
						int multipleOf = TextureUtil2.GetMultipleOf(platformFormat);
						if (this.PlatformWidth % multipleOf != 0 || this.PlatformHeight % multipleOf != 0)
						{
							this.PlatformIssueString += string.Format("Only textures with width/height being multiple of 4 can be compressed to {0} format.\n\n", textureImporterFormatString);
							this.PlatformFormat = TextureUtil2.GetUncompressedFormat(this.PlatformFormat);
						}
					}
					if (!this.PlatformIsPowerOfTwo && !flag && TextureUtil2.IsPOTRequired(platformFormat))
					{
						this.PlatformIssueString += string.Format("Only POT textures can be compressed to {0} format.\n\n", textureImporterFormatString);
						this.PlatformFormat = TextureUtil2.GetUncompressedFormat(this.PlatformFormat);
					}
					if (!flag && !this.PlatformIsSquare && TextureUtil2.IsSquareRequired(platformFormat))
					{
						this.PlatformIssueString += string.Format("Only square textures can be compressed to {0} format.\n\n", textureImporterFormatString);
						this.PlatformFormat = TextureUtil2.GetUncompressedFormat(this.PlatformFormat);
					}
				}
				string fileExtension;
				if (Globals.WarnLossyCompressedSourceTexture && (fileExtension = this.FileExtension) != null && (fileExtension == "jpg" || fileExtension == "jpeg" || fileExtension == "gif"))
				{
					this.PlatformIssueString += string.Format("The source texture file uses a lossy compression format. Unity applies its own compression on top. To achieve better quality, without additional memory/performance cost, use uncompressed or lossless compressed images as source textures in your project.\n\n", new object[0]);
				}
				if (Globals.WarnLegacyCubemap && this.IsLegacyCubemap)
				{
					this.PlatformIssueString += string.Format("Legacy Cubemap asset found. Use the Cubemap texture import type instead.\n\n", new object[0]);
				}
				this.PlatformHasIssue = !string.IsNullOrEmpty(this.PlatformIssueString);
				this.PlatformIssueString = this.PlatformIssueString.Trim();
				this.PlatformStorageSize = TextureUtil2.GetRuntimeMemorySize(this.PlatformFormat, this.PlatformWidth, this.PlatformHeight, this.MipmapEnabled, false, this.TextureShape).Gpu;
				TextureUtil2.RuntimeMemoryUsage runtimeMemorySize = TextureUtil2.GetRuntimeMemorySize(this.PlatformFormat, this.PlatformWidth, this.PlatformHeight, this.MipmapEnabled, this.IsReadable, this.TextureShape);
				this.PlatformGpuSize = runtimeMemorySize.Gpu;
				this.PlatformCpuSize = runtimeMemorySize.Cpu;
				if ((int)this.PlatformFormat == 3 && Globals.GpuExpandRgb24ToRgba32)
				{
					this.PlatformGpuSize = TextureUtil2.GetRuntimeMemorySize((TextureImporterFormat)4, this.PlatformWidth, this.PlatformHeight, this.MipmapEnabled, this.IsReadable, this.TextureShape).Gpu;
				}
				this.PlatformRuntimeSize = this.PlatformGpuSize + this.PlatformCpuSize;
			}

			// Token: 0x040001D0 RID: 464
			private const int ImporterSettingsLength = 32;

			// Token: 0x040001D1 RID: 465
			private static CultureInfo _enusCulture = CultureInfo.GetCultureInfo("en-US");

			// Token: 0x040001D2 RID: 466
			private Texture _assetIcon;

			// Token: 0x040001D3 RID: 467
			public string AssetGuid;

			// Token: 0x040001D4 RID: 468
			public string AssetHash;

			// Token: 0x040001D5 RID: 469
			public bool HasError;

			// Token: 0x040001D6 RID: 470
			public bool PlatformHasIssue;

			// Token: 0x040001D7 RID: 471
			public string PlatformIssueString = "";

			// Token: 0x040001D8 RID: 472
			private string _assetPath;

			// Token: 0x040001D9 RID: 473
			private string _fileExtension;

			// Token: 0x040001DA RID: 474
			private string _assetName;

			// Token: 0x040001DB RID: 475
			public int OrgWidth;

			// Token: 0x040001DC RID: 476
			private string _orgWidthString;

			// Token: 0x040001DD RID: 477
			public int OrgHeight;

			// Token: 0x040001DE RID: 478
			private string _orgHeightString;

			// Token: 0x040001DF RID: 479
			public int AnisoLevel;

			// Token: 0x040001E0 RID: 480
			public int AbsAnisoLevel;

			// Token: 0x040001E1 RID: 481
			private string _anisoLevelString;

			// Token: 0x040001E2 RID: 482
			public bool MipmapEnabled;

			// Token: 0x040001E3 RID: 483
			public TextureImporterMipFilter MipmapFilter;

			// Token: 0x040001E4 RID: 484
			public bool MipmapBorder;

			// Token: 0x040001E5 RID: 485
			public bool MipmapFadeout;

			// Token: 0x040001E6 RID: 486
			public float MipmapBias;

			// Token: 0x040001E7 RID: 487
			public bool MipmapPreserveCoverage;

			// Token: 0x040001E8 RID: 488
			public bool StreamingMipmaps;

			// Token: 0x040001E9 RID: 489
			public int StreamingMipmapsPriority;

			// Token: 0x040001EA RID: 490
			public TextureImporterGenerateCubemap GenerateCubemap;

			// Token: 0x040001EB RID: 491
			public float HeightmapScale;

			// Token: 0x040001EC RID: 492
			public bool HeightmapToNormalmap;

			// Token: 0x040001ED RID: 493
			public TextureImporterNormalFilter NormalFilter;

			// Token: 0x040001EE RID: 494
			public TextureImporterNPOTScale NPOTScale;

			// Token: 0x040001EF RID: 495
			public TextureWrapMode WrapModeU;

			// Token: 0x040001F0 RID: 496
			public TextureWrapMode WrapModeV;

			// Token: 0x040001F1 RID: 497
			public TextureWrapMode WrapModeW;

			// Token: 0x040001F2 RID: 498
			public FilterMode FilterMode;

			// Token: 0x040001F3 RID: 499
			public TextureImporterType TextureType;

			// Token: 0x040001F4 RID: 500
			public TextureImporterShape TextureShape;

			// Token: 0x040001F5 RID: 501
			public TextureImporterAlphaSource AlphaSource;

			// Token: 0x040001F6 RID: 502
			public bool AlphaIsTransparency;

			// Token: 0x040001F7 RID: 503
			public bool AlphaAllowSplitting;

			// Token: 0x040001F8 RID: 504
			public bool HasAlpha;

			// Token: 0x040001F9 RID: 505
			public bool PlatformIsPowerOfTwo;

			// Token: 0x040001FA RID: 506
			public bool PlatformIsSquare;

			// Token: 0x040001FB RID: 507
			public bool IsReadable;

			// Token: 0x040001FC RID: 508
			public bool sRGBTexture;

			// Token: 0x040001FD RID: 509
			public SpriteImportMode SpriteImportMode;

			// Token: 0x040001FE RID: 510
			private string _spriteImportModeString;

			// Token: 0x040001FF RID: 511
			public string SpritePackingTag = "";

			// Token: 0x04000200 RID: 512
			public float SpritePixelPerUnit;

			// Token: 0x04000201 RID: 513
			private string _spritePixelToUnitString;

			// Token: 0x04000202 RID: 514
			public bool IsLegacyCubemap;

			// Token: 0x04000203 RID: 515
			public TextureImporterFormat PlatformFormat;

			// Token: 0x04000204 RID: 516
			public int MaxTextureSize;

			// Token: 0x04000205 RID: 517
			private string _maxTextureSizeString;

			// Token: 0x04000206 RID: 518
			private string _widthString;

			// Token: 0x04000207 RID: 519
			public int PlatformWidth;

			// Token: 0x04000208 RID: 520
			public int PlatformHeight;

			// Token: 0x04000209 RID: 521
			private string _heightString;

			// Token: 0x0400020A RID: 522
			public int PlatformRuntimeSize;

			// Token: 0x0400020B RID: 523
			private string _runtimeSizeString;

			// Token: 0x0400020C RID: 524
			public int PlatformCpuSize;

			// Token: 0x0400020D RID: 525
			private string _cpuSizeString;

			// Token: 0x0400020E RID: 526
			public int PlatformGpuSize;

			// Token: 0x0400020F RID: 527
			private string _gpuSizeString;

			// Token: 0x04000210 RID: 528
			public int PlatformStorageSize;

			// Token: 0x04000211 RID: 529
			private string _storageSizeString;

			// Token: 0x04000212 RID: 530
			private string _pixelRatioString;

			// Token: 0x04000213 RID: 531
			private string _pixelRatioTooltip;

			// Token: 0x04000214 RID: 532
			private string _pixelRatioPercentageString;

			// Token: 0x04000215 RID: 533
			public int RendererCount;

			// Token: 0x04000216 RID: 534
			private Listbox.Model.ImporterPlatformSettings[] ImporterSettings;

			// Token: 0x02000074 RID: 116
			private struct ImporterPlatformSettings
			{
				// Token: 0x04000217 RID: 535
				public bool available;

				// Token: 0x04000218 RID: 536
				public bool overridden;

				// Token: 0x04000219 RID: 537
				public bool allowsAlphaSplitting;

				// Token: 0x0400021A RID: 538
				public int compressionQuality;

				// Token: 0x0400021B RID: 539
				public bool crunchedCompression;

				// Token: 0x0400021C RID: 540
				public TextureImporterFormat format;

				// Token: 0x0400021D RID: 541
				public TextureImporterCompression textureCompression;

				// Token: 0x0400021E RID: 542
				public int maxTextureSize;

				// Token: 0x0400021F RID: 543
				public TextureImporterFormat platformFormat;

				// Token: 0x04000220 RID: 544
				public TextureResizeAlgorithm resizeAlgorithm;
			}
		}

		// Token: 0x02000075 RID: 117
		public enum TextFilterMode
		{
			// Token: 0x04000222 RID: 546
			All,
			// Token: 0x04000223 RID: 547
			Name,
			// Token: 0x04000224 RID: 548
			Path,
			// Token: 0x04000225 RID: 549
			SpritePackingTag
		}

		// Token: 0x02000076 RID: 118
		public class TypeFilter
		{
			// Token: 0x0600038A RID: 906 RVA: 0x00031EAC File Offset: 0x000300AC
			public TypeFilter(string name, TextureImporterType type, TextureImporterShape shape)
			{
				this.Name = name;
				this.Type = type;
				this.Shape = shape;
			}

			// Token: 0x0600038B RID: 907 RVA: 0x00031ECC File Offset: 0x000300CC
			public override int GetHashCode()
			{
				return Listbox.TypeFilter.ComputeHashCode(this.Type, this.Shape);
			}

			// Token: 0x0600038C RID: 908 RVA: 0x00031EEC File Offset: 0x000300EC
			public static int ComputeHashCode(TextureImporterType type, TextureImporterShape shape)
			{
				int num = 256 * (int)shape;
				return num + (int)type;
			}

			// Token: 0x04000226 RID: 550
			public string Name;

			// Token: 0x04000227 RID: 551
			public TextureImporterType Type;

			// Token: 0x04000228 RID: 552
			public TextureImporterShape Shape;
		}
	}
}
