using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EditorFramework
{
	// Token: 0x02000027 RID: 39
	public class EditorApplication2
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000130 RID: 304 RVA: 0x0000A110 File Offset: 0x00008310
		// (remove) Token: 0x06000131 RID: 305 RVA: 0x0000A144 File Offset: 0x00008344
		public static event EditorApplication2.PluginDestroyDelegate PluginDestroy;

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000132 RID: 306 RVA: 0x0000A178 File Offset: 0x00008378
		public static string ProjectName
		{
			get
			{
				string projectPath = EditorApplication2.ProjectPath;
				return FileUtil2.GetFileNameWithoutExtension(projectPath);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000133 RID: 307 RVA: 0x0000A194 File Offset: 0x00008394
		public static string ProjectPath
		{
			get
			{
				string text = Application.dataPath;
				if (text.EndsWith("/Assets/", StringComparison.OrdinalIgnoreCase))
				{
					text = text.Substring(0, text.Length - "/Assets/".Length);
				}
				else if (text.EndsWith("/Assets", StringComparison.OrdinalIgnoreCase))
				{
					text = text.Substring(0, text.Length - "/Assets".Length);
				}
				return text;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000134 RID: 308 RVA: 0x0000A1F8 File Offset: 0x000083F8
		public static string LibraryPath
		{
			get
			{
				return EditorApplication2.ProjectPath + "/Library";
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000135 RID: 309 RVA: 0x0000A209 File Offset: 0x00008409
		// (set) Token: 0x06000136 RID: 310 RVA: 0x0000A210 File Offset: 0x00008410
		public static int MajorVersion { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000137 RID: 311 RVA: 0x0000A218 File Offset: 0x00008418
		// (set) Token: 0x06000138 RID: 312 RVA: 0x0000A21F File Offset: 0x0000841F
		public static int MinorVersion { get; private set; }

		// Token: 0x06000139 RID: 313 RVA: 0x0000A228 File Offset: 0x00008428
		static EditorApplication2()
		{
			string[] array = Application.unityVersion.Split(new char[]
			{
				'.'
			});
			EditorApplication2.MajorVersion = int.Parse(array[0]);
			EditorApplication2.MinorVersion = int.Parse(array[1]);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000A267 File Offset: 0x00008467
		public static bool IsVersionOrHigher(int major, int minor)
		{
			return EditorApplication2.MajorVersion > major || (EditorApplication2.MajorVersion == major && EditorApplication2.MinorVersion >= minor);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000A288 File Offset: 0x00008488
		public static void OnPluginDestroy()
		{
			if (EditorApplication2.PluginDestroy != null)
			{
				EditorApplication2.PluginDestroyDelegate pluginDestroy = EditorApplication2.PluginDestroy;
				pluginDestroy();
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000A2A8 File Offset: 0x000084A8
		public static void FrameObject(Object obj)
		{
			if (obj == null)
			{
				return;
			}
			Object activeObject = Selection.activeObject;
			Object[] objects = Selection.objects;
			try
			{
				Selection.activeObject = obj;
				Selection.objects = new Object[]
				{
					obj
				};
				if (null != SceneView.lastActiveSceneView)
				{
					SceneView.lastActiveSceneView.FrameSelected();
				}
			}
			finally
			{
				Selection.activeObject = activeObject;
				Selection.objects = objects;
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000A31C File Offset: 0x0000851C
		public static void FindReferencesInScene(Object obj)
		{
			if (null == obj)
			{
				Debug.LogError(string.Format("FindReferencesInScene: The specified obj is null.", new object[0]));
				return;
			}
			string text = AssetDatabase.GetAssetPath(obj);
			string filter;
			if (string.IsNullOrEmpty(text))
			{
				filter = string.Format("ref:{0}:\"\"", obj.GetInstanceID());
			}
			else
			{
				if (text.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase))
				{
					text = text.Substring("Assets/".Length);
				}
				if (AssetDatabase.IsMainAsset(obj))
				{
					filter = string.Format("ref:\"{0}\"", text);
				}
				else
				{
					filter = string.Format("ref:{0}:\"{1}\"", obj.GetInstanceID(), text);
				}
			}
			SearchableEditorWindow2.SetHierarchySearchFilter(filter, 0);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000A3C8 File Offset: 0x000085C8
		public static void OpenPdfManual()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string location = executingAssembly.Location;
			string directoryName = Path.GetDirectoryName(executingAssembly.Location);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(executingAssembly.Location);
			string text = Path.Combine(directoryName, fileNameWithoutExtension + ".pdf");
			if (!File.Exists(text))
			{
				EditorUtility.DisplayDialog("", string.Format("Could not find file '{0}'", text), "OK");
				return;
			}
			EditorUtility.OpenWithDefaultApp(text);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000A438 File Offset: 0x00008638
		public static string CombinePluginPath(string fileName)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string location = executingAssembly.Location;
			string directoryName = Path.GetDirectoryName(executingAssembly.Location);
			Path.GetFileNameWithoutExtension(executingAssembly.Location);
			return Path.Combine(directoryName, fileName);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000A473 File Offset: 0x00008673
		public static void OpenAssets(Object[] objects)
		{
			EditorApplication2.OpenAssets(objects, 3);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000A47C File Offset: 0x0000867C
		public static void OpenAssets(Object[] objects, int dialogthreshold)
		{
			List<string> list = new List<string>();
			foreach (Object @object in objects)
			{
				string assetPath = AssetDatabase.GetAssetPath(@object);
				if (!string.IsNullOrEmpty(assetPath))
				{
					list.Add(assetPath);
				}
			}
			EditorApplication2.OpenAssets(list.ToArray(), dialogthreshold);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000A4CA File Offset: 0x000086CA
		public static void OpenAssets(string[] paths)
		{
			EditorApplication2.OpenAssets(paths, 3);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000A4D4 File Offset: 0x000086D4
		public static void OpenAssets(string[] paths, int dialogthreshold)
		{
			if (paths == null || paths.Length == 0)
			{
				return;
			}
			if (dialogthreshold > 0 && paths.Length > dialogthreshold && !EditorUtility.DisplayDialog("Open Assets", string.Format("You are about to open {0} assets, do you want to continue?", paths.Length), "Open", "Cancel"))
			{
				return;
			}
			foreach (string text in paths)
			{
				EditorUtility.OpenWithDefaultApp(text);
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000A537 File Offset: 0x00008737
		public static void ShowInExplorer(string[] assetPaths)
		{
			EditorApplication2.ShowInExplorer(assetPaths, 3);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000A540 File Offset: 0x00008740
		public static void ShowInExplorer(string[] assetPaths, int dialogthreshold)
		{
			if (assetPaths == null || assetPaths.Length == 0)
			{
				return;
			}
			string text = (Application.platform == null) ? "Reveal in Finder" : "Show in Explorer";
			if (dialogthreshold > 0 && assetPaths.Length > dialogthreshold && !EditorUtility.DisplayDialog(text, string.Format("You are about to open {0} windows, do you want to continue?", assetPaths.Length), "Continue", "Cancel"))
			{
				return;
			}
			foreach (string text2 in assetPaths)
			{
				EditorUtility.RevealInFinder(text2);
			}
		}

		// Token: 0x02000028 RID: 40
		// (Invoke) Token: 0x06000148 RID: 328
		public delegate void PluginDestroyDelegate();
	}
}
