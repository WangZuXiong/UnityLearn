using UnityEngine;

namespace ui.set
{
    public delegate void OnValueChangeHandle(SetModel setModel);

    public class SetModel
    {
       

        public SetData SetData { get; set; }
        public event OnValueChangeHandle OnValueChange;

        public SetModel()
        {
            SetData = new SetData();
            SetData.HasBGM = PlayerPrefs.GetInt("HasBGM") == 1;
            SetData.HasSFX = PlayerPrefs.GetInt("HasBGM") == 1;
        }


        public void UpdataData()
        {
            if (OnValueChange != null)
            {
                OnValueChange(this);
            }
        }
    }
}
