using System;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000062 RID: 98
	public class GUIToolbarMenuItem
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0000F035 File Offset: 0x0000D235
		// (set) Token: 0x06000234 RID: 564 RVA: 0x0000F03D File Offset: 0x0000D23D
		public GUIToolbarMenu Owner { get; internal set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000F048 File Offset: 0x0000D248
		public bool IsChecked
		{
			get
			{
				if (this.QueryStatus == null || this.Owner == null)
				{
					return false;
				}
				GUIControlStatus guicontrolStatus = this.QueryStatus(this);
				return (guicontrolStatus & GUIControlStatus.Checked) != GUIControlStatus.None;
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000F07C File Offset: 0x0000D27C
		public GUIToolbarMenuItem(string text)
		{
			this.Text = new GUIContent(text);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000F090 File Offset: 0x0000D290
		public GUIToolbarMenuItem(string text, Action<GUIToolbarMenuItem> exec) : this(text, exec, null)
		{
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000F09B File Offset: 0x0000D29B
		public GUIToolbarMenuItem(string text, Action<GUIToolbarMenuItem> exec, Func<GUIToolbarMenuItem, GUIControlStatus> query)
		{
			this.Text = new GUIContent(text);
			this.Execute = exec;
			this.QueryStatus = query;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000F0BD File Offset: 0x0000D2BD
		public override string ToString()
		{
			return this.Text.text;
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0000F0CA File Offset: 0x0000D2CA
		public static GUIToolbarMenuItem Separator
		{
			get
			{
				return new GUIToolbarMenuItem("-");
			}
		}

		// Token: 0x04000162 RID: 354
		public GUIContent Text;

		// Token: 0x04000163 RID: 355
		public Action<GUIToolbarMenuItem> Execute;

		// Token: 0x04000164 RID: 356
		public Func<GUIToolbarMenuItem, GUIControlStatus> QueryStatus;

		// Token: 0x04000165 RID: 357
		public object Tag;
	}
}
