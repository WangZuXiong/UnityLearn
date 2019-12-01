using System;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000052 RID: 82
	public static class GUIStyles
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000DA00 File Offset: 0x0000BC00
		public static GUIStyle White
		{
			get
			{
				if (GUIStyles._whitestyle != null)
				{
					return GUIStyles._whitestyle;
				}
				GUIStyles._whitestyle = new GUIStyle(GUI.skin.label);
				GUIStyles._whitestyle.normal.textColor = Color.white;
				GUIStyles._whitestyle.normal.background = EditorGUIUtility.whiteTexture;
				GUIStyles._whitestyle.active.textColor = Color.white;
				GUIStyles._whitestyle.active.background = EditorGUIUtility.whiteTexture;
				GUIStyles._whitestyle.onActive.textColor = Color.white;
				GUIStyles._whitestyle.onActive.background = EditorGUIUtility.whiteTexture;
				GUIStyles._whitestyle.onFocused.textColor = Color.white;
				GUIStyles._whitestyle.onFocused.background = EditorGUIUtility.whiteTexture;
				GUIStyles._whitestyle.onHover.textColor = Color.white;
				GUIStyles._whitestyle.onHover.background = EditorGUIUtility.whiteTexture;
				GUIStyles._whitestyle.onNormal.textColor = Color.white;
				GUIStyles._whitestyle.onNormal.background = EditorGUIUtility.whiteTexture;
				GUIStyles._whitestyle.stretchHeight = true;
				GUIStyles._whitestyle.stretchWidth = true;
				GUIStyles._whitestyle.padding = new RectOffset();
				GUIStyles._whitestyle.margin = new RectOffset();
				GUIStyles._whitestyle.border = new RectOffset();
				return GUIStyles._whitestyle;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000DB68 File Offset: 0x0000BD68
		public static GUIStyle Clear
		{
			get
			{
				if (GUIStyles._clear != null)
				{
					return GUIStyles._clear;
				}
				GUIStyles._clear = new GUIStyle(GUI.skin.label);
				GUIStyles._clear.normal.textColor = Color.clear;
				GUIStyles._clear.normal.background = null;
				GUIStyles._clear.active.textColor = Color.clear;
				GUIStyles._clear.active.background = null;
				GUIStyles._clear.onActive.textColor = Color.clear;
				GUIStyles._clear.onActive.background = null;
				GUIStyles._clear.onFocused.textColor = Color.clear;
				GUIStyles._clear.onFocused.background = null;
				GUIStyles._clear.onHover.textColor = Color.clear;
				GUIStyles._clear.onHover.background = null;
				GUIStyles._clear.onNormal.textColor = Color.clear;
				GUIStyles._clear.onNormal.background = null;
				GUIStyles._clear.stretchHeight = true;
				GUIStyles._clear.stretchWidth = true;
				GUIStyles._clear.padding = new RectOffset();
				GUIStyles._clear.margin = new RectOffset();
				GUIStyles._clear.border = new RectOffset();
				return GUIStyles._clear;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000DCB8 File Offset: 0x0000BEB8
		public static GUIStyle Hyperlink
		{
			get
			{
				if (GUIStyles._hyperlinkStyle != null)
				{
					return GUIStyles._hyperlinkStyle;
				}
				GUIStyles._hyperlinkStyle = new GUIStyle(EditorStyles.label);
				GUIStyles._hyperlinkStyle.wordWrap = true;
				GUIStyles._hyperlinkStyle.normal.textColor = GUIColors.Hyperlink;
				GUIStyles._hyperlinkStyle.onNormal.textColor = GUIColors.Hyperlink;
				GUIStyles._hyperlinkStyle.hover.textColor = GUIColors.Hyperlink;
				GUIStyles._hyperlinkStyle.onHover.textColor = GUIColors.Hyperlink;
				GUIStyles._hyperlinkStyle.active.textColor = GUIColors.Hyperlink;
				GUIStyles._hyperlinkStyle.onActive.textColor = GUIColors.Hyperlink;
				return GUIStyles._hyperlinkStyle;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000DD69 File Offset: 0x0000BF69
		public static GUIStyle LabelRight
		{
			get
			{
				if (GUIStyles._labelRight != null)
				{
					return GUIStyles._labelRight;
				}
				GUIStyles._labelRight = new GUIStyle(EditorStyles.label);
				GUIStyles._labelRight.alignment = TextAnchor.UpperRight;
				return GUIStyles._labelRight;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000DD97 File Offset: 0x0000BF97
		public static GUIStyle BoldLabelRight
		{
			get
			{
				if (GUIStyles._boldLabelRight != null)
				{
					return GUIStyles._boldLabelRight;
				}
				GUIStyles._boldLabelRight = new GUIStyle(EditorStyles.boldLabel);
				GUIStyles._boldLabelRight.alignment = TextAnchor.UpperRight;
				return GUIStyles._boldLabelRight;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000DDC5 File Offset: 0x0000BFC5
		public static GUIStyle LabelCenter
		{
			get
			{
				if (GUIStyles._labelCenter != null)
				{
					return GUIStyles._labelCenter;
				}
				GUIStyles._labelCenter = new GUIStyle(EditorStyles.label);
				GUIStyles._labelCenter.alignment = TextAnchor.UpperCenter;
				return GUIStyles._labelCenter;
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000DDF4 File Offset: 0x0000BFF4
		public static GUIStyle GetTempImageOnly(Texture2D normal, Texture2D active)
		{
			if (GUIStyles._tempImageStyle == null)
			{
				GUIStyles._tempImageStyle = new GUIStyle(EditorStyles.toolbarButton);
				GUIStyles._tempImageStyle.margin = new RectOffset(0, 0, 0, 0);
				GUIStyles._tempImageStyle.contentOffset = new Vector2(0f, 0f);
				GUIStyles._tempImageStyle.border = new RectOffset(0, 0, 0, 0);
				GUIStyles._tempImageStyle.fixedHeight = 16f;
				GUIStyles._tempImageStyle.fixedWidth = 16f;
				GUIStyles._tempImageStyle.imagePosition = ImagePosition.ImageOnly;
				GUIStyles._tempImageStyle.padding = new RectOffset(0, 0, 0, 0);
			}
			GUIStyles._tempImageStyle.fixedHeight = (float)normal.height;
			GUIStyles._tempImageStyle.fixedWidth = (float)normal.width;
			GUIStyles._tempImageStyle.normal.background = normal;
			GUIStyles._tempImageStyle.active.background = active;
			return GUIStyles._tempImageStyle;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000DEDC File Offset: 0x0000C0DC
		public static GUIStyle AcceptImageButton
		{
			get
			{
				if (GUIStyles._applyButtonStyle == null)
				{
					GUIStyles._applyButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
					GUIStyles._applyButtonStyle.margin = new RectOffset(0, 0, 0, 0);
					GUIStyles._applyButtonStyle.contentOffset = new Vector2(0f, 0f);
					GUIStyles._applyButtonStyle.border = new RectOffset(0, 0, 0, 0);
					GUIStyles._applyButtonStyle.fixedHeight = 16f;
					GUIStyles._applyButtonStyle.fixedWidth = 16f;
					GUIStyles._applyButtonStyle.imagePosition = ImagePosition.ImageOnly;
					GUIStyles._applyButtonStyle.padding = new RectOffset(0, 0, 0, 0);
				}
				GUIStyles._applyButtonStyle.normal.background = Images.Accept16x16;
				GUIStyles._applyButtonStyle.active.background = Images.AcceptPressed16x16;
				return GUIStyles._applyButtonStyle;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000DFAC File Offset: 0x0000C1AC
		public static GUIStyle RevertImageButton
		{
			get
			{
				if (GUIStyles._revertButtonStyle == null)
				{
					GUIStyles._revertButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
					GUIStyles._revertButtonStyle.margin = new RectOffset(0, 0, 0, 0);
					GUIStyles._revertButtonStyle.contentOffset = new Vector2(0f, 0f);
					GUIStyles._revertButtonStyle.border = new RectOffset(0, 0, 0, 0);
					GUIStyles._revertButtonStyle.fixedHeight = 16f;
					GUIStyles._revertButtonStyle.fixedWidth = 16f;
					GUIStyles._revertButtonStyle.imagePosition = ImagePosition.ImageOnly;
					GUIStyles._revertButtonStyle.padding = new RectOffset(0, 0, 0, 0);
				}
				GUIStyles._revertButtonStyle.normal.background = Images.Revert16x16;
				GUIStyles._revertButtonStyle.active.background = Images.RevertPressed16x16;
				return GUIStyles._revertButtonStyle;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000E07C File Offset: 0x0000C27C
		public static GUIStyle NextImageButton
		{
			get
			{
				if (GUIStyles._nextButtonStyle == null)
				{
					GUIStyles._nextButtonStyle = new GUIStyle(GUI.skin.label);
					GUIStyles._nextButtonStyle.margin = new RectOffset(0, 0, 0, 0);
					GUIStyles._nextButtonStyle.contentOffset = new Vector2(0f, 0f);
					GUIStyles._nextButtonStyle.border = new RectOffset(0, 0, 0, 0);
					GUIStyles._nextButtonStyle.fixedHeight = 16f;
					GUIStyles._nextButtonStyle.fixedWidth = 16f;
					GUIStyles._nextButtonStyle.imagePosition = ImagePosition.ImageOnly;
					GUIStyles._nextButtonStyle.padding = new RectOffset(0, 0, 0, 0);
				}
				GUIStyles._nextButtonStyle.normal.background = Images.Next16x16;
				GUIStyles._nextButtonStyle.active.background = Images.NextPressed16x16;
				return GUIStyles._nextButtonStyle;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000E150 File Offset: 0x0000C350
		public static GUIStyle CancelImageButton
		{
			get
			{
				if (GUIStyles._cancelButtonStyle == null)
				{
					GUIStyles._cancelButtonStyle = new GUIStyle(EditorStyles.label);
					GUIStyles._cancelButtonStyle.margin = new RectOffset(0, 0, 4, 0);
					GUIStyles._cancelButtonStyle.contentOffset = new Vector2(0f, 0f);
					GUIStyles._cancelButtonStyle.border = new RectOffset(0, 0, 0, 0);
					GUIStyles._cancelButtonStyle.fixedHeight = 11f;
					GUIStyles._cancelButtonStyle.fixedWidth = 11f;
					GUIStyles._cancelButtonStyle.imagePosition = ImagePosition.ImageOnly;
					GUIStyles._cancelButtonStyle.padding = new RectOffset(0, 0, 0, 0);
				}
				GUIStyles._cancelButtonStyle.normal.background = Images.Cancel16x16;
				GUIStyles._cancelButtonStyle.active.background = Images.CancelPressed16x16;
				return GUIStyles._cancelButtonStyle;
			}
		}

		// Token: 0x04000142 RID: 322
		private static GUIStyle _whitestyle;

		// Token: 0x04000143 RID: 323
		private static GUIStyle _revertButtonStyle;

		// Token: 0x04000144 RID: 324
		private static GUIStyle _applyButtonStyle;

		// Token: 0x04000145 RID: 325
		private static GUIStyle _nextButtonStyle;

		// Token: 0x04000146 RID: 326
		private static GUIStyle _hyperlinkStyle;

		// Token: 0x04000147 RID: 327
		private static GUIStyle _clear;

		// Token: 0x04000148 RID: 328
		private static GUIStyle _cancelButtonStyle;

		// Token: 0x04000149 RID: 329
		private static GUIStyle _tempImageStyle;

		// Token: 0x0400014A RID: 330
		private static GUIStyle _labelRight;

		// Token: 0x0400014B RID: 331
		private static GUIStyle _labelCenter;

		// Token: 0x0400014C RID: 332
		private static GUIStyle _boldLabelRight;
	}
}
