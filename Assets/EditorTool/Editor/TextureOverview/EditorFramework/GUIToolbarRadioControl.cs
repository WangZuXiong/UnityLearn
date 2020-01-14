using System;

namespace EditorFramework
{
	// Token: 0x02000055 RID: 85
	public abstract class GUIToolbarRadioControl : GUIControl
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000E6AC File Offset: 0x0000C8AC
		// (set) Token: 0x0600020E RID: 526 RVA: 0x0000E6B4 File Offset: 0x0000C8B4
		public GUIToolbarRadioGroup RadioGroup { get; private set; }

		// Token: 0x0600020F RID: 527 RVA: 0x0000E6BD File Offset: 0x0000C8BD
		public GUIToolbarRadioControl(GUIToolbarRadioGroup group) : base(group.Editor, group)
		{
			this.RadioGroup = group;
		}
	}
}
