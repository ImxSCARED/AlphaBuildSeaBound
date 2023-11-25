using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private JournalMenu journalMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject firstPMBtn;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private AudioSource openMenuSound;
    [SerializeField] private AudioSource exitMenuSound;

    public Scene scene;

    public bool pauseState = false;
    public bool audioMenuState = false;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    void Update()
    {
        //if audio menu is open, closes audio_menu
        if (Input.GetKeyDown(KeyCode.Escape) && audioMenuState == true)
        {
            CloseAudioMenu();
            return;
        }

        //opens pause_menu
        if (Input.GetKeyDown(KeyCode.Escape) && (pauseState == false) && (audioMenuState == false))
        {
            OpenPauseMenu();
            return;
        }

        //if audio menu is not open, closes pause_menu 
        if (Input.GetKeyDown(KeyCode.Escape) && pauseState == true)
        {
            Return();
        }
    }

    public void PauseMenuToggle()
    {
        //if audio menu is open, closes audio_menu
        if (audioMenuState == true)
        {
            CloseAudioMenu();
            return;
        }

        //opens pause_menu
        if (audioMenuState == false)
        {
            journalMenu.Continue();

            OpenPauseMenu();
            return;
        }

        //if audio menu is not open, closes pause_menu 
        if (pauseState == true)
        {
            Return();
        }
    }

    //Return
    public void Return()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        pauseState = false;
    }


    //Opens pause_menu
    private void OpenPauseMenu()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        pauseState = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstPMBtn);
    }

    //options button to open options
    public void AudioOptions()
    {
        pauseMenu.SetActive(false);
        audioMenu.SetActive(true);
        audioMenuState = true;
        pauseState = false;
    }

    //closes audio menu and returns to pause_menu
    public void CloseAudioMenu()
    {
        OpenPauseMenu();
        audioMenu.SetActive(false);
        audioMenuState = false;
        return;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Myles_MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
