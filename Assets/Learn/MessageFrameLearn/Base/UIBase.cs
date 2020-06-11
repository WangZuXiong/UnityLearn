using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBase
{

    public ushort[] msgIds;

    public override void ProcessEvent(MsgBase tempMsg)
    {
       
    }

    public void RegistSelf(MonoBase mono, params ushort[] msg)
    {
        UIManager.Instance.RegistMsg(mono, msg);
    }

    public void UnRegistSelf(MonoBase mono, params ushort[] msg)
    {
        UIManager.Instance.UnRegistMsg(mono, msg);
    }

    public void SendMsg(MsgBase msg)
    {
        UIManager.Instance.SendMsg(msg);
    }

    private void OnDestroy()
    {
        if (msgIds != null)
        {
            UnRegistSelf(this, msgIds);
        }
    }
}
