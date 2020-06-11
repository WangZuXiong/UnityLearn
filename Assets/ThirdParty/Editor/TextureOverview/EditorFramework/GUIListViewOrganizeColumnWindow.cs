using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x0200004D RID: 77
	internal class GUIListViewOrganizeColumnWindow : EditorWindow
	{
		// Token: 0x060001D4 RID: 468 RVA: 0x0000D0C4 File Offset: 0x0000B2C4
		public void SetListView(GUIListView view)
		{
			this.ListView = view;
			this._items = new List<GUIListViewOrganizeColumnWindow.Item>();
			for (int i = 0; i < view.Columns.Count; i++)
			{
				GUIListViewColumn guilistViewColumn = view.Columns[i];
				GUIListViewOrganizeColumnWindow.Item item = new GUIListViewOrganizeColumnWindow.Item();
				item.Column = guilistViewColumn;
				item.OrgY = (float)(i * this._itemheight);
				item.FadeY = item.OrgY;
				item.Y = item.OrgY;
				item.Visible = guilistViewColumn.Visible;
				this._items.Add(item);
			}
			for (int j = 0; j < this._items.Count; j++)
			{
				GUIListViewOrganizeColumnWindow.Item item2 = this._items[j];
				item2.OrgItemAbove = ((j > 0) ? this._items[j - 1] : null);
				item2.OrgItemBelow = ((j < this._items.Count - 1) ? this._items[j + 1] : null);
			}
			base.minSize = new Vector2(275f, (float)(view.Columns.Count * this._itemheight + 44));
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000D1E0 File Offset: 0x0000B3E0
		private void OnDestroy()
		{
			if (this.ListView != null)
			{
				this.ListView.SavePrefs();
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000D1F8 File Offset: 0x0000B3F8
		private void DrawItem(GUIListViewOrganizeColumnWindow.Item item, Rect rect, bool active)
		{
			rect.x += 2f;
			rect.width -= 4f;
			if (active)
			{
				Color color = GUI.color;
				GUI.color = GUIColors.ActiveSelection;
				EditorGUI.DrawPreviewTexture(rect, EditorGUIUtility.whiteTexture);
				EditorGUIUtility.AddCursorRect(rect, MouseCursor.MoveArrow);
				float num = 4f;
				Rect rect2;
				rect2 = new Rect(rect.x, 0f, rect.width, num);
				if (item.OrgItemAbove != null && item.OrgItemBelow != null)
				{
					rect2.y = item.OrgItemAbove.FadeY + (float)this._itemheight - num * 0.5f;
					rect2.height = item.OrgItemBelow.FadeY - item.OrgItemAbove.FadeY - (float)this._itemheight + num;
				}
				if (item.OrgItemAbove != null && item.OrgItemBelow == null)
				{
					rect2.y = item.OrgItemAbove.FadeY + (float)this._itemheight - num * 0.5f;
					rect2.height = num;
				}
				if (item.OrgItemAbove == null && item.OrgItemBelow != null)
				{
					rect2.y = item.OrgItemBelow.FadeY - num * 0.5f;
					rect2.height = num;
				}
				EditorGUI.DrawPreviewTexture(rect2, EditorGUIUtility.whiteTexture);
				GUI.color = color;
			}
			else
			{
				EditorGUI.HelpBox(rect, "", 0);
			}
			GUIListViewColumn column = item.Column;
			string text = string.IsNullOrEmpty(column.PopupText) ? column.Text : column.PopupText;
			EditorGUI2.Label(rect, GUIListViewOrganizeColumnWindow.TempContent(text, column.Image, ""), active);
			rect.x += rect.width - 16f;
			rect.width = 16f;
			if (GUI.Toggle(rect, item.Visible, GUIListViewOrganizeColumnWindow.TempContent("", null, "Show Column")) != item.Visible)
			{
				item.Visible = !item.Visible;
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000D404 File Offset: 0x0000B604
		private void OnGUI()
		{
			if (this.ListView == null)
			{
				base.Close();
			}
			for (int i = 0; i < this._items.Count; i++)
			{
				GUIListViewOrganizeColumnWindow.Item item = this._items[i];
				if (item != this._dragitem)
				{
					float num = item.Y - item.FadeY;
					item.FadeY += Mathf.Clamp(num * Time.deltaTime * 1000f, -Mathf.Abs(num), Mathf.Abs(num));
					Rect rect;
					rect=new Rect(0f, item.FadeY, base.position.width, (float)this._itemheight);
					this.DrawItem(item, rect, false);
					if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition) && Event.current.button == 0)
					{
						this._dragitem = item;
						Event.current.Use();
					}
				}
			}
			if (this._dragitem != null)
			{
				Rect rect2;
				rect2=new Rect(0f, this._dragitem.FadeY, base.position.width, (float)this._itemheight);
				this.DrawItem(this._dragitem, rect2, true);
				if ((int)Event.current.type == 3 && Event.current.button == 0)
				{
					this._dragitem.FadeY = Event.current.mousePosition.y - (float)this._itemheight * 0.5f;
					this._dragitem.FadeY = Mathf.Clamp(this._dragitem.FadeY, 0f, (float)(this._items.Count * this._itemheight - this._itemheight));
					int num2 = (int)(this._dragitem.FadeY * (float)this._itemheight) / this._itemheight;
					this._items.Sort((GUIListViewOrganizeColumnWindow.Item a, GUIListViewOrganizeColumnWindow.Item b) => a.FadeY.CompareTo(b.FadeY));
					for (int j = 0; j < this._items.Count; j++)
					{
						GUIListViewOrganizeColumnWindow.Item item2 = this._items[j];
						if (item2 != this._dragitem)
						{
							item2.Y = (float)(j * this._itemheight);
						}
					}
				}
				if ((int)Event.current.type == 1 && Event.current.button == 0)
				{
					for (int k = 0; k < this._items.Count; k++)
					{
						GUIListViewOrganizeColumnWindow.Item item3 = this._items[k];
						item3.FadeY = (float)(k * this._itemheight);
						item3.Y = (float)(k * this._itemheight);
						item3.OrgY = item3.FadeY;
						item3.OrgItemAbove = ((k > 0) ? this._items[k - 1] : null);
						item3.OrgItemBelow = ((k < this._items.Count - 1) ? this._items[k + 1] : null);
					}
					this._dragitem = null;
					Event.current.Use();
				}
			}
			Rect rect3;
			rect3=new Rect(2f, base.position.height - 40f, base.position.width - 128f, 36f);
			EditorGUI.HelpBox(rect3, "Drag&drop items to change column order", MessageType.Info);
			Rect rect4;
			rect4=new Rect(base.position.width - 122f, base.position.height - 24f, 58f, 20f);
			if (GUI.Button(rect4, "Cancel"))
			{
				base.Close();
				return;
			}
			Rect rect5;
			rect5=new Rect(base.position.width - 60f, base.position.height - 24f, 58f, 20f);
			if (GUI.Button(rect5, "OK"))
			{
				this.ListView.Columns.Clear();
				for (int l = 0; l < this._items.Count; l++)
				{
					GUIListViewColumn column = this._items[l].Column;
					column.Visible = this._items[l].Visible;
					this.ListView.Columns.Add(column);
				}
				this.ListView.Editor.Repaint();
				base.Close();
				return;
			}
			base.Repaint();
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000D88C File Offset: 0x0000BA8C
		private static GUIContent TempContent(string text, Texture image, string tooltip)
		{
			if (GUIListViewOrganizeColumnWindow._tempcontent == null)
			{
				GUIListViewOrganizeColumnWindow._tempcontent = new GUIContent();
			}
			GUIListViewOrganizeColumnWindow._tempcontent.text = text;
			GUIListViewOrganizeColumnWindow._tempcontent.tooltip = tooltip;
			GUIListViewOrganizeColumnWindow._tempcontent.image = image;
			return GUIListViewOrganizeColumnWindow._tempcontent;
		}

		// Token: 0x04000122 RID: 290
		private static GUIContent _tempcontent;

		// Token: 0x04000123 RID: 291
		private int _itemheight = 22;

		// Token: 0x04000124 RID: 292
		private List<GUIListViewOrganizeColumnWindow.Item> _items = new List<GUIListViewOrganizeColumnWindow.Item>();

		// Token: 0x04000125 RID: 293
		private GUIListViewOrganizeColumnWindow.Item _dragitem;

		// Token: 0x04000126 RID: 294
		public GUIListView ListView;

		// Token: 0x0200004E RID: 78
		private class Item
		{
			// Token: 0x04000128 RID: 296
			public GUIListViewColumn Column;

			// Token: 0x04000129 RID: 297
			public bool Visible;

			// Token: 0x0400012A RID: 298
			public float Y;

			// Token: 0x0400012B RID: 299
			public float OrgY;

			// Token: 0x0400012C RID: 300
			public float FadeY;

			// Token: 0x0400012D RID: 301
			public GUIListViewOrganizeColumnWindow.Item OrgItemAbove;

			// Token: 0x0400012E RID: 302
			public GUIListViewOrganizeColumnWindow.Item OrgItemBelow;
		}
	}
}
