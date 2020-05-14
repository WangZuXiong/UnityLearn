using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public partial class Lab1 : MonoBehaviour
{
    private GameObject _original;


    private void Awake()
    {
        _original = Resources.Load<GameObject>("TestImage");
        GameObject img = Instantiate(_original, transform);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }

        if (Input.GetMouseButtonDown(2))
        {
            
        }
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
}
