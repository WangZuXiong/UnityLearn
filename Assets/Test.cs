using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace memoryTest
{
    public class Test : MonoBehaviour
    {
        private Dictionary<string, Texture> _spriteDict;
        [SerializeField]
        private RawImage _rawImg;
        private void Start()
        {
            _spriteDict = new Dictionary<string, Texture>
            {
                { "4", Resources.Load<Texture>("ATM") }
            };
            //就算没有被Image.sprite 或者 RawImage.texture引用也会被load到内存中

            //被Image.sprite 或者 RawImage.texture引用后 引用计数增加了 在内存中所占空间大小不变化
            _rawImg.texture = _spriteDict["4"];
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
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
                _spriteDict = null;
                _rawImg.texture = null;
                Resources.UnloadUnusedAssets();


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
}



public class Base : MonoBehaviour
{
    protected virtual void OnDestroy()
    {
        if (this != null)
        {
            var type = GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++)
            {
                try
                {
                    fields[i].SetValue(this, null);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        Resources.UnloadUnusedAssets();
    }
}

public class Test1 : Base
{
    private Image _img;
    private Sprite _sprite;

    protected override void OnDestroy()
    {
        _img.sprite = null;
        base.OnDestroy();
    }
}