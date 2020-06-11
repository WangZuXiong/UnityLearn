using UnityEngine;
public delegate void OnPlayerInfoChangedEvent(int infoType);

public class Test : MonoBehaviour
{
    private void Awake()
    {
        PlayerInfo.Instance.OnPlayerInfoChanged += OnPlayerInfoChanged;

    }

    private void OnDestroy()
    {
        PlayerInfo.Instance.OnPlayerInfoChanged -= OnPlayerInfoChanged;
    }

    private void OnPlayerInfoChanged(int infoType)
    {
        //do something
    }
}


public class PlayerInfo
{
    public static PlayerInfo Instance;
    public event OnPlayerInfoChangedEvent OnPlayerInfoChanged;

    void CallBackFunc(int type)
    {
        OnPlayerInfoChanged?.Invoke(type);
    }
}

//高内聚 - MVC
//低耦合 - 观察者模式

//MVC + 事件分发系统 （EventDispatcher）