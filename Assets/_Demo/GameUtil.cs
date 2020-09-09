using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtil : MonoBehaviour
{
    private GameUtil() { }

    public static GameUtil Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Delay(float delay, Action callback)
    {
        StartCoroutine(DelayCoroutine(delay, callback));
    }

    IEnumerator DelayCoroutine(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}
