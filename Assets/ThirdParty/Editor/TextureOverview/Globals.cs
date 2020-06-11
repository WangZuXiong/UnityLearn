using System;
using System.IO;
using EditorFramework;
using UnityEditor;

namespace TextureOverview
{
	// Token: 0x0200006C RID: 108
	public static class Globals
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0002D27A File Offset: 0x0002B47A
		public static int ProductVersionNumber
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0002D27E File Offset: 0x0002B47E
		public static bool IsBeta
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0002D281 File Offset: 0x0002B481
		public static int MinimumMajorVersion
		{
			get
			{
				return 2018;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0002D288 File Offset: 0x0002B488
		public static int MinimumMinorVersion
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0002D28B File Offset: 0x0002B48B
		// (set) Token: 0x06000276 RID: 630 RVA: 0x0002D292 File Offset: 0x0002B492
		public static string ProductTitle ="texture overview crack";

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0002D29A File Offset: 0x0002B49A
		public static string ProductName
		{
			get
			{
				return "Texture Overview";
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0002D2A1 File Offset: 0x0002B4A1
		public static string ProductId
		{
			get
			{
				return "TextureOverview_v2";
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0002D2A8 File Offset: 0x0002B4A8
		public static string ProductAssetStoreUrl
		{
			get
			{
				if (Globals.IsPro)
				{
					return "https://www.assetstore.unity3d.com/#/content/10832";
				}
				return "https://www.assetstore.unity3d.com/en/#!/content/16937";
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600027A RID: 634 RVA: 0x0002D2BC File Offset: 0x0002B4BC
		public static string ProductFeedbackUrl
		{
			get
			{
				return "http://forum.unity3d.com/threads/197707-Released-Texture-Overview-Plugin";
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0002D2C3 File Offset: 0x0002B4C3
		// (set) Token: 0x0600027C RID: 636 RVA: 0x0002D2F8 File Offset: 0x0002B4F8
		public static bool GpuExpandRgb24ToRgba32
		{
			get
			{
				if (Globals._gpuExpandRgb24ToRgba32 == -1)
				{
					Globals._gpuExpandRgb24ToRgba32 = (EditorPrefs.GetBool(string.Format("{0}.GpuExpandRgb24ToRgba32", Globals.ProductId), true) ? 1 : 0);
				}
				return Globals._gpuExpandRgb24ToRgba32 > 0;
			}
			set
			{
				int num = value ? 1 : 0;
				if (num != Globals._gpuExpandRgb24ToRgba32)
				{
					Globals._gpuExpandRgb24ToRgba32 = num;
					EditorPrefs.SetBool(string.Format("{0}.GpuExpandRgb24ToRgba32", Globals.ProductId), value);
				}
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0002D330 File Offset: 0x0002B530
		// (set) Token: 0x0600027E RID: 638 RVA: 0x0002D364 File Offset: 0x0002B564
		public static bool WarnCompressionFail
		{
			get
			{
				if (Globals._warnCompressionFail == -1)
				{
					Globals._warnCompressionFail = (EditorPrefs.GetBool(string.Format("{0}.WarnCompressionFail", Globals.ProductId), true) ? 1 : 0);
				}
				return Globals._warnCompressionFail > 0;
			}
			set
			{
				int num = value ? 1 : 0;
				if (num != Globals._warnCompressionFail)
				{
					Globals._warnCompressionFail = num;
					EditorPrefs.SetBool(string.Format("{0}.WarnCompressionFail", Globals.ProductId), value);
				}
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600027F RID: 639 RVA: 0x0002D39C File Offset: 0x0002B59C
		// (set) Token: 0x06000280 RID: 640 RVA: 0x0002D3D0 File Offset: 0x0002B5D0
		public static bool WarnLossyCompressedSourceTexture
		{
			get
			{
				if (Globals._warnLossyCompressedSourceTexture == -1)
				{
					Globals._warnLossyCompressedSourceTexture = (EditorPrefs.GetBool(string.Format("{0}.WarnLossyCompressedSourceTexture", Globals.ProductId), true) ? 1 : 0);
				}
				return Globals._warnLossyCompressedSourceTexture > 0;
			}
			set
			{
				int num = value ? 1 : 0;
				if (num != Globals._warnLossyCompressedSourceTexture)
				{
					Globals._warnLossyCompressedSourceTexture = num;
					EditorPrefs.SetBool(string.Format("{0}.WarnLossyCompressedSourceTexture", Globals.ProductId), value);
				}
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0002D408 File Offset: 0x0002B608
		// (set) Token: 0x06000282 RID: 642 RVA: 0x0002D448 File Offset: 0x0002B648
		public static bool WarnLegacyCubemap
		{
			get
			{
				bool flag = true;
				if (Globals._warnLegacyCubemap == -1)
				{
					Globals._warnLegacyCubemap = (EditorPrefs.GetBool(string.Format("{0}.WarnLegacyCubemap", Globals.ProductId), flag) ? 1 : 0);
				}
				return Globals._warnLegacyCubemap > 0;
			}
			set
			{
				int num = value ? 1 : 0;
				if (num != Globals._warnLegacyCubemap)
				{
					Globals._warnLegacyCubemap = num;
					EditorPrefs.SetBool(string.Format("{0}.WarnLegacyCubemap", Globals.ProductId), value);
				}
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0002D480 File Offset: 0x0002B680
		// (set) Token: 0x06000284 RID: 644 RVA: 0x0002D4B4 File Offset: 0x0002B6B4
		public static bool CountRendererInSceneMode
		{
			get
			{
				if (Globals._countRenderer == -1)
				{
					Globals._countRenderer = (EditorPrefs.GetBool(string.Format("{0}.CountRenderer", Globals.ProductId), true) ? 1 : 0);
				}
				return Globals._countRenderer > 0;
			}
			set
			{
				int num = value ? 1 : 0;
				if (num != Globals._countRenderer)
				{
					Globals._countRenderer = num;
					EditorPrefs.SetBool(string.Format("{0}.CountRenderer", Globals.ProductId), value);
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0002D4EC File Offset: 0x0002B6EC
		// (set) Token: 0x06000286 RID: 646 RVA: 0x0002D520 File Offset: 0x0002B720
		public static bool ShowEditorAssets
		{
			get
			{
				if (Globals._showEditorAssets == -1)
				{
					Globals._showEditorAssets = (EditorPrefs.GetBool(string.Format("{0}.ShowEditorAssets", Globals.ProductId), false) ? 1 : 0);
				}
				return Globals._showEditorAssets > 0;
			}
			set
			{
				int num = value ? 1 : 0;
				if (num != Globals._showEditorAssets)
				{
					Globals._showEditorAssets = num;
					EditorPrefs.SetBool(string.Format("{0}.ShowEditorAssets", Globals.ProductId), value);
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0002D558 File Offset: 0x0002B758
		// (set) Token: 0x06000288 RID: 648 RVA: 0x0002D58C File Offset: 0x0002B78C
		public static bool ShowPackageAssets
		{
			get
			{
				if (Globals._showPackageAssets == -1)
				{
					Globals._showPackageAssets = (EditorPrefs.GetBool(string.Format("{0}.ShowPackageAssets", Globals.ProductId), true) ? 1 : 0);
				}
				return Globals._showPackageAssets > 0;
			}
			set
			{
				int num = value ? 1 : 0;
				if (num != Globals._showPackageAssets)
				{
					Globals._showPackageAssets = num;
					EditorPrefs.SetBool(string.Format("{0}.ShowPackageAssets", Globals.ProductId), value);
				}
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0002D5C4 File Offset: 0x0002B7C4
		public static bool IsPro
		{
			get
			{
				if (Globals._IsPro == -1)
				{
					string path = EditorApplication2.CombinePluginPath("TextureOverviewSource.zip");
					Globals._IsPro = (File.Exists(path) ? 1 : 0);
				}
				return Globals._IsPro > 0;
			}
		}

		// Token: 0x040001AC RID: 428
		private static int _gpuExpandRgb24ToRgba32 = -1;

		// Token: 0x040001AD RID: 429
		private static int _warnCompressionFail = -1;

		// Token: 0x040001AE RID: 430
		private static int _warnLossyCompressedSourceTexture = -1;

		// Token: 0x040001AF RID: 431
		private static int _warnLegacyCubemap = -1;

		// Token: 0x040001B0 RID: 432
		private static int _countRenderer = -1;

		// Token: 0x040001B1 RID: 433
		private static int _showEditorAssets = -1;

		// Token: 0x040001B2 RID: 434
		private static int _showPackageAssets = -1;

		// Token: 0x040001B3 RID: 435
		private static int _IsPro = -1;
	}
}
