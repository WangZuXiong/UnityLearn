using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Test1 : MonoBehaviour
{
    [SerializeField]
    private Vector2 _pos;
    [SerializeField]
    private Vector2 _size;


    private void Update()
    {
        var t = Foo(Camera.main, GameObject.Find("Target"), _size);
        GetComponent<Image>().material.SetVector("_info", t);
    }


    public Vector4 Foo(Camera camera, GameObject target, Vector2 offsetSize)
    {
        var targetRect = target.GetComponent<RectTransform>();
        var targetCenterPos = new Vector2(
            -(targetRect.pivot.x - .5f) * targetRect.rect.width * targetRect.localScale.x,
            -(targetRect.pivot.y - .5f) * targetRect.rect.height * targetRect.localScale.y);
        var targetWorldPoint = target.transform.TransformPoint(targetCenterPos);

        var imgRect = transform.GetComponent<RectTransform>();
        var xScale = Screen.width / (float)(imgRect.rect.width * imgRect.localScale.x);
        var yScale = Screen.height / (float)(imgRect.rect.height * imgRect.localScale.y);


        var targetScreenPos = camera.WorldToScreenPoint(targetWorldPoint);
        var imgCenterPos = new Vector2(
            -(imgRect.pivot.x - .5f) * imgRect.rect.width * imgRect.localScale.x,
            -(imgRect.pivot.y - .5f) * imgRect.rect.height * imgRect.localScale.y);
        var imgLeftBottomPos = new Vector2(
            imgCenterPos.x - 0.5f * imgRect.rect.width * imgRect.localScale.x,
            imgCenterPos.y - 0.5f * imgRect.rect.height * imgRect.localScale.y);

        var imgLeftBottomPosWorldPoint = transform.TransformPoint(imgLeftBottomPos);
        var imgScreenPos = camera.WorldToScreenPoint(imgLeftBottomPosWorldPoint);


        return new Vector4(
            (targetScreenPos.x - imgScreenPos.x) * xScale,
            (targetScreenPos.y - imgScreenPos.y) * yScale,
            (targetRect.rect.width + offsetSize.x) * xScale,
            (targetRect.rect.height + offsetSize.y) * yScale);
    }
}
