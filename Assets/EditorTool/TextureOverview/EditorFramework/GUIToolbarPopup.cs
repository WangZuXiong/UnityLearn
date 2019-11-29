using System;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x0200005E RID: 94
	public class GUIToolbarPopup : GUIControl
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000EA01 File Offset: 0x0000CC01
		// (set) Token: 0x06000222 RID: 546 RVA: 0x0000EA0C File Offset: 0x0000CC0C
		public GUIToolbarPopupItem SelectedItem
		{
			get
			{
				return this._selectedItem;
			}
			set
			{
				for (int i = 0; i < this.Items.Length; i++)
				{
					if (value == this.Items[i])
					{
						this._selectedindex = i;
						this._selectedItem = value;
						return;
					}
				}
				this._selectedItem = null;
				this._selectedindex = -1;
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000EA54 File Offset: 0x0000CC54
		public GUIToolbarPopup(GUIToolbar toolbar) : base(toolbar.Editor, toolbar)
		{
			this.Style = EditorStyles.toolbarPopup;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000EA90 File Offset: 0x0000CC90
		protected override void DoGUI()
		{
			if (this._items.Length != this.Items.Length)
			{
				this._items = new GUIContent[this.Items.Length];
				for (int i = 0; i < this.Items.Length; i++)
				{
					this._items[i] = this.Items[i].Text;
				}
			}
			float num = float.MaxValue;
			float num2 = float.MinValue;
			foreach (GUIContent guicontent in this._items)
			{
				float num3;
				float num4;
				this.Style.CalcMinMaxWidth(guicontent, out num3, out num4);
				num = Mathf.Min(num, num3);
				num2 = Mathf.Max(num2, num4);
			}
			int num5 = EditorGUILayout.Popup(this._selectedindex, this._items, this.Style, new GUILayoutOption[]
			{
				GUILayout.MinWidth(num2),
				GUILayout.MaxWidth(num2)
			});
			if (num5 != this._selectedindex)
			{
				this._selectedindex = num5;
				this._selectedItem = this.Items[num5];
				this.Text = this._selectedItem.Text.text;
				if (this.Execute != null)
				{
					this.Execute(this);
				}
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000EBBC File Offset: 0x0000CDBC
		public bool SelectTag(object tag)
		{
			for (int i = 0; i < this.Items.Length; i++)
			{
				GUIToolbarPopupItem guitoolbarPopupItem = this.Items[i];
				if (guitoolbarPopupItem.Tag != null && guitoolbarPopupItem.Tag.Equals(tag))
				{
					this.SelectedItem = guitoolbarPopupItem;
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000155 RID: 341
		private GUIContent[] _items = new GUIContent[0];

		// Token: 0x04000156 RID: 342
		private int _selectedindex = -1;

		// Token: 0x04000157 RID: 343
		private GUIToolbarPopupItem _selectedItem;

		// Token: 0x04000158 RID: 344
		public GUIToolbarPopupItem[] Items = new GUIToolbarPopupItem[0];
	}
}
