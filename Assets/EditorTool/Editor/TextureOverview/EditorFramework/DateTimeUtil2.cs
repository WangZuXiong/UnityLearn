using System;

namespace EditorFramework
{
	// Token: 0x02000026 RID: 38
	public static class DateTimeUtil2
	{
		// Token: 0x0600012F RID: 303 RVA: 0x0000A0D0 File Offset: 0x000082D0
		public static string GetPrettyDateTimeString(DateTime datetime)
		{
			if (datetime.Date == DateTime.Today)
			{
				return string.Format("Today {0}", datetime.ToShortTimeString());
			}
			return string.Format("{0} {1}", datetime.ToShortDateString(), datetime.ToShortTimeString());
		}
	}
}
