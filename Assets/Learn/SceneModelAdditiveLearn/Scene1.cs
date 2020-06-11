using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Scene1 : MonoBehaviour
{
    private void Awake()
    {
        transform.GetComponentInChildren<Button>().onClick.AddListener(GotoScene2);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnsceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnsceneLoad;
    }

    private void OnsceneLoad(Scene scene, LoadSceneMode mode)
    {
        Debug.LogError("OnsceneLoad:" + scene.name);
        if (scene.name.Equals("Scene2"))
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Scene2"));
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Scene1"));
        }
    }

    private void GotoScene2()
    {
        SceneManager.LoadScene("Scene2", LoadSceneMode.Additive);
    }
}
