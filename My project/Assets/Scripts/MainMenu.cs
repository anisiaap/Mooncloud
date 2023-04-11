using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //SceneManager.UnloadScene("MainMenuScene");
        SceneManager.LoadScene("main_game_scene", LoadSceneMode.Single);
    }

    public void EnterOptions()
    {
        //SceneManager.UnloadScene("MainMenuScene");
        SceneManager.LoadScene("OptionsScene", LoadSceneMode.Single);
    }

    public void EnterMenu()
    {
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("PauseScene");
    }

    public void QuitApp()
    {
        Application.Quit();
    }

}