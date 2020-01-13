using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000008 RID: 8
	public abstract class GUIListView : GUIControl
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002DDE File Offset: 0x00000FDE
		// (set) Token: 0x06000027 RID: 39 RVA: 0x00002DE6 File Offset: 0x00000FE6
		public Vector2 ItemSize { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002DEF File Offset: 0x00000FEF
		// (set) Token: 0x06000029 RID: 41 RVA: 0x00002DF7 File Offset: 0x00000FF7
		public Vector2 ItemMargin { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002E00 File Offset: 0x00001000
		// (set) Token: 0x0600002B RID: 43 RVA: 0x00002E08 File Offset: 0x00001008
		public bool FullRowSelect { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002E11 File Offset: 0x00001011
		// (set) Token: 0x0600002D RID: 45 RVA: 0x00002E19 File Offset: 0x00001019
		public virtual GUIListViewMode Mode { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002E22 File Offset: 0x00001022
		// (set) Token: 0x0600002F RID: 47 RVA: 0x00002E2A File Offset: 0x0000102A
		public GUIListViewHeaderStyle HeaderStyle { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002E33 File Offset: 0x00001033
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00002E3B File Offset: 0x0000103B
		public int HeaderHeight { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002E44 File Offset: 0x00001044
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002E4C File Offset: 0x0000104C
		public List<GUIListViewColumn> Columns { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002E55 File Offset: 0x00001055
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002E5D File Offset: 0x0000105D
		public GUIListViewColumn FlexibleColumn { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002E66 File Offset: 0x00001066
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00002E6E File Offset: 0x0000106E
		public bool HotTracking { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002E77 File Offset: 0x00001077
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002E7F File Offset: 0x0000107F
		public bool ShowColumnLines { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002E88 File Offset: 0x00001088
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002E90 File Offset: 0x00001090
		public bool MultiSelect { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002E99 File Offset: 0x00001099
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002EA1 File Offset: 0x000010A1
		public bool Enabled { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002EAA File Offset: 0x000010AA
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00002EB2 File Offset: 0x000010B2
		public bool DragDropEnabled { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002EBB File Offset: 0x000010BB
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00002EC3 File Offset: 0x000010C3
		public GUIListViewColumn DragColumn { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002ECC File Offset: 0x000010CC
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002ED4 File Offset: 0x000010D4
		public bool PanningEnabled { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002EDD File Offset: 0x000010DD
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00002EE5 File Offset: 0x000010E5
		public bool RightClickSelect { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002EEE File Offset: 0x000010EE
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00002EF6 File Offset: 0x000010F6
		public string EditorPrefsPath { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002F00 File Offset: 0x00001100
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00002F38 File Offset: 0x00001138
		public object[] SelectedItems
		{
			get
			{
				object[] array = new object[this._selection.Keys.Count];
				this._selection.Keys.CopyTo(array, 0);
				return array;
			}
			set
			{
				this._selection = new Dictionary<object, GUIListView.ItemSelInfo>();
				if (value != null)
				{
					for (int i = 0; i < value.Length; i++)
					{
						object key = value[i];
						this._selection.Add(key, new GUIListView.ItemSelInfo(Time.realtimeSinceStartup, false));
					}
				}
				this.DoSelectionChange();
				base.Editor.Repaint();
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002F8F File Offset: 0x0000118F
		public int SelectedItemsCount
		{
			get
			{
				return this._selection.Count;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002F9C File Offset: 0x0000119C
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00002FA4 File Offset: 0x000011A4
		public string EmptyText
		{
			get
			{
				return this._emptyText;
			}
			set
			{
				this._emptyText = value;
				base.Editor.Repaint();
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002FB8 File Offset: 0x000011B8
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00002FF0 File Offset: 0x000011F0
		public GUIStyle EmptyTextStyle
		{
			get
			{
				if (this._emptyTextStyle == null)
				{
					this._emptyTextStyle = new GUIStyle(EditorStyles.boldLabel);
					this._emptyTextStyle.alignment = TextAnchor.MiddleCenter;
					this._emptyTextStyle.wordWrap = true;
				}
				return this._emptyTextStyle;
			}
			set
			{
				this._emptyTextStyle = value;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002FF9 File Offset: 0x000011F9
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00003001 File Offset: 0x00001201
		public Texture2D BackgroundImage { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000051 RID: 81 RVA: 0x0000300A File Offset: 0x0000120A
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00003012 File Offset: 0x00001212
		public GUIListViewBackgroundImageLayout BackgroundImageLayout { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000053 RID: 83 RVA: 0x0000301B File Offset: 0x0000121B
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00003023 File Offset: 0x00001223
		public Color BackgroundImageColor { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000055 RID: 85 RVA: 0x0000302C File Offset: 0x0000122C
		public GUIListViewColumn FirstVisibleColumn
		{
			get
			{
				for (int i = 0; i < this.Columns.Count; i++)
				{
					if (this.Columns[i].Visible)
					{
						return this.Columns[i];
					}
				}
				return null;
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003070 File Offset: 0x00001270
		public GUIListView(EditorWindow editor, GUIControl parent) : base(editor, parent)
		{
			this.LayoutOptions = new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(true),
				GUILayout.ExpandHeight(true)
			};
			this._columnresize = new GUIListViewResizeColumnContext(this);
			this.ItemSize = new Vector2(128f, 22f);
			this.ItemMargin = new Vector2(1f, 0f);
			this.Mode = GUIListViewMode.Details;
			this.HeaderStyle = GUIListViewHeaderStyle.Nonclickable;
			this.HeaderHeight = 22;
			this.FullRowSelect = false;
			this.HotTracking = false;
			this.MultiSelect = false;
			this.Columns = new List<GUIListViewColumn>();
			this.RightClickSelect = true;
			this.EditorPrefsPath = "";
			this.ShowColumnLines = true;
			this.PanningEnabled = true;
			this.Enabled = true;
			this.BackgroundImageLayout = GUIListViewBackgroundImageLayout.None;
			this.BackgroundImageColor = new Color(1f, 1f, 1f, 0.1f);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000319C File Offset: 0x0000139C
		public void Reset()
		{
			this._scrollbarPos = new Vector2(0f, 0f);
			this._selection.Clear();
			this.DoSelectionChange();
			this._lastRepaintContext = default(GUIListView.DrawContext);
			this._lastRepaintContext.CurEvent = Event.current;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000031EC File Offset: 0x000013EC
		public bool SavePrefs()
		{
			if (string.IsNullOrEmpty(this.EditorPrefsPath))
			{
				return false;
			}
			this.TryInitColumnIndexes();
			foreach (GUIListViewColumn guilistViewColumn in this.Columns)
			{
				string str = this.EditorPrefsPath + "." + ((!string.IsNullOrEmpty(guilistViewColumn.SerializeName)) ? guilistViewColumn.SerializeName : guilistViewColumn.Text) + ".";
				EditorPrefs.SetFloat(str + "width", guilistViewColumn.Width);
				EditorPrefs.SetInt(str + "index", guilistViewColumn.Index);
				EditorPrefs.SetBool(str + "visible", guilistViewColumn.Visible);
				EditorPrefs.SetInt(str + "sortmode", (int)guilistViewColumn.SortMode);
				EditorPrefs.SetInt(str + "sortprio", (int)guilistViewColumn.SortPrio);
			}
			this.OnSavePrefs();
			return true;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000032FC File Offset: 0x000014FC
		protected virtual void OnSavePrefs()
		{
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003310 File Offset: 0x00001510
		public bool LoadPrefs()
		{
			if (string.IsNullOrEmpty(this.EditorPrefsPath))
			{
				return false;
			}
			this.TryInitColumnIndexes();
			foreach (GUIListViewColumn guilistViewColumn in this.Columns)
			{
				string str = this.EditorPrefsPath + "." + ((!string.IsNullOrEmpty(guilistViewColumn.SerializeName)) ? guilistViewColumn.SerializeName : guilistViewColumn.Text) + ".";
				guilistViewColumn.Width = EditorPrefs.GetFloat(str + "width", guilistViewColumn.Width);
				guilistViewColumn.Index = EditorPrefs.GetInt(str + "index", guilistViewColumn.Index);
				guilistViewColumn.Visible = EditorPrefs.GetBool(str + "visible", guilistViewColumn.Visible);
				guilistViewColumn.SortMode = (GUIListViewSortMode)EditorPrefs.GetInt(str + "sortmode", 0);
				guilistViewColumn.SortPrio = (long)EditorPrefs.GetInt(str + "sortprio", -1);
			}
			this.Columns.Sort((GUIListViewColumn a, GUIListViewColumn b) => a.Index - b.Index);
			this.OnLoadPrefs();
			return true;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000345C File Offset: 0x0000165C
		protected virtual void OnLoadPrefs()
		{
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003460 File Offset: 0x00001660
		private void TryInitColumnIndexes()
		{
			bool flag = true;
			foreach (GUIListViewColumn guilistViewColumn in this.Columns)
			{
				if (guilistViewColumn.Index != 0)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				for (int i = 0; i < this.Columns.Count; i++)
				{
					this.Columns[i].Index = i;
				}
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000034E8 File Offset: 0x000016E8
		public bool IsSelected(object model)
		{
			GUIListView.ItemSelInfo itemSelInfo;
			return model != null && this._selection.TryGetValue(model, out itemSelInfo);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003510 File Offset: 0x00001710
		private object GetLastSelectedItem()
		{
			float num = -1f;
			object result = null;
			foreach (KeyValuePair<object, GUIListView.ItemSelInfo> keyValuePair in this._selection)
			{
				if (keyValuePair.Value.Time > num && keyValuePair.Value.Clicked)
				{
					result = keyValuePair.Key;
					num = keyValuePair.Value.Time;
				}
			}
			return result;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003598 File Offset: 0x00001798
		protected override void DoGUI()
		{
			if (this.HotTracking && !base.Editor.wantsMouseMove)
			{
				throw new GUIListViewException("In order to use GUIListView.HotTracking you must set EditorWindow.wantsMouseMove to true.");
			}
			GUIListView._focusedWindow = EditorWindow.focusedWindow;
			bool enabled = this.Enabled;
			if (!enabled)
			{
				EditorGUI.BeginDisabledGroup(true);
			}
			this._drawid++;
			GUIListView.DrawContext drawContext = default(GUIListView.DrawContext);
			drawContext.CurEvent = Event.current;
			bool flag = false;
			this._selectedItemsPerFrame.Clear();
			foreach (KeyValuePair<object, GUIListView.ItemSelInfo> keyValuePair in this._selection)
			{
				this._selectedItemsPerFrame.Add(keyValuePair.Key);
			}
			drawContext.ControlId = GUIUtility.GetControlID(FocusType.Passive);
			GUI.SetNextControlName(base.ControlName);
			if (Event.current.Equals(Event.KeyboardEvent("tab")) && GUI.GetNameOfFocusedControl() != base.ControlName)
			{
				Event.current.Use();
				GUIListView._activeListView = this;
				if (this._selection.Count == 0 && this.OnGetItemCount() > 0)
				{
					this.SelectItem(this.OnGetItem(0), true);
					this.DoSelectionChange();
				}
			}
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < this.Columns.Count; i++)
			{
				this.Columns[i].Index = i;
				this.Columns[i].RealWidth = this.Columns[i].Width;
				this.Columns[i].DisplayIndex = -1;
				if (this.Columns[i].Visible)
				{
					this.Columns[i].DisplayIndex = num2++;
					num += this.Columns[i].RealWidth + this.ItemMargin.x;
				}
			}
			num += GUI.skin.verticalScrollbar.CalcSize(GUIListView.TempContent("Wg", null, null)).x;
			float num3 = 0f;
			if (this.Mode == GUIListViewMode.Details && this.HeaderStyle != GUIListViewHeaderStyle.None)
			{
				num3 = (float)this.HeaderHeight;
			}
			drawContext.HeaderHeight = num3;
			drawContext.TotalItemCount = this.OnGetItemCount();
			drawContext.ClientRect = GUILayoutUtility.GetRect(1f, 1f, this.LayoutOptions);
			drawContext.VScrollRect = drawContext.ClientRect;
			drawContext.VScrollRect.width = GUI.skin.verticalScrollbar.CalcSize(GUIListView.TempContent("Wg", null, null)).x;
			drawContext.VScrollRect.x = drawContext.ClientRect.xMax - drawContext.VScrollRect.width;
			drawContext.ListRect = drawContext.ClientRect;
			drawContext.ListRect.width = drawContext.ListRect.width - drawContext.VScrollRect.width;
			if (num > drawContext.ListRect.width)
			{
				drawContext.HScrollRect = drawContext.ClientRect;
				drawContext.HScrollRect.height = GUI.skin.horizontalScrollbar.CalcSize(GUIListView.TempContent("Wg", null, null)).y;
				drawContext.HScrollRect.y = drawContext.ClientRect.yMax - drawContext.HScrollRect.height - 1f;
				drawContext.HScrollRect.width = drawContext.HScrollRect.width - drawContext.VScrollRect.width;
				drawContext.ScrollBounds.x = num;
				float num4 = GUI.HorizontalScrollbar(drawContext.HScrollRect, this._scrollbarPos.x, drawContext.ListRect.width, 0f, num);
				this._scrollbarPos.x = num4;
				drawContext.HScrollOffset = -num4;
				drawContext.ListRect.height = drawContext.ListRect.height - drawContext.HScrollRect.height;
				drawContext.VScrollRect.height = drawContext.VScrollRect.height - drawContext.HScrollRect.height;
			}
			drawContext.ColumnCount = 1;
			switch (this.Mode)
			{
			case GUIListViewMode.List:
			case GUIListViewMode.Details:
				drawContext.ColumnCount = 1;
				break;
			case GUIListViewMode.Tile:
				drawContext.ColumnCount = (int)Mathf.Max(1f, drawContext.ListRect.width / (this.ItemSize.x + this.ItemMargin.x));
				break;
			}
			drawContext.TotalRowCount = Mathf.Max(0, drawContext.TotalItemCount / drawContext.ColumnCount);
			drawContext.RowCount = (int)Mathf.Ceil((drawContext.ListRect.height + this.ItemSize.y + this.ItemMargin.y) / (this.ItemSize.y + this.ItemMargin.y));
			drawContext.RowCount = Math.Min(drawContext.RowCount, drawContext.TotalRowCount);
			drawContext.VisibleRowCount = (int)(Mathf.Ceil(drawContext.ListRect.height - num3 + 2f) / (this.ItemSize.y + this.ItemMargin.y));
			drawContext.VisibleRowCount = Math.Min(drawContext.VisibleRowCount, drawContext.TotalRowCount);
			drawContext.TopRow = (int)this._scrollbarPos.y;
			drawContext.TopVisibleRow = drawContext.TopRow;
			drawContext.BottomRow = drawContext.TopRow + drawContext.RowCount - 1;
			drawContext.BottomVisibleRow = drawContext.TopVisibleRow + drawContext.VisibleRowCount - 1;
			drawContext.VScrollOffset = ((float)drawContext.TopRow - this._scrollbarPos.y) * (this.ItemSize.y + this.ItemMargin.y);
			drawContext.FirstItem = drawContext.TopRow * drawContext.ColumnCount;
			drawContext.LastItem = Mathf.Min(drawContext.TotalItemCount - 1, drawContext.BottomRow * drawContext.ColumnCount + drawContext.ColumnCount);
			drawContext.FirstVisibleItem = drawContext.TopVisibleRow * drawContext.ColumnCount;
			drawContext.LastVisibleItem = Mathf.Min(drawContext.TotalItemCount - 1, drawContext.BottomVisibleRow * drawContext.ColumnCount + drawContext.ColumnCount);
			drawContext.VisibleItemCount = drawContext.LastVisibleItem - drawContext.FirstVisibleItem;
			if (drawContext.VisibleItemCount >= 0)
			{
				drawContext.VisibleItemCount++;
			}
			bool flag2 = drawContext.TotalRowCount > drawContext.VisibleRowCount;
			drawContext.ScrollBounds.y = (float)((flag2 ? drawContext.TotalRowCount : 0) - (flag2 ? drawContext.VisibleRowCount : 0));
			float num5 = GUI.VerticalScrollbar(drawContext.VScrollRect, this._scrollbarPos.y, (float)(flag2 ? drawContext.VisibleRowCount : 0), 0f, (float)(flag2 ? drawContext.TotalRowCount : 0));
			if (this._scrollbarPos.y != num5)
			{
				this.SetVScrollbarPos(drawContext, num5);
				base.Editor.Repaint();
			}
			if ((int)Event.current.type != 7 && drawContext.TotalItemCount == this._lastRepaintContext.TotalItemCount)
			{
				drawContext = this._lastRepaintContext;
			}
			drawContext.IsMouseInside = (Event.current.mousePosition.y >= drawContext.ClientRect.yMin && Event.current.mousePosition.x >= drawContext.ClientRect.xMin && Event.current.mousePosition.y <= drawContext.ClientRect.yMax && Event.current.mousePosition.x <= drawContext.ClientRect.xMax);
			if (drawContext.IsMouseInside && Event.current.type == EventType.MouseDown)
			{
				GUIListView._activeListView = this;
				GUI.FocusControl(base.ControlName);
				GUIUtility.keyboardControl = drawContext.ControlId;
				flag = true;
			}
			if (this.PanningEnabled)
			{
				this.DoPanning(drawContext);
			}
			switch (this.Mode)
			{
			case GUIListViewMode.List:
			case GUIListViewMode.Tile:
				this.DoListTileView(ref drawContext);
				break;
			case GUIListViewMode.Details:
				this.DoDetailsView(ref drawContext);
				break;
			}
			if (drawContext.TotalItemCount == 0 && !string.IsNullOrEmpty(this.EmptyText))
			{
				Rect listRect = drawContext.ListRect;
				listRect.x += 10f;
				listRect.width -= 20f;
				listRect.y += 10f;
				listRect.height -= 20f;
				GUI.Label(listRect, this.EmptyText, this.EmptyTextStyle);
			}
			if ((int)Event.current.type == 7)
			{
				foreach (KeyValuePair<object, int> keyValuePair2 in this._visibleItems)
				{
					object key = keyValuePair2.Key;
					int value = keyValuePair2.Value;
					if (value != this._drawid)
					{
						this.OnItemHide(key);
					}
					else
					{
						this._newvisibiles.Add(key, this._drawid);
					}
				}
				Dictionary<object, int> visibleItems = this._visibleItems;
				this._visibleItems = this._newvisibiles;
				this._newvisibiles = visibleItems;
				this._newvisibiles.Clear();
			}
			if (this.HotTracking && (int)Event.current.type == 2)
			{
				base.Editor.Repaint();
			}
			if ((int)Event.current.type == 7)
			{
				this._lastRepaintContext = drawContext;
			}
			if (flag)
			{
				Event.current.Use();
			}
			if (this._animateScroll && Mathf.Abs(this._animateScrollPos.y - this._scrollbarPos.y) > 0.01f)
			{
				this._scrollbarPos.y = this._scrollbarPos.y + (this._animateScrollPos.y - this._scrollbarPos.y) * 0.1f;
				if (Mathf.Abs(this._animateScrollPos.y - this._scrollbarPos.y) < 0.01f)
				{
					this._animateScroll = false;
				}
				base.Editor.Repaint();
			}
			if (!enabled)
			{
				EditorGUI.EndDisabledGroup();
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003FF4 File Offset: 0x000021F4
		private void DoDebugWindow(int id)
		{
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label(string.Format("IsMouseInside: {0}", this._lastRepaintContext.IsMouseInside), new GUILayoutOption[0]);
			GUILayout.Label(string.Format("TopRow: {0}, BottomRow: {1}", this._lastRepaintContext.TopRow, this._lastRepaintContext.BottomRow), new GUILayoutOption[0]);
			GUILayout.Label(string.Format("TopVisRow: {0}, BottomVisRow: {1}", this._lastRepaintContext.TopVisibleRow, this._lastRepaintContext.BottomVisibleRow), new GUILayoutOption[0]);
			GUILayout.Label(string.Format("RowCount: {0}, VisRowCount: {1}", this._lastRepaintContext.RowCount, this._lastRepaintContext.VisibleRowCount), new GUILayoutOption[0]);
			GUILayout.Label(string.Format("VisItemCount: {0}", this._lastRepaintContext.VisibleItemCount), new GUILayoutOption[0]);
			GUILayout.Label(string.Format("FirstItem: {0}, LastItem: {1}", this._lastRepaintContext.FirstItem, this._lastRepaintContext.LastItem), new GUILayoutOption[0]);
			GUILayout.Label(string.Format("FirstVisItem: {0}, LastVisItem: {1}", this._lastRepaintContext.FirstVisibleItem, this._lastRepaintContext.LastVisibleItem), new GUILayoutOption[0]);
			GUILayout.Label(string.Format("HeaderHeight: {0}", this._lastRepaintContext.HeaderHeight), new GUILayoutOption[0]);
			GUILayout.Label(string.Format("HScrollRect: {0}", this._lastRepaintContext.HScrollRect), new GUILayoutOption[0]);
			GUILayout.Label(string.Format("VScrollRect: {0}", this._lastRepaintContext.VScrollRect), new GUILayoutOption[0]);
			GUILayout.Label(string.Format("TotalRows: {0}, TotalItems: {1}", this._lastRepaintContext.TotalRowCount, this._lastRepaintContext.TotalItemCount), new GUILayoutOption[0]);
			GUILayout.Label(string.Format("IsPanning: {0}", this._ispanning), new GUILayoutOption[0]);
			GUILayout.EndVertical();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004228 File Offset: 0x00002428
		private static GUIContent TempContent(string text, Texture image, string tooltip)
		{
			if (GUIListView._tempcontent == null)
			{
				GUIListView._tempcontent = new GUIContent();
			}
			GUIListView._tempcontent.text = text;
			GUIListView._tempcontent.tooltip = tooltip;
			GUIListView._tempcontent.image = image;
			return GUIListView._tempcontent;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004264 File Offset: 0x00002464
		private void DoBackgroundImage(GUIListView.DrawContext context)
		{
			if (null == this.BackgroundImage || (int)Event.current.type != 7)
			{
				return;
			}
			Rect listRect = context.ListRect;
			listRect.y -= context.HeaderHeight;
			Color color = GUI.color;
			GUI.color = this.BackgroundImageColor;
			switch (this.BackgroundImageLayout)
			{
			case GUIListViewBackgroundImageLayout.Tile:
				for (float num = 0f; num < listRect.height; num += (float)this.BackgroundImage.height)
				{
					for (float num2 = 0f; num2 < listRect.width; num2 += (float)this.BackgroundImage.width)
					{
						GUI.DrawTexture(new Rect(num2, num, (float)this.BackgroundImage.width, (float)this.BackgroundImage.height), this.BackgroundImage);
					}
				}
				break;
			case GUIListViewBackgroundImageLayout.Center:
				GUI.DrawTexture(new Rect(listRect.width * 0.5f - (float)this.BackgroundImage.width * 0.5f, listRect.height * 0.5f - (float)this.BackgroundImage.height * 0.5f, (float)this.BackgroundImage.width, (float)this.BackgroundImage.height), this.BackgroundImage);
				break;
			case GUIListViewBackgroundImageLayout.TopLeft:
				GUI.DrawTexture(new Rect(0f, 0f, (float)this.BackgroundImage.width, (float)this.BackgroundImage.height), this.BackgroundImage);
				break;
			case GUIListViewBackgroundImageLayout.TopRight:
				GUI.DrawTexture(new Rect(listRect.width - (float)this.BackgroundImage.width, 0f, (float)this.BackgroundImage.width, (float)this.BackgroundImage.height), this.BackgroundImage);
				break;
			case GUIListViewBackgroundImageLayout.BottomLeft:
				GUI.DrawTexture(new Rect(0f, listRect.height - (float)this.BackgroundImage.height, (float)this.BackgroundImage.width, (float)this.BackgroundImage.height), this.BackgroundImage);
				break;
			case GUIListViewBackgroundImageLayout.BottomRight:
				GUI.DrawTexture(new Rect(listRect.width - (float)this.BackgroundImage.width, listRect.height - (float)this.BackgroundImage.height, (float)this.BackgroundImage.width, (float)this.BackgroundImage.height), this.BackgroundImage);
				break;
			case GUIListViewBackgroundImageLayout.Strech:
				GUI.DrawTexture(new Rect(0f, 0f, listRect.width, listRect.height), this.BackgroundImage, 0);
				break;
			}
			GUI.color = color;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004510 File Offset: 0x00002710
		private void DoPanning(GUIListView.DrawContext context)
		{
			if (!this.HasFocus)
			{
				return;
			}
			bool flag = Event.current.button == 2;
			if (!this._ispanning && context.IsMouseInside && Event.current.type == EventType.MouseDown && flag)
			{
				this._animateScroll = false;
				this._ispanning = true;
				EditorGUIUtility.SetWantsMouseJumping(1);
				EditorGUIUtility.AddCursorRect(context.ListRect, MouseCursor.Zoom);
				Event.current.Use();
			}
			if (this._ispanning && (int)Event.current.rawType == 1 && flag)
			{
				this._ispanning = false;
				EditorGUIUtility.SetWantsMouseJumping(0);
				Event.current.Use();
			}
			if (this._ispanning)
			{
				EditorGUIUtility.AddCursorRect(context.ListRect, MouseCursor.Pan);
				if ((int)Event.current.type == 3 && flag)
				{
					this._scrollbarPos.x = this._scrollbarPos.x - Event.current.delta.x;
					this._scrollbarPos.y = this._scrollbarPos.y - Event.current.delta.y * (1f / this.ItemSize.y);
					this._scrollbarPos.x = Mathf.Clamp(this._scrollbarPos.x, 0f, context.ScrollBounds.x);
					this._scrollbarPos.y = Mathf.Clamp(this._scrollbarPos.y, 0f, context.ScrollBounds.y);
					Event.current.Use();
				}
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004694 File Offset: 0x00002894
		private void DoSelectionChange()
		{
			this.OnSelectionChange();
			if (this.SelectionChange != null)
			{
				this.SelectionChange(this);
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000046B0 File Offset: 0x000028B0
		protected virtual void OnSelectionChange()
		{
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000046B4 File Offset: 0x000028B4
		private void DoListTileView(ref GUIListView.DrawContext context)
		{
			GUI.BeginGroup(new Rect(context.ListRect.x, context.ListRect.y, context.ListRect.width, context.ListRect.height));
			this.DoBackgroundImage(context);
			bool flag = this.CanHandleInput(context);
			int num = 0;
			float num2 = (context.ListRect.width - (this.ItemSize.x + this.ItemMargin.x) * (float)context.ColumnCount) / (float)context.ColumnCount;
			int i = context.FirstItem;
			while (i <= context.LastItem)
			{
				int num3 = i;
				int num4 = 0;
				while (num4 < context.ColumnCount && i <= context.LastItem && context.VisibleItemCount > 0)
				{
					object obj = this.OnGetItem(i);
					bool selected = this.IsSelected(obj);
					context.Column = null;
					context.RowRect = new Rect(context.HScrollOffset, this.ItemMargin.y + context.VScrollOffset + (float)num * (this.ItemSize.y + this.ItemMargin.y), context.ListRect.width + Mathf.Abs(context.HScrollOffset), this.ItemSize.y);
					context.ItemRect = new Rect(context.HScrollOffset + this.ItemMargin.x + (float)num4 * (this.ItemSize.x + this.ItemMargin.x) + num2 * (float)num4, context.RowRect.y, this.ItemSize.x, context.RowRect.height);
					context.Selected = selected;
					context.HotTracking = false;
					context.ItemIndex = i;
					context.Item = obj;
					if (this.HotTracking)
					{
						context.HotTracking = (this.FullRowSelect ? context.RowRect.Contains(Event.current.mousePosition) : context.ItemRect.Contains(Event.current.mousePosition));
					}
					this.DoItemBackground(context);
					if (flag)
					{
						this.DoHandleItemInput(context);
					}
					this.DoItem(context);
					num4++;
					i++;
				}
				if (this.FullRowSelect)
				{
					context.ItemRect = context.RowRect;
					this.DoHandleInput(context);
				}
				if (i == num3)
				{
					break;
				}
				num++;
			}
			this.DoHandleInput(context);
			this.DoPostHandleInput(context);
			GUI.EndGroup();
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00004939 File Offset: 0x00002B39
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00004940 File Offset: 0x00002B40
		public static bool DebugColumnHeader { get; set; }

		// Token: 0x06000069 RID: 105 RVA: 0x00004948 File Offset: 0x00002B48
		private void DoColumnHeader(GUIListView.DrawContext context)
		{
			if (this._columnHeaderTextStyle == null)
			{
				this._columnHeaderTextStyle = new GUIStyle(EditorStyles.label);
			}
			if (this.HeaderStyle == GUIListViewHeaderStyle.None)
			{
				return;
			}
			bool flag = (int)Event.current.type == 1 && Event.current.button == 0;
			GUI.BeginGroup(new Rect(context.ListRect.x, context.ListRect.y, context.ListRect.width, context.HeaderHeight));
			float num = context.HScrollOffset;
			Rect rect;
			rect = new Rect(num, 0f, context.ListRect.width + Mathf.Abs(context.HScrollOffset), context.HeaderHeight);
			EditorGUI.HelpBox(new Rect(rect.x, rect.y - 1f, rect.width, rect.height + 1f), string.Empty, 0);
			if (this.FlexibleColumn != null)
			{
				float num2 = 0f;
				for (int i = 0; i < this.Columns.Count; i++)
				{
					GUIListViewColumn guilistViewColumn = this.Columns[i];
					if (guilistViewColumn.Visible && this.FlexibleColumn != guilistViewColumn)
					{
						num2 += guilistViewColumn.Width;
					}
				}
				this.FlexibleColumn.RealWidth = Mathf.Max(this.FlexibleColumn.Width, context.ListRect.width - num2 - (float)this.Columns.Count);
			}
			for (int j = 0; j < this.Columns.Count; j++)
			{
				GUIListViewColumn guilistViewColumn2 = this.Columns[j];
				if (!guilistViewColumn2.Visible)
				{
					guilistViewColumn2.ColumnRect = default(Rect);
				}
				else
				{
					this._columnHeaderTextStyle.alignment = guilistViewColumn2.TextAlignment;
					Rect rect2;
					rect2=new Rect(num, rect.y, guilistViewColumn2.RealWidth, context.HeaderHeight);
					if ((int)Event.current.type == 7)
					{
						guilistViewColumn2.ColumnRect = new Rect(rect2.x, rect2.y, rect2.width, context.ListRect.height);
					}
					if (this._columnLMBDown == guilistViewColumn2 && rect2.Contains(Event.current.mousePosition))
					{
						Rect rect3 = rect2;
						rect3.x -= 2f;
						rect3.width += 2f;
						EditorGUI.HelpBox(new Rect(rect3.x, rect3.y - 1f, rect3.width, rect3.height + 1f), string.Empty, 0);
					}
					Rect rect4 = rect2;
					rect4.width -= 3f;
					if (GUIListView.DebugColumnHeader)
					{
						GUI.Label(rect4, rect4.width.ToString(), this._columnHeaderTextStyle);
					}
					else
					{
						Vector2 iconSize = EditorGUIUtility.GetIconSize();
						EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
						GUI.Label(rect4, GUIListView.TempContent(guilistViewColumn2.Text, guilistViewColumn2.Image, guilistViewColumn2.Tooltip), this._columnHeaderTextStyle);
						EditorGUIUtility.SetIconSize(iconSize);
					}
					GUI.BeginGroup(guilistViewColumn2.ColumnRect);
					if ((int)Event.current.type == 7)
					{
						int sortIconCount = this.GetSortIconCount(guilistViewColumn2);
						Rect rect5;
						rect5=new Rect(0f, 0f, guilistViewColumn2.ColumnRect.width, guilistViewColumn2.ColumnRect.height);
						rect5.y += 2f;
						rect5.height = (float)GUIListViewColumn.SortIcon.height;
						rect5.x += rect5.width / 2f;
						rect5.x -= (float)(sortIconCount * GUIListViewColumn.SortIcon.width / 2);
						rect5.width = (float)GUIListViewColumn.SortIcon.width;
						for (int k = 0; k < sortIconCount; k++)
						{
							switch (guilistViewColumn2.SortMode)
							{
							case GUIListViewSortMode.Ascending:
								GUI.DrawTextureWithTexCoords(rect5, GUIListViewColumn.SortIcon, new Rect(0f, 0f, 1f, 1f));
								break;
							case GUIListViewSortMode.Descending:
								GUI.DrawTextureWithTexCoords(rect5, GUIListViewColumn.SortIcon, new Rect(0f, 1f, 1f, -1f));
								break;
							}
							rect5.x += rect5.width;
						}
					}
					GUI.EndGroup();
					if ((int)Event.current.type == 7)
					{
						Rect rect6 = rect;
						rect6.x = rect2.x + rect2.width - this.ItemMargin.x;
						rect6.width = 1f;
						Color color = GUI.color;
						GUI.color = new Color(0f, 0f, 0f, 0.1f);
						GUI.DrawTexture(rect6, EditorGUIUtility.whiteTexture);
						GUI.color = color;
					}
					num += guilistViewColumn2.RealWidth + this.ItemMargin.x;
					if (this.HeaderStyle != GUIListViewHeaderStyle.None && this.HeaderStyle != GUIListViewHeaderStyle.Nonclickable)
					{
						Rect rect7 = rect;
						rect7.height -= 1f;
						rect7.x = rect4.x;
						rect7.width = rect4.width;
						//Debug.Log("_columnLMBDown="+_columnLMBDown+"  rect:"+rect7.Contains(Event.current.mousePosition)+" eType:"+(int)Event.current.type);
						if (this._columnLMBDown == guilistViewColumn2 && rect7.Contains(Event.current.mousePosition) && (int)Event.current.type == 1 && Event.current.button == 0 && Event.current.clickCount == 1)
						{
							this.DoColumnClick(guilistViewColumn2);
							this._columnLMBDown = null;
							Event.current.Use();
						}
						
						//Debug.Log("------rect:"+rect7.Contains(Event.current.mousePosition)+" eType:"+Event.current.type);
						if (rect7.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.clickCount == 1)
						{
							this._columnLMBDown = guilistViewColumn2;
							Event.current.Use();
						}
					}
					if (this.FlexibleColumn != guilistViewColumn2 && guilistViewColumn2.IsResizable && !this._columnresize.IsResizing)
					{
						Rect rect8 = rect2;
						rect8.y += 1f;
						rect8.height -= 2f;
						rect8.x += rect8.width - 4f;
						rect8.width = 8f;
						EditorGUIUtility.AddCursorRect(rect8, MouseCursor.ResizeHorizontal);
						if (rect8.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.clickCount == 1)
						{
							this._columnresize.Begin(guilistViewColumn2);
							Event.current.Use();
						}
					}
				}
			}
			if (this._columnresize.IsResizing)
			{
				EditorGUIUtility.AddCursorRect(new Rect(0f, 0f, context.ListRect.width, context.ListRect.height), MouseCursor.ResizeHorizontal);
				if ((int)Event.current.type == 7)
				{
					this._columnresize.Update();
				}
				base.Editor.Repaint();
				if ((int)Event.current.type == 4 && (int)Event.current.keyCode == 27)
				{
					this._columnresize.Cancel();
					Event.current.Use();
				}
				if ((int)Event.current.rawType == 1 && Event.current.button == 0)
				{
					this._columnresize.End();
					Event.current.Use();
				}
			}
			if (this.HeaderStyle == GUIListViewHeaderStyle.ClickablePopup && rect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown && Event.current.button == 1 && Event.current.clickCount == 1)
			{
				GUIUtility.hotControl = 0;
				GenericMenu genericMenu = new GenericMenu();
				for (int l = 0; l < this.Columns.Count; l++)
				{
					GUIListViewColumn guilistViewColumn3 = this.Columns[l];
					string text = string.IsNullOrEmpty(guilistViewColumn3.PopupText) ? guilistViewColumn3.Text : guilistViewColumn3.PopupText;
					genericMenu.AddItem(new GUIContent(text.Replace("/", "\\"), guilistViewColumn3.Tooltip), guilistViewColumn3.Visible, new GenericMenu.MenuFunction2(this.OnColumnPopupMenu), guilistViewColumn3);
				}
				genericMenu.AddItem(new GUIContent(string.Empty), false, null);
				genericMenu.AddItem(new GUIContent("Organize Columns"), false, new GenericMenu.MenuFunction(this.OnOrganizeColumnPopupMenu));
				genericMenu.ShowAsContext();
				Event.current.Use();
			}
			GUI.EndGroup();
			if (!context.IsMouseInside || flag)
			{
				this._columnLMBDown = null;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00005218 File Offset: 0x00003418
		private void OnColumnPopupMenu(object userData)
		{
			GUIListViewColumn guilistViewColumn = (GUIListViewColumn)userData;
			guilistViewColumn.Visible = !guilistViewColumn.Visible;
			base.Editor.Repaint();
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00005248 File Offset: 0x00003448
		private void OnOrganizeColumnPopupMenu()
		{
			GUIListViewOrganizeColumnWindow window = EditorWindow.GetWindow<GUIListViewOrganizeColumnWindow>(true, "Organize Columns", true);
			if (null == window)
			{
				return;
			}
			window.SetListView(this);
			window.ShowUtility();
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000527C File Offset: 0x0000347C
		private bool CanHandleInput(GUIListView.DrawContext context)
		{
		
			if (!context.IsMouseInside)
			{
				return false;
			}
			if (!this.HasFocus)
			{
				return false;
			}
			int hotControl = GUIUtility.hotControl;
			if (hotControl != 0 && hotControl != context.ControlId)
			{
				return false;
			}
			if (GUIUtility.keyboardControl != context.ControlId)
			{
				return false;
			}
			Event current = Event.current;
			switch ((int)current.type)
			{
			case 0:
			case 1:
			case 3:
				goto IL_74;
			case 4:
				if ((int)current.keyCode != 319)
				{
					return false;
				}
				goto IL_74;
			}
			return false;
			IL_74:
			return !this._columnresize.IsResizing;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00005310 File Offset: 0x00003510
		private void DoDetailsView(ref GUIListView.DrawContext context)
		{
			float num = 0f;
			this.DoColumnHeader(context);
			GUI.BeginGroup(new Rect(context.ListRect.x, context.ListRect.y + context.HeaderHeight, context.ListRect.width, context.ListRect.height - 1f - context.HeaderHeight));
			this.DoBackgroundImage(context);
			bool flag = this.CanHandleInput(context);
			int num2 = context.FirstItem;
			while (num2 <= context.LastItem && context.VisibleItemCount > 0)
			{
				context.RowRect = new Rect(context.HScrollOffset, this.ItemMargin.y + context.VScrollOffset + num * (this.ItemSize.y + this.ItemMargin.y), context.ListRect.width + Mathf.Abs(context.HScrollOffset), this.ItemSize.y);
				context.ItemRect = context.RowRect;
				context.Item = this.OnGetItem(num2);
				context.Selected = this.IsSelected(context.Item);
				context.ItemIndex = num2;
				float num3 = this.ItemMargin.x;
				for (int i = 0; i < this.Columns.Count; i++)
				{
					GUIListViewColumn guilistViewColumn = this.Columns[i];
					context.Column = guilistViewColumn;
					context.ItemRect = new Rect(context.HScrollOffset + num3, context.ItemRect.y, guilistViewColumn.RealWidth, context.ItemRect.height);
					context.HotTracking = false;
					if (this.HotTracking)
					{
						if (this.FullRowSelect)
						{
							context.HotTracking = context.RowRect.Contains(Event.current.mousePosition);
						}
						if (!this.FullRowSelect && i == 0)
						{
							context.HotTracking = context.ItemRect.Contains(Event.current.mousePosition);
						}
					}
					if (this.FullRowSelect && guilistViewColumn.IsPrimaryColumn)
					{
						this.DoItemBackground(context);
						if (flag)
						{
							this.DoHandleItemInput(context);
						}
					}
					if (guilistViewColumn.Visible)
					{
						bool flag2 = context.ItemRect.xMax >= 0f && context.ItemRect.xMin <= context.ListRect.width;
						if (flag2)
						{
							if (!this.FullRowSelect)
							{
								this.DoItemBackground(context);
								if (flag)
								{
									this.DoHandleItemInput(context);
								}
							}
							this.DoItem(context);
						}
						num3 += this.ItemMargin.x + guilistViewColumn.RealWidth;
					}
				}
				if (this.FullRowSelect)
				{
					context.ItemRect = context.RowRect;
					this.DoHandleInput(context);
				}
				num += 1f;
				num2++;
			}
			if (this.ShowColumnLines && context.TotalItemCount > 0 && (int)Event.current.type == 7)
			{
				Color color = GUI.color;
				GUI.color = new Color(0f, 0f, 0f, 0.1f);
				float num4 = this.ItemMargin.x;
				for (int j = 0; j < this.Columns.Count; j++)
				{
					GUIListViewColumn guilistViewColumn2 = this.Columns[j];
					if (guilistViewColumn2.Visible)
					{
						Rect rect;
						rect = new Rect(context.HScrollOffset + num4, context.VScrollOffset + num * (this.ItemSize.y + this.ItemMargin.y), guilistViewColumn2.RealWidth, this.ItemSize.y);
						bool flag3 = rect.xMax >= 0f && rect.xMin <= context.ListRect.width;
						if (flag3)
						{
							Rect rect2 = rect;
							rect2.x += rect2.width - 1f - this.ItemMargin.x;
							rect2.width = 1f;
							rect2.y = 0f;
							rect2.height = context.ListRect.height;
							GUI.DrawTexture(rect2, EditorGUIUtility.whiteTexture);
						}
						num4 += this.ItemMargin.x + guilistViewColumn2.RealWidth;
					}
				}
				GUI.color = color;
			}
			GUI.EndGroup();
			this.DoHandleInput(context);
			this.DoPostHandleInput(context);
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600006E RID: 110 RVA: 0x0000578D File Offset: 0x0000398D
		private bool HasFocus
		{
			get
			{
				return !(GUIListView._focusedWindow != base.Editor) && GUIListView._activeListView == this;
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000057B0 File Offset: 0x000039B0
		private void DoItemBackground(GUIListView.DrawContext context)
		{
			Event current = Event.current;
			if ((int)current.type != 7)
			{
				return;
			}
			if (!context.Selected)
			{
				EditorGUIUtility.AddCursorRect(context.RowRect, 0);
			}
			Color color = GUI.color;
			GUIListViewDrawItemBackgroundArgs guilistViewDrawItemBackgroundArgs = default(GUIListViewDrawItemBackgroundArgs);
			guilistViewDrawItemBackgroundArgs.Model = context.Item;
			guilistViewDrawItemBackgroundArgs.ModelIndex = context.ItemIndex;
			guilistViewDrawItemBackgroundArgs.Column = context.Column;
			guilistViewDrawItemBackgroundArgs.Selected = context.Selected;
			guilistViewDrawItemBackgroundArgs.Texture = EditorGUIUtility.whiteTexture;
			guilistViewDrawItemBackgroundArgs.SelectedItems = this._selectedItemsPerFrame;
			guilistViewDrawItemBackgroundArgs.Rect = context.ItemRect;
			guilistViewDrawItemBackgroundArgs.Rect.x = guilistViewDrawItemBackgroundArgs.Rect.x - (this.ItemMargin.x + 1f);
			if (context.Selected)
			{
				if (this.HasFocus)
				{
					guilistViewDrawItemBackgroundArgs.Color = GUIColors.ActiveSelection;
				}
				else
				{
					guilistViewDrawItemBackgroundArgs.Color = GUIColors.InactiveSelection;
				}
			}
			else
			{
				guilistViewDrawItemBackgroundArgs.Color = Color.clear;
				if (context.Column != null)
				{
					guilistViewDrawItemBackgroundArgs.Color = context.Column.Color;
				}
			}
			if (this.FullRowSelect && (context.Column == null || (context.Column != null && context.Column.DisplayIndex == 0)))
			{
				guilistViewDrawItemBackgroundArgs.Rect = context.RowRect;
			}
			if (!this.FullRowSelect && (context.Column == null || (context.Column != null && context.Column.DisplayIndex == 0)))
			{
				guilistViewDrawItemBackgroundArgs.Rect = context.ItemRect;
			}
			this.OnDrawItemBackground(ref guilistViewDrawItemBackgroundArgs);
			if (!guilistViewDrawItemBackgroundArgs.Handled && (int)current.type == 7 && guilistViewDrawItemBackgroundArgs.Color.a > 0f)
			{
				GUI.color = guilistViewDrawItemBackgroundArgs.Color;
				GUI.DrawTexture(guilistViewDrawItemBackgroundArgs.Rect, guilistViewDrawItemBackgroundArgs.Texture);
			}
			GUI.color = color;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00005987 File Offset: 0x00003B87
		protected virtual void OnDrawItemBackground(ref GUIListViewDrawItemBackgroundArgs args)
		{
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000598C File Offset: 0x00003B8C
		protected void DoChanged()
		{
			if (this.Changed != null)
			{
				Action<GUIListView> changed = this.Changed;
				changed(this);
			}
			base.Editor.Repaint();
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000059BC File Offset: 0x00003BBC
		private void DoItem(GUIListView.DrawContext context)
		{
			Event current = Event.current;
			if ((int)current.type == 7)
			{
				this._visibleItems[context.Item] = this._drawid;
			}
			if ((int)current.type == 8)
			{
				return;
			}
			GUIListViewDrawItemArgs args = default(GUIListViewDrawItemArgs);
			args.Model = context.Item;
			args.ModelIndex = context.ItemIndex;
			args.ItemRect = context.ItemRect;
			args.Column = context.Column;
			args.Selected = context.Selected;
			if (context.Column == null || context.Column.DisplayIndex == 0)
			{
				args.ItemRect.x = args.ItemRect.x + 2f;
				args.ItemRect.width = args.ItemRect.width - 2f;
			}
			Color contentColor = GUI.contentColor;
			GUI.contentColor = Color.white;
			this.OnDrawItem(args);
			GUI.contentColor = contentColor;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00005AAC File Offset: 0x00003CAC
		protected static Rect DrawItemImageHelper(ref Rect itemrect, Texture image, Vector2 imagesize)
		{
			if (null == image)
			{
				return default(Rect);
			}
			Rect rect = itemrect;
			rect.width = imagesize.x;
			rect.height = imagesize.y;
			rect.y += (itemrect.height - rect.height) * 0.5f;
			if ((int)Event.current.type == 7)
			{
				GUI.DrawTexture(rect, image, ScaleMode.ScaleAndCrop, true);
			}
			itemrect.x += rect.width + 1f;
			itemrect.width -= rect.width + 1f;
			return rect;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00005B5C File Offset: 0x00003D5C
		protected static Rect DrawItemImageHelper(ref Rect itemrect, GUIContent content, Vector2 imagesize)
		{
			Rect rect = itemrect;
			rect.width = imagesize.x;
			rect.height = imagesize.y;
			rect.y += (itemrect.height - rect.height) * 0.5f;
			GUI.Label(rect, content, GUIStyles.Clear);
			itemrect.x += rect.width + 1f;
			itemrect.width -= rect.width + 1f;
			return rect;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00005BEF File Offset: 0x00003DEF
		private void SetVScrollbarPos(GUIListView.DrawContext context, float value)
		{
			this._animateScroll = false;
			this._scrollbarPos.y = Mathf.Clamp(value, 0f, context.ScrollBounds.y);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00005C1A File Offset: 0x00003E1A
		private bool IsItemVisible(GUIListView.DrawContext context, int index)
		{
			return index >= context.FirstVisibleItem && index <= context.LastVisibleItem;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00005C38 File Offset: 0x00003E38
		public void ScrollIntoView(int index)
		{
			this._animateScrollPos.x = this._scrollbarPos.x;
			this._animateScroll = true;
			GUIListView.DrawContext lastRepaintContext = this._lastRepaintContext;
			this._animateScrollPos.y = Mathf.Clamp(((float)index - (float)(lastRepaintContext.VisibleItemCount + lastRepaintContext.ColumnCount - 1) * 0.5f) / (float)lastRepaintContext.ColumnCount, 0f, lastRepaintContext.ScrollBounds.y);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00005CB0 File Offset: 0x00003EB0
		private void EnsureVisible(GUIListView.DrawContext context, int index)
		{
			if (index < context.FirstVisibleItem)
			{
				this.SetVScrollbarPos(context, (float)index / (float)context.ColumnCount);
			}
			if (index >= context.LastVisibleItem)
			{
				this.SetVScrollbarPos(context, (float)(index - context.VisibleItemCount + context.ColumnCount) / (float)context.ColumnCount);
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00005D08 File Offset: 0x00003F08
		private void DoHandleInput(GUIListView.DrawContext context)
		{
			Event current = Event.current;
			Rect rect;
			rect = new Rect(0f, 0f, context.ClientRect.width, context.ClientRect.height);
			if (rect.Contains(current.mousePosition) && (int)current.type == 6)
			{
				this.SetVScrollbarPos(context, this._scrollbarPos.y + current.delta.y * 0.2f);
				current.Use();
			}
			if (!this.HasFocus)
			{
				return;
			}
			if (GUIUtility.hotControl != 0 && GUIUtility.hotControl != context.ControlId)
			{
				return;
			}
			if (GUIUtility.keyboardControl != context.ControlId)
			{
				return;
			}
			if (((int)current.type == 13 || (int)current.type == 14) && string.Equals(current.commandName, "SelectAll", StringComparison.OrdinalIgnoreCase))
			{
				if ((int)current.type == 14)
				{
					this._selection.Clear();
					for (int i = 0; i < this.OnGetItemCount(); i++)
					{
						this._selection.Add(this.OnGetItem(i), new GUIListView.ItemSelInfo(Time.realtimeSinceStartup, false));
					}
					this.DoSelectionChange();
				}
				current.Use();
			}
			if ((int)current.type == 4 && (int)current.keyCode == 281)
			{
				int num = this.FindItemIndex(this._activeItem);
				if (this.IsItemVisible(context, num + context.ColumnCount + 1))
				{
					num = context.LastVisibleItem;
				}
				else
				{
					this.SetVScrollbarPos(context, (float)(num / context.ColumnCount));
					num += context.VisibleItemCount;
					num = Mathf.Min(context.TotalItemCount - 1, num);
				}
				if (num >= 0 && num < context.TotalItemCount)
				{
					if (current.shift && this.MultiSelect)
					{
						this.DoHandleShiftSelection(num);
					}
					else
					{
						this._selection.Clear();
						this._selection.Add(this.OnGetItem(num), new GUIListView.ItemSelInfo(Time.realtimeSinceStartup, true));
						this.DoSelectionChange();
					}
					this._activeItem = this.OnGetItem(num);
				}
				current.Use();
			}
			if ((int)current.type == 4 && (int)current.keyCode == 280)
			{
				int num2 = this.FindItemIndex(this._activeItem);
				if (this.IsItemVisible(context, num2 - context.ColumnCount - 1))
				{
					num2 = context.FirstVisibleItem;
				}
				else
				{
					num2 -= context.VisibleItemCount;
					num2 = Mathf.Max(0, num2);
					this.SetVScrollbarPos(context, (float)(num2 / context.ColumnCount));
				}
				if (num2 >= 0 && num2 < context.TotalItemCount)
				{
					if (current.shift && this.MultiSelect)
					{
						this.DoHandleShiftSelection(num2);
					}
					else
					{
						this._selection.Clear();
						this._selection.Add(this.OnGetItem(num2), new GUIListView.ItemSelInfo(Time.realtimeSinceStartup, true));
						this.DoSelectionChange();
					}
					this._activeItem = this.OnGetItem(num2);
				}
				current.Use();
			}
			if ((int)current.type == 4 && (int)current.keyCode == 278)
			{
				this.EnsureVisible(context, 0);
				if (current.shift && this.MultiSelect)
				{
					if (context.TotalItemCount > 0)
					{
						this.DoHandleShiftSelection(0);
					}
				}
				else
				{
					this._selection.Clear();
					if (context.TotalItemCount > 0)
					{
						this._selection.Add(this.OnGetItem(0), new GUIListView.ItemSelInfo(Time.realtimeSinceStartup, true));
						this.DoSelectionChange();
					}
				}
				if (context.TotalItemCount > 0)
				{
					this._activeItem = this.OnGetItem(0);
				}
				current.Use();
			}
			if ((int)current.type == 4 && (int)current.keyCode == 279)
			{
				this.EnsureVisible(context, context.TotalItemCount - 1);
				if (current.shift && this.MultiSelect)
				{
					if (context.TotalItemCount > 0)
					{
						this.DoHandleShiftSelection(this.OnGetItemCount() - 1);
					}
				}
				else
				{
					this._selection.Clear();
					if (context.TotalItemCount > 0)
					{
						this._selection.Add(this.OnGetItem(context.TotalItemCount - 1), new GUIListView.ItemSelInfo(Time.realtimeSinceStartup, true));
						this.DoSelectionChange();
					}
				}
				if (context.TotalItemCount > 0)
				{
					this._activeItem = this.OnGetItem(this.OnGetItemCount() - 1);
				}
				current.Use();
			}
			if (context.ColumnCount > 1 && (int)current.type == 4 && (int)current.keyCode == 276 && !current.control)
			{
				for (int j = 1; j < context.TotalItemCount; j++)
				{
					object obj = this.OnGetItem(j);
					if (object.ReferenceEquals(obj, this._activeItem))
					{
						int num3 = Math.Max(0, j - 1);
						obj = this.OnGetItem(num3);
						if (current.shift && this.MultiSelect)
						{
							this.DoHandleShiftSelection(num3);
						}
						else
						{
							this._selection.Clear();
							this._selection.Add(obj, new GUIListView.ItemSelInfo(Time.realtimeSinceStartup, true));
							this.DoSelectionChange();
						}
						this._activeItem = obj;
						this.EnsureVisible(context, num3);
						break;
					}
				}
				current.Use();
			}
			if ((int)current.type == 4 && (int)current.keyCode == 273 && !current.control)
			{
				for (int k = 1; k < context.TotalItemCount; k++)
				{
					object objA = this.OnGetItem(k);
					if (object.ReferenceEquals(objA, this._activeItem))
					{
						int num4 = Math.Max(0, k - context.ColumnCount);
						object obj2 = this.OnGetItem(num4);
						if (current.shift && this.MultiSelect)
						{
							this.DoHandleShiftSelection(num4);
						}
						else
						{
							this._selection.Clear();
							this._selection.Add(obj2, new GUIListView.ItemSelInfo(Time.realtimeSinceStartup, true));
							this.DoSelectionChange();
						}
						this._activeItem = obj2;
						this.EnsureVisible(context, num4);
						break;
					}
				}
				current.Use();
			}
			if (context.ColumnCount > 1 && (int)current.type == 4 && (int)current.keyCode == 275 && !current.control)
			{
				for (int l = 0; l < context.TotalItemCount - 1; l++)
				{
					object obj3 = this.OnGetItem(l);
					if (object.ReferenceEquals(obj3, this._activeItem))
					{
						int index = Math.Min(context.TotalItemCount - 1, l + 1);
						obj3 = this.OnGetItem(index);
						if (current.shift && this.MultiSelect)
						{
							this.DoHandleShiftSelection(l);
						}
						else
						{
							this._selection.Clear();
							this._selection.Add(obj3, new GUIListView.ItemSelInfo(Time.realtimeSinceStartup, true));
							this.DoSelectionChange();
						}
						this._activeItem = obj3;
						this.EnsureVisible(context, index);
						break;
					}
				}
				current.Use();
			}
			if ((int)current.type == 4 && (int)current.keyCode == 274 && !current.control)
			{
				for (int m = 0; m < context.TotalItemCount - 1; m++)
				{
					object objA2 = this.OnGetItem(m);
					if (object.ReferenceEquals(objA2, this._activeItem))
					{
						int num5 = Math.Min(context.TotalItemCount - 1, m + context.ColumnCount);
						object obj4 = this.OnGetItem(num5);
						if (current.shift && this.MultiSelect)
						{
							this.DoHandleShiftSelection(num5);
						}
						else
						{
							this._selection.Clear();
							this._selection.Add(obj4, new GUIListView.ItemSelInfo(Time.realtimeSinceStartup, true));
							this.DoSelectionChange();
						}
						this._activeItem = obj4;
						this.EnsureVisible(context, num5);
						break;
					}
				}
				current.Use();
			}
			if ((int)current.type == 4 && (int)current.keyCode == 274 && current.control)
			{
				this.SetVScrollbarPos(context, this._scrollbarPos.y + 0.5f);
				current.Use();
			}
			if ((int)current.type == 4 && (int)current.keyCode == 273 && current.control)
			{
				this.SetVScrollbarPos(context, this._scrollbarPos.y - 0.5f);
				current.Use();
			}
			if ((int)current.type == 4 && (int)current.keyCode == 275 && current.control)
			{
				this._scrollbarPos.x = this._scrollbarPos.x + 10f;
				current.Use();
			}
			if ((int)current.type == 4 && (int)current.keyCode == 276 && current.control)
			{
				this._scrollbarPos.x = this._scrollbarPos.x - 10f;
				current.Use();
			}
			if ((int)current.type == 4 && current.isKey && !current.control && current.character >= ' ')
			{
				float num6 = Time.realtimeSinceStartup - this._lastKeyPressTime;
				if (num6 > 0.7f)
				{
					this._findWord = "";
				}
				this._findWord += current.character;
				this._lastKeyPressTime = Time.realtimeSinceStartup;
				current.Use();
				int num7 = this.DoFindItemByKeyword(context);
				if (num7 != -1)
				{
					object obj5 = this.OnGetItem(num7);
					this._selection.Clear();
					this._selection.Add(obj5, new GUIListView.ItemSelInfo(Time.realtimeSinceStartup, true));
					this.DoSelectionChange();
					this._activeItem = obj5;
					this.ScrollIntoView(num7);
				}
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00006668 File Offset: 0x00004868
		private int DoFindItemByKeyword(GUIListView.DrawContext context)
		{
			GUIListViewGetItemKeywordArgs args = default(GUIListViewGetItemKeywordArgs);
			args.Column = this.FirstVisibleColumn;
			int num = 0;
			if (this.SelectedItemsCount > 0)
			{
				num = this.FindItemIndex(this.SelectedItems[0]) + 1;
			}
			int num2 = this.OnGetItemCount();
			for (int i = num; i < num2 + num; i++)
			{
				args.ModelIndex = i % num2;
				args.Model = this.OnGetItem(args.ModelIndex);
				string text = this.OnGetItemKeyword(args);
				if (text != null && text.StartsWith(this._findWord, StringComparison.OrdinalIgnoreCase))
				{
					return args.ModelIndex;
				}
			}
			return -1;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00006700 File Offset: 0x00004900
		private void DoPostHandleInput(GUIListView.DrawContext context)
		{
			Event current = Event.current;
			Rect rect;
			rect =new Rect(context.ListRect.x, (float)(context.VisibleRowCount + 1) * (this.ItemSize.y + this.ItemMargin.y) + context.HeaderHeight, context.ListRect.width, Mathf.Max(0f, context.ListRect.height - (float)(context.VisibleRowCount + 1) * (this.ItemSize.y + this.ItemMargin.y)));
			bool flag = current.button == 0 && current.type == EventType.MouseDown && rect.Contains(current.mousePosition);
			bool flag2 = current.button == 1 && current.type == EventType.MouseDown && rect.Contains(current.mousePosition);
			bool flag3 = flag || (this.RightClickSelect && flag2);
			if (flag3 && !current.control && !current.shift)
			{
				current.Use();
				if (this._selection.Count > 0)
				{
					this._selection.Clear();
					this.DoSelectionChange();
				}
			}
			if ((int)current.type == 4 && current.isKey && this._activeItem != null)
			{
				this.DoItemKeyDown();
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00006844 File Offset: 0x00004A44
		private int FindItemIndex(object item)
		{
			int num = this.OnGetItemCount();
			for (int i = 0; i < num; i++)
			{
				object objA = this.OnGetItem(i);
				if (object.ReferenceEquals(objA, item))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00006878 File Offset: 0x00004A78
		private void SelectItem(object item, bool clicked)
		{
			if (!this._selection.ContainsKey(item))
			{
				this._selection.Add(item, new GUIListView.ItemSelInfo(Time.realtimeSinceStartup, clicked));
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000068A0 File Offset: 0x00004AA0
		private void DoHandleShiftSelection(int toindex)
		{
			object lastSelectedItem = this.GetLastSelectedItem();
			int num = this.FindItemIndex(lastSelectedItem);
			if (num != -1)
			{
				GUIListView.ItemSelInfo value = this._selection[lastSelectedItem];
				this._selection.Clear();
				this._selection.Add(lastSelectedItem, value);
				int num2 = 0;
				while (toindex != num)
				{
					object obj = this.OnGetItem(num);
					if (obj != lastSelectedItem)
					{
						this.SelectItem(obj, false);
					}
					num += (int)Mathf.Sign((float)(toindex - num));
					if (++num2 > 1000000)
					{
						break;
					}
				}
				this._activeItem = this.OnGetItem(toindex);
				this.SelectItem(this._activeItem, false);
				this.DoSelectionChange();
			}
			base.Editor.Repaint();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00006948 File Offset: 0x00004B48
		private void DoHandleItemInput(GUIListView.DrawContext context)
		{
			Event current = Event.current;
			Rect itemRect = this.FullRowSelect ? context.RowRect : context.ItemRect;
			bool flag = current.button == 0 && current.type == EventType.MouseDown && itemRect.Contains(current.mousePosition);
			bool flag2 = current.button == 0 && (int)current.type == 1 && itemRect.Contains(current.mousePosition);
			bool flag3 = current.button == 1 && current.type == EventType.MouseDown && itemRect.Contains(current.mousePosition);
			bool flag4 = current.button == 1 && (int)current.type == 1 && itemRect.Contains(current.mousePosition);
			bool flag5 = current.button == 0 && (int)current.type == 3 && itemRect.Contains(current.mousePosition);
			bool flag6 = flag;
			if (this.RightClickSelect)
			{
				flag6 = (flag || flag3);
			}
			bool flag7 = (int)current.keyCode == 319 && (int)current.type == 4 && object.ReferenceEquals(context.Item, this._activeItem);
			if ((current.clickCount == 1 && flag4) || flag7)
			{
				this._activeItem = context.Item;
				this.DoItemContextMenu(context, !flag7);
				current.Use();
			}
			if (!itemRect.Contains(current.mousePosition))
			{
				return;
			}
//			if (flag && !this.OnIsItemPositionClickable(new GUIListViewIsItemPositionClickableArgs
//			{
//				Model = context.Item,
//				ModelIndex = context.ItemIndex,
//				ItemRect = itemRect,
//				Column = context.Column,
//				MousePosition = current.mousePosition
//			}))
//			{
//				return;
//			}
			if (this.DragDropEnabled && flag5 && this._columnLMBDown == null && (context.ColumnCount == 0 || this.DragColumn == null || this.DragColumn.ColumnRect.Contains(current.mousePosition)))
			{
				this._activeItem = context.Item;
				this.DoBeginDrag(context.ItemIndex);
				current.Use();
			}
			if (current.clickCount == 1 && flag6)
			{
				this._activeItem = context.Item;
				this.DoItemClick(context);
				base.Editor.Repaint();
			}
			if (current.clickCount == 2 && flag6)
			{
				this._activeItem = context.Item;
				this.DoItemDoubleClick(context);
				base.Editor.Repaint();
			}
			if (this.MultiSelect && this._selection.Count > 0 && current.clickCount == 1 && current.control && current.shift && flag6)
			{
				int num = this.FindItemIndex(this.GetLastSelectedItem());
				if (num != -1)
				{
					int num2 = 0;
					while (context.ItemIndex != num)
					{
						this.SelectItem(this.OnGetItem(num), false);
						num += (int)Mathf.Sign((float)(context.ItemIndex - num));
						if (++num2 > 1000000)
						{
							break;
						}
					}
					this._activeItem = context.Item;
					this.SelectItem(context.Item, false);
					this.DoSelectionChange();
					current.Use();
				}
				base.Editor.Repaint();
				return;
			}
			if (this.MultiSelect && this._selection.Count > 0 && current.clickCount == 1 && !current.control && current.shift && flag6)
			{
				this._activeItem = context.Item;
				this.DoHandleShiftSelection(context.ItemIndex);
				current.Use();
				base.Editor.Repaint();
				return;
			}
			if (current.clickCount == 1 && current.control && flag6)
			{
				this._activeItem = context.Item;
				if (!this.MultiSelect)
				{
					this._selection.Clear();
				}
				if (context.Selected)
				{
					this._selection.Remove(context.Item);
				}
				else
				{
					this._selection.Add(context.Item, new GUIListView.ItemSelInfo(Time.realtimeSinceStartup, true));
				}
				this.DoSelectionChange();
				if (context.Selected != this.IsSelected(context.Item))
				{
					current.Use();
				}
				base.Editor.Repaint();
				return;
			}
			if (current.clickCount == 1 && !current.control && flag6 && !context.Selected)
			{
				this._activeItem = context.Item;
				this._selection.Clear();
				this._selection.Add(context.Item, new GUIListView.ItemSelInfo(Time.realtimeSinceStartup, true));
				this.DoSelectionChange();
				current.Use();
				base.Editor.Repaint();
				return;
			}
			if (current.clickCount == 1 && !current.control && !current.shift && flag2 && context.Selected)
			{
				this._activeItem = context.Item;
				int count = this._selection.Count;
				this._selection.Clear();
				this._selection.Add(context.Item, new GUIListView.ItemSelInfo(Time.realtimeSinceStartup, true));
				if (!context.Selected || count != this._selection.Count)
				{
					this.DoSelectionChange();
					current.Use();
				}
				base.Editor.Repaint();
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00006E8C File Offset: 0x0000508C
		public void Sort(GUIListViewColumn column, GUIListViewSortMode mode)
		{
			foreach (GUIListViewColumn guilistViewColumn in this.Columns)
			{
				guilistViewColumn.SortMode = GUIListViewSortMode.None;
			}
			if (column != null)
			{
				column.SortMode = mode;
				column.SortPrio = DateTime.Now.Ticks;
			}
			this.Sort();
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00006F04 File Offset: 0x00005104
		public void Sort()
		{
			this.DoSortItems();
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00006F30 File Offset: 0x00005130
		private bool DoSortItems()
		{
			object[] array = this.OnBeforeSortItems();
			if (array == null)
			{
				return false;
			}
			bool flag = false;
			foreach (GUIListViewColumn guilistViewColumn in this.Columns)
			{
				if (guilistViewColumn.SortMode != GUIListViewSortMode.None)
				{
					flag = true;
					break;
				}
			}
			GUIListViewColumn firstVisibleColumn = this.FirstVisibleColumn;
			if (!flag && firstVisibleColumn != null)
			{
				firstVisibleColumn.SortMode = GUIListViewSortMode.Ascending;
				firstVisibleColumn.SortPrio = DateTime.Now.Ticks;
			}
			GUIListView.GUIListViewModelComparer guilistViewModelComparer = new GUIListView.GUIListViewModelComparer();
			foreach (GUIListViewColumn guilistViewColumn2 in this.Columns)
			{
				if (guilistViewColumn2.SortMode != GUIListViewSortMode.None)
				{
					guilistViewModelComparer.Columns.Add(guilistViewColumn2);
				}
			}
			guilistViewModelComparer.Columns.Sort(delegate(GUIListViewColumn a, GUIListViewColumn b)
			{
				if (a.SortPrio < b.SortPrio)
				{
					return -1;
				}
				if (a.SortPrio > b.SortPrio)
				{
					return 1;
				}
				return 0;
			});
			if (guilistViewModelComparer.Columns.Count > 0)
			{
				Array.Sort<object>(array, guilistViewModelComparer);
			}
			this.OnAfterSortItems(array);
			this.DoChanged();
			return true;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00007068 File Offset: 0x00005268
		protected virtual object[] OnBeforeSortItems()
		{
			return null;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000706B File Offset: 0x0000526B
		protected virtual void OnAfterSortItems(object[] models)
		{
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00007070 File Offset: 0x00005270
		public void SaveCsv()
		{
			string text = EditorPrefs.GetString(string.Format("{0}.RecentCsvExportPath", this.EditorPrefsPath), Path.Combine(EditorApplication2.ProjectPath, EditorApplication2.ProjectName + ".csv"));
			text = EditorUtility.SaveFilePanel("Export as CSV...", FileUtil2.GetDirectoryName(text), EditorApplication2.ProjectName, "csv");
			text = text.Replace('\\', '/');
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			EditorPrefs.SetString(string.Format("{0}.RecentCsvExportPath", this.EditorPrefsPath), text);
			StringBuilder stringBuilder = this.ExportCsv();
			try
			{
				File.WriteAllText(text, stringBuilder.ToString(), Encoding.UTF8);
			}
			catch (Exception ex)
			{
				string text2 = string.Format("Could not write file '{0}'.\n\nMake sure the file is not in use by another application.", text);
				EditorUtility.DisplayDialog("ERROR", text2, "OK");
				throw ex;
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000713C File Offset: 0x0000533C
		public StringBuilder ExportCsv()
		{
			return this.ExportCsv(',');
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00007148 File Offset: 0x00005348
		public StringBuilder ExportCsv(char separator)
		{
			StringBuilder stringBuilder = new StringBuilder(this.OnGetItemCount() * 256);
			for (int i = 0; i < this.Columns.Count; i++)
			{
				GUIListViewColumn guilistViewColumn = this.Columns[i];
				stringBuilder.Append('"' + guilistViewColumn.Text + '"');
				if (i + 1 < this.Columns.Count)
				{
					stringBuilder.Append(separator);
				}
			}
			stringBuilder.Append('\n');
			for (int j = 0; j < this.OnGetItemCount(); j++)
			{
				object model = this.OnGetItem(j);
				GUIListViewGetItemTextArgs args = default(GUIListViewGetItemTextArgs);
				args.Model = model;
				args.ModelIndex = j;
				for (int k = 0; k < this.Columns.Count; k++)
				{
					args.Column = this.Columns[k];
					string arg = this.OnGetItemText(args) ?? "";
					stringBuilder.Append('"' + arg + '"');
					if (k + 1 < this.Columns.Count)
					{
						stringBuilder.Append(separator);
					}
				}
				stringBuilder.Append('\n');
			}
			return stringBuilder;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00007284 File Offset: 0x00005484
		private void DoBeginDrag(int index)
		{
			UnityEngine.Object[] array;
			string[] array2;
			this.OnBeginDrag(index, out array, out array2);
			if (array != null || array2 != null)
			{
				if (array != null)
				{
					DragAndDrop.objectReferences = array;
				}
				if (array2 != null)
				{
					DragAndDrop.paths = array2;
				}
				DragAndDrop.PrepareStartDrag();
				DragAndDrop.StartDrag("");
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000072C3 File Offset: 0x000054C3
		protected virtual void OnBeginDrag(int index, out UnityEngine.Object[] objects, out string[] paths)
		{
			objects = null;
			paths = null;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000072CC File Offset: 0x000054CC
		private int GetSortIconCount(GUIListViewColumn column)
		{
			if (column.SortMode == GUIListViewSortMode.None)
			{
				return -1;
			}
			int num = 1;
			foreach (GUIListViewColumn guilistViewColumn in this.Columns)
			{
				if (guilistViewColumn != column && guilistViewColumn.SortPrio != -1L && guilistViewColumn.SortPrio < column.SortPrio)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00007348 File Offset: 0x00005548
		private void DoColumnClick(GUIListViewColumn column)
		{
			if (column.CompareFunc == null)
			{
				return;
			}
			GUIListViewSortMode sortMode = GUIListViewSortMode.None;
			switch (column.SortMode)
			{
			case GUIListViewSortMode.None:
			case GUIListViewSortMode.Descending:
				sortMode = GUIListViewSortMode.Ascending;
				break;
			case GUIListViewSortMode.Ascending:
				sortMode = GUIListViewSortMode.Descending;
				break;
			}
			Event current = Event.current;
			if (current.control && current.shift)
			{
				column.SortPrio = -1L;
				sortMode = GUIListViewSortMode.None;
			}
			else if (current.control)
			{
				if (column.SortPrio == -1L)
				{
					column.SortPrio = DateTime.Now.Ticks;
				}
			}
			else
			{
				for (int i = 0; i < this.Columns.Count; i++)
				{
					this.Columns[i].SortPrio = -1L;
					if (column != this.Columns[i])
					{
						this.Columns[i].SortMode = GUIListViewSortMode.None;
					}
				}
				column.SortPrio = DateTime.Now.Ticks;
			}
			column.SortMode = sortMode;
			this.DoSortItems();
			if (this.ColumnClick != null)
			{
				this.ColumnClick(this, column);
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00007450 File Offset: 0x00005650
		private void DoItemContextMenu(GUIListView.DrawContext context, bool fromMouse)
		{
			GUIListViewContextMenuArgs guilistViewContextMenuArgs = new GUIListViewContextMenuArgs
			{
				Model = context.Item,
				ModelIndex = context.ItemIndex,
				Column = context.Column,
				Selected = context.Selected,
				MenuLocation = (fromMouse ? Event.current.mousePosition : new Vector2(context.ItemRect.x, context.ItemRect.y))
			};
			this.OnItemContextMenu(guilistViewContextMenuArgs);
			if (this.ItemContextMenu != null)
			{
				this.ItemContextMenu(this, guilistViewContextMenuArgs);
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000074EC File Offset: 0x000056EC
		protected virtual void OnItemContextMenu(GUIListViewContextMenuArgs args)
		{
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000074F0 File Offset: 0x000056F0
		private void DoItemClick(GUIListView.DrawContext context)
		{
			GUIListViewItemClickArgs guilistViewItemClickArgs = new GUIListViewItemClickArgs
			{
				Model = context.Item,
				ModelIndex = context.ItemIndex
			};
			this.OnItemClick(guilistViewItemClickArgs);
			if (this.ItemClick != null)
			{
				this.ItemClick(this, guilistViewItemClickArgs);
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000753D File Offset: 0x0000573D
		protected virtual void OnItemClick(GUIListViewItemClickArgs args)
		{
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00007540 File Offset: 0x00005740
		private void DoItemKeyDown()
		{
			GUIListViewItemKeyDownArgs guilistViewItemKeyDownArgs = default(GUIListViewItemKeyDownArgs);
			guilistViewItemKeyDownArgs.Model = this._activeItem;
			guilistViewItemKeyDownArgs.ModelIndex = this.FindItemIndex(this._activeItem);
			this.OnItemKeyDown(ref guilistViewItemKeyDownArgs);
			if (guilistViewItemKeyDownArgs.Handled)
			{
				Event.current.Use();
				base.Editor.Repaint();
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000759B File Offset: 0x0000579B
		protected virtual void OnItemKeyDown(ref GUIListViewItemKeyDownArgs args)
		{
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000075A0 File Offset: 0x000057A0
		private void DoItemDoubleClick(GUIListView.DrawContext context)
		{
			GUIListViewItemDoubleClickArgs guilistViewItemDoubleClickArgs = new GUIListViewItemDoubleClickArgs
			{
				Model = context.Item,
				ModelIndex = context.ItemIndex
			};
			this.OnItemDoubleClick(guilistViewItemDoubleClickArgs);
			if (this.ItemDoubleClick != null)
			{
				this.ItemDoubleClick(this, guilistViewItemDoubleClickArgs);
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000075ED File Offset: 0x000057ED
		protected virtual void OnItemDoubleClick(GUIListViewItemDoubleClickArgs args)
		{
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000075EF File Offset: 0x000057EF
		protected virtual void OnItemHide(object model)
		{
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000075F1 File Offset: 0x000057F1
		protected virtual string OnGetItemText(GUIListViewGetItemTextArgs args)
		{
			return null;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000075F4 File Offset: 0x000057F4
		protected virtual string OnGetItemKeyword(GUIListViewGetItemKeywordArgs args)
		{
			return null;
		}

		// Token: 0x06000097 RID: 151
		protected abstract int OnGetItemCount();

		// Token: 0x06000098 RID: 152
		protected abstract object OnGetItem(int index);

		// Token: 0x06000099 RID: 153
		protected abstract void OnDrawItem(GUIListViewDrawItemArgs args);

		// Token: 0x0600009A RID: 154 RVA: 0x000075F7 File Offset: 0x000057F7
		protected virtual bool OnIsItemPositionClickable(GUIListViewIsItemPositionClickableArgs args)
		{
			return false;
		}

		// Token: 0x04000025 RID: 37
		private Vector2 _scrollbarPos;

		// Token: 0x04000026 RID: 38
		private Dictionary<object, GUIListView.ItemSelInfo> _selection = new Dictionary<object, GUIListView.ItemSelInfo>();

		// Token: 0x04000027 RID: 39
		private Dictionary<object, int> _visibleItems = new Dictionary<object, int>();

		// Token: 0x04000028 RID: 40
		private Dictionary<object, int> _newvisibiles = new Dictionary<object, int>();

		// Token: 0x04000029 RID: 41
		private int _drawid;

		// Token: 0x0400002A RID: 42
		private GUIListView.DrawContext _lastRepaintContext;

		// Token: 0x0400002B RID: 43
		private GUIListViewResizeColumnContext _columnresize;

		// Token: 0x0400002C RID: 44
		private string _emptyText;

		// Token: 0x0400002D RID: 45
		private GUIStyle _emptyTextStyle;

		// Token: 0x0400002E RID: 46
		public Action<GUIListView, GUIListViewColumn> ColumnClick;

		// Token: 0x0400002F RID: 47
		public Action<GUIListView, GUIListViewItemClickArgs> ItemClick;

		// Token: 0x04000030 RID: 48
		public Action<GUIListView, GUIListViewItemDoubleClickArgs> ItemDoubleClick;

		// Token: 0x04000031 RID: 49
		public Action<GUIListView, GUIListViewContextMenuArgs> ItemContextMenu;

		// Token: 0x04000032 RID: 50
		public Action<GUIListView> SelectionChange;

		// Token: 0x04000033 RID: 51
		public Action<GUIListView> Changed;

		// Token: 0x04000034 RID: 52
		private List<object> _selectedItemsPerFrame = new List<object>(128);

		// Token: 0x04000035 RID: 53
		private float _lastKeyPressTime;

		// Token: 0x04000036 RID: 54
		private string _findWord = "";

		// Token: 0x04000037 RID: 55
		private bool _animateScroll;

		// Token: 0x04000038 RID: 56
		private static GUIContent _tempcontent;

		// Token: 0x04000039 RID: 57
		private bool _ispanning;

		// Token: 0x0400003A RID: 58
		private GUIStyle _columnHeaderTextStyle;

		// Token: 0x0400003B RID: 59
		private GUIListViewColumn _columnLMBDown;

		// Token: 0x0400003C RID: 60
		private static GUIListView _activeListView;

		// Token: 0x0400003D RID: 61
		private static EditorWindow _focusedWindow;

		// Token: 0x0400003E RID: 62
		private object _activeItem;

		// Token: 0x0400003F RID: 63
		private Vector2 _animateScrollPos;

		// Token: 0x02000009 RID: 9
		private struct ItemSelInfo
		{
			// Token: 0x0600009D RID: 157 RVA: 0x000075FA File Offset: 0x000057FA
			public ItemSelInfo(float time, bool clicked)
			{
				this = default(GUIListView.ItemSelInfo);
				this.Time = time;
				this.Clicked = clicked;
			}

			// Token: 0x04000057 RID: 87
			public float Time;

			// Token: 0x04000058 RID: 88
			public bool Clicked;
		}

		// Token: 0x0200000A RID: 10
		private class GUIListViewModelComparer : IComparer<object>
		{
			// Token: 0x0600009F RID: 159 RVA: 0x00007624 File Offset: 0x00005824
			public int Compare(object x, object y)
			{
				for (int i = 0; i < this.Columns.Count; i++)
				{
					GUIListViewColumn guilistViewColumn = this.Columns[i];
					if (guilistViewColumn != null && guilistViewColumn.CompareFunc != null)
					{
						object obj = (guilistViewColumn.SortMode == GUIListViewSortMode.Ascending) ? x : y;
						object obj2 = (guilistViewColumn.SortMode == GUIListViewSortMode.Ascending) ? y : x;
						bool flag = null == obj;
						bool flag2 = null == obj2;
						if (!flag || !flag2)
						{
							if (!flag && flag2)
							{
								return 1;
							}
							if (flag && !flag2)
							{
								return -1;
							}
							int num = guilistViewColumn.CompareFunc(obj, obj2);
							if (num != 0)
							{
								if (num < 0)
								{
									return -1;
								}
								return 1;
							}
						}
					}
				}
				return 0;
			}

			// Token: 0x04000059 RID: 89
			public List<GUIListViewColumn> Columns = new List<GUIListViewColumn>();
		}

		// Token: 0x0200000B RID: 11
		private struct DrawContext
		{
			// Token: 0x0400005A RID: 90
			public Vector2 ScrollBounds;

			// Token: 0x0400005B RID: 91
			public Rect ClientRect;

			// Token: 0x0400005C RID: 92
			public Rect ListRect;

			// Token: 0x0400005D RID: 93
			public Rect VScrollRect;

			// Token: 0x0400005E RID: 94
			public Rect HScrollRect;

			// Token: 0x0400005F RID: 95
			public int ColumnCount;

			// Token: 0x04000060 RID: 96
			public Rect ItemRect;

			// Token: 0x04000061 RID: 97
			public int RowCount;

			// Token: 0x04000062 RID: 98
			public int VisibleRowCount;

			// Token: 0x04000063 RID: 99
			public int TotalRowCount;

			// Token: 0x04000064 RID: 100
			public int TopRow;

			// Token: 0x04000065 RID: 101
			public int TopVisibleRow;

			// Token: 0x04000066 RID: 102
			public int BottomRow;

			// Token: 0x04000067 RID: 103
			public int BottomVisibleRow;

			// Token: 0x04000068 RID: 104
			public float VScrollOffset;

			// Token: 0x04000069 RID: 105
			public float HScrollOffset;

			// Token: 0x0400006A RID: 106
			public int FirstItem;

			// Token: 0x0400006B RID: 107
			public int LastItem;

			// Token: 0x0400006C RID: 108
			public int FirstVisibleItem;

			// Token: 0x0400006D RID: 109
			public int LastVisibleItem;

			// Token: 0x0400006E RID: 110
			public int VisibleItemCount;

			// Token: 0x0400006F RID: 111
			public GUIListViewColumn Column;

			// Token: 0x04000070 RID: 112
			public Rect RowRect;

			// Token: 0x04000071 RID: 113
			public bool Selected;

			// Token: 0x04000072 RID: 114
			public bool HotTracking;

			// Token: 0x04000073 RID: 115
			public object Item;

			// Token: 0x04000074 RID: 116
			public int ItemIndex;

			// Token: 0x04000075 RID: 117
			public int TotalItemCount;

			// Token: 0x04000076 RID: 118
			public float HeaderHeight;

			// Token: 0x04000077 RID: 119
			public int ControlId;

			// Token: 0x04000078 RID: 120
			public bool IsMouseInside;

			// Token: 0x04000079 RID: 121
			public Event CurEvent;
		}
	}
}
