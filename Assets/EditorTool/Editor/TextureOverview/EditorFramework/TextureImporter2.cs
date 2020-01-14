using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000079 RID: 121
	public static class TextureImporter2
	{
		// Token: 0x060003BB RID: 955 RVA: 0x00033394 File Offset: 0x00031594
		static TextureImporter2()
		{
			if (TextureImporter2._type == null)
			{
				Debug.LogWarning("Could not find type 'TextureImporter'.");
				return;
			}
			TextureImporter2._getWidthAndHeight = TextureImporter2._type.GetMethod("GetWidthAndHeight", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
			{
				typeof(int).MakeByRefType(),
				typeof(int).MakeByRefType()
			}, null);
			if (TextureImporter2._getWidthAndHeight == null)
			{
				Debug.LogWarning("Could not find method 'TextureImporter.GetWidthAndHeight(ref int, ref int)'.");
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00033428 File Offset: 0x00031628
		public static void GetWidthAndHeight(TextureImporter importer, ref int width, ref int height)
		{
			if (TextureImporter2._getWidthAndHeight == null)
			{
				return;
			}
			object[] array = new object[]
			{
				width,
				height
			};
			TextureImporter2._getWidthAndHeight.Invoke(importer, array);
			width = (int)array[0];
			height = (int)array[1];
		}

		// Token: 0x0400023B RID: 571
		private static Type _type = typeof(TextureImporter).Assembly.GetType("UnityEditor.TextureImporter");

		// Token: 0x0400023C RID: 572
		private static MethodInfo _getWidthAndHeight;
	}
}
