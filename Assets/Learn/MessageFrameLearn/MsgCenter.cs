using System;
using UnityEngine;

public class MsgCenter : MonoBehaviour
{
    public static MsgCenter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SendToMsg(MsgBase msg)
    {
        ManagerId managerId = msg.GetManager();
        switch (managerId)
        {
            case ManagerId.AssetManager: break;
            case ManagerId.AudioManager: break;
            case ManagerId.GameManager: break;
            case ManagerId.NetManager: break;
            case ManagerId.UIManager: break;
            default:
                Debug.LogError("msg");
                break;
        }
    }
}

