using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectMemoryTest : MonoBehaviour
{
    public static GameObjectMemoryTest Instance;
    private Action action;

    private void Awake()
    {
        Instance = this;
        action = () =>
        {
            var texture = Resources.Load<Texture2D>("ATM");
        };

        action.Invoke();
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Resources.UnloadUnusedAssets();
        }
        else if (Input.GetMouseButtonDown(2))
        {

        }

    }
}
