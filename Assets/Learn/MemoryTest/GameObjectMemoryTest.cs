using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectMemoryTest : MonoBehaviour
{
    
 
    //非静态成员变量
    //private Image _img;
    //静态成员变量
    private Image _img;


    public static GameObjectMemoryTest Intsance;
    private void Awake()
    {
        Intsance = this;
        _img = transform.Find("Image").GetComponent<Image>();
    }


    private void OnDestroy()
    {
    
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            

        }
        else if (Input.GetMouseButtonDown(2))
        {

        }

    }

    
}

