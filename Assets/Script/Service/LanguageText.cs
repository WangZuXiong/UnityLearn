using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class LanguageText : MonoBehaviour
{
    public string Key;

    private void Awake()
    {
        GetComponent<Text>().text = LanguageService.Instance.GetString(Key);
    }
}


