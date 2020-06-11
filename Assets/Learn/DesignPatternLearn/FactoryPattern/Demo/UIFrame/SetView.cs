using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ui.set;
public class SetView : BaseView
{
    private Toggle _toggleSFX;
    private Toggle _toggleBGM;

    public SetView()
    {
        _viewName = "SetView";
    }

    private void Awake()
    {
        Transform content = this.transform.Find("Content").transform;
        Button buttonClose = content.transform.Find("ButtonClose").GetComponent<Button>();
        Button buttonOK = content.transform.Find("ButtonOK").GetComponent<Button>();
        _toggleSFX = content.Find("ToggleSFX").GetComponent<Toggle>();
        _toggleBGM = content.Find("ToggleBGM").GetComponent<Toggle>();

        buttonClose.onClick.AddListener(OnButtonClose);
        buttonOK.onClick.AddListener(OnButtonOK);
        _toggleSFX.onValueChanged.AddListener(OnToggleSFX);
        _toggleBGM.onValueChanged.AddListener(OnToggleBGM);
    }

    public void UpdateView(SetModel setModel)
    {
        this._toggleSFX.isOn = setModel.SetData.HasSFX;
        this._toggleBGM.isOn = setModel.SetData.HasBGM;
    }

    private void OnToggleBGM(bool arg0)
    {
        //throw new NotImplementedException();
    }

    private void OnToggleSFX(bool arg0)
    {
        //throw new NotImplementedException();
    }

    private void OnButtonClose()
    {
        Hide();
        SetController.Instance.Dispose();
    }

    private void OnButtonOK()
    {


        Hide();
    }

}
