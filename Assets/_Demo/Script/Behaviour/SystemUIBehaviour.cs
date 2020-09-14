using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemUIBehaviour : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Demo");
    }
}
