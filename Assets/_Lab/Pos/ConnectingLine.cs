using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingLine : MonoBehaviour
{
    private static readonly float tan82 = Mathf.Tan(82f * Mathf.Deg2Rad);
    private static readonly float sin82 = Mathf.Sin(82f * Mathf.Deg2Rad);

    private PlayoffsTeamLineController _top;
    private PlayoffsTeamLineController _bottom;

    public enum LineType
    {
        Top,
        Bottom
    }

    private void Awake()
    {
        _top = transform.Find("Animation/Top").GetComponent<PlayoffsTeamLineController>();
        _bottom = transform.Find("Animation/Bottom").GetComponent<PlayoffsTeamLineController>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target1"></param>
    /// <param name="target2"></param>
    /// <param name="offset"></param>
    /// <param name="lineCount">线条的数量2或者3</param>
    public void SetLine(Transform target1, Transform target2, Vector3 offset, float targetParentScale, int lineCount = 3)
    {
        Init(transform.Find("Line/Top"), _top, target1, offset, targetParentScale, lineCount);
        Init(transform.Find("Line/Bottom"), _bottom, target2, offset, targetParentScale, lineCount);
    }
    //90-9.5=80.5
    //90-8=82
    private void Init(Transform lineParent, PlayoffsTeamLineController anima, Transform target, Vector3 offset, float targetParentScale, int lineCount)
    {
        var lines = new RectTransform[3];

        var count = lineParent.childCount;
        for (int i = 0; i < count; i++)
        {
            lines[i] = lineParent.GetChild(i).GetComponent<RectTransform>();
        }

        var relativePos = transform.InverseTransformPoint(target.position) + offset;


        var h = Mathf.Abs(relativePos.y);
        var x = h / tan82;
        var yLength = h / sin82;

        var targetInLeft = relativePos.x < 0;
        var targetInTop = relativePos.y > 0;
        //右上左下-
        var t = (targetInLeft && !targetInTop) || (!targetInLeft && targetInTop) ? -1 : 1;

        //var scale = 1 / GetComponentInParent<Canvas>().transform.localScale.x;



        if (lineCount == 3)
        {
            var xLengthHalf = (Mathf.Abs(relativePos.x) + x * t) * 0.5f;
            lines[0].position = target.position;
            lines[0].sizeDelta = new Vector2(xLengthHalf, 4);
            lines[0].pivot = targetInLeft ? new Vector2(0, 0.5f) : new Vector2(1, 0.5f);
            lines[1].position = target.TransformPoint(new Vector3(xLengthHalf * (targetInLeft ? 1 : -1) * targetParentScale, 0));  //length 受到target缩放影响
            lines[1].pivot = targetInTop ? new Vector2(0, 0.5f) : new Vector2(1, 0.5f);
            lines[1].sizeDelta = new Vector2(yLength, 4);
            lines[2].position = transform.TransformPoint(new Vector3(xLengthHalf * (targetInLeft ? -1 : 1), 0));
            lines[2].sizeDelta = new Vector2(xLengthHalf, 4);
            lines[2].pivot = targetInLeft ? new Vector2(0, 0.5f) : new Vector2(1, 0.5f);
        }
        else if (lineCount == 2)
        {
            var xLength = Mathf.Abs(relativePos.x) + x * t;
            lines[0].position = target.position;
            lines[0].sizeDelta = new Vector2(xLength, 4);
            lines[0].pivot = targetInLeft ? new Vector2(0, 0.5f) : new Vector2(1, 0.5f);
            lines[1].position = target.TransformPoint(new Vector3(xLength * (targetInLeft ? 1 : -1) * targetParentScale, 0));//length 受到target缩放影响
            lines[1].pivot = targetInTop ? new Vector2(0, 0.5f) : new Vector2(1, 0.5f);
            lines[1].sizeDelta = new Vector2(yLength, 4);
            lines[2].position = transform.position;
            lines[2].sizeDelta = Vector2.zero;
            lines[2].pivot = Vector2.zero;
        }

        for (int i = 0; i < anima.lengths.Length; i++)
        {
            anima.lengths[i] = lines[i].sizeDelta.x;
        }

        for (int i = 0; i < anima.transform.childCount; i++)
        {
            var rect = anima.transform.GetChild(i).GetComponent<RectTransform>();
            rect.pivot = lines[i].pivot;
            rect.position = lines[i].position;
        }
    }


    public void PlayAnimation(LineType lineType, float time, float deley)
    {
        if (lineType.Equals(LineType.Top))
        {
            _top.Clear();
            _top.Play(time, deley);
            _top.transform.SetAsLastSibling();
        }
        else if (lineType.Equals(LineType.Bottom))
        {
            _bottom.Clear();
            _bottom.Play(time, deley);
            _bottom.transform.SetAsLastSibling();
        }
    }

    public void ResetAnimation()
    {
        _top.Clear();
        _bottom.Clear();
    }
}