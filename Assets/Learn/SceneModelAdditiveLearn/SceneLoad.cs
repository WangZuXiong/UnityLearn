using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoad : MonoBehaviour
{
    public Text Text;

    private void Start()
    {
        Text.text = "加载数据等在这里操作" + Random.Range(0, 100);
        SceneManager.LoadScene("Scene1", LoadSceneMode.Additive);
    }
}
