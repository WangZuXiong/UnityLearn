using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000063 RID: 99
	public class GUIToolbarMenu : GUIControl
	{
		// Token: 0x0600023B RID: 571 RVA: 0x0000F0D6 File Offset: 0x0000D2D6
		public GUIToolbarMenu(GUIToolbar toolbar) : base(toolbar.Editor, toolbar)
		{
			this.Style = EditorStyles.toolbarDropDown;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000F108 File Offset: 0x0000D308
		public void Add(GUIToolbarMenuItem item)
		{
			if (item.Owner != null)
			{
				throw new ArgumentException("'item' has been added to a toolbar already");
			}
			List<GUIToolbarMenuItem> list = new List<GUIToolbarMenuItem>(this.Items);
			item.Owner = this;
			list.Add(item);
			this.Items = list.ToArray();
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000F150 File Offset: 0x0000D350
		protected override void DoGUI()
		{
			this.CheckItemRebuild();
			bool flag = GUILayout.Button(GUIContent2.Temp(this.Text, this.Image, this.Tooltip), this.Style, this.LayoutOptions);
			if ((int)Event.current.type == 7)
			{
				this._rect = GUILayoutUtility.GetLastRect();
				this._rect.y = this._rect.y + this._rect.height;
				this._rect.width = (this._rect.height = 0f);
			}
			if (flag)
			{
				if (this.Execute != null)
				{
					this.Execute(this);
					this.CheckItemRebuild();
				}
				GenericMenu genericMenu = new GenericMenu();
				foreach (GUIToolbarMenuItem guitoolbarMenuItem in this.Items)
				{
					GUIControlStatus guicontrolStatus = GUIControlStatus.Enable | GUIControlStatus.Visible;
					if (guitoolbarMenuItem.QueryStatus != null)
					{
						guicontrolStatus = guitoolbarMenuItem.QueryStatus(guitoolbarMenuItem);
					}
					if ((guicontrolStatus & GUIControlStatus.Visible) != GUIControlStatus.None)
					{
						if (guitoolbarMenuItem.Text.text == "-")
						{
							genericMenu.AddSeparator("");
						}
						else if ((guicontrolStatus & GUIControlStatus.Enable) != GUIControlStatus.None && guitoolbarMenuItem.Execute != null)
						{
							genericMenu.AddItem(guitoolbarMenuItem.Text, (guicontrolStatus & GUIControlStatus.Checked) != GUIControlStatus.None, new GenericMenu.MenuFunction2(this.OnMenu), guitoolbarMenuItem);
						}
						else
						{
							genericMenu.AddItem(guitoolbarMenuItem.Text, (guicontrolStatus & GUIControlStatus.Checked) != GUIControlStatus.None, null);
						}
					}
				}
				genericMenu.DropDown(this._rect);
			}
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000F2C4 File Offset: 0x0000D4C4
		private void CheckItemRebuild()
		{
			if (this._items.Length != this.Items.Length)
			{
				this._items = new GUIContent[this.Items.Length];
				for (int i = 0; i < this.Items.Length; i++)
				{
					this._items[i] = this.Items[i].Text;
				}
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000F320 File Offset: 0x0000D520
		private void OnMenu(object userdata)
		{
			GUIToolbarMenuItem guitoolbarMenuItem = (GUIToolbarMenuItem)userdata;
			if (guitoolbarMenuItem != null && guitoolbarMenuItem.Execute != null)
			{
				guitoolbarMenuItem.Execute(guitoolbarMenuItem);
			}
		}

		// Token: 0x04000167 RID: 359
		private GUIContent[] _items = new GUIContent[0];

		// Token: 0x04000168 RID: 360
		private Rect _rect;

		// Token: 0x04000169 RID: 361
		public GUIToolbarMenuItem[] Items = new GUIToolbarMenuItem[0];
	}
}
