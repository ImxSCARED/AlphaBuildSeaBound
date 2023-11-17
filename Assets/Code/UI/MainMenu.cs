using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject mainMenu;

    private bool settingsMenuState = false;
    private bool levelMenuState = true;

    private void Awake()
    {
    
    }

    public void OpenAudioMenu()
    {
        settingsMenu.SetActive(true);
        settingsMenuState = true;
        mainMenu.SetActive(false);
        FindObjectOfType<AudioManager>().PlaySound("MenuToggle");
    }

    public void CloseAudioMenu()
    {
        settingsMenu.SetActive(false);
        settingsMenuState = false;
        mainMenu.SetActive(true);
        FindObjectOfType<AudioManager>().PlaySound("MenuToggle");
        return;
    }

    public void StartLevel()
    {
        FindObjectOfType<AudioManager>().PlaySound("MenuToggle");
        SceneManager.LoadScene("Myles_Beta_Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
 
    }


}

