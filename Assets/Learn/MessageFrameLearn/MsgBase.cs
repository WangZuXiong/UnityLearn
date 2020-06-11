public class MsgBase
{
    public ushort MsgId;

    public MsgBase(ushort msgId)
    {
        MsgId = msgId;
    }

    public ManagerId GetManager()
    {
        int tempId = MsgId / FrameTool.MsgSpan;
        return (ManagerId)(tempId * FrameTool.MsgSpan);
    }
}
