using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000022 RID: 34
	public class BuildPlayerWindow2
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00009744 File Offset: 0x00007944
		static BuildPlayerWindow2()
		{
			if (BuildPlayerWindow2._type != null)
			{
				BuildPlayerWindow2._getValidPlatforms = BuildPlayerWindow2._type.GetMethod("GetValidPlatforms", BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
			}
			if (BuildPlayerWindow2._type == null)
			{
				Debug.LogWarning("Could not find type 'UnityEditor.Build.BuildPlatforms'.");
			}
			if (BuildPlayerWindow2._getValidPlatforms == null)
			{
				Debug.LogWarning("Could not find method 'UnityEditor.Build.BuildPlatforms.GetValidPlatforms'.");
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000097B8 File Offset: 0x000079B8
		public static BuildPlatform2 FindPlatform(string name)
		{
			List<BuildPlatform2> validPlatforms = BuildPlayerWindow2.GetValidPlatforms();
			for (int i = 0; i < validPlatforms.Count; i++)
			{
				if (string.Equals(name, validPlatforms[i].Name, StringComparison.OrdinalIgnoreCase))
				{
					return validPlatforms[i];
				}
			}
			return null;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000097FC File Offset: 0x000079FC
		public static Texture2D GetPlatformImage(BuildTargetGroup platform)
		{
			if (BuildPlayerWindow2._platformImages == null)
			{
				BuildPlayerWindow2.GetValidPlatforms();
				BuildPlayerWindow2._platformImages = new Texture2D[32];
				foreach (BuildPlatform2 buildPlatform in BuildPlayerWindow2._validplatforms)
				{
					BuildPlayerWindow2._platformImages[(int)buildPlatform.TargetGroup] = (buildPlatform.Image as Texture2D);
				}
			}
			if (platform < 0 && (int)platform >= BuildPlayerWindow2._platformImages.Length)
			{
				return null;
			}
			return BuildPlayerWindow2._platformImages[(int)platform];
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00009890 File Offset: 0x00007A90
		public static List<BuildTargetGroup> GetValidPlatformGroups()
		{
			if (BuildPlayerWindow2._validplatformgroups != null)
			{
				return BuildPlayerWindow2._validplatformgroups;
			}
			BuildPlayerWindow2._validplatformgroups = new List<BuildTargetGroup>();
			foreach (BuildPlatform2 buildPlatform in BuildPlayerWindow2.GetValidPlatforms())
			{
				BuildPlayerWindow2._validplatformgroups.Add(buildPlatform.TargetGroup);
			}
			return BuildPlayerWindow2._validplatformgroups;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00009908 File Offset: 0x00007B08
		public static List<BuildPlatform2> GetValidPlatforms()
		{
			if (BuildPlayerWindow2._validplatforms != null)
			{
				return new List<BuildPlatform2>(BuildPlayerWindow2._validplatforms);
			}
			List<BuildPlatform2> list = new List<BuildPlatform2>();
			if (BuildPlayerWindow2._getValidPlatforms == null)
			{
				return list;
			}
			object value = BuildPlayerWindow2._type.GetProperty("instance", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
			IEnumerable enumerable = BuildPlayerWindow2._getValidPlatforms.Invoke(value, null) as IEnumerable;
			if (enumerable == null)
			{
				return list;
			}
			foreach (object obj in enumerable)
			{
				BuildPlatform2 buildPlatform = new BuildPlatform2();
				Type type = obj.GetType();
				FieldInfo field = type.GetField("name");
				if (field != null)
				{
					buildPlatform.Name = (field.GetValue(obj) as string);
				}
				FieldInfo field2 = type.GetField("targetGroup");
				if (field2 != null)
				{
					buildPlatform.TargetGroup = (BuildTargetGroup)field2.GetValue(obj);
				}
				PropertyInfo property = type.GetProperty("DefaultTarget");
				if (property != null)
				{
					buildPlatform.DefaultTarget = (BuildTarget)property.GetValue(obj, null);
				}
				FieldInfo field3 = type.GetField("title");
				if (field3 != null)
				{
					GUIContent guicontent = field3.GetValue(obj) as GUIContent;
					if (guicontent != null)
					{
						buildPlatform.DisplayName = guicontent.text;
						buildPlatform.Image = guicontent.image;
						buildPlatform.SmallIcon = guicontent.image;
					}
				}
				FieldInfo field4 = type.GetField("smallIcon");
				if (field4 != null)
				{
					buildPlatform.SmallIcon = (field4.GetValue(obj) as Texture2D);
				}
				list.Add(buildPlatform);
			}
			BuildPlayerWindow2._validplatforms = new List<BuildPlatform2>(list);
			return list;
		}

		// Token: 0x040000B5 RID: 181
		private static Type _type = typeof(BuildFailedException).Assembly.GetType("UnityEditor.Build.BuildPlatforms");

		// Token: 0x040000B6 RID: 182
		private static MethodInfo _getValidPlatforms;

		// Token: 0x040000B7 RID: 183
		private static List<BuildPlatform2> _validplatforms;

		// Token: 0x040000B8 RID: 184
		private static Texture2D[] _platformImages;

		// Token: 0x040000B9 RID: 185
		private static List<BuildTargetGroup> _validplatformgroups;
	}
}
