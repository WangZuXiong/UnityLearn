using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EditorFramework
{
	// Token: 0x02000030 RID: 48
	public static class FindAssetUsage
	{
		// Token: 0x06000186 RID: 390 RVA: 0x0000B7BC File Offset: 0x000099BC
		public static FindAssetUsage.Result InProject(IEnumerable<string> findpaths, IEnumerable<Type> findtypes)
		{
			EditorUtility2.UnloadUnusedAssetsImmediate();
			Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
			foreach (string text in findpaths)
			{
				if (!string.IsNullOrEmpty(text) && !dictionary.ContainsKey(text))
				{
					dictionary.Add(text, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
				}
			}
			string text2 = "";
			foreach (Type type in findtypes)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					text2 += ", ";
				}
				text2 += type.Name;
			}
			using (EditorGUI2.ModalProgressBar modalProgressBar = new EditorGUI2.ModalProgressBar(string.Format("Searching {0} assets...", text2), true))
			{
				List<string> pathsByType = AssetDatabase2.GetPathsByType(findtypes);
				for (int i = 0; i < pathsByType.Count; i++)
				{
					string text3 = pathsByType[i];
					if (modalProgressBar.TotalElapsedTime > 1f && modalProgressBar.ElapsedTime > 0.1f)
					{
						float progress = (float)i / (float)pathsByType.Count;
						string text4 = string.Format("[{1} remaining] {0}", FileUtil2.GetFileNameWithoutExtension(text3), pathsByType.Count - i - 1);
						if (modalProgressBar.Update(text4, progress))
						{
							break;
						}
					}
					string[] dependencies = AssetDatabase.GetDependencies(new string[]
					{
						text3
					});
					foreach (string text5 in dependencies)
					{
						Dictionary<string, string> dictionary2;
						if (!string.Equals(text5, text3, StringComparison.OrdinalIgnoreCase) && dictionary.TryGetValue(text5, out dictionary2))
						{
							dictionary2[text3] = text3;
						}
					}
				}
			}
			FindAssetUsage.Result result = new FindAssetUsage.Result();
			foreach (KeyValuePair<string, Dictionary<string, string>> keyValuePair in dictionary)
			{
				FindAssetUsage.ResultEntry resultEntry = new FindAssetUsage.ResultEntry();
				resultEntry.AssetPath = keyValuePair.Key;
				foreach (KeyValuePair<string, string> keyValuePair2 in keyValuePair.Value)
				{
					resultEntry.Add(keyValuePair2.Key);
				}
				result.Entries.Add(resultEntry);
			}
			return result;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000BA4C File Offset: 0x00009C4C
		public static FindAssetUsage.Result InProject(IEnumerable<Object> findobjs, IEnumerable<Type> findtypes)
		{
			EditorUtility2.UnloadUnusedAssetsImmediate();
			Dictionary<Object, Dictionary<string, string>> dictionary = new Dictionary<Object, Dictionary<string, string>>();
			foreach (Object @object in findobjs)
			{
				if (@object != null && !dictionary.ContainsKey(@object))
				{
					dictionary.Add(@object, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
				}
			}
			string text = "";
			foreach (Type type in findtypes)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ", ";
				}
				text += type.Name;
			}
			using (EditorGUI2.ModalProgressBar modalProgressBar = new EditorGUI2.ModalProgressBar(string.Format("Searching {0} assets...", text), true))
			{
				List<string> pathsByType = AssetDatabase2.GetPathsByType(findtypes);
				for (int i = 0; i < pathsByType.Count; i++)
				{
					string text2 = pathsByType[i];
					if (modalProgressBar.TotalElapsedTime > 1f && modalProgressBar.ElapsedTime > 0.1f)
					{
						float progress = (float)i / (float)pathsByType.Count;
						string text3 = string.Format("[{1} remaining] {0}", FileUtil2.GetFileNameWithoutExtension(text2), pathsByType.Count - i - 1);
						if (modalProgressBar.Update(text3, progress))
						{
							break;
						}
					}
					Object object2 = AssetDatabase.LoadMainAssetAtPath(text2);
					Object[] array = EditorUtility.CollectDependencies(new Object[]
					{
						object2
					});
					foreach (Object object3 in array)
					{
						Dictionary<string, string> dictionary2;
						if (!(object3 == null) && dictionary.TryGetValue(object3, out dictionary2))
						{
							dictionary2[text2] = text2;
						}
					}
					if (i % 25 == 0)
					{
						EditorUtility2.UnloadUnusedAssetsImmediate();
					}
				}
			}
			FindAssetUsage.Result result = new FindAssetUsage.Result();
			foreach (KeyValuePair<Object, Dictionary<string, string>> keyValuePair in dictionary)
			{
				FindAssetUsage.ResultEntry resultEntry = new FindAssetUsage.ResultEntry();
				resultEntry.Asset = keyValuePair.Key;
				foreach (KeyValuePair<string, string> keyValuePair2 in keyValuePair.Value)
				{
					resultEntry.Add(keyValuePair2.Key);
				}
				result.Entries.Add(resultEntry);
			}
			return result;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000BCF0 File Offset: 0x00009EF0
		public static FindAssetUsage.Result InProject(IEnumerable<Type> findobjs, IEnumerable<Type> findtypes)
		{
			EditorUtility2.UnloadUnusedAssetsImmediate();
			Dictionary<Type, Dictionary<string, string>> dictionary = new Dictionary<Type, Dictionary<string, string>>();
			foreach (Type type in findobjs)
			{
				if (type != null && !dictionary.ContainsKey(type))
				{
					dictionary.Add(type, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
				}
			}
			string text = "";
			foreach (Type type2 in findtypes)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ", ";
				}
				text += type2.Name;
			}
			using (EditorGUI2.ModalProgressBar modalProgressBar = new EditorGUI2.ModalProgressBar(string.Format("Searching {0} assets...", text), true))
			{
				List<string> pathsByType = AssetDatabase2.GetPathsByType(findtypes);
				for (int i = 0; i < pathsByType.Count; i++)
				{
					string text2 = pathsByType[i];
					if (modalProgressBar.TotalElapsedTime > 1f && modalProgressBar.ElapsedTime > 0.1f)
					{
						float progress = (float)i / (float)pathsByType.Count;
						string text3 = string.Format("[{1} remaining] {0}", FileUtil2.GetFileNameWithoutExtension(text2), pathsByType.Count - i - 1);
						if (modalProgressBar.Update(text3, progress))
						{
							break;
						}
					}
					Object @object = AssetDatabase.LoadMainAssetAtPath(text2);
					Object[] array = EditorUtility.CollectDependencies(new Object[]
					{
						@object
					});
					foreach (Object object2 in array)
					{
						Dictionary<string, string> dictionary2;
						if (!(object2 == null) && dictionary.TryGetValue(object2.GetType(), out dictionary2))
						{
							dictionary2[text2] = text2;
						}
					}
					if (i % 25 == 0)
					{
						EditorUtility2.UnloadUnusedAssetsImmediate();
					}
				}
			}
			FindAssetUsage.Result result = new FindAssetUsage.Result();
			foreach (KeyValuePair<Type, Dictionary<string, string>> keyValuePair in dictionary)
			{
				FindAssetUsage.ResultEntry resultEntry = new FindAssetUsage.ResultEntry();
				resultEntry.AssetType = keyValuePair.Key;
				resultEntry.AssetPath = keyValuePair.Key.Name;
				foreach (KeyValuePair<string, string> keyValuePair2 in keyValuePair.Value)
				{
					resultEntry.Add(keyValuePair2.Key);
				}
				result.Entries.Add(resultEntry);
			}
			return result;
		}

		// Token: 0x02000031 RID: 49
		[Serializable]
		public class AssetProxy
		{
			// Token: 0x17000033 RID: 51
			// (get) Token: 0x06000189 RID: 393 RVA: 0x0000BFAC File Offset: 0x0000A1AC
			public string Name
			{
				get
				{
					if (this._name == null)
					{
						if (this.Asset != null)
						{
							this._name = this.Asset.name;
						}
						if (this.AssetPath != null)
						{
							this._name = FileUtil2.GetFileNameWithoutExtension(this.AssetPath);
						}
					}
					return this._name ?? "<unnamed>";
				}
			}

			// Token: 0x0600018A RID: 394 RVA: 0x0000C008 File Offset: 0x0000A208
			public Object LoadAsset()
			{
				if (this.Asset != null)
				{
					return this.Asset;
				}
				if (this.AssetPath != null)
				{
					return AssetDatabase.LoadMainAssetAtPath(this.AssetPath);
				}
				return null;
			}

			// Token: 0x040000D6 RID: 214
			private string _name;

			// Token: 0x040000D7 RID: 215
			[SerializeField]
			public Object Asset;

			// Token: 0x040000D8 RID: 216
			[SerializeField]
			public string AssetPath;

			// Token: 0x040000D9 RID: 217
			[SerializeField]
			public Type AssetType;
		}

		// Token: 0x02000032 RID: 50
		[Serializable]
		public class ResultEntry : FindAssetUsage.AssetProxy
		{
			// Token: 0x0600018C RID: 396 RVA: 0x0000C03C File Offset: 0x0000A23C
			public void Add(string path)
			{
				FindAssetUsage.AssetProxy assetProxy = new FindAssetUsage.AssetProxy();
				assetProxy.AssetPath = path;
				this.Findings.Add(assetProxy);
			}

			// Token: 0x0600018D RID: 397 RVA: 0x0000C064 File Offset: 0x0000A264
			public void Add(Object obj)
			{
				FindAssetUsage.AssetProxy assetProxy = new FindAssetUsage.AssetProxy();
				assetProxy.Asset = obj;
				this.Findings.Add(assetProxy);
			}

			// Token: 0x040000DA RID: 218
			[SerializeField]
			public List<FindAssetUsage.AssetProxy> Findings = new List<FindAssetUsage.AssetProxy>();
		}

		// Token: 0x02000033 RID: 51
		[Serializable]
		public class Result
		{
			// Token: 0x040000DB RID: 219
			[SerializeField]
			public List<FindAssetUsage.ResultEntry> Entries = new List<FindAssetUsage.ResultEntry>();
		}
	}
}
