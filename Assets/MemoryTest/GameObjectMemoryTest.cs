using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectMemoryTest : MonoBehaviour
{
    public static GameObjectMemoryTest Instance { get; set; }

    private void Awake()
    {
        Instance = this;
        _img = transform.Find("Image").GetComponent<Image>();
    }


    private GameObject _cube;

    private GameObject _instance;

    private GameObject _temp;

    private static Image _img;

    private void OnBtn()
    {
        Debug.LogError("btn click");
    }

    private void OnDestroy()
    {

        //Instance = null;
        //Resources.UnloadUnusedAssets();
        //不能被unload

        //_btn = null;
        //Resources.UnloadUnusedAssets();
        //能被unload

        //将btn的作用域改为Awake（）
        //Resources.UnloadUnusedAssets();
        //能被unload

        //btn 换成 img之后的结果是一样
        //Resources.UnloadUnusedAssets();
        //不能被unload

        //去除static的instance
        //Resources.UnloadUnusedAssets();
        //能被unload

        //去除image
        //Resources.UnloadUnusedAssets();
        //能被unload

        //image为static
        //_img = null;
        Resources.UnloadUnusedAssets();
    }

    void Start()
    {
        //_cube = Instantiate(Resources.Load<GameObject>("Cube"));
        //_cube.name = "Test Cube";


        //问题resources original 的内存在profiler中能否看到？实例化出来之后的gameobject destroy之后的是否会从内存中unload？
        //场景中本来存在的go，Destroy()-Resources.UnloadUnusedAssets()之后内存 能被回收
        //场景中本来存在的go， Destroy()-Resources.UnloadUnusedAssets()之后内存 能被回收

        //_cubeInstance = GameObject.Find("Cube").gameObject;

    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {


            //Destroy(_staticInstance.gameObject);
            //Resources.UnloadUnusedAssets();





            //Assets 中Cube(original)的内存占用被清理了 Scene中Cube(Clone)的内存未被清理
            //_cube = null;
            //Resources.UnloadUnusedAssets();

            //Assets 中Cube(original)的内存占用被清理了  Scene中Cube(Clone)的内存占用被清理了
            //Destroy(_cube);
            //_cube = null;
            //Resources.UnloadUnusedAssets();



            //test上的texture也会被load到内存中
            //_temp = Instantiate(Resources.Load<GameObject>("Test"));
        }
        else if (Input.GetMouseButtonDown(2))
        {
            //销毁之后调用resources.unloadUnusedAssets之后能将texture充内存中卸载
            //Destroy(_temp);
            //Resources.UnloadUnusedAssets();
        }

    }
}
