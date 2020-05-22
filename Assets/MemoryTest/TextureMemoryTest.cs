using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


public class TextureMemoryTest : MonoBehaviour
{
    private Dictionary<string, Texture> _spriteDict;
    [SerializeField]
    private RawImage _rawImg;
    private Image _img;


    private void Start()
    {
        //_spriteDict = new Dictionary<string, Texture>
        //{
        //    { "4", Resources.Load<Texture>("ATM") }
        //};
        //就算没有被Image.sprite 或者 RawImage.texture引用也会被load到内存中

        //被Image.sprite 或者 RawImage.texture引用后 引用计数增加了 在内存中所占空间大小不变化
        //_rawImg.texture = _spriteDict["4"];

        //问题：从服务器上面下载下来的texture 赋值给gameobject之后,Destroy Gameobject 之前是都要对其中引用置空？是否置空之后texture才能从内存中Unload
        //- 需要对Image.sprite或者RawImage.texture置空，置空之后下载下来的texture没有引用之后调用Resources.UnloadUnusedAssets才能被unload。不是对Image或者RawImage置空

        _img = transform.Find("Image").GetComponent<Image>();
        var path = "http://192.168.1.243:8082/basketball/my_team_logo/dh6.png";

        StartCoroutine(AssetsService.Instance.DownTexture(path, false, (t) =>
        {
            _img.sprite = t;
        }));


        //问题：场景中原先有一个Image,其引用这一张图片，如果Destroy Image所在的Gameobject，那么这张图片是否能从内存中unload
        //- 不能，需要 将图片的Sprite引用置空之后才能unload

        //问题：场景中原有的 Image 中的material,其引用这一张图片，要将 Image.Material 置空，再调用 Resources.UnloadUnusedAssets() 可以unload 图片的内存

        //Resource Instantiate 到场景中的gameobject的对象 destroy之后，再调用 Resources.UnloadUnusedAssets() 可以unload gameObject中引用的所有资源的内存



        //GameObject img = Instantiate(Resources.Load<GameObject>("Lab1"), transform);

    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _img = null;
            Resources.UnloadUnusedAssets();

            //以下方式都不能断开Texture 与 Image的Sprite的引用
            //Destroy(_img);
            //_img.sprite = null;
            //_img.material = null;
            //Destroy(_img.gameObject);
            //_img = null;
            //Resources.UnloadUnusedAssets();

            //这才是断开引用的正确方式
            //_img.sprite = null;
            //_rawImg.texture = null;
            //_spriteDict = null;
            //_spriteDict.Clear();


            //Destroy(gameObject);
            //Destroy(transform.GetChild(0).gameObject);
            //_original = null;
            //Resources.UnloadUnusedAssets();

            //RawImage.texture的引用被断开了 但是由于_spriteDict对图片的引用未被断开 所以在内存中所占空间大小不变化
            //_rawImg.texture = null;
            //Resources.UnloadUnusedAssets();

            //RawImage.texture的引用被断开了 _spriteDict对图片的引用被断开 >>>>>>在内存中所占空间被清理掉<<<<<<
            //_rawImg.texture = null;
            //_spriteDict.Clear();
            //Resources.UnloadUnusedAssets();

            //RawImage.texture的引用被断开了 _spriteDict对图片的引用被断开 >>>>>>在内存中所占空间被清理掉<<<<<<
            //_rawImg.texture = null;
            //_spriteDict = null;
            //Resources.UnloadUnusedAssets();

            //RawImage.texture的引用被断开了 _spriteDict对图片的引用被断开 >>>>>>在内存中所占空间被清理掉<<<<<<  引用断开顺序无关紧要
            //_spriteDict = null;
            //_rawImg.texture = null;
            //Resources.UnloadUnusedAssets();


            //RawImage.texture的引用未被断开了 _spriteDict对图片的引用被断开 所以在内存中所占空间大小不变化
            //List<FieldInfo> infoList = new List<FieldInfo>();
            //if (this != null)
            //{
            //    Type type = this.GetType();

            //    FieldInfo[] propertyInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            //    for (int i = 0; i < propertyInfos.Length; i++)
            //    {
            //        Type type2 = propertyInfos[i].GetType();
            //        infoList.Add(propertyInfos[i]);
            //    }


            //    for (int i = 0; i < infoList.Count; i++)
            //    {
            //        try
            //        {
            //            infoList[i].SetValue(this, null);
            //        }
            //        catch (Exception e)
            //        {
            //            throw e;
            //        }
            //    }
            //}
            //Resources.UnloadUnusedAssets();

            //RawImage.texture的引用被断开了 _spriteDict对图片的引用被断开 >>>>>>在内存中所占空间被清理掉<<<<<<
            //_rawImg.texture = null;
            //List<FieldInfo> infoList = new List<FieldInfo>();
            //if (this != null)
            //{
            //    Type type = this.GetType();

            //    FieldInfo[] propertyInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            //    for (int i = 0; i < propertyInfos.Length; i++)
            //    {
            //        Type type2 = propertyInfos[i].GetType();
            //        infoList.Add(propertyInfos[i]);
            //    }

            //    for (int i = 0; i < infoList.Count; i++)
            //    {
            //        try
            //        {
            //            infoList[i].SetValue(this, null);
            //        }
            //        catch (Exception e)
            //        {
            //            throw e;
            //        }
            //    }
            //}
            //Resources.UnloadUnusedAssets();  
        }
    }

    private void OnDestroy()
    {
        //_img = null;

        Resources.UnloadUnusedAssets();  

        //_spriteDict对图片的引用会自动被断开

        //依然存在RawImage.texture的引用  在内存中所占空间大小不变化
        //_rawImg = null;

        //RawImage.texture的引用被断开了  在内存中所占空间大小不变化
        //_rawImg.texture = null;

        //依然存在RawImage.texture的引用  在内存中所占空间大小未被清理掉
        //_rawImg = null;
        //Resources.UnloadUnusedAssets();

        //RawImage.texture的引用被断开了 >>>>>>在内存中所占空间被清理掉<<<<<<
        //_rawImg.texture = null;
        //Resources.UnloadUnusedAssets();

        //依然存在RawImage.texture的引用  在内存中所占空间大小不变化
        //_rawImg = null;
        //_spriteDict.Clear();

        //RawImage.texture的引用被断开了  在内存中所占空间大小不变化
        //_rawImg.texture = null;
        //_spriteDict.Clear();

        //RawImage.texture的引用被断开了  在内存中所占空间大小不变化
        //_rawImg.texture = null;
        //_rawImg = null;
        //_spriteDict.Clear();
        //_spriteDict = null;


        //RawImage.texture的引用未被断开了  在内存中所占空间大小不变化
        //foreach (var item in _spriteDict)
        //{
        //    Destroy(item.Value);
        //}

        //RawImage.texture的引用被断开了  在内存中所占空间大小不变化
        //_rawImg.texture = null;
        //foreach (var item in _spriteDict)
        //{
        //    Destroy(item.Value);
        //}         
    }
}




//public class Base : MonoBehaviour
//{
//    protected virtual void OnDestroy()
//    {
//        if (this != null)
//        {
//            var type = GetType();
//            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
//            for (int i = 0; i < fields.Length; i++)
//            {
//                try
//                {
//                    fields[i].SetValue(this, null);
//                }
//                catch (Exception e)
//                {
//                    throw e;
//                }
//            }
//        }
//        Resources.UnloadUnusedAssets();
//    }
//}

//public class Test1 : Base
//{
//    private Image _img;
//    private Sprite _sprite;

//    protected override void OnDestroy()
//    {
//        _img.sprite = null;
//        base.OnDestroy();
//    }
//}