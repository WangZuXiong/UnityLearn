using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseView : MonoBehaviour
{
    protected string _viewName;



    public virtual void Show()
    {
        gameObject.SetActive(true);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    public virtual void Hide()
    {
        transform.localScale = Vector3.zero;
        Destroy(gameObject);
    }


}
