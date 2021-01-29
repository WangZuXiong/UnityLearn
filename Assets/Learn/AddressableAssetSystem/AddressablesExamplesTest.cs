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
        var handleSp = Addressables.LoadAssetAsync<Sprite>("Assets/RawResources/Textures/CommonUI.png[1]");
        GetComponent<Image>().sprite = await handleSp.Task;

        AddressablesExamples.handle1 = handleSp;



        var handleSp2 = Addressables.LoadAssetAsync<Sprite>("Assets/RawResources/Textures/atm (1).png[atm (1)_1]");
        GetComponent<Image>().sprite = await handleSp2.Task;

        AddressablesExamples.handle2 = handleSp2;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        //Addressables.Release(handle1);
        //Addressables.Release(handle2);

        Resources.UnloadUnusedAssets();
    }
}
