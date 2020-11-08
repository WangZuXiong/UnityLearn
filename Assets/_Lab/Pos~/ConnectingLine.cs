using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ConnectingLine : MonoBehaviour
{
    private static readonly float tan82 = Mathf.Tan(82 * Mathf.Deg2Rad);
    private static readonly float sin82 = Mathf.Sin(82 * Mathf.Deg2Rad);

    private PlayoffsTeamLineController _top;
    private PlayoffsTeamLineController _bottom;

    public enum LineType
    {
        Top,
        Bottom
    }

    private void Awake()
    {
        //_top = transform.Find("Animation/Top").GetComponent<PlayoffsTeamLineController>();
        //_bottom = transform.Find("Animation/Bottom").GetComponent<PlayoffsTeamLineController>();
    }

    public void SetLine(Transform target1, Transform target2)
    {
       
        //var screen2 = Camera.main.WorldToScreenPoint(target2.position);

      


        Init(transform.Find("Line/Top"), _top, target1);
        //Init(transform.Find("Line/Bottom"), _bottom, target2, new Vector2(Mathf.Abs(screen2.x - currentscreen2.x), Mathf.Abs(screen2.y - currentscreen2.y)));
    }

    private void Init(Transform lineParent, PlayoffsTeamLineController anima, Transform target)
    {
        var currentscreen2 = Camera.main.WorldToScreenPoint(transform.position);
        var screen1 = Camera.main.WorldToScreenPoint(target.position);
        Vector2 t1 = new Vector2(Mathf.Abs(screen1.x - currentscreen2.x), Mathf.Abs(screen1.y - currentscreen2.y));


        var worldPosDiffer = new Vector2(Mathf.Abs(target.position.x - transform.position.x), Mathf.Abs(target.position.y - transform.position.y));


        var lines = new RectTransform[3];

        var count = lineParent.childCount;
        for (int i = 0; i < count; i++)
        {
            lines[i] = lineParent.GetChild(i).GetComponent<RectTransform>();
        }

        var h = t1.y;
        var x = h / tan82;
        var yLength = h / sin82;

        var targetInLeft = target.position.x < transform.position.x;
        var targetInTop = target.position.y > transform.position.y;
        //右上左下-
        var t = (targetInLeft && !targetInTop) || (!targetInLeft && targetInTop) ? -1 : 1;
        var xLength = (t1.x + x * t) * 0.5f;
        //var scale = 1 / GetComponentInParent<Canvas>().transform.localScale.x;
        lines[0].position = target.position;
        lines[0].sizeDelta = new Vector2(xLength, 4);
        lines[0].pivot = targetInLeft ? new Vector2(0, 0.5f) : new Vector2(1, 0.5f);


        var worldXLenth = (worldPosDiffer.x + (worldPosDiffer.y / tan82) * t) * 0.5f;


        lines[1].position = targetInLeft ? target.position + new Vector3(worldXLenth, 0) : target.position - new Vector3(worldXLenth, 0);
        lines[1].pivot = targetInTop ? new Vector2(0, 0.5f) : new Vector2(1, 0.5f);
        lines[1].sizeDelta = new Vector2(yLength, 4);
        
        lines[2].position = targetInLeft ? transform.position - new Vector3(worldXLenth, 0) : transform.position + new Vector3(worldXLenth, 0);
        lines[2].sizeDelta = new Vector2(xLength, 4);
        lines[2].pivot = targetInLeft ? new Vector2(0, 0.5f) : new Vector2(1, 0.5f);


        //for (int i = 0; i < anima.lengths.Length; i++)
        //{
        //    anima.lengths[i] = lines[i].sizeDelta.x;
        //}

        //for (int i = 0; i < anima.transform.childCount; i++)
        //{
        //    var rect = anima.transform.GetChild(i).GetComponent<RectTransform>();
        //    rect.pivot = lines[i].pivot;
        //    rect.position = lines[i].position;
        //}
    }


    public void PlayAnimation(LineType lineType, float time, float deley)
    {
        if (lineType.Equals(LineType.Top))
        {
            _top.Clear();
            _top.Play(time, deley);
        }
        else if (lineType.Equals(LineType.Bottom))
        {
            _bottom.Clear();
            _bottom.Play(time, deley);
        }
    }

    public void ResetAnimation()
    {
        _top.Clear();
        _bottom.Clear();
    }
}
