using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/// <summary>
/// 把控件的Script注册到UIManager
/// 可以直接查找物体
/// 把物体本身注册到UIManager
/// </summary>
public class UIBehaviour : MonoBehaviour
{
    private void Awake()
    {
        UIManager.Instance.RegistGameObject(name, gameObject);
    }

    public void AddButtonListenter(UnityAction unityAction)
    {
        if (unityAction != null)
        {
            Button button = transform.GetComponent<Button>();
            button.onClick.AddListener(unityAction);
        }
    }

    public void RemoveButtonListenter(UnityAction unityAction)
    {
        if (unityAction != null)
        {
            Button button = transform.GetComponent<Button>();
            button.onClick.RemoveListener(unityAction);
        }
    }

    public void SetImageSprite(Sprite sprite)
    {
        if (sprite != null)
        {
            Image image = transform.GetComponent<Image>();
            image.sprite = sprite;
        }
    }
}
