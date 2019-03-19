using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ExclamationAlert : MonoBehaviour
{
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void RestartLevel()
    {
        Info.IsGameOn = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
