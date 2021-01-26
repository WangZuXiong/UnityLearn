using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddressablesExamples : MonoBehaviour
{
    //[SerializeField]
    //private AssetReference _asset;

    AsyncOperationHandle handle;
    AsyncOperationHandle handle1;
    AsyncOperationHandle handle2;


    GameObject cube;

    async void Start()
    {
        //IEnumerable enumerable = new object[] { "Cube Texture", "Test Label 1" };
        //IList<Texture2D> textures = await Addressables.LoadAssetsAsync<Texture2D>(enumerable, (t) =>
        //{
        //    t.name = "1";
        //}, Addressables.MergeMode.None, true).Task;

        //_instance.GetComponent<Renderer>().material.mainTexture = textures[0];



        //_original = await AddressableAssetManager.LoadAssetAsync<GameObject>("Cube");
        //var pos = new Vector3(0, 0, 0);
        //for (int i = 0; i < 20; i++)
        //{
        //    var cube = Instantiate(_original, transform);
        //    pos.x = i;
        //    cube.transform.rotation = Quaternion.identity;
        //    cube.transform.position = pos;
        //}


        //load texure 之后如何查看内存占用情况？
        //对texture的引用移除之后 内存会自动释放么？
        //如何释放？

        //transform.Find("Raw Image 1").GetComponent<RawImage>().texture = await Addressables.LoadAssetAsync<Texture>(_key).Task; 
        //transform.Find("Raw Image 2").GetComponent<RawImage>().texture = await Addressables.LoadAssetAsync<Texture>(_key).Task;

        //Raw Image 1 和 Raw Image 2 都依赖于同一个asset
        //Addressables.LoadAssetAsync ref +1
        //Addressables.Release ref -1
        //ref = 0资源才会unload



        //A B A 情况 需要 
        //Addressables.Release(key1) 2次
        //Addressables.Release(key2) 1次
        //才能unload所有的texture内存

        //var key1 = "Assets/RawResources/Textures/BgRanking.png";
        //var key2 = "Assets/RawResources/Textures/Texture2.png";
        //transform.Find("Raw Image 1").GetComponent<RawImage>().texture = await Addressables.LoadAssetAsync<Texture>(key1).Task;
        //transform.Find("Raw Image 1").GetComponent<RawImage>().texture = await Addressables.LoadAssetAsync<Texture>(key2).Task;
        //transform.Find("Raw Image 1").GetComponent<RawImage>().texture = await Addressables.LoadAssetAsync<Texture>(key1).Task;



        //Addressables.LoadAssetAsync  两遍  需要Addressables.Release两遍么？
        //是的


        //var key1 = "Assets/RawResources/Textures/BgRanking.png";
        //AsyncOperationHandle<Texture> asyncOperationHandle = Addressables.LoadAssetAsync<Texture>(key1);
        //handle = asyncOperationHandle;//泛型->非泛型 回收非泛型的handel 也能释放引用计数
        //transform.Find("Raw Image 1").GetComponent<RawImage>().texture = await asyncOperationHandle.Task;


        //Addressables.LoadAssetAsync 一个Prefab a ,然后实例化a得到A，需要把Addressables.ReleaseInstance(A)才能释放内存么
        //不需要

        //var key1 = "Cube";
        //AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(key1);
        //handle = asyncOperationHandle;
        //var original = await asyncOperationHandle.Task;
        //cube = GameObject.Instantiate(original);


        //如何加载图集中的一个精灵?
        //var handle =  Addressables.LoadAssetAsync<Sprite>("Assets/RawResources/Textures/atm (1).png[atm (1)_1]");
        //var sp = await handle.Task;
        //transform.Find("Image").GetComponent<Image>().sprite = sp;

        //Addressable 加载图集中的一张精灵 会把整张图集的内存加到内存里面么?
        handle = Addressables.LoadAssetAsync<Sprite>("Assets/RawResources/Textures/CommonUI.png[1]");
        await handle.Task;



        //Addressables.LoadAssetAsync A  Addressables.LoadAssetAsync A 第一次个第二次的耗时是一样的么？Addressables.LoadAssetAsync 会不会缓存？
        //不一样第一次耗时275ms，第一次之后耗时0ms;很有可能会缓存
        //for (int i = 0; i < 10; i++)
        //{
        //    await Foo();
        //}


        //这种情况下，所有的Addressables.LoadAssetAsync 是同时加载，耗时都是400+ms
        //for (int i = 0; i < 10; i++)
        //{
        //    Foo1();
        //}



        //InitializeAsync 指向唯一操作的AsyncOperationHandle  那么循环10次 他们都需要耗时么
        //第一次耗时1000ms 第二次耗时3  之后耗时0ms
        //for (int i = 0; i < 10; i++)
        //{
        //    await Instatiate();
        //}



        //InstantiateAsync的结果都是唯一的实例，回收 hanle1 不会吧handle2 回收掉吧？
        handle1 = Addressables.InstantiateAsync("Cube 1");
        await handle1.Task;

        handle2 = Addressables.InstantiateAsync("Cube 1");
        await handle2.Task;


        //回收了Asset 再去回收它的handle  回收了instance 再去回收它的handle 会报错么？

    }


    private async Task Instatiate()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();


        await Addressables.InstantiateAsync("Cube 1").Task;


        stopwatch.Stop();
        UnityEngine.Debug.LogError(stopwatch.ElapsedMilliseconds);

    }



    private async void Foo1()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();


        await Addressables.LoadAssetAsync<Texture2D>("Assets/RawResources/Textures/CommonUI.png").Task;


        stopwatch.Stop();
        UnityEngine.Debug.LogError(stopwatch.ElapsedMilliseconds);
    }


    private async Task Foo()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();


        await Addressables.LoadAssetAsync<Texture2D>("Assets/RawResources/Textures/CommonUI.png").Task;


        stopwatch.Stop();
        UnityEngine.Debug.LogError(stopwatch.ElapsedMilliseconds);
    }





    [SerializeField]
    private string _key = "Assets/RawResources/Textures/BgRanking.png";


    [SerializeField]
    private int _index = 0;
    [SerializeField]
    private GameObject _original;




    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Addressables.Release(handle1);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Addressables.Release(handle2);

            //Addressables.Release(transform.Find("Raw Image 1").GetComponent<RawImage>().texture);

            //AddressableAssetManager.ReleaseByKey(_key);

            //Addressables.Release(handle);

        }


        if (Input.GetMouseButtonDown(2))
        {
            //AddressableAssetManager.ReleaseAll();


            //Addressables.ReleaseInstance(cube);
        }
    }
}
