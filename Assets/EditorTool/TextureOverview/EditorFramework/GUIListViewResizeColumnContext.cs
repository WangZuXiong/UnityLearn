using System;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000050 RID: 80
	internal class GUIListViewResizeColumnContext
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0000D8E8 File Offset: 0x0000BAE8
		public bool IsResizing
		{
			get
			{
				return null != this._column;
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000D8F6 File Offset: 0x0000BAF6
		public GUIListViewResizeColumnContext(GUIListView listview)
		{
			this._listview = listview;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000D905 File Offset: 0x0000BB05
		public void Begin(GUIListViewColumn column)
		{
			this._column = column;
			this._originalWidth = column.Width;
			this._beginningMousePosition = Event.current.mousePosition;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000D92A File Offset: 0x0000BB2A
		public void End()
		{
			this._column.Width = this._currentWidth;
			this._column = null;
			this._listview.SavePrefs();
			this._listview.Editor.Repaint();
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000D960 File Offset: 0x0000BB60
		public void Update()
		{
			this._currentWidth = this._originalWidth - (float)((int)(this._beginningMousePosition.x - Event.current.mousePosition.x));
			this._currentWidth = Mathf.Min(this._column.MaxWidth, Mathf.Max(this._column.MinWidth, this._currentWidth));
			this._column.Width = this._currentWidth;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000D9D4 File Offset: 0x0000BBD4
		public void Cancel()
		{
			this._column.Width = this._originalWidth;
			this._column = null;
			this._listview.Editor.Repaint();
		}

		// Token: 0x04000134 RID: 308
		private GUIListView _listview;

		// Token: 0x04000135 RID: 309
		private GUIListViewColumn _column;

		// Token: 0x04000136 RID: 310
		private float _originalWidth;

		// Token: 0x04000137 RID: 311
		private float _currentWidth;

		// Token: 0x04000138 RID: 312
		private Vector2 _beginningMousePosition;
	}
}
