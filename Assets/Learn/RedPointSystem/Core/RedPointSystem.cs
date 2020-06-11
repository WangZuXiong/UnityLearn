using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPointSystem : MonoBehaviour
{
    /// <summary>
    /// 红点树变化通知
    /// </summary>
    /// <param name="node"></param>
    public delegate void OnPointNumChange(RedPointNode node);
    /// <summary>
    /// 根节点
    /// </summary>
    private RedPointNode _rootNode;

    private static List<string> RedPointTreeList = new List<string>
    {
        RedPointConst.MAIN,
        RedPointConst.MAIL,
        RedPointConst.MAIL_TEAM,
        RedPointConst.MAIL_SYSTEM,
        RedPointConst.MAIL_ALIANCE,
        RedPointConst.TASK,
        RedPointConst.ALIANCE,
    };

    /// <summary>
    /// 初始化
    /// </summary>
    public void InitRedPointTreeNode()
    {
        _rootNode = new RedPointNode
        {
            NodeName = RedPointConst.MAIN
        };

        for (int i = 0; i < RedPointTreeList.Count; i++)
        {
            var node = _rootNode;
            var treeNodeArray = RedPointTreeList[i].Split('.');
            if (!treeNodeArray[0].Equals(_rootNode.NodeName))
            {
                Debug.LogError("red point tree root node error :" + treeNodeArray[0]);
                continue;
            }

            if (treeNodeArray.Length > 1)
            {
                for (int j = 1; j < treeNodeArray.Length; j++)
                {
                    if (!node.ChildDict.ContainsKey(treeNodeArray[j]))
                    {
                        node.ChildDict.Add(treeNodeArray[j], new RedPointNode());
                    }

                    node.ChildDict[treeNodeArray[j]].NodeName = treeNodeArray[j];
                    node.ChildDict[treeNodeArray[j]].Parent = node;

                    node = node.ChildDict[treeNodeArray[j]];
                }
            }
        }
    }
    /// <summary>
    /// 绑定事件回调
    /// </summary>
    /// <param name="strNode"></param>
    /// <param name="callBack"></param>
    public void SetRedPointNodeCallBack(string strNode, OnPointNumChange callBack)
    {
        var node = FindNode(strNode);
        if (node != null)
        {
            node.OnPointNumChange = callBack;
        }
    }
    /// <summary>
    /// 驱动层
    /// </summary>
    /// <param name="strNode"></param>
    /// <param name="redPointNum"></param>
    public void SetInvoke(string strNode, int redPointNum)
    {
        var node = FindNode(strNode);
        if (node != null)
        {
            node.SetredPointNum(redPointNum);
        }
    }

    /// <summary>
    /// 寻找到对应的节点
    /// </summary>
    /// <param name="strNode"></param>
    /// <returns></returns>
    private RedPointNode FindNode(string strNode)
    {
        var nodeList = strNode.Split('.');
        if (nodeList.Length == 1)
        {
            if (!nodeList[0].Equals(RedPointConst.MAIN))
            {
                Debug.LogError("get wrong root node current is " + nodeList[0]);
                return null;
            }
        }

        var node = _rootNode;
        for (int i = 1; i < nodeList.Length; i++)
        {
            if (!node.ChildDict.ContainsKey(nodeList[i]))
            {
                Debug.LogError("does not contains child node:" + nodeList[i]);
                return null;
            }
            node = node.ChildDict[nodeList[i]];
            //找到最后一个节点
            if (i == nodeList.Length - 1)
            {
                return node;
            }
        }
        return null;
    }
}
