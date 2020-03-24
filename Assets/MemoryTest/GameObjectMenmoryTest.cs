using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectMenmoryTest : MonoBehaviour
{
    private GameObject _cube;

    private GameObject _temp;

    void Start()
    {
        _cube = Instantiate(Resources.Load<GameObject>("Cube"));
        _cube.name = "Test Cube";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //Assets 中Cube(original)的内存占用被清理了 Scene中Cube(Clone)的内存未被清理
            _cube = null;
            Resources.UnloadUnusedAssets();

            //Assets 中Cube(original)的内存占用被清理了  Scene中Cube(Clone)的内存占用被清理了
            //Destroy(_cube);
            //_cube = null;
            //Resources.UnloadUnusedAssets();



            //test上的texture也会被load到内存中
            _temp = Instantiate(Resources.Load<GameObject>("Test"));
        }
        else if (Input.GetMouseButtonDown(2))
        {
            //销毁之后调用resources.unloadUnusedAssets之后能将texture充内存中卸载
            Destroy(_temp);
            Resources.UnloadUnusedAssets();
        }

    }
}
