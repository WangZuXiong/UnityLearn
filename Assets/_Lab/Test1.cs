using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Test1 : MonoBehaviour
{
    //[SerializeField]
    //private Vector2 _pos;
    [SerializeField]
    private Vector2 _size;

    private void Start()
    {

    }

    private void Update()
    {
        //Vector3 t = transform.TransformPoint(new Vector3(0, 0, 1));
        //Debug.LogError(t);
        //return;
        var t = Foo(Camera.main, GameObject.Find("Target"), _size);
        //GetComponent<Image>().material.SetVector("_info", new Vector4(_pos.x, _pos.y, _size.x, _size.y));
        GetComponent<Image>().material.SetVector("_info", t);
    }


    public Vector4 Foo(Camera camera, GameObject ui, Vector2 maskSize)
    {
        var rectTrans = ui.GetComponent<RectTransform>();
        var centerPos = new Vector2(
            -(rectTrans.pivot.x - .5f) * rectTrans.rect.width * rectTrans.localScale.x,
            -(rectTrans.pivot.y - .5f) * rectTrans.rect.height * rectTrans.localScale.y);
        var worldPoint = ui.transform.TransformPoint(centerPos);
        var screenPos = camera.WorldToScreenPoint(worldPoint);
        return new Vector4(screenPos.x, screenPos.y, rectTrans.rect.width + maskSize.x, rectTrans.rect.height + maskSize.y);
    }
}
