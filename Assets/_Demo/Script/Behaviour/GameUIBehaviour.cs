using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIBehaviour : MonoBehaviour
{
    public void Show()
    {
        GetComponent<Canvas>().enabled = true;
    }
}
