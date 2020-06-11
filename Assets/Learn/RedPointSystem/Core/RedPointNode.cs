using System;
using System.Collections.Generic;

public class RedPointNode
{
    /// <summary>
    /// 界面名称
    /// </summary>
    public string NodeName;
    /// <summary>
    /// 红点的数量
    /// </summary>
    public int PointNum;
    /// <summary>
    /// 父节点
    /// </summary>
    public RedPointNode Parent;
    public RedPointSystem.OnPointNumChange OnPointNumChange;
    /// <summary>
    /// 子节点
    /// </summary>
    public Dictionary<string, RedPointNode> ChildDict = new Dictionary<string, RedPointNode>();

    /// <summary>
    /// 设置当前节点的红点数量
    /// </summary>
    /// <param name="redPointNum"></param>
    internal void SetredPointNum(int redPointNum)
    {
        //红点数量只能设置叶子节点
        if (ChildDict.Count > 0)
        {
            UnityEngine.Debug.LogError("only can set leaf node");
            return;
        }

        PointNum = redPointNum;

        NotifyPointNumChange();
        if (Parent != null)
        {
            Parent.ChangeParentRedPointNum();
        }
    }

    private void NotifyPointNumChange()
    {
        OnPointNumChange?.Invoke(this);
    }

    private void ChangeParentRedPointNum()
    {
        var num = 0;
        var enumerator = ChildDict.GetEnumerator();
        while (enumerator.MoveNext())
        {
            num += enumerator.Current.Value.PointNum;
        }
        //红点数量有变化
        if (num != PointNum)
        {
            PointNum = num;
            NotifyPointNumChange();
        }
    }
}
