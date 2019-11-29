using System;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000081 RID: 129
	public static class TextureUtil2
	{
		// Token: 0x060003D6 RID: 982 RVA: 0x00034550 File Offset: 0x00032750
		static TextureUtil2()
		{
			if (TextureUtil2._type != null)
			{
				TextureUtil2._getStorageMemorySize = TextureUtil2._type.GetMethod("GetStorageMemorySize", BindingFlags.Static | BindingFlags.Public);
				TextureUtil2._getGLWidth = TextureUtil2._type.GetMethod("GetGPUWidth", BindingFlags.Static | BindingFlags.Public, null, new Type[]
				{
					typeof(Texture)
				}, null);
				TextureUtil2._getGLHeight = TextureUtil2._type.GetMethod("GetGPUHeight", BindingFlags.Static | BindingFlags.Public, null, new Type[]
				{
					typeof(Texture)
				}, null);
			}
			if (TextureUtil2._type == null)
			{
				Debug.LogWarning("Could not find type 'TextureUtil'.");
			}
			if (TextureUtil2._getStorageMemorySize == null)
			{
				Debug.LogWarning("Could not find method 'TextureUtil.GetStorageMemorySize'.");
			}
			if (TextureUtil2._getGLWidth == null)
			{
				Debug.LogWarning("Could not find method 'TextureUtil.GetGLWidth(Texture)' or 'TextureUtil.GetGPUWidth(Texture)'.");
			}
			if (TextureUtil2._getGLHeight == null)
			{
				Debug.LogWarning("Could not find method 'TextureUtil.GetGLHeight(Texture)' or 'TextureUtil.GetGPUHeight(Texture)'.");
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00034638 File Offset: 0x00032838
		public static int GetStorageMemorySize(Texture texture)
		{
			if (TextureUtil2._getStorageMemorySize == null)
			{
				return 0;
			}
			object[] parameters = new object[]
			{
				texture
			};
			return (int)TextureUtil2._getStorageMemorySize.Invoke(null, parameters);
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0003466C File Offset: 0x0003286C
		public static TextureUtil2.RuntimeMemoryUsage GetRuntimeMemorySize(TextureImporterFormat format, int width, int height, bool hasmips, bool isreadable, TextureImporterShape shape)
		{
			TextureImporterFormatDatabase.Entry entry = TextureImporterFormatDatabase.Find(format);
			float num = entry.BitsPerPixel;
			switch ((int)format)
			{
			case 48:
			case 49:
			case 50:
			case 51:
			case 52:
			case 53:
			case 54:
			case 55:
			case 56:
			case 57:
			case 58:
			case 59:
				width = Mathf.CeilToInt((float)width / (float)entry.BlockSize) * entry.BlockSize;
				height = Mathf.CeilToInt((float)height / (float)entry.BlockSize) * entry.BlockSize;
				num = 1f / (float)(entry.BlockSize * entry.BlockSize) * 128f;
				break;
			}
			float num2 = (float)(width * height) * num / 8f;
			if (hasmips)
			{
				num2 *= 1.33333337f;
			}
			bool flag = (int)shape == 2;
			if (flag)
			{
				num2 *= 6f;
			}
			TextureUtil2.RuntimeMemoryUsage result = default(TextureUtil2.RuntimeMemoryUsage);
			result.Gpu = Mathf.RoundToInt(num2);
			if (isreadable && !flag && entry.CanBeReadable)
			{
				result.Cpu = Mathf.RoundToInt(num2);
			}
			return result;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00034770 File Offset: 0x00032970
		public static Color GetTextureImporterTypeChartColor(TextureImporterType type, TextureImporterShape shape)
		{
			if ((int)shape == 2)
			{
				return new Color32(225, 151, 76, byte.MaxValue);
			}
			switch ((int)type)
			{
			case 0:
				return new Color32(211, 94, 96, byte.MaxValue);
			case 1:
				return new Color32(114, 147, 203, byte.MaxValue);
			case 2:
				return new Color32(144, 103, 167, byte.MaxValue);
			case 4:
				return new Color32(204, 194, 16, byte.MaxValue);
			case 6:
				return new Color32(128, 133, 133, byte.MaxValue);
			case 7:
				return Color.white;
			case 8:
				return new Color32(132, 186, 91, byte.MaxValue);
			case 10:
				return new Color32(171, 104, 87, byte.MaxValue);
			}
			return new Color32(171, 104, 87, byte.MaxValue);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x000348B8 File Offset: 0x00032AB8
		public static string GetTextureShapeString(TextureImporterShape value)
		{
			switch ((int)value)
			{
			case 1:
				return "2D";
			case 2:
				return "Cube";
			default:
				return value.ToString();
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000348F0 File Offset: 0x00032AF0
		public static string GetAlphaSourceString(TextureImporterAlphaSource value)
		{
			switch ((int)value)
			{
			case 0:
				return "None";
			case 1:
				return "Input Alpha";
			case 2:
				return "From Grayscale";
			default:
				return value.ToString();
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00034930 File Offset: 0x00032B30
		public static string GetTextureTypeString(TextureImporterType type, TextureImporterShape shape)
		{
			if ((int)shape == 2)
			{
				return "Cubemap";
			}
			switch ((int)type)
			{
			case 0:
				return "Default";
			case 1:
				return "Normal map";
			case 2:
				return "Editor GUI & Legacy UI";
			case 4:
				return "Cookie";
			case 6:
				return "Lightmap";
			case 7:
				return "Cursor";
			case 8:
				return "Sprite (2D & UI)";
			case 10:
				return "Single Channel";
			}
			return type.ToString();
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000349BC File Offset: 0x00032BBC
		public static string GetWrapModeString(TextureWrapMode value)
		{
			switch ((int)value)
			{
			case -1:
			case 0:
				return "Repeat";
			case 1:
				return "Clamp";
			case 2:
				return "Mirror";
			case 3:
				return "Mirror Once";
			default:
				return value.ToString();
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00034A0C File Offset: 0x00032C0C
		public static string GetWrapModeString(TextureImporterShape shape, TextureWrapMode u, TextureWrapMode v, TextureWrapMode w)
		{
			if ((int)shape == 2)
			{
				if (u == v && v == w)
				{
					return TextureUtil2.GetWrapModeString(u);
				}
				return string.Format("Per-axis ({0}|{1}|{2})", TextureUtil2.GetWrapModeString(u), TextureUtil2.GetWrapModeString(v), TextureUtil2.GetWrapModeString(w));
			}
			else
			{
				if (u == v)
				{
					return TextureUtil2.GetWrapModeString(u);
				}
				return string.Format("Per-axis ({0}|{1})", TextureUtil2.GetWrapModeString(u), TextureUtil2.GetWrapModeString(v));
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00034A6C File Offset: 0x00032C6C
		public static string GetFilterModeString(FilterMode value)
		{
			switch ((int)value)
			{
			case -1:
			case 1:
				return "Bilinear";
			case 0:
				return "Point";
			case 2:
				return "Trilinear";
			default:
				return value.ToString();
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00034AB4 File Offset: 0x00032CB4
		public static string GetNPOTScaleString(TextureImporterNPOTScale value)
		{
			switch ((int)value)
			{
			case 0:
				return "None";
			case 1:
				return "To Nearest";
			case 2:
				return "To Larger";
			case 3:
				return "To Smaller";
			default:
				return value.ToString();
			}
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00034B00 File Offset: 0x00032D00
		public static string GetResizeAlgorithmString(TextureResizeAlgorithm value)
		{
			switch ((int)value)
			{
			case 0:
				return "Mitchell";
			case 1:
				return "Bilinear";
			default:
				return value.ToString();
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00034B38 File Offset: 0x00032D38
		public static bool HasAlphaChannel(TextureImporterFormat value)
		{
			TextureImporterFormatDatabase.Entry entry = TextureImporterFormatDatabase.Find(value);
			return entry != null && entry.HasAlphaChannel;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00034B58 File Offset: 0x00032D58
		public static string GetCompressionString(TextureImporterCompression value)
		{
			switch ((int)value)
			{
			case 0:
				return "None";
			case 1:
				return "Normal";
			case 2:
				return "High";
			case 3:
				return "Low";
			default:
				return value.ToString();
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00034BA4 File Offset: 0x00032DA4
		public static string GetCompressionQualityString(int value)
		{
			if (value == 0)
			{
				return "Fast";
			}
			if (value == 50)
			{
				return "Normal";
			}
			if (value != 100)
			{
				return string.Format("{0}", value);
			}
			return "Best";
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00034BE8 File Offset: 0x00032DE8
		public static bool CanUseCrunchCompression(TextureImporterFormat value)
		{
			TextureImporterFormatDatabase.Entry entry = TextureImporterFormatDatabase.Find(value);
			return entry != null && entry.CanUseCrunchCompression;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00034C08 File Offset: 0x00032E08
		public static bool CanUseCompressionQuality(TextureImporterFormat value)
		{
			TextureImporterFormatDatabase.Entry entry = TextureImporterFormatDatabase.Find(value);
			return entry != null && entry.CanUseCompressionQuality;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00034C28 File Offset: 0x00032E28
		public static bool IsCompressedFormat(TextureImporterFormat value)
		{
			TextureImporterFormatDatabase.Entry entry = TextureImporterFormatDatabase.Find(value);
			return entry != null && entry.Format != entry.UncompressedFormat;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00034C54 File Offset: 0x00032E54
		public static TextureImporterFormat GetTextureImporterFormatFromTextureFormat(TextureFormat value)
		{
			TextureImporterFormatDatabase.Entry entry = TextureImporterFormatDatabase.Find(value);
			if (entry != null)
			{
				return entry.Format;
			}
			return TextureImporterFormat.RGBA32;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00034C74 File Offset: 0x00032E74
		public static string GetTextureImporterFormatString(TextureImporterFormat value)
		{
			if (value < 0)
			{
				return "Auto";
			}
			TextureImporterFormatDatabase.Entry entry = TextureImporterFormatDatabase.Find(value);
			if (entry != null)
			{
				return entry.Name;
			}
			return value.ToString();
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00034CA8 File Offset: 0x00032EA8
		public static string GetTextureImporterFormatLongString(TextureImporterFormat value)
		{
			if (value < 0)
			{
				return "Auto";
			}
			TextureImporterFormatDatabase.Entry entry = TextureImporterFormatDatabase.Find(value);
			if (entry != null)
			{
				return entry.LongName;
			}
			return value.ToString();
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00034CDC File Offset: 0x00032EDC
		public static int GetPOTSize(int size, TextureImporterNPOTScale scale)
		{
			if (scale == null)
			{
				return size;
			}
			if ((int)scale == 3)
			{
				for (int i = 1; i < 16; i++)
				{
					int num = 1 << i;
					if (size < num)
					{
						return 1 << i - 1;
					}
				}
			}
			if ((int)scale == 2)
			{
				for (int j = 1; j < 16; j++)
				{
					int num2 = 1 << j;
					if (size <= num2)
					{
						return num2;
					}
				}
			}
			if ((int)scale == 1)
			{
				int k = 1;
				while (k < 16)
				{
					int num3 = 1 << k - 1;
					int num4 = 1 << k;
					if (size > num3 && size <= num4)
					{
						int num5 = size - num3;
						int num6 = num4 - size;
						if (num5 < num6)
						{
							return num3;
						}
						return num4;
					}
					else
					{
						k++;
					}
				}
			}
			return size;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00034D7D File Offset: 0x00032F7D
		public static int GetMultipleOf(TextureImporterFormat format)
		{
			return TextureImporterFormatDatabase.Find(format).MustBeMultipleOf;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00034D8A File Offset: 0x00032F8A
		public static bool IsSquareRequired(TextureImporterFormat format)
		{
			return TextureImporterFormatDatabase.Find(format).MustBeSquare;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00034D97 File Offset: 0x00032F97
		public static bool IsPOTRequired(TextureImporterFormat format)
		{
			return TextureImporterFormatDatabase.Find(format).MustBePot;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00034DA4 File Offset: 0x00032FA4
		public static bool IsPOTRequiredForMipMaps(TextureImporterFormat format)
		{
			return TextureImporterFormatDatabase.Find(format).MustBePotForMipMaps;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00034DB1 File Offset: 0x00032FB1
		public static TextureImporterFormat GetUncompressedFormat(TextureImporterFormat format)
		{
			return TextureImporterFormatDatabase.Find(format).UncompressedFormat;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00034DC0 File Offset: 0x00032FC0
		public static void GetOriginalWidthAndHeight(Texture texture, TextureImporter importer, out int width, out int height)
		{
			width = 0;
			height = 0;
			Cubemap cubemap = texture as Cubemap;
			if (null != cubemap)
			{
				width = TextureUtil2.GetGLWidth(texture);
				height = TextureUtil2.GetGLHeight(texture);
				return;
			}
			if (null != importer)
			{
				TextureImporter2.GetWidthAndHeight(importer, ref width, ref height);
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00034E08 File Offset: 0x00033008
		private static int GetGLWidth(Texture texture)
		{
			if (TextureUtil2._getGLWidth == null)
			{
				return -1;
			}
			object[] parameters = new object[]
			{
				texture
			};
			return (int)TextureUtil2._getGLWidth.Invoke(null, parameters);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00034E3C File Offset: 0x0003303C
		private static int GetGLHeight(Texture texture)
		{
			if (TextureUtil2._getGLHeight == null)
			{
				return -1;
			}
			object[] parameters = new object[]
			{
				texture
			};
			return (int)TextureUtil2._getGLHeight.Invoke(null, parameters);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00034E70 File Offset: 0x00033070
		public static string ToSourceCode(Texture2D texture)
		{
			StringBuilder stringBuilder = new StringBuilder(4096);
			Color32[] pixels = texture.GetPixels32();
			stringBuilder.AppendLine("new Color32[] {");
			int i = 0;
			while (i < pixels.Length)
			{
				int num = 0;
				while (num < 6 && i < pixels.Length)
				{
					Color32 color = pixels[i];
					stringBuilder.Append(string.Format(" new Color32({0},{1},{2},{3}),", new object[]
					{
						color.r,
						color.g,
						color.b,
						color.a
					}));
					i++;
					num++;
				}
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine("};");
			return stringBuilder.ToString();
		}

		// Token: 0x04000266 RID: 614
		private static Type _type = typeof(TextureImporter).Assembly.GetType("UnityEditor.TextureUtil");

		// Token: 0x04000267 RID: 615
		private static MethodInfo _getStorageMemorySize;

		// Token: 0x04000268 RID: 616
		private static MethodInfo _getGLWidth;

		// Token: 0x04000269 RID: 617
		private static MethodInfo _getGLHeight;

		// Token: 0x02000082 RID: 130
		public struct RuntimeMemoryUsage
		{
			// Token: 0x0400026A RID: 618
			public int Cpu;

			// Token: 0x0400026B RID: 619
			public int Gpu;
		}
	}
}
