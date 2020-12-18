using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddressablesExamples : MonoBehaviour
{
    void Start()
    {
        Start3();

        Start0();
    }


    void Start0()
    {
        Addressables.InstantiateAsync("Assets/RawResources/Prefabs/Image.prefab");

        Addressables.LoadAssetAsync<GameObject>("Assets/RawResources/Prefabs/Image.prefab").Completed += OnCompleted;

        Addressables.LoadAssetAsync<Texture2D>("Assets/RawResources/Textures/Texture_A.png").Completed += (t) =>
        {
            if (t.Status == AsyncOperationStatus.Succeeded)
            {
                gameObject.AddComponent<RawImage>().texture = t.Result;
            }
        };

    }

    private void OnCompleted(AsyncOperationHandle<GameObject> obj)
    {
        GameObject gameObject = obj.Result;
        GameObject.Instantiate(gameObject, transform);
    }


    public IEnumerator Start1()
    {
        AsyncOperationHandle<Texture2D> handle = Addressables.LoadAssetAsync<Texture2D>("mytexture");
        yield return handle;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Texture2D texture = handle.Result;
            // The texture is ready for use.
            // ...
            // Release the asset after its use:
            Addressables.Release(handle);
        }
    }


    public async void Start2()
    {
        //Addressables.LoadAssetsAsync
        AsyncOperationHandle<Texture2D> handle = Addressables.LoadAssetAsync<Texture2D>("mytexture");
        await handle.Task;
        // The task is complete. Be sure to check the Status is successful before storing the Result.
    }


    public async void Start3()
    {
        var handle = Addressables.InstantiateAsync("Assets/RawResources/Prefabs/Image.prefab");
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {

        }
        else if (handle.Status == AsyncOperationStatus.Failed)
        {
            
        }
    }
}
