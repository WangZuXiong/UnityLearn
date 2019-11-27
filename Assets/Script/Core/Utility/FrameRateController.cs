using UnityEngine;
public class FrameRateController : MonoBehaviour
{
    [SerializeField]
    private int _lowFrame = 20;
    [SerializeField]
    private int _heightFrame = 30;
    [SerializeField]
    private int _screenIdleFrame;

    private int _changeFrameLimit = 30000;

    //降帧，并非所有场景都需要60帧（腾讯桌球游戏场景60帧，其他场景30帧；天天酷跑，在开始游戏前，FPS被限制为30，
    //游戏开始之后FPS才为60。天天飞车的FPS为30，但是当用户一段时间不点击界面后，FPS自动降）

    private void LateUpdate()
    {
        if (Time.frameCount % 2 == 0)
        {
            _screenIdleFrame += 2;
            if (_screenIdleFrame > _changeFrameLimit)
            {
                _screenIdleFrame = 0;
                Application.targetFrameRate = _lowFrame;
                Debug.LogWarning("长时间无操作，降帧");
            }
        }

        if (Input.anyKeyDown)
        {
            _screenIdleFrame = 0;
            if (Application.targetFrameRate != _heightFrame)
            {
                Application.targetFrameRate = _heightFrame;
            }   
        }
    }
}

