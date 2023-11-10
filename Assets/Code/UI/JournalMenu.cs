using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] PlayerInput m_playerInput;

    public bool journalState = false;
    private int J_Section = 1;
    private int J_DiaryPage = 1;
    private int J_FishPage = 1;
    public Scene scene;
    public bool MapState;
    public bool FishState;
    public bool DiaryState;
    InputAction Up;
    InputAction Down;
    InputAction Left;
    InputAction Right;
    InputAction JournalToggle;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        Up = m_playerInput.actions["Up"];
        Down = m_playerInput.actions["Down"];
        Left = m_playerInput.actions["Left"];
        Right = m_playerInput.actions["Right"];
        JournalToggle = m_playerInput.actions["JournalToggle"];
    }

    void Update()
    {
        m_playerInput.SwitchCurrentActionMap("UI");
        if (journalState)
        {
            //Closes journal menu
            if (JournalToggle.WasPerformedThisFrame())
            {
                Continue();
                return;
            }

            //Changes Section and Pages
            if (Down.WasPerformedThisFrame())
            {
                J_Section = J_Section + 1;
                if (J_Section > 3)
                {
                    J_Section = 3;
                }
                return;
            }

            if (Up.WasPerformedThisFrame())
            {
                J_Section = J_Section - 1;
                if (J_Section < 1)
                {
                    J_Section = 1;
                }
                return;
            }

            if (Right.WasPerformedThisFrame() && J_Section == 2)
            {
                J_FishPage = J_FishPage + 1;
                if (J_FishPage > 3)
                {
                    J_FishPage = 3;
                }
                return;
            }

            if (Left.WasPerformedThisFrame() && J_Section == 2)
            {
                J_FishPage = J_FishPage - 1;
                if (J_FishPage < 1)
                {
                    J_FishPage = 1;
                }
                return;
            }

            if (Right.WasPerformedThisFrame() && J_Section == 3)
            {
                J_DiaryPage = J_DiaryPage + 1;
                if (J_DiaryPage > 3)
                {
                    J_DiaryPage = 3;
                }
                return;
            }

            if (Left.WasPerformedThisFrame() && J_Section == 3)
            {
                J_DiaryPage = J_DiaryPage - 1;
                if (J_DiaryPage < 1)
                {
                    J_DiaryPage = 1;
                }
                return;
            }

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
        else
        {
            //Opens journal menu
            if (JournalToggle.WasPerformedThisFrame())
            {
                OpenJournalMenu();
                J_Section = 1;
                return;
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
