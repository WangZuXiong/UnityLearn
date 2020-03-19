using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePanelController : MonoBehaviour
{
    protected virtual void Awake()
    {
        //var btnClose = GetBtnClose();
        //if (btnClose != null)
        //{
        //    btnClose.onClick.AddListener(OnBtnClose);
        //}
    }


    //protected virtual void OnBtnClose()
    //{
    //    PopupController.instance.ClosePopup();
    //}

    //protected Button GetBtnClose()
    //{
    //    return transform.Find("BtnClose").GetComponent<Button>();
    //}
}
