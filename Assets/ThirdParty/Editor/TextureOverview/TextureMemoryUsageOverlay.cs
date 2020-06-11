using System;
using EditorFramework;
using UnityEditor;
using UnityEngine;

namespace TextureOverview
{
	// Token: 0x0200007D RID: 125
	public class TextureMemoryUsageOverlay
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x000337C8 File Offset: 0x000319C8
		// (set) Token: 0x060003CA RID: 970 RVA: 0x00033878 File Offset: 0x00031A78
		public bool Visible
		{
			get
			{
				if (this._visible < 0)
				{
					this._visible = EditorPrefs.GetInt(string.Format("{0}.MemoryUsage.Visible", Globals.ProductId), 0);
					this._position.x = EditorPrefs.GetFloat(string.Format("{0}.MemoryUsage.Position.X", Globals.ProductId), this._position.x);
					this._position.y = EditorPrefs.GetFloat(string.Format("{0}.MemoryUsage.Position.Y", Globals.ProductId), this._position.y);
					this._VisiblePanels = (TextureMemoryUsageOverlay.Panels)EditorPrefs.GetInt(string.Format("{0}.MemoryUsage.VisiblePanels", Globals.ProductId), 9);
				}
				return this._visible > 0;
			}
			set
			{
				if (this._visible != (value ? 1 : 0))
				{
					this._visible = (value ? 1 : 0);
					EditorPrefs.SetInt(string.Format("{0}.MemoryUsage.Visible", Globals.ProductId), this._visible);
					EditorPrefs.SetInt(string.Format("{0}.MemoryUsage.VisiblePanels", Globals.ProductId), (int)this._VisiblePanels);
				}
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060003CB RID: 971 RVA: 0x000338D8 File Offset: 0x00031AD8
		private int VisiblePanelCount
		{
			get
			{
				int num = 0;
				if ((this._VisiblePanels & TextureMemoryUsageOverlay.Panels.RuntimeMemory) != (TextureMemoryUsageOverlay.Panels)0)
				{
					num++;
				}
				if ((this._VisiblePanels & TextureMemoryUsageOverlay.Panels.StorageMemory) != (TextureMemoryUsageOverlay.Panels)0)
				{
					num++;
				}
				if ((this._VisiblePanels & TextureMemoryUsageOverlay.Panels.GraphicsMemory) != (TextureMemoryUsageOverlay.Panels)0)
				{
					num++;
				}
				if ((this._VisiblePanels & TextureMemoryUsageOverlay.Panels.MainMemory) != (TextureMemoryUsageOverlay.Panels)0)
				{
					num++;
				}
				return num;
			}
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00033920 File Offset: 0x00031B20
		public void OnGUI(EditorWindow owner)
		{
			this._Owner = owner;
			if (!this.Visible || this._Owner == null || this.RuntimeUsage == null || this.StorageUsage == null || this.CpuUsage == null || this.GpuUsage == null)
			{
				return;
			}
			Rect position = this._position;
			this._position = this.ClampPosition(owner, GUI.Window(0, this._position, new GUI.WindowFunction(this.DrawOverlayWindow), ""));
			if (position != this._position)
			{
				EditorPrefs.SetFloat(string.Format("{0}.MemoryUsage.Position.X", Globals.ProductId), this._position.x);
				EditorPrefs.SetFloat(string.Format("{0}.MemoryUsage.Position.Y", Globals.ProductId), this._position.y);
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x000339E8 File Offset: 0x00031BE8
		private void DrawOverlayWindow(int id)
		{
			this._position.width = (float)(Mathf.Max(1, this.VisiblePanelCount) * 335);
			EditorGUI.DrawPreviewTexture(new Rect(0f, 16f, this._position.width, this._position.height), EditorStyles.toolbar.normal.background);
			if (GUI.Button(new Rect(this._position.width - 16f, 0f, 16f, 16f), new GUIContent("X", "Close Overlay"), EditorStyles.label))
			{
				this.Visible = false;
				return;
			}
			EditorGUILayout.BeginVertical(new GUILayoutOption[0]);
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if ((this._VisiblePanels & TextureMemoryUsageOverlay.Panels.RuntimeMemory) != (TextureMemoryUsageOverlay.Panels)0)
			{
				this.DrawUsageInfo(this.RuntimeUsage, "Runtime (Graphics + Main) Memory Usage");
				EditorGUILayout2.Separator(new GUILayoutOption[]
				{
					GUILayout.Width(1f),
					GUILayout.ExpandHeight(true)
				});
			}
			if ((this._VisiblePanels & TextureMemoryUsageOverlay.Panels.GraphicsMemory) != (TextureMemoryUsageOverlay.Panels)0)
			{
				this.DrawUsageInfo(this.GpuUsage, "Graphics Memory Usage");
				EditorGUILayout2.Separator(new GUILayoutOption[]
				{
					GUILayout.Width(1f),
					GUILayout.ExpandHeight(true)
				});
			}
			if ((this._VisiblePanels & TextureMemoryUsageOverlay.Panels.MainMemory) != (TextureMemoryUsageOverlay.Panels)0)
			{
				this.DrawUsageInfo(this.CpuUsage, "Main Memory Usage");
				EditorGUILayout2.Separator(new GUILayoutOption[]
				{
					GUILayout.Width(1f),
					GUILayout.ExpandHeight(true)
				});
			}
			if ((this._VisiblePanels & TextureMemoryUsageOverlay.Panels.StorageMemory) != (TextureMemoryUsageOverlay.Panels)0)
			{
				this.DrawUsageInfo(this.StorageUsage, "Storage Memory Usage");
				EditorGUILayout2.Separator(new GUILayoutOption[]
				{
					GUILayout.Width(1f),
					GUILayout.ExpandHeight(true)
				});
			}
			EditorGUILayout.EndHorizontal();
			if (this.VisiblePanelCount == 0)
			{
				GUILayout.FlexibleSpace();
			}
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			TextureMemoryUsageOverlay.Panels panels = (TextureMemoryUsageOverlay.Panels)EditorGUILayout2.EnumFlagsField(this._VisiblePanels, new GUILayoutOption[0]);
			if (panels != this._VisiblePanels)
			{
				this._VisiblePanels = panels;
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			GUI.DragWindow();
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00033C08 File Offset: 0x00031E08
		private void DrawUsageInfo(TextureMemoryUsageInfo usageInfo, string title)
		{
			float num = 80f;
			float num2 = 65f;
			float num3 = 120f;
			float num4 = 50f;
			EditorGUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label(title, EditorStyles.boldLabel, new GUILayoutOption[0]);
			GUILayout.Space(2f);
			if (usageInfo.UsagePerType.Count > 0)
			{
				float num5 = 1f / usageInfo.UsagePerType[0].Percentage;
				foreach (TextureMemoryUsageData textureMemoryUsageData in usageInfo.UsagePerType)
				{
					EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
					GUILayout.Label(TextureUtil2.GetTextureTypeString(textureMemoryUsageData.TextureType, textureMemoryUsageData.TextureShape), new GUILayoutOption[]
					{
						GUILayout.Width(num)
					});
					GUILayout.Label(EditorUtility2.FormatBytes(textureMemoryUsageData.Size), GUIStyles.LabelRight, new GUILayoutOption[]
					{
						GUILayout.Width(num2)
					});
					Color color = GUIColors.TintIfPlaying(TextureUtil2.GetTextureImporterTypeChartColor(textureMemoryUsageData.TextureType, textureMemoryUsageData.TextureShape));
					EditorGUI.DrawRect(GUILayoutUtility.GetRect(1f, 19f, new GUILayoutOption[]
					{
						GUILayout.Width(Mathf.Max(1f, textureMemoryUsageData.Percentage * num3 * num5))
					}), color);
					GUILayout.Label(string.Format("{0:F1}%", textureMemoryUsageData.Percentage * 100f), new GUILayoutOption[]
					{
						GUILayout.Width(num4)
					});
					EditorGUILayout.EndHorizontal();
					GUILayout.Space(1f);
				}
			}
			GUILayout.Label(string.Format("= {0}", EditorUtility2.FormatBytes(usageInfo.TotalSize)), GUIStyles.BoldLabelRight, new GUILayoutOption[]
			{
				GUILayout.Width(8f + num + num2)
			});
			EditorGUILayout.EndVertical();
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00033E10 File Offset: 0x00032010
		private Rect ClampPosition(EditorWindow owner, Rect overlayPosition)
		{
			float num = 32f;
			if (overlayPosition.xMax <= num)
			{
				overlayPosition.x = num - overlayPosition.width;
			}
			if (overlayPosition.x >= owner.position.width - num)
			{
				overlayPosition.x = owner.position.width - num;
			}
			if (overlayPosition.yMax <= num)
			{
				overlayPosition.y = num - overlayPosition.height;
			}
			if (overlayPosition.y >= owner.position.height - num)
			{
				overlayPosition.y = owner.position.height - num;
			}
			return overlayPosition;
		}

		// Token: 0x04000248 RID: 584
		private Rect _position = new Rect(20f, 20f, 665f, 254f);

		// Token: 0x04000249 RID: 585
		private int _visible = -1;

		// Token: 0x0400024A RID: 586
		private TextureMemoryUsageOverlay.Panels _VisiblePanels = TextureMemoryUsageOverlay.Panels.RuntimeMemory | TextureMemoryUsageOverlay.Panels.StorageMemory;

		// Token: 0x0400024B RID: 587
		private EditorWindow _Owner;

		// Token: 0x0400024C RID: 588
		public TextureMemoryUsageInfo CpuUsage;

		// Token: 0x0400024D RID: 589
		public TextureMemoryUsageInfo GpuUsage;

		// Token: 0x0400024E RID: 590
		public TextureMemoryUsageInfo RuntimeUsage;

		// Token: 0x0400024F RID: 591
		public TextureMemoryUsageInfo StorageUsage;

		// Token: 0x0200007E RID: 126
		[Flags]
		private enum Panels
		{
			// Token: 0x04000251 RID: 593
			RuntimeMemory = 1,
			// Token: 0x04000252 RID: 594
			GraphicsMemory = 2,
			// Token: 0x04000253 RID: 595
			MainMemory = 4,
			// Token: 0x04000254 RID: 596
			StorageMemory = 8
		}
	}
}
