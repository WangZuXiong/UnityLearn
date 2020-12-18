using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddressablesExamples : MonoBehaviour
{
    [SerializeField]
    private GameObject _instance;

    //[SerializeField]
    //private AssetReference _asset;

    async void Start()
    {
        ////1
        //_asset.LoadAssetAsync<GameObject>().Completed += OnLoadedCompleted;

        ////2
        //AsyncOperationHandle<GameObject> handle1 = _asset.LoadAssetAsync<GameObject>();
        //GameObject original = await handle1.Task;
        //_instance = Instantiate(original);

        ////3
        //GameObject handle2 = await _asset.LoadAssetAsync<GameObject>().Task;
        //_instance = Instantiate(handle2);

        ////4
        //_asset.InstantiateAsync().Completed += OnInstantiatedCompleted;

        ////5
        //AsyncOperationHandle<GameObject> handle3 = _asset.InstantiateAsync();
        //_instance = await handle3.Task;

        //6
        await Addressables.InstantiateAsync("Cube").Task;
        await Addressables.InstantiateAsync("Cube 1").Task;

        //for (int i = 0; i < 10; i++)
        //{
        //    var cube = await Addressables.InstantiateAsync("Cube 1").Task;
        //    cube.transform.localPosition = new Vector3(i, 0, 0);
        //}


        //IEnumerable enumerable = new object[] { "Cube Texture", "Test Label 1" };
        //IList<Texture2D> textures = await Addressables.LoadAssetsAsync<Texture2D>(enumerable, (t) =>
        //{
        //    t.name = "1";
        //}, Addressables.MergeMode.None, true).Task;

        //_instance.GetComponent<Renderer>().material.mainTexture = textures[0];
    }

    private void OnInstantiatedCompleted(AsyncOperationHandle<GameObject> obj)
    {
        _instance = obj.Result;
    }

    private void OnLoadedCompleted(AsyncOperationHandle<GameObject> obj)
    {
        _instance = Instantiate(obj.Result);
    }

    private void OnDestroy()
    {
        //_asset.ReleaseInstance(_instance);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Addressables.ReleaseInstance(_instance);
            //Addressables.Release(_asset);
        }
    }
}
