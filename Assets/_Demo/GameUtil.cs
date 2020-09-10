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

    public void Delay(Action callback, float delay)
    {
        StartCoroutine(DelayCoroutine(callback, delay));
    }

    IEnumerator DelayCoroutine(Action callback, float delay)
    {
        var wfs = new WaitForSeconds(0.1f);
        while (delay > 0)
        {
            yield return wfs;
            delay -= 0.1f;
        }
        callback?.Invoke();
    }

    public void Delay(MonoBehaviour mono, Action callback, float delay)
    {
        mono.StopAllCoroutines();
        mono.StartCoroutine(DelayCoroutine(callback, delay));
    }
}
