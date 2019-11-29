using System;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x0200003A RID: 58
	public static class GUIColors
	{
		// Token: 0x060001B9 RID: 441 RVA: 0x0000CD30 File Offset: 0x0000AF30
		internal static void Update()
		{
			string @string = EditorPrefs.GetString("Playmode tint", "");
			if (!string.Equals(GUIColors._playmodetintString, @string))
			{
				GUIColors._playmodetintString = @string;
				if (!string.IsNullOrEmpty(@string))
				{
					string[] array = @string.Split(new char[]
					{
						';'
					});
					if (array.Length == 5)
					{
						Color playmodetint = default(Color);
						bool flag = true;
						flag &= float.TryParse(array[1], out playmodetint.r);
						flag &= float.TryParse(array[2], out playmodetint.g);
						flag &= float.TryParse(array[3], out playmodetint.b);
						flag &= float.TryParse(array[4], out playmodetint.a);
						if (flag)
						{
							GUIColors._playmodetint = playmodetint;
						}
					}
				}
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000CDE3 File Offset: 0x0000AFE3
		public static Color PlaymodeTint
		{
			get
			{
				return GUIColors._playmodetint;
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000CDEA File Offset: 0x0000AFEA
		public static Color TintIfPlaying(Color color)
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				color *= GUIColors._playmodetint;
			}
			return color;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000CE01 File Offset: 0x0000B001
		public static Color TextColor(bool selected)
		{
			if (!selected)
			{
				return GUIColors.Text;
			}
			return GUIColors.SelectedText;
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000CE14 File Offset: 0x0000B014
		public static Color Text
		{
			get
			{
				Color color = EditorStyles.label.normal.textColor;
				if (EditorApplication.isPlayingOrWillChangePlaymode)
				{
					color *= GUIColors._playmodetint;
				}
				return color;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000CE45 File Offset: 0x0000B045
		public static Color SelectedText
		{
			get
			{
				return GUIColors.TintIfPlaying(Color.white);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000CE54 File Offset: 0x0000B054
		public static Color ActiveSelection
		{
			get
			{
				Color color;
				if (EditorGUIUtility.isProSkin)
				{
					color = new Color(0.239215687f, 0.3764706f, 0.5686275f, 1f);
				}
				else
				{
					color = new Color(0.239215687f, 0.5019608f, 0.8745098f, 1f);
				}
				return GUIColors.TintIfPlaying(color);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000CEA8 File Offset: 0x0000B0A8
		public static Color Hyperlink
		{
			get
			{
				Color result;
				result = new Color(0f, 0.478431374f, 0.8f, 1f);
				return result;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000CED4 File Offset: 0x0000B0D4
		public static Color InactiveSelection
		{
			get
			{
				Color color;
				if (EditorGUIUtility.isProSkin)
				{
					color = new Color(0.282352954f, 0.282352954f, 0.282352954f, 1f);
				}
				else
				{
					color = new Color(0.5568628f, 0.5568628f, 0.5568628f, 1f);
				}
				return GUIColors.TintIfPlaying(color);
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000CF28 File Offset: 0x0000B128
		public static Color GetPrefabTypeColor(UnityEngine.Object obj)
		{
			if (null == obj)
			{
				return EditorStyles.label.normal.textColor;
			}
			switch ((int)PrefabUtility.GetPrefabType(obj))
			{
			case 1:
			case 2:
			case 3:
			case 4:
				return new Color(0f, 0f, 0.5f);
			case 5:
			case 6:
				return new Color(0.7f, 0.3f, 0.3f);
			default:
				return EditorStyles.label.normal.textColor;
			}
		}

		// Token: 0x040000E4 RID: 228
		private static Color _playmodetint = Color.white;

		// Token: 0x040000E5 RID: 229
		private static string _playmodetintString;
	}
}
