using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000018 RID: 24
	public static class AssetDatabase2
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x00008540 File Offset: 0x00006740
		static AssetDatabase2()
		{
			AssetDatabase2._extensionToAssetType.Add("hdr", typeof(Cubemap));
			AssetDatabase2._extensionToAssetType.Add("asset", typeof(AssetDatabase2.UnityAsset));
			AssetDatabase2._extensionToAssetType.Add("unity", typeof(AssetDatabase2.UnityScene));
			AssetDatabase2._extensionToAssetType.Add("anim", typeof(AnimationClip));
			AssetDatabase2._extensionToAssetType.Add("physicmaterial", typeof(PhysicMaterial));
			AssetDatabase2._extensionToAssetType.Add("shader", typeof(Shader));
			AssetDatabase2._extensionToAssetType.Add("cubemap", typeof(Cubemap));
			AssetDatabase2._extensionToAssetType.Add("mat", typeof(Material));
			AssetDatabase2._extensionToAssetType.Add("material", typeof(Material));
			AssetDatabase2._extensionToAssetType.Add("wav", typeof(AudioClip));
			AssetDatabase2._extensionToAssetType.Add("mp3", typeof(AudioClip));
			AssetDatabase2._extensionToAssetType.Add("ogg", typeof(AudioClip));
			AssetDatabase2._extensionToAssetType.Add("aif", typeof(AudioClip));
			AssetDatabase2._extensionToAssetType.Add("aiff", typeof(AudioClip));
			AssetDatabase2._extensionToAssetType.Add("xm", typeof(AudioClip));
			AssetDatabase2._extensionToAssetType.Add("mod", typeof(AudioClip));
			AssetDatabase2._extensionToAssetType.Add("it", typeof(AudioClip));
			AssetDatabase2._extensionToAssetType.Add("s3m", typeof(AudioClip));
			AssetDatabase2._extensionToAssetType.Add("exr", typeof(Texture));
			AssetDatabase2._extensionToAssetType.Add("psd", typeof(Texture2D));
			AssetDatabase2._extensionToAssetType.Add("tif", typeof(Texture2D));
			AssetDatabase2._extensionToAssetType.Add("tiff", typeof(Texture2D));
			AssetDatabase2._extensionToAssetType.Add("jpg", typeof(Texture2D));
			AssetDatabase2._extensionToAssetType.Add("jpeg", typeof(Texture2D));
			AssetDatabase2._extensionToAssetType.Add("tga", typeof(Texture2D));
			AssetDatabase2._extensionToAssetType.Add("png", typeof(Texture2D));
			AssetDatabase2._extensionToAssetType.Add("gif", typeof(Texture2D));
			AssetDatabase2._extensionToAssetType.Add("bmp", typeof(Texture2D));
			AssetDatabase2._extensionToAssetType.Add("iff", typeof(Texture2D));
			AssetDatabase2._extensionToAssetType.Add("pict", typeof(Texture2D));
			AssetDatabase2._extensionToAssetType.Add("prefab", typeof(GameObject));
			AssetDatabase2._extensionToAssetType.Add("fbx", typeof(GameObject));
			AssetDatabase2._extensionToAssetType.Add("obj", typeof(GameObject));
			AssetDatabase2._extensionToAssetType.Add("max", typeof(GameObject));
			AssetDatabase2._extensionToAssetType.Add("blend", typeof(GameObject));
			AssetDatabase2._extensionToAssetType.Add("txt", typeof(TextAsset));
			AssetDatabase2._extensionToAssetType.Add("html", typeof(TextAsset));
			AssetDatabase2._extensionToAssetType.Add("htm", typeof(TextAsset));
			AssetDatabase2._extensionToAssetType.Add("xml", typeof(TextAsset));
			AssetDatabase2._extensionToAssetType.Add("bytes", typeof(TextAsset));
			AssetDatabase2._extensionToAssetType.Add("json", typeof(TextAsset));
			AssetDatabase2._extensionToAssetType.Add("csv", typeof(TextAsset));
			AssetDatabase2._extensionToAssetType.Add("yaml", typeof(TextAsset));
			AssetDatabase2._extensionToAssetType.Add("fnt", typeof(TextAsset));
			AssetDatabase2._extensionToAssetType.Add("ttf", typeof(Font));
			AssetDatabase2._extensionToAssetType.Add("otf", typeof(Font));
			AssetDatabase2._extensionToAssetType.Add("dfont", typeof(Font));
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000089F4 File Offset: 0x00006BF4
		public static string GetRelativeAssetPath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return "";
			}
			string text = path.Replace('\\', '/');
			if (text.StartsWith(Application.dataPath))
			{
				return text.Substring(Application.dataPath.Length - "Assets".Length);
			}
			return path;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00008A44 File Offset: 0x00006C44
		public static string GUIDToLibraryPath(string assetGuid)
		{
			if (string.IsNullOrEmpty(assetGuid) || assetGuid.Length < 2)
			{
				return "";
			}
			string text = assetGuid.Substring(0, 2);
			string text2 = Application.dataPath.Substring(0, Application.dataPath.Length - "/Assets".Length);
			return string.Concat(new string[]
			{
				text2,
				"/Library/metadata/",
				text,
				"/",
				assetGuid
			});
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00008AC0 File Offset: 0x00006CC0
		public static long GetStorageSize(string assetPath)
		{
			string text = AssetDatabase.AssetPathToGUID(assetPath);
			if (string.IsNullOrEmpty(text) || text.Length < 2)
			{
				return -1L;
			}
			assetPath = AssetDatabase2.GUIDToLibraryPath(text);
			if (File.Exists(assetPath))
			{
				return new FileInfo(assetPath).Length;
			}
			return -1L;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00008B06 File Offset: 0x00006D06
		public static string GetTextMetaFilePathFromAssetPath(string assetPath)
		{
			return AssetDatabase.GetTextMetaFilePathFromAssetPath(assetPath);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00008B10 File Offset: 0x00006D10
		public static DateTime GetAssetLastWriteTime(string assetPath)
		{
			DateTime dateTime = DateTime.MinValue;
			try
			{
				dateTime = File.GetLastWriteTime(assetPath);
			}
			catch
			{
				dateTime = DateTime.MinValue;
			}
			string textMetaFilePathFromAssetPath = AssetDatabase2.GetTextMetaFilePathFromAssetPath(assetPath);
			if (string.IsNullOrEmpty(textMetaFilePathFromAssetPath))
			{
				return DateTime.MinValue;
			}
			DateTime dateTime2;
			try
			{
				dateTime2 = File.GetLastWriteTime(textMetaFilePathFromAssetPath);
			}
			catch
			{
				dateTime2 = DateTime.MinValue;
			}
			DateTime now = DateTime.Now;
			TimeSpan t = now - dateTime;
			TimeSpan t2 = now - dateTime2;
			if (t < t2)
			{
				return dateTime;
			}
			return dateTime2;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00008BA0 File Offset: 0x00006DA0
		public static bool IsAssetType(string assetPath, Type type)
		{
			return AssetDatabase2.GetAssetType(assetPath) == type;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00008BAC File Offset: 0x00006DAC
		public static Type GetAssetType(string assetPath)
		{
			string fileExtension = FileUtil2.GetFileExtension(assetPath);
			Type result;
			if (!string.IsNullOrEmpty(fileExtension) && AssetDatabase2._extensionToAssetType.TryGetValue(fileExtension, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00008BDC File Offset: 0x00006DDC
		public static List<string> GetPathsByType(Type type)
		{
			return AssetDatabase2.GetPathsByType(new Type[]
			{
				type
			});
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00008BFC File Offset: 0x00006DFC
		public static List<string> GetPathsByType(IEnumerable<Type> types)
		{
			string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
			List<string> list = new List<string>(128);
			foreach (string text in allAssetPaths)
			{
				foreach (Type type in types)
				{
					if (AssetDatabase2.IsAssetType(text, type))
					{
						list.Add(text);
						break;
					}
				}
			}
			return list;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00008C84 File Offset: 0x00006E84
		public static bool Reimport(List<string> paths, int confirmcount)
		{
			return AssetDatabase2.Reimport(paths, confirmcount, 0);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00008C90 File Offset: 0x00006E90
		public static bool Reimport(List<string> paths, int confirmcount, ImportAssetOptions importoptions)
		{
			if (confirmcount > 0 && paths.Count >= confirmcount)
			{
				StringBuilder stringBuilder = new StringBuilder(128);
				int num = 0;
				foreach (string str in paths)
				{
					if (num < 3)
					{
						stringBuilder.AppendLine("'" + str + "'");
					}
					else if (num == 3)
					{
						stringBuilder.AppendLine("...");
					}
					num++;
				}
				string text = "Reimport asset?";
				if (paths.Count > 1)
				{
					text = string.Format("Reimport {0} assets?", paths.Count);
				}
				if (!EditorUtility.DisplayDialog(text, stringBuilder.ToString(), "Reimport", "Cancel"))
				{
					return false;
				}
			}
			foreach (string text2 in paths)
			{
				AssetDatabase.ImportAsset(text2, importoptions);
			}
			AssetDatabase.Refresh();
			return true;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00008DB0 File Offset: 0x00006FB0
		public static void Delete(List<string> paths)
		{
			if (paths == null || paths.Count == 0)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder(128);
			int num = 0;
			foreach (string str in paths)
			{
				if (num < 3)
				{
					stringBuilder.AppendLine("'" + str + "'");
				}
				else if (num == 3)
				{
					stringBuilder.AppendLine("...");
				}
				num++;
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("You cannot undo this action.");
			string text = "Delete asset?";
			if (paths.Count > 1)
			{
				text = string.Format("Delete {0} assets?", paths.Count);
			}
			if (!EditorUtility.DisplayDialog(text, stringBuilder.ToString(), "Delete", "Cancel"))
			{
				return;
			}
			using (EditorGUI2.ModalProgressBar modalProgressBar = new EditorGUI2.ModalProgressBar("Deleting...", true))
			{
				for (int i = 0; i < paths.Count; i++)
				{
					string text2 = paths[i];
					if (paths.Count > 10)
					{
						float progress = (float)i / (float)paths.Count;
						if (modalProgressBar.Update(text2, progress))
						{
							break;
						}
					}
					AssetDatabase.DeleteAsset(text2);
				}
			}
			AssetDatabase.Refresh();
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00008F0C File Offset: 0x0000710C
		public static List<string> GetPathsInDirectory(string directory, SearchOption searchOption)
		{
			directory = directory.Replace('\\', '/');
			if (directory[directory.Length - 1] != '/')
			{
				directory += '/';
			}
			List<string> list = new List<string>(64);
			foreach (string text in AssetDatabase.GetAllAssetPaths())
			{
				if (searchOption == SearchOption.AllDirectories)
				{
					if (text.StartsWith(directory, StringComparison.OrdinalIgnoreCase))
					{
						list.Add(text);
					}
				}
				else if (string.Equals(directory, FileUtil2.GetDirectoryName(text) + '/', StringComparison.OrdinalIgnoreCase))
				{
					list.Add(text);
				}
			}
			return list;
		}

		// Token: 0x0400009D RID: 157
		private static Dictionary<string, Type> _extensionToAssetType = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x02000019 RID: 25
		public class UnityScene
		{
		}

		// Token: 0x0200001A RID: 26
		public class UnityAsset
		{
		}
	}
}
