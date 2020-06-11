using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNode
{
    //当前的数据
    public MonoBase data;
    //下一个节点
    public EventNode next;

    public EventNode(MonoBase monoBase)
    {
        data = monoBase;
        next = null;
    }
}

public class ManagerBase : MonoBase
{
    //存储罅隙
    public Dictionary<ushort, EventNode> EventTree = new Dictionary<ushort, EventNode>();
    /// <summary>
    /// 来了消息 通知整个消息链表
    /// </summary>
    /// <param name="tempMsg"></param>
    public override void ProcessEvent(MsgBase tempMsg)
    {
        if (!EventTree.ContainsKey(tempMsg.MsgId))
        {
            Debug.LogError("msg no contain msgid ==" + tempMsg.MsgId);
            Debug.LogError("msg manager == " + tempMsg.GetManager());
            return;
        }

        EventNode temp = EventTree[tempMsg.MsgId];
        do
        {
            //策略模式
            temp.data.ProcessEvent(tempMsg);
        } while (temp.next != null);
    }

    /// <summary>
    /// 注册多个msg
    /// </summary>
    /// <param name="monoBase">要注册的脚本</param>
    /// <param name="msgs">脚本可以注册多个msg</param>
    public void RegistMsg(MonoBase monoBase, params ushort[] msgs)
    {
        for (int i = 0; i < msgs.Length; i++)
        {
            EventNode eventNode = new EventNode(monoBase);
            EventTree.Add(msgs[i], eventNode);
        }
    }
    /// <summary>
    /// 注册一个消息
    /// </summary>
    /// <param name="id">msg id</param>
    /// <param name="eventNode">链表</param>
    public void RegistMsg(ushort id, EventNode eventNode)
    {
        //数据链路里面没有这个消息
        if (!EventTree.ContainsKey(id))
        {
            EventTree.Add(id, eventNode);
        }
        else
        {
            EventNode tempNode = EventTree[id];
            //找到最后一个
            while (tempNode.next != null)
            {
                tempNode = tempNode.next;
            }
            //直接挂到最后一个
            tempNode.next = eventNode;
        }
    }
    /// <summary>
    /// 注销多个消息
    /// 去掉一个脚本的诺干个消息
    /// </summary>
    /// <param name="node"></param>
    /// <param name="msgs"></param>
    public void UnRegistMsg(MonoBase node, params ushort[] msgs)
    {
        for (int i = 0; i < msgs.Length; i++)
        {
            ushort id = msgs[i];
            UnRegistMsg(id, node);
        }
    }
    /// <summary>
    /// 注销一个消息
    /// 去掉一个消息链表
    /// </summary>
    /// <param name="id"></param>
    /// <param name="node"></param>
    public void UnRegistMsg(ushort id, MonoBase node)
    {
        if (!EventTree.ContainsKey(id))
        {
            Debug.LogError("no contains is == " + id);
            return;
        }
        else
        {
            EventNode temp = EventTree[id];
            if (temp.data == node)//去掉头部，包含两种情况
            {
                EventNode header = temp;

                //已经存在这个消息
                if (header.next != null) //后面有多个节点
                {
                    header.data = temp.next.data;
                    header.next = temp.next.next;
                }
                else//只有一个节点
                {
                    EventTree.Remove(id);
                }
            }
            else//去掉尾部和中间的部分
            {
                while (temp.next != null && temp.next.data != null)
                {
                    temp = temp.next;
                }//走完这个循环，表示已经找到该节点

                //没有引用会自动释放
                if (temp.next != null)//去掉中间的
                {
                    temp.next = temp.next.next;
                }
                else//去掉尾部的
                {
                    temp.next = null;
                }
            }
        }
    }
}
