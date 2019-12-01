using System;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000061 RID: 97
	public class GUIToolbarSlider : GUIControl
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000EF31 File Offset: 0x0000D131
		// (set) Token: 0x0600022C RID: 556 RVA: 0x0000EF39 File Offset: 0x0000D139
		public float MinValue { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000EF42 File Offset: 0x0000D142
		// (set) Token: 0x0600022E RID: 558 RVA: 0x0000EF4A File Offset: 0x0000D14A
		public float MaxValue { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000EF53 File Offset: 0x0000D153
		// (set) Token: 0x06000230 RID: 560 RVA: 0x0000EF5B File Offset: 0x0000D15B
		public float Value { get; set; }

		// Token: 0x06000231 RID: 561 RVA: 0x0000EF64 File Offset: 0x0000D164
		public GUIToolbarSlider(GUIToolbar toolbar, float minvalue, float maxvalue) : base(toolbar.Editor, toolbar)
		{
			this.Style = GUI.skin.horizontalSlider;
			this.LayoutOptions = new GUILayoutOption[]
			{
				GUILayout.MinWidth(64f),
				GUILayout.MaxWidth(64f)
			};
			this.MinValue = minvalue;
			this.MaxValue = maxvalue;
			this.Value = minvalue;
			this._value = minvalue;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000EFD4 File Offset: 0x0000D1D4
		protected override void DoGUI()
		{
			float num = GUILayout.HorizontalSlider(this.Value, this.MinValue, this.MaxValue, this.LayoutOptions);
			if (Mathf.Abs(num - this._value) > 0.001f)
			{
				this.Value = num;
				if (this.Execute != null)
				{
					this.Execute(this);
				}
			}
			this._value = num;
		}

		// Token: 0x0400015E RID: 350
		private float _value;
	}
}
