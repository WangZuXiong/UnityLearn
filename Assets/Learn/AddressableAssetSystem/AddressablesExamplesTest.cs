using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddressablesExamplesTest : MonoBehaviour
{


    // Start is called before the first frame update
    async void Start()
    {
        return;
        var handleSp = Addressables.LoadAssetAsync<Sprite>("Assets/RawResources/Textures/CommonUI.png[1]");
        GetComponent<Image>().sprite = await handleSp.Task;

        AddressablesExamples.handle1 = handleSp;



        //var handleSp2 = Addressables.LoadAssetAsync<Sprite>("Assets/RawResources/Textures/atm (1).png[atm (1)_1]");
        //GetComponent<Image>().sprite = await handleSp2.Task;

        //AddressablesExamples.handle2 = handleSp2;



        var handleAudio = Addressables.LoadAssetAsync<AudioClip>("Audio");
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = await handleAudio.Task;
        audioSource.Play();
        AddressablesExamples.handle2 = handleAudio;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        return;
        //Addressables.Release(handle1);
        Addressables.Release(AddressablesExamples.handle2);

        Resources.UnloadUnusedAssets();
    }
}
