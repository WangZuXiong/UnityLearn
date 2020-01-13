using System;

namespace EditorFramework
{
	// Token: 0x02000023 RID: 35
	public interface ICacheFileEntry
	{
		// Token: 0x0600011F RID: 287
		string GetAssetGuid();

		// Token: 0x06000120 RID: 288
		string GetAssetHash();

		// Token: 0x06000121 RID: 289
		void Serialize(BinarySerializer data);
	}
}
