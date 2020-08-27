using System;
using System.Collections.Generic;
using System.IO;

namespace EditorFramework
{
	// Token: 0x02000004 RID: 4
	public static class AssetBundleManifestParser
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000021D8 File Offset: 0x000003D8
		public static AssetBundleManifest2 Load(string path)
		{
			if (!File.Exists(path))
			{
				return new AssetBundleManifest2();
			}
			string[] array = File.ReadAllLines(path);
			AssetBundleManifest2 assetBundleManifest = new AssetBundleManifest2();
			assetBundleManifest.Name = Path.GetFileNameWithoutExtension(path);
			assetBundleManifest.Path = path;
			assetBundleManifest.Assets = new List<string>(array.Length);
			AssetBundleManifestParser.Category category = AssetBundleManifestParser.Category.None;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (!string.IsNullOrEmpty(text))
				{
					if (text.StartsWith("Assets:", StringComparison.OrdinalIgnoreCase))
					{
						category = AssetBundleManifestParser.Category.Assets;
					}
					else if (text.StartsWith("ClassTypes:", StringComparison.OrdinalIgnoreCase))
					{
						category = AssetBundleManifestParser.Category.ClassTypes;
					}
					else if (text.StartsWith("Dependencies:", StringComparison.OrdinalIgnoreCase))
					{
						category = AssetBundleManifestParser.Category.Dependencies;
					}
					else if (text.StartsWith("AssetBundleInfos:", StringComparison.OrdinalIgnoreCase))
					{
						category = AssetBundleManifestParser.Category.AssetBundleInfos;
					}
					else
					{
						switch (category)
						{
						case AssetBundleManifestParser.Category.Assets:
							AssetBundleManifestParser.ReadAssets(array, ref i, assetBundleManifest);
							break;
						case AssetBundleManifestParser.Category.Dependencies:
							assetBundleManifest.Dependencies.Add(text);
							break;
						case AssetBundleManifestParser.Category.AssetBundleInfos:
							AssetBundleManifestParser.ReadAssetBundleInfos(array, ref i, assetBundleManifest);
							break;
						}
					}
				}
			}
			return assetBundleManifest;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000022DC File Offset: 0x000004DC
		private static void ReadAssetBundleInfos(string[] lines, ref int index, AssetBundleManifest2 target)
		{
			int num = 0;
			for (int i = index; i < lines.Length; i++)
			{
				string text = lines[i].Trim();
				if (text.Length == 0)
				{
					return;
				}
				string a = string.Format("Info_{0}:", num);
				if (string.Equals(a, text, StringComparison.OrdinalIgnoreCase))
				{
					i++;
					text = lines[i].Trim();
					if (text.StartsWith("Name: ", StringComparison.OrdinalIgnoreCase))
					{
						target.Dependencies.Add(text.Substring("Name: ".Length));
						num++;
					}
				}
				index = i;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002364 File Offset: 0x00000564
		private static void ReadAssets(string[] lines, ref int index, AssetBundleManifest2 target)
		{
			for (int i = index; i < lines.Length; i++)
			{
				string text = lines[i];
				if (text.Length == 0)
				{
					break;
				}
				if (text[0] != '-')
				{
					return;
				}
				string item = text.Substring(2);
				target.Assets.Add(item);
				index = i;
			}
		}

		// Token: 0x02000005 RID: 5
		private enum Category
		{
			// Token: 0x04000009 RID: 9
			None,
			// Token: 0x0400000A RID: 10
			Assets,
			// Token: 0x0400000B RID: 11
			ClassTypes,
			// Token: 0x0400000C RID: 12
			Dependencies,
			// Token: 0x0400000D RID: 13
			AssetBundleInfos
		}
	}
}
