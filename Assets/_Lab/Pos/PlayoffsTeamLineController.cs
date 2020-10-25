using UnityEngine;
using UnityEngine.UI;

public class PlayoffsTeamLineController : MonoBehaviour
{
    public float[] lengths = new float[] { 0, 0, 0, };

    private Image _line1;
    private Image _line2;
    private Image _line3;

    private float _time;

    public Color Color0;
    public Color Color1;
    //线条是否完成
    private bool _isComplete;

    private void Awake()
    {
        _line1 = transform.Find("Line1").GetComponent<Image>();
        _line2 = transform.Find("Line2").GetComponent<Image>();
        _line3 = transform.Find("Line3").GetComponent<Image>();
    }
    /// <summary>
    /// 设置线条的样式（颜色）
    /// </summary>
    /// <param name="isHome"></param>
    public void SetLineType(bool isHome = false)
    {
        if (isHome)
        {
            _line1.color = Color0;
            _line2.color = Color0;
            _line3.color = Color0;
        }
        else
        {
            _line1.color = Color1;
            _line2.color = Color1;
            _line3.color = Color1;
        }
    }

    public void Reset()
    {
        _line1.rectTransform.sizeDelta = new Vector2(0, _line1.rectTransform.sizeDelta.y);
        _line2.rectTransform.sizeDelta = new Vector2(0, _line2.rectTransform.sizeDelta.y);
        _line3.rectTransform.sizeDelta = new Vector2(0, _line3.rectTransform.sizeDelta.y);
    }
    public void SetFinish()
    {
        _line1.rectTransform.sizeDelta = new Vector2(lengths[0], _line1.rectTransform.sizeDelta.y);
        _line2.rectTransform.sizeDelta = new Vector2(lengths[1], _line2.rectTransform.sizeDelta.y);
        _line3.rectTransform.sizeDelta = new Vector2(lengths[2], _line3.rectTransform.sizeDelta.y);
    }

    public void Play(float time, float deley)
    {
        _time = time;
        CancelInvoke("Play");
        Invoke("Play", deley);
    }
    /// <summary>
    /// 如果已经初始化完成不会再初始化一遍
    /// </summary>
    /// <param name="time"></param>
    /// <param name="deley"></param>
    public void PlayNoReset(float time, float deley)
    {
        CancelInvoke("Play");
        if (_isComplete)
        {
            return;
        }
        _time = time;
        Invoke("Play", deley);
    }

    private void Play()
    {
        //Reset();
        //float lengthSum = lengths[0] + lengths[1] + lengths[2];
        //float time1 = (lengths[0] / lengthSum) * _time;
        //float time2 = (lengths[1] / lengthSum) * _time;
        //float time3 = (lengths[2] / lengthSum) * _time;

        //_isComplete = false;
        //if (time1 != 0)
        //{
        //    Go.to(_line1.rectTransform, time1, new GoTweenConfig()
        //        .sizeDelta(new Vector2(lengths[0], _line1.rectTransform.sizeDelta.y))
        //        .setEaseType(GoEaseType.Linear)
        //        );
        //}
        //if (time2 != 0)
        //{
        //    Go.to(_line2.rectTransform, time2, new GoTweenConfig()
        //        .setDelay(time1)
        //        .sizeDelta(new Vector2(lengths[1], _line2.rectTransform.sizeDelta.y))
        //        .setEaseType(GoEaseType.Linear)
        //        );
        //}
        //if (time3 != 0)
        //{
        //    Go.to(_line3.rectTransform, time3, new GoTweenConfig()
        //        .setDelay(time1 + time2)
        //        .sizeDelta(new Vector2(lengths[2], _line3.rectTransform.sizeDelta.y))
        //        .setEaseType(GoEaseType.Linear)
        //        .onComplete((t) =>
        //        {
        //            _isComplete = true;
        //        })
        //        );
        //}
    }

    public void Clear()
    {
        _isComplete = false;
        Reset();
        CancelInvoke("Play");
        //Go.killAllTweensWithTarget(_line1.rectTransform);
        //Go.killAllTweensWithTarget(_line2.rectTransform);
        //Go.killAllTweensWithTarget(_line3.rectTransform);

    }


#if UNITY_EDITOR

    //开发的时候使用
    [ContextMenu("设置长度")]
    private void SetWidth()
    {
        _line1 = transform.Find("Line1").GetComponent<Image>();
        _line2 = transform.Find("Line2").GetComponent<Image>();
        _line3 = transform.Find("Line3").GetComponent<Image>();

        lengths[0] = _line1.transform.GetComponent<RectTransform>().sizeDelta.x;
        lengths[1] = _line2.transform.GetComponent<RectTransform>().sizeDelta.x;
        lengths[2] = _line3.transform.GetComponent<RectTransform>().sizeDelta.x;
    }

#endif

}
