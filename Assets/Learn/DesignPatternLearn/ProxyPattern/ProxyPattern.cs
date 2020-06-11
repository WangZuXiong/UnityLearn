using UnityEngine;

//代理模式


public delegate void PlayFinish();

//实现层
public class DelegateAnimation : MonoBehaviour
{
    public Animation Anima;
    private PlayFinish _playFinish;
    private float _timeCount;

    public DelegateAnimation(PlayFinish playFinish)
    {
        _playFinish = playFinish;
    }


    public void PlayAnimation()
    {
        Anima.Play();
    }


    public bool IsFinish()
    {
        return Anima.isPlaying;
    }


    private void Update()
    {
        if (!IsFinish())
        {
            _timeCount += Time.deltaTime;

            if (_timeCount > 0.5f)
            {
                Debug.Log("play particle");
            }
        }
    }
}



//调用层
public class AnimationManager : MonoBehaviour
{
    private DelegateAnimation _delegateAnimation;

    //声明和代理一样的函数
    private void PlayFinish1()
    {
        Debug.Log("play pictal");
    }

    // Use this for initialization
    void Start()
    {
        _delegateAnimation = new DelegateAnimation(PlayFinish1);
        _delegateAnimation.PlayAnimation();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

//优点
//多个实现层 容易管理