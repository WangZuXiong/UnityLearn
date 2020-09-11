using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemUI : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Demo");
    }
}
