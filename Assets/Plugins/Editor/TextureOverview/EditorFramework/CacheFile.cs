using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000024 RID: 36
	public class CacheFile<T> where T : ICacheFileEntry, new()
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00009AD0 File Offset: 0x00007CD0
		public bool IsEmpty
		{
			get
			{
				return this._lut.Count == 0;
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00009AE0 File Offset: 0x00007CE0
		public CacheFile(uint fileVersion, string appTitle, string cacheFileName)
		{
			this._fileVersion = fileVersion;
			this._appTitle = appTitle;
			this.MoveCacheFileToLibrary(cacheFileName);
			this._cachePath = Path.Combine(EditorApplication2.LibraryPath, cacheFileName + ".cache");
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00009B30 File Offset: 0x00007D30
		public void Read()
		{
			this._isDirty = false;
			this._lut = new Dictionary<string, T>();
			try
			{
				if (FileUtil2.Exists(this._cachePath))
				{
					if (FileUtil2.IsReadOnly(this._cachePath))
					{
						this.ShowCacheFileReadOnlyMessage(false);
					}
					using (MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(this._cachePath)))
					{
						using (BinaryReader binaryReader = new BinaryReader(memoryStream))
						{
							uint num = binaryReader.ReadUInt32();
							if (num != 322416638u)
							{
								Debug.Log(string.Format("{0}: Cache file '{1}' contains an invalid header magic.", this._appTitle, this._cachePath));
							}
							else
							{
								int num2 = binaryReader.ReadInt32();
								if ((long)num2 != (long)((ulong)this._fileVersion))
								{
									Debug.Log(string.Format("{0}: Incompatible cache file detected, generating new one. ('{1}').\nThe most likely reason is you upgraded the plugin to a newer version. Existing cache file version is '{2}', required version is '{3}'.", new object[]
									{
										this._appTitle,
										this._cachePath,
										num2,
										this._fileVersion
									}));
								}
								else
								{
									BinarySerializer data = new BinarySerializer(binaryReader);
									int num3 = binaryReader.ReadInt32();
									for (int i = 0; i < num3; i++)
									{
										T value = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
										value.Serialize(data);
										string assetGuid = value.GetAssetGuid();
										this._lut[assetGuid] = value;
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Format("{0}: Could not read cache file '{1}'.\n{2}", this._appTitle, this._cachePath, ex.ToString()));
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00009D18 File Offset: 0x00007F18
		public bool Delete()
		{
			if (File.Exists(this._cachePath))
			{
				new FileInfo(this._cachePath)
				{
					IsReadOnly = false
				}.Delete();
				Debug.Log(string.Format("{0}: Deleted cache file '{1}'.", this._appTitle, this._cachePath));
				return true;
			}
			return false;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00009D69 File Offset: 0x00007F69
		public void Write()
		{
			this.Write(false);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00009D74 File Offset: 0x00007F74
		public void Write(bool writeAlways)
		{
			if (!writeAlways && !this._isDirty)
			{
				return;
			}
			if (FileUtil2.Exists(this._cachePath))
			{
				if (FileUtil2.IsReadOnly(this._cachePath))
				{
					this.ShowCacheFileReadOnlyMessage(true);
				}
				if (FileUtil2.IsReadOnly(this._cachePath))
				{
					return;
				}
			}
			using (MemoryStream memoryStream = new MemoryStream(1048576))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write(322416638u);
					binaryWriter.Write(this._fileVersion);
					List<T> list = new List<T>(this._lut.Values);
					binaryWriter.Write(list.Count);
					BinarySerializer data = new BinarySerializer(binaryWriter);
					foreach (T t in list)
					{
						t.Serialize(data);
					}
				}
				File.WriteAllBytes(this._cachePath, memoryStream.ToArray());
				this._isDirty = false;
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00009E9C File Offset: 0x0000809C
		public bool TryGetEntry(string assetguid, out T model)
		{
			return this.TryGetEntry(assetguid, out model, true);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00009EA8 File Offset: 0x000080A8
		public bool TryGetEntry(string assetguid, out T model, bool checkTimeStamp)
		{
			model = default(T);
			if (!this._lut.TryGetValue(assetguid, out model))
			{
				return false;
			}
			if (checkTimeStamp)
			{
				string text = AssetDatabase.GUIDToAssetPath(assetguid);
				Hash128 assetDependencyHash = AssetDatabase.GetAssetDependencyHash(text);
				Hash128 hash = Hash128.Parse(model.GetAssetHash());
				if (assetDependencyHash != hash)
				{
					this._lut.Remove(assetguid);
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00009F0C File Offset: 0x0000810C
		public void UpdateEntry(T model)
		{
			string assetGuid = model.GetAssetGuid();
			this._lut[assetGuid] = model;
			this._isDirty = true;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00009F3C File Offset: 0x0000813C
		private void ShowCacheFileReadOnlyMessage(bool messagebox)
		{
			string text = string.Format("Cache file is not writable ({0}).\n\nThe cache file stores data to improve the plugin performance, especially the startup time.\n\nDo not add this file to revision control software like SVN, Perforce or Git.", AssetDatabase2.GetRelativeAssetPath(this._cachePath));
			if (messagebox)
			{
				if (EditorUtility.DisplayDialog(this._appTitle, text, "Make writable and save", "Ignore"))
				{
					FileInfo fileInfo = new FileInfo(this._cachePath);
					fileInfo.IsReadOnly = false;
					Debug.Log(string.Format("{0}: Sucessfully removed read-only attribute of '{1}'.", this._appTitle, AssetDatabase2.GetRelativeAssetPath(this._cachePath)));
					return;
				}
			}
			else
			{
				Debug.LogWarning(string.Format("{0}: {1}", this._appTitle, text));
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00009FC4 File Offset: 0x000081C4
		private void MoveCacheFileToLibrary(string cacheFileName)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string directoryName = Path.GetDirectoryName(executingAssembly.Location);
			string text = Path.Combine(directoryName, cacheFileName + ".cache");
			if (File.Exists(text))
			{
				string text2 = Path.Combine(EditorApplication2.LibraryPath, cacheFileName + ".cache");
				if (File.Exists(text2))
				{
					FileInfo fileInfo = new FileInfo(text2);
					fileInfo.IsReadOnly = false;
				}
				File.Copy(text, text2, true);
				new FileInfo(text)
				{
					IsReadOnly = false
				}.Delete();
				AssetDatabase.Refresh();
			}
		}

		// Token: 0x040000BA RID: 186
		private const uint FileMagic = 322416638u;

		// Token: 0x040000BB RID: 187
		private uint _fileVersion;

		// Token: 0x040000BC RID: 188
		private string _appTitle;

		// Token: 0x040000BD RID: 189
		private string _cachePath;

		// Token: 0x040000BE RID: 190
		private Dictionary<string, T> _lut = new Dictionary<string, T>();

		// Token: 0x040000BF RID: 191
		private bool _isDirty;
	}
}
