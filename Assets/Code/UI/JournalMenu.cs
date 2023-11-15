using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class JournalMenu : MonoBehaviour
{
    [SerializeField] private Canvas journalMenu;
    [SerializeField] private GameObject MapSection;
    [SerializeField] private GameObject FishSection;
    [SerializeField] private GameObject DiarySection;
    [SerializeField] private AudioSource openMenuSound;
    [SerializeField] private AudioSource exitMenuSound;
    [SerializeField] private PauseMenu pauseMenu;

    public bool journalState = false;
    private int J_Section = 1;
    private int J_DiaryPage = 1;
    private int J_FishPage = 1;
    public Scene scene;
    public bool MapState;
    public bool FishState;
    public bool DiaryState;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    void Update()
    {
        if (journalState)
        {
            MapState = J_Section == 1;

            FishState = J_Section == 2;

            DiaryState = J_Section == 3;

            if (MapState == true)
            {
                MapSection.SetActive(true);
                FishSection.SetActive(false);
                DiarySection.SetActive(false);
            }
            else if (FishState == true)
            {
                MapSection.SetActive(false);
                FishSection.SetActive(true);
                DiarySection.SetActive(false);
            }
            else if (DiaryState == true)
            {
                MapSection.SetActive(false);
                FishSection.SetActive(false);
                DiarySection.SetActive(true);
            }
        }
    }

    public void SectionUp()
    {
        J_Section = J_Section - 1;
        if (J_Section < 1)
        {
            J_Section = 1;
        }
    }

    public void SectionDown()
    {
        J_Section = J_Section + 1;
        if (J_Section > 3)
        {
            J_Section = 3;
        }
    }

    public void DiaryPageNext()
    {
        J_DiaryPage = J_DiaryPage + 1;
        if (J_DiaryPage > 3)
        {
            J_DiaryPage = 3;
        }
    }

    public void DiaryPagePrev()
    {
        J_DiaryPage = J_DiaryPage - 1;
        if (J_DiaryPage < 1)
        {
            J_DiaryPage = 1;
        }
    }

    public void FishPageNext()
    {
        J_DiaryPage = J_DiaryPage + 1;
        if (J_DiaryPage > 3)
        {
            J_DiaryPage = 3;
        }
    }

    public void FishPagePrev()
    {
        J_DiaryPage = J_DiaryPage - 1;
        if (J_DiaryPage < 1)
        {
            J_DiaryPage = 1;
        }
    }

    public void ToggleJournal()
    {
        if (!pauseMenu.pauseState)
        {
            if (journalState)
            {
                Continue();
            }
            else
            {
                OpenJournalMenu();
                J_Section = 1;
            }
        }
    }

    public void Continue()
    {
        Time.timeScale = 1;
        journalMenu.enabled = false;
        journalState = false;
    }

    private void OpenJournalMenu()
    {
        Time.timeScale = 0;
        journalMenu.enabled = true;
        journalState = true;
    }
}
