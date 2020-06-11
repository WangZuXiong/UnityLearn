using System;
using System.IO;

namespace EditorFramework
{
	// Token: 0x0200001E RID: 30
	public class BinarySerializer
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00009291 File Offset: 0x00007491
		public bool IsReader
		{
			get
			{
				return this._reader != null;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000101 RID: 257 RVA: 0x0000929F File Offset: 0x0000749F
		public bool IsWriter
		{
			get
			{
				return this._writer != null;
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000092AD File Offset: 0x000074AD
		public BinarySerializer(BinaryReader reader)
		{
			this._reader = reader;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000092BC File Offset: 0x000074BC
		public BinarySerializer(BinaryWriter writer)
		{
			this._writer = writer;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000092CB File Offset: 0x000074CB
		public void Serialize(ref string value)
		{
			if (this._reader != null)
			{
				value = this._reader.ReadString();
				return;
			}
			this._writer.Write(value ?? "");
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000092F9 File Offset: 0x000074F9
		public void Serialize(ref bool value)
		{
			if (this._reader != null)
			{
				value = this._reader.ReadBoolean();
				return;
			}
			this._writer.Write(value);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000931E File Offset: 0x0000751E
		public void Serialize(ref char value)
		{
			if (this._reader != null)
			{
				value = this._reader.ReadChar();
				return;
			}
			this._writer.Write(value);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00009343 File Offset: 0x00007543
		public void Serialize(ref byte value)
		{
			if (this._reader != null)
			{
				value = this._reader.ReadByte();
				return;
			}
			this._writer.Write(value);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00009368 File Offset: 0x00007568
		public void Serialize(ref short value)
		{
			if (this._reader != null)
			{
				value = this._reader.ReadInt16();
				return;
			}
			this._writer.Write(value);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000938D File Offset: 0x0000758D
		public void Serialize(ref ushort value)
		{
			if (this._reader != null)
			{
				value = this._reader.ReadUInt16();
				return;
			}
			this._writer.Write(value);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000093B2 File Offset: 0x000075B2
		public void Serialize(ref int value)
		{
			if (this._reader != null)
			{
				value = this._reader.ReadInt32();
				return;
			}
			this._writer.Write(value);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000093D7 File Offset: 0x000075D7
		public void Serialize(ref uint value)
		{
			if (this._reader != null)
			{
				value = this._reader.ReadUInt32();
				return;
			}
			this._writer.Write(value);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000093FC File Offset: 0x000075FC
		public void Serialize(ref float value)
		{
			if (this._reader != null)
			{
				value = this._reader.ReadSingle();
				return;
			}
			this._writer.Write(value);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00009421 File Offset: 0x00007621
		public void Serialize(ref long value)
		{
			if (this._reader != null)
			{
				value = this._reader.ReadInt64();
				return;
			}
			this._writer.Write(value);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00009446 File Offset: 0x00007646
		public void Serialize(ref ulong value)
		{
			if (this._reader != null)
			{
				value = this._reader.ReadUInt64();
				return;
			}
			this._writer.Write(value);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000946B File Offset: 0x0000766B
		public int SerializeInt32(int value)
		{
			if (this._reader != null)
			{
				value = this._reader.ReadInt32();
			}
			else
			{
				this._writer.Write(value);
			}
			return value;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00009494 File Offset: 0x00007694
		public void SerializeEnum<T>(ref T value)
		{
			if (this._reader != null)
			{
				int num = 0;
				this.Serialize(ref num);
				value = (T)((object)num);
				return;
			}
			int num2 = Convert.ToInt32(value);
			this.Serialize(ref num2);
		}

		// Token: 0x040000AA RID: 170
		private readonly BinaryReader _reader;

		// Token: 0x040000AB RID: 171
		private readonly BinaryWriter _writer;
	}
}
