using UnityEngine;
using System.Collections.Generic;

public class UIManager : ManagerBase
{
    public static UIManager Instance { get; private set; }
    /// <summary>
    /// 规定了开发方式
    /// 消耗内存 换取速度 和 方便 
    /// </summary>
    private Dictionary<string, GameObject> _sonMember = new Dictionary<string, GameObject>();
    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetGameObject(string name)
    {
        if (_sonMember.ContainsKey(name))
        {
            return _sonMember[name];
        }
        return null;
    }

    public void RegistGameObject(string name, GameObject obj)
    {
        if (!_sonMember.ContainsKey(name))
        {
            _sonMember.Add(name, obj);
        }
    }

    public void UnRegistGameObject(string name)
    {
        if (_sonMember.ContainsKey(name))
        {
            _sonMember.Remove(name);
        }
    }
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msg"></param>
    public void SendMsg(MsgBase msg)
    {
        if (msg.GetManager() == ManagerId.UIManager)
        {
            //本模块自己处理
            ProcessEvent(msg);
        }
        else //交给msgCenter处理
        {
            MsgCenter.Instance.SendToMsg(msg);
        }
    }
}