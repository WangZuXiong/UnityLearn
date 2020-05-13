using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public partial class Lab1 : MonoBehaviour
{
    private GameObject _cubeOriginal;
    public GameObject _cube;


    private void Awake()
    {
        _cubeOriginal = Resources.Load<GameObject>("Cube");
        DestroyImmediate(_cubeOriginal, true);
    }
    Texture2D texture;
    Texture2D texture2;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //_cube = Instantiate(_cubeOriginal, transform);


            //var lab = GameObject.FindObjectOfType<Lab>();
            ////lab.Cube = _cube;


            texture = new Texture2D(1024, 1024);
            texture.name = "by new";


            //texture2 = Resources.Load<Texture2D>("ATM");
          

            var bytes= File.ReadAllBytes(@"E:\wangzuxiong\UnityLearn\Assets\Resources\ATM.jpg");

            texture2 = new Texture2D(0, 0);
            texture2.name = "by resources";
            texture2.LoadImage(bytes);
            //lab.texture2D = texture;
        }

        if (Input.GetMouseButtonDown(2))
        {
            //var lab = GameObject.FindObjectOfType<Lab>();
            //lab.texture2D = null;
            //Destroy(gameObject);
            ////texture = null;
            //Destroy(texture);
            texture = null;
            texture2 = null;
            Resources.UnloadAsset(texture);
            Resources.UnloadAsset(texture2);

            ////Resources.UnloadUnusedAssets();
        }
    }
}
