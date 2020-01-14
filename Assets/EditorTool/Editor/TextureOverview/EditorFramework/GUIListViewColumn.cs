using System;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x0200000E RID: 14
	public class GUIListViewColumn
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00007CEF File Offset: 0x00005EEF
		public bool IsPrimaryColumn
		{
			get
			{
				return this.DisplayIndex == 0;
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00007CFA File Offset: 0x00005EFA
		public GUIListViewColumn()
		{
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00007D38 File Offset: 0x00005F38
		public GUIListViewColumn(string text, string tooltip, Texture image, float width, GUIListViewColumn.CompareDelelgate comparefunc)
		{
			this.Text = text;
			this.Tooltip = tooltip;
			this.Image = image;
			this.Width = width;
			this.CompareFunc = comparefunc;
			this.Color = Color.clear;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00007DB0 File Offset: 0x00005FB0
		public static Texture2D SortIcon
		{
			get
			{
				if (null == GUIListViewColumn._sortArrayUp)
				{
					GUIListViewColumn._sortArrayUp = new Texture2D(8, 4, TextureFormat.ARGB32, false, true);
					GUIListViewColumn._sortArrayUp.hideFlags = HideFlags.DontSave;
					GUIListViewColumn._sortArrayUp.SetPixels32(GUIListViewColumn._sortArrowUpPixelData);
					GUIListViewColumn._sortArrayUp.Apply();
				}
				return GUIListViewColumn._sortArrayUp;
			}
		}

		// Token: 0x04000081 RID: 129
		internal int Index;

		// Token: 0x04000082 RID: 130
		internal float RealWidth;

		// Token: 0x04000083 RID: 131
		internal Rect ColumnRect;

		// Token: 0x04000084 RID: 132
		internal long SortPrio = -1L;

		// Token: 0x04000085 RID: 133
		public string SerializeName;

		// Token: 0x04000086 RID: 134
		public string Text;

		// Token: 0x04000087 RID: 135
		public string PopupText;

		// Token: 0x04000088 RID: 136
		public TextAnchor TextAlignment = TextAnchor.MiddleLeft;

		// Token: 0x04000089 RID: 137
		public string Tooltip;

		// Token: 0x0400008A RID: 138
		public Texture Image;

		// Token: 0x0400008B RID: 139
		public float Width;

		// Token: 0x0400008C RID: 140
		public Color Color;

		// Token: 0x0400008D RID: 141
		public GUIListViewSortMode SortMode;

		// Token: 0x0400008E RID: 142
		public float MinWidth = 16f;

		// Token: 0x0400008F RID: 143
		public float MaxWidth = float.PositiveInfinity;

		// Token: 0x04000090 RID: 144
		public bool Visible = true;

		// Token: 0x04000091 RID: 145
		public bool IsResizable = true;

		// Token: 0x04000092 RID: 146
		public GUIListViewColumn.CompareDelelgate CompareFunc;

		// Token: 0x04000093 RID: 147
		public int DisplayIndex;

		// Token: 0x04000094 RID: 148
		private static Texture2D _sortArrayUp;

		// Token: 0x04000095 RID: 149
		private static readonly Color32[] _sortArrowUpPixelData = new Color32[]
		{
			new Color32(74, 73, 74, byte.MaxValue),
			new Color32(134, 133, 134, byte.MaxValue),
			new Color32(134, 133, 134, byte.MaxValue),
			new Color32(134, 133, 134, byte.MaxValue),
			new Color32(161, 160, 161, byte.MaxValue),
			new Color32(161, 160, 161, byte.MaxValue),
			new Color32(161, 160, 161, byte.MaxValue),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0),
			new Color32(74, 73, 74, byte.MaxValue),
			new Color32(74, 73, 74, byte.MaxValue),
			new Color32(134, 133, 134, byte.MaxValue),
			new Color32(115, 113, 115, byte.MaxValue),
			new Color32(115, 113, 115, byte.MaxValue),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0),
			new Color32(74, 73, 74, byte.MaxValue),
			new Color32(74, 73, 74, byte.MaxValue),
			new Color32(115, 113, 115, byte.MaxValue),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0),
			new Color32(74, 73, 74, byte.MaxValue),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0)
		};

		// Token: 0x0200000F RID: 15
		// (Invoke) Token: 0x060000BF RID: 191
		public delegate int CompareDelelgate(object x, object y);
	}
}
