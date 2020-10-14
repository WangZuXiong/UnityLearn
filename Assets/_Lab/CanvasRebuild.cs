using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class CanvasRebuild : MonoBehaviour
{

    IList<ICanvasElement> m_LayoutRebuildQueue;//Layout
    IList<ICanvasElement> m_GraphicRebuildQueue;//Graphic

    private void Awake()
    {
        System.Type type = typeof(CanvasUpdateRegistry);
        FieldInfo field = type.GetField("m_LayoutRebuildQueue", BindingFlags.NonPublic | BindingFlags.Instance);
        m_LayoutRebuildQueue = (IList<ICanvasElement>)field.GetValue(CanvasUpdateRegistry.instance);
        field = type.GetField("m_GraphicRebuildQueue", BindingFlags.NonPublic | BindingFlags.Instance);
        m_GraphicRebuildQueue = (IList<ICanvasElement>)field.GetValue(CanvasUpdateRegistry.instance);

        //是指UGUI库中Layout组件调整RectTransform尺寸、Graphic组件更新Mesh和Material，以及Mask执行Cull的过程  导致Rebuild
        //也就是一下这些情况等等
        //Text 文字变化
        //Recttransform 长宽变化（位移、缩放和旋转不会导致Rebuild） animation修改长宽不会导致rebuild
        //Image 更新Sprite 更新Material
        //添加Mask Mask的Disable和Enable
    }

    private void Update()
    {
        for (int j = 0; j < m_LayoutRebuildQueue.Count; j++)
        {
            var rebuild = m_LayoutRebuildQueue[j];
            if (ObjectValidForUpdate(rebuild))
            {
                Debug.LogFormat("{0}引起{1}网格重建   Layout", rebuild.transform.name, rebuild.transform.GetComponent<Graphic>().canvas.name);
            }
        }

        for (int j = 0; j < m_GraphicRebuildQueue.Count; j++)
        {
            var element = m_GraphicRebuildQueue[j];
            if (ObjectValidForUpdate(element))
            {
                Debug.LogFormat("{0}引起{1}网格重建   Graphic", element.transform.name, element.transform.GetComponent<Graphic>().canvas.name);
            }
        }
    }
    private bool ObjectValidForUpdate(ICanvasElement element)
    {
        var valid = element != null;

        var isUnityObject = element is Object;
        if (isUnityObject)
            valid = (element as Object) != null; //Here we make use of the overloaded UnityEngine.Object == null, that checks if the native object is alive.

        return valid;
    }
}