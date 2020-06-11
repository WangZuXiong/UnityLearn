using System;
using System.IO;

namespace EditorFramework
{
	// Token: 0x0200002F RID: 47
	public static class FileUtil2
	{
		// Token: 0x0600017F RID: 383 RVA: 0x0000B654 File Offset: 0x00009854
		public static string GetFileExtension(string assetPath)
		{
			if (string.IsNullOrEmpty(assetPath))
			{
				return "";
			}
			int num = assetPath.LastIndexOf('.');
			if (num != -1)
			{
				return assetPath.Substring(num + 1);
			}
			return "";
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000B68C File Offset: 0x0000988C
		public static string GetFileName(string assetPath)
		{
			if (assetPath != null)
			{
				int length = assetPath.Length;
				int num = length;
				while (--num >= 0)
				{
					char c = assetPath[num];
					if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar || c == Path.VolumeSeparatorChar)
					{
						return assetPath.Substring(num + 1, length - num - 1);
					}
				}
			}
			return assetPath;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000B6E0 File Offset: 0x000098E0
		public static string GetDirectoryName(string assetPath)
		{
			if (string.IsNullOrEmpty(assetPath))
			{
				return "";
			}
			string directoryName = Path.GetDirectoryName(assetPath);
			if (string.IsNullOrEmpty(directoryName))
			{
				return "";
			}
			return directoryName.Replace('\\', '/');
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000B71C File Offset: 0x0000991C
		public static string GetFileNameWithoutExtension(string assetPath)
		{
			if (string.IsNullOrEmpty(assetPath))
			{
				return "";
			}
			string fileName = FileUtil2.GetFileName(assetPath);
			int num = fileName.LastIndexOf('.');
			if (num == -1)
			{
				return fileName;
			}
			return fileName.Substring(0, num);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000B755 File Offset: 0x00009955
		public static bool Exists(string assetPath)
		{
			return File.Exists(assetPath);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000B764 File Offset: 0x00009964
		public static bool IsReadOnly(string assetPath)
		{
			if (!FileUtil2.Exists(assetPath))
			{
				return false;
			}
			FileInfo fileInfo = new FileInfo(assetPath);
			return fileInfo.IsReadOnly;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000B788 File Offset: 0x00009988
		public static long GetFileSize(string assetPath)
		{
			try
			{
				return new FileInfo(assetPath).Length;
			}
			catch (Exception)
			{
			}
			return -1L;
		}
	}
}
