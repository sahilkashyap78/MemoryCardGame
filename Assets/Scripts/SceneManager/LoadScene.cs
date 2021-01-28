using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    private const int MAIN_MENU = 0;
    private const int PLAYSCENE = 1;

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU);
    }

    public void LoadPlayScene()
    {
        SceneManager.LoadScene(PLAYSCENE);
    }

}
