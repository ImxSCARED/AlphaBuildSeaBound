using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject mainMenu;

    public AudioSource open;
    public AudioSource close;

    private bool settingsMenuState = false;
    private bool levelMenuState = true;

    public void OpenAudioMenu()
    {
        settingsMenu.SetActive(true);
        settingsMenuState = true;
        mainMenu.SetActive(false);
        open.Play();
    }

    public void CloseAudioMenu()
    {
        settingsMenu.SetActive(false);
        settingsMenuState = false;
        mainMenu.SetActive(true);
        close.Play();
        return;
    }

    public void StartLevel()
    {
        open.Play();
        SceneManager.LoadScene("Scene_LevelAlpha(M)");
    }

    public void QuitGame()
    {
        Application.Quit();
 
    }


}

