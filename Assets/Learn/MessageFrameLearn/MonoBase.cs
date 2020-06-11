using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoBase : MonoBehaviour
{
    /// <summary>
    /// 处理一个消息
    /// </summary>
    /// <param name="tempMsg"></param>
    public abstract void ProcessEvent(MsgBase tempMsg);
}
