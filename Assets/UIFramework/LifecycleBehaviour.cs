using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifecycleBehaviour : MonoBehaviour
{
    public UIView View;

    private void Start()
    {
        if (View != null)
        {
            View.OnStart();
        }
    }

    private void OnDestroy()
    {
        View = null;
    }
}
