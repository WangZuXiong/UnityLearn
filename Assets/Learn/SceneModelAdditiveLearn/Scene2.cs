using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Scene2 : MonoBehaviour
{
    private void Awake()
    {
        transform.GetComponentInChildren<Button>().onClick.AddListener(GotoScene1);
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
        if (scene.name.Equals("Scene1"))
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Scene1"));
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Scene2"));
        }
    }

    private void GotoScene1()
    {
        SceneManager.LoadScene("Scene1", LoadSceneMode.Additive);
    }
}
