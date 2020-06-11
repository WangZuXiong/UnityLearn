using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ui.set;

public class SetController : MonoBehaviour
{
    public SetView SetView { get; set; }
    public SetModel SetModel { get; set; }
    public static SetController Instance { get; private set; }
    private Button _buttonSet;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _buttonSet = this.transform.Find("ButtonSet").GetComponent<Button>();
        _buttonSet.onClick.AddListener(OnButtonSetClick);
      

    }

    private void OnButtonSetClick()
    {
        SetModel = new SetModel();
        SetView = (SetView)UIFactory.Instance.GetUIView("SetView");
        SetView.Show();

        SetModel.OnValueChange += SetView.UpdateView;
        SetModel.UpdataData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Dispose()
    {
        SetModel.OnValueChange -= SetView.UpdateView;
        SetView = null;
        SetModel = null;
    }
}
