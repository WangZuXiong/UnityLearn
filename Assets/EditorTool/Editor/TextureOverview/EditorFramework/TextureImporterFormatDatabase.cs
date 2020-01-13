using System;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x0200007F RID: 127
	internal static class TextureImporterFormatDatabase
	{
		// Token: 0x060003D1 RID: 977 RVA: 0x00033EF0 File Offset: 0x000320F0
		public static TextureImporterFormatDatabase.Entry Find(TextureImporterFormat format)
		{
			if (format < 0)
			{
				return new TextureImporterFormatDatabase.Entry();
			}
			if ((int)format >= TextureImporterFormatDatabase._List.Length)
			{
				return new TextureImporterFormatDatabase.Entry();
			}
			TextureImporterFormatDatabase.Entry entry = TextureImporterFormatDatabase._List[(int)format];
			if (entry == null)
			{
				return new TextureImporterFormatDatabase.Entry();
			}
			return entry;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00033F2C File Offset: 0x0003212C
		public static TextureImporterFormatDatabase.Entry Find(TextureFormat format)
		{
			for (int i = 0; i < TextureImporterFormatDatabase._List.Length; i++)
			{
				if (TextureImporterFormatDatabase._List[i] != null && TextureImporterFormatDatabase._List[i].RuntimeFormat == format)
				{
					return TextureImporterFormatDatabase._List[i];
				}
			}
			return new TextureImporterFormatDatabase.Entry();
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00033F70 File Offset: 0x00032170
		private static void Add(int formatNum, int uncompressedFormatNum, int runtimeFormatNum, float bitsPerPixel, bool hasAlphaChannel, bool mustBeSquare, bool mustBePot, bool mustBePotForMipMaps, int minimumSize, int multipleOf, int blockSize, bool canUseCompressionQuality, bool canUseCrunchCompression, bool canBeReadable, string prettyName)
		{
			TextureImporterFormat format = (TextureImporterFormat) formatNum;
			TextureImporterFormat uncompressedFormat = (TextureImporterFormat) uncompressedFormatNum;
			TextureFormat runtimeFormat = (TextureFormat) runtimeFormatNum;
			TextureImporterFormatDatabase.Entry entry = new TextureImporterFormatDatabase.Entry();
			entry.Name = format.ToString();
			entry.LongName = prettyName;
			entry.BitsPerPixel = bitsPerPixel;
			entry.HasAlphaChannel = hasAlphaChannel;
			entry.Format = format;
			entry.UncompressedFormat = uncompressedFormat;
			entry.MinimumSize = minimumSize;
			entry.MustBeMultipleOf = multipleOf;
			entry.BlockSize = blockSize;
			entry.MustBePot = mustBePot;
			entry.MustBePotForMipMaps = mustBePotForMipMaps;
			entry.MustBeSquare = mustBeSquare;
			entry.CanUseCompressionQuality = canUseCompressionQuality;
			entry.CanUseCrunchCompression = canUseCrunchCompression;
			entry.CanBeReadable = canBeReadable;
			entry.RuntimeFormat = runtimeFormat;
			TextureImporterFormatDatabase._List[(int)format] = entry;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00034010 File Offset: 0x00032210
		static TextureImporterFormatDatabase()
		{
			TextureImporterFormatDatabase.Add(1, 1, 1, 8f, true, false, false, false, 0, 0, 0, false, false, true, "Alpha 8");
			TextureImporterFormatDatabase.Add(2, 2, 2, 16f, true, false, false, false, 0, 0, 0, false, false, false, "ARGB 16 bit");
			TextureImporterFormatDatabase.Add(3, 3, 3, 24f, false, false, false, false, 0, 0, 0, false, false, true, "RGB 24 bit");
			TextureImporterFormatDatabase.Add(4, 4, 4, 32f, true, false, false, false, 0, 0, 0, false, false, true, "RGBA 32 bit");
			TextureImporterFormatDatabase.Add(5, 5, 5, 32f, true, false, false, false, 0, 0, 0, false, false, true, "ARGB 32 bit");
			TextureImporterFormatDatabase.Add(7, 7, 7, 16f, false, false, false, false, 0, 0, 0, false, false, false, "RGB 16 bit");
			TextureImporterFormatDatabase.Add(10, 3, 10, 4f, false, false, false, true, 0, 4, 4, false, true, true, "RGB Compressed DXT1");
			TextureImporterFormatDatabase.Add(12, 4, 12, 8f, true, false, false, true, 0, 4, 4, false, true, true, "RGBA Compressed DXT5");
			TextureImporterFormatDatabase.Add(13, 13, 13, 16f, true, false, false, false, 0, 0, 0, false, false, false, "RGBA 16 bit");
			TextureImporterFormatDatabase.Add(17, 17, 17, 16f, true, false, false, false, 0, 0, 0, false, false, false, "RGBA Half");
			TextureImporterFormatDatabase.Add(24, 3, 24, 8f, false, false, false, true, 0, 4, 4, false, false, false, "RGB HDR Compressed BC6H");
			TextureImporterFormatDatabase.Add(25, 3, 25, 8f, true, false, false, true, 0, 4, 4, false, false, false, "RGB(A) Compressed BC7");
			TextureImporterFormatDatabase.Add(26, 3, 26, 4f, false, false, false, true, 0, 4, 4, false, false, false, "R Compressed BC4");
			TextureImporterFormatDatabase.Add(27, 3, 27, 8f, false, false, false, true, 0, 4, 4, false, false, false, "RG Compressed BC5");
			TextureImporterFormatDatabase.Add(28, 3, 28, 4f, false, false, false, true, 0, 4, 4, true, false, true, "RGB Crunched DXT1");
			TextureImporterFormatDatabase.Add(29, 4, 29, 8f, true, false, false, true, 0, 4, 4, true, false, true, "RGBA Crunched DXT5");
			TextureImporterFormatDatabase.Add(30, 3, 30, 2f, true, true, true, true, 8, 0, 4, true, false, false, "ARGB 16 bit");
			TextureImporterFormatDatabase.Add(31, 4, 31, 2f, true, true, true, true, 8, 0, 4, true, false, false, "RGBA Compressed PVRTC 2 bits");
			TextureImporterFormatDatabase.Add(32, 3, 32, 4f, false, true, true, true, 8, 0, 4, true, false, false, "RGB Compressed PVRTC 4 bits");
			TextureImporterFormatDatabase.Add(33, 4, 33, 4f, true, true, true, true, 8, 0, 4, true, false, false, "RGBA Compressed PVRTC 4 bits");
			TextureImporterFormatDatabase.Add(34, 3, 34, 4f, false, false, true, true, 0, 4, 4, true, false, false, "RGB Compressed ETC 4 bits");
			TextureImporterFormatDatabase.Add(41, 3, 41, 4f, false, false, true, true, 0, 4, 4, true, false, false, "R Compressed EAC 4 bit");
			TextureImporterFormatDatabase.Add(42, 3, 42, 4f, false, false, true, true, 0, 4, 4, true, false, false, "EAC_R_SIGNED");
			TextureImporterFormatDatabase.Add(43, 3, 43, 4f, false, false, true, true, 0, 4, 4, true, false, false, "EAC_RG");
			TextureImporterFormatDatabase.Add(44, 3, 44, 4f, false, false, true, true, 0, 4, 4, true, false, false, "EAC_RG_SIGNED");
			TextureImporterFormatDatabase.Add(45, 3, 45, 4f, false, false, false, true, 0, 4, 4, true, false, false, "RGB Compressed ETC2 4 bits");
			TextureImporterFormatDatabase.Add(46, 4, 46, 4f, true, false, false, true, 0, 4, 4, true, false, false, "RGB+1 bit Alpha Compressed ETC2 4 bits");
			TextureImporterFormatDatabase.Add(47, 4, 47, 8f, true, false, false, true, 0, 4, 4, true, false, false, "RGBA Compressed ETC2 8 bits");
			TextureImporterFormatDatabase.Add(48, 3, 48, 0f, false, false, false, true, 0, 0, 4, true, false, false, "RGB Compressed ASTC 4x4 block");
			TextureImporterFormatDatabase.Add(49, 3, 49, 0f, false, false, false, true, 0, 0, 5, true, false, false, "RGB Compressed ASTC 5x5 block");
			TextureImporterFormatDatabase.Add(50, 3, 50, 0f, false, false, false, true, 0, 0, 6, true, false, false, "RGB Compressed ASTC 6x6 block");
			TextureImporterFormatDatabase.Add(51, 3, 51, 0f, false, false, false, true, 0, 0, 8, true, false, false, "RGB Compressed ASTC 8x8 block");
			TextureImporterFormatDatabase.Add(52, 3, 52, 0f, false, false, false, true, 0, 0, 10, true, false, false, "RGB Compressed ASTC 10x10 block");
			TextureImporterFormatDatabase.Add(53, 3, 53, 0f, false, false, false, true, 0, 0, 12, true, false, false, "RGB Compressed ASTC 12x12 block");
			TextureImporterFormatDatabase.Add(54, 4, 54, 0f, true, false, false, true, 0, 0, 4, true, false, false, "RGBA Compressed ASTC 4x4 block");
			TextureImporterFormatDatabase.Add(55, 4, 55, 0f, true, false, false, true, 0, 0, 5, true, false, false, "RGBA Compressed ASTC 5x5 block");
			TextureImporterFormatDatabase.Add(56, 4, 56, 0f, true, false, false, true, 0, 0, 6, true, false, false, "RGBA Compressed ASTC 6x6 block");
			TextureImporterFormatDatabase.Add(57, 4, 57, 0f, true, false, false, true, 0, 0, 8, true, false, false, "RGBA Compressed ASTC 8x8 block");
			TextureImporterFormatDatabase.Add(58, 4, 58, 0f, true, false, false, true, 0, 0, 10, true, false, false, "RGBA Compressed ASTC 10x10 block");
			TextureImporterFormatDatabase.Add(59, 4, 59, 0f, true, false, false, true, 0, 0, 12, true, false, false, "RGBA Compressed ASTC 12x12 block");
			TextureImporterFormatDatabase.Add(64, 3, 64, 4f, false, true, true, true, 0, 4, 4, true, false, false, "RGB Crunched ETC 4 bits");
			TextureImporterFormatDatabase.Add(65, 4, 65, 8f, true, false, false, true, 0, 4, 4, true, false, false, "RGBA Crunched ETC2 8 bits");
			TextureImporterFormatDatabase.Add(63, 63, 63, 8f, false, false, false, true, 0, 0, 0, false, false, false, "R 8");
		}

		// Token: 0x04000255 RID: 597
		private static TextureImporterFormatDatabase.Entry[] _List = new TextureImporterFormatDatabase.Entry[256];

		// Token: 0x02000080 RID: 128
		public class Entry
		{
			// Token: 0x04000256 RID: 598
			public TextureImporterFormat Format;

			// Token: 0x04000257 RID: 599
			public TextureImporterFormat UncompressedFormat;

			// Token: 0x04000258 RID: 600
			public TextureFormat RuntimeFormat;

			// Token: 0x04000259 RID: 601
			public float BitsPerPixel;

			// Token: 0x0400025A RID: 602
			public bool MustBeSquare;

			// Token: 0x0400025B RID: 603
			public bool MustBePot;

			// Token: 0x0400025C RID: 604
			public bool MustBePotForMipMaps;

			// Token: 0x0400025D RID: 605
			public int MinimumSize;

			// Token: 0x0400025E RID: 606
			public int MustBeMultipleOf;

			// Token: 0x0400025F RID: 607
			public bool CanUseCompressionQuality;

			// Token: 0x04000260 RID: 608
			public bool CanUseCrunchCompression;

			// Token: 0x04000261 RID: 609
			public bool CanBeReadable;

			// Token: 0x04000262 RID: 610
			public int BlockSize;

			// Token: 0x04000263 RID: 611
			public string Name = "";

			// Token: 0x04000264 RID: 612
			public string LongName = "";

			// Token: 0x04000265 RID: 613
			public bool HasAlphaChannel;
		}
	}
}
