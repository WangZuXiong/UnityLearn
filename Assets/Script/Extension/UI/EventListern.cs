using UnityEngine;

public delegate void OnDoubleClickHandler();

public class EventListern : MonoBehaviour
{
    private float _time1;
    private float _time2;
    public event OnDoubleClickHandler OnDoubleClickEvent;

    private void CheckDoubleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _time2 = Time.realtimeSinceStartup;

            if (_time2 - _time1 < 0.2f)
            {
                OnDoubleClickEvent?.Invoke();
            }
            _time1 = _time2;
        }
    }
}
