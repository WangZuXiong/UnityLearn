using UnityEngine;

public static class MicrophoneManager
{
    private static MicrophoneImpl _microphoneImpl = new MicrophoneImpl();
}

public class MicrophoneImpl
{
    private string _currentDeviceName;
    private int _minFreq;
    private int _maxFreq;
    private int _lengthSec;
    private int _freq;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="deviceNames">设备名单</param>
    /// <returns>判断设备是否有麦克风</returns>
    private bool CheckDevice(out string[] deviceNames)
    {
        if (Microphone.devices != null && Microphone.devices.Length > 0)
        {
            deviceNames = Microphone.devices;
            return true;
        }
        deviceNames = default;
        return false;
    }

    public AudioClip Start()
    {
        AudioClip audioClip = Microphone.Start(null, false, _lengthSec, _freq);
        return audioClip;
    }

    public void End()
    {
        Microphone.End(null);
    }


    public MicrophoneImpl()
    {
        //Microphone.GetDeviceCaps(deviceName, out int minFreq, out int maxFreq);
    }
}