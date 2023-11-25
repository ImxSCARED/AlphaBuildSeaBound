using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject settingsFirstButton;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject MMFirstButton;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject creditsFirstButton;

    private CanvasGroup currentCG;
    private bool fadeOutStart;


    private void Update()
    {
        if(fadeOutStart)
            currentCG.alpha = Mathf.MoveTowards(currentCG.alpha, 0, 2*Time.deltaTime);

        
    }
    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
        AudioManager.instance.PlaySound("MainSettings");
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MMFirstButton);
        AudioManager.instance.PlaySound("MainBack");
        return;
    }
    public void OpenCreditsMenu()
    {
        creditsMenu.SetActive(true);
        mainMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(creditsFirstButton);
        AudioManager.instance.PlaySound("MainSettings");
    }

    public void CloseCreditsMenu()
    {
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MMFirstButton);
        AudioManager.instance.PlaySound("MainBack");
        return;
    }

    public void StartLevel()
    {
        AudioManager.instance.PlaySound("MainStart");
        Invoke("Delay", 5);
    }

    public void QuitGame()
    {
        Application.Quit();
 
    }

    public void FadeOut(CanvasGroup CG)
    {
        currentCG = CG;
        fadeOutStart = true;
    }

    public void Delay()
    {
        Debug.Log("and his music was electric...");
        SceneManager.LoadScene("Beta scene");
    }

    public void Awake()
    {
        AudioManager.instance.PlayTrack("Track_MainMenu");
    }
}

