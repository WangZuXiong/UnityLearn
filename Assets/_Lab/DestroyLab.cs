using System;
using UnityEngine;
using UnityEditor;
public class DestroyLab : MonoBehaviour
{
    private void Awake()
    {
        MsgBase msgBase = new MsgBase(3003);
        Debug.Log(msgBase.GetManager().ToString());


        //Debug.Log(transform.childCount);//6
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    Destroy(transform.GetChild(i).gameObject);
        //    Debug.Log(transform.childCount);//6 6 6 6 6 6
        //}
        //Debug.Log(transform.childCount);//6
        //能销毁所有子节点



        //---------------------------------
        //Debug.Log(transform.childCount);//6
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    Destroy(transform.GetChild(0).gameObject);//6 6 6 6 6 6
        //    Debug.Log(transform.childCount);
        //}
        //Debug.Log(transform.childCount);//6
        //只能销毁第一个子节点


        Debug.Log(transform.childCount);//6
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
            Debug.Log(transform.childCount);//5 4 3
        }
        Debug.Log(transform.childCount);//3
        //只能销毁前三个子节点

        //Debug.Log(transform.childCount);//6
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    DestroyImmediate(transform.GetChild(0).gameObject);
        //    Debug.Log(transform.childCount);//5 4 3
        //}
        //Debug.Log(transform.childCount);//3
        //只能销毁前三个子节点


        //结论：DestroyImmediate销毁游戏物体之后childCount能立即更新

    }
}