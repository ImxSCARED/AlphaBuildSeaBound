using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // --EDITOR VARIABLES--
    [SerializeField]
    PlayerInput m_playerInput;
    [SerializeField]
    MovementController m_playerController;
    [SerializeField]
    CameraController m_cameraController;
    [SerializeField]
    Fishing m_playerFishing;
    [SerializeField]
    PlayerManager m_playerManager;
    [SerializeField]
    JournalMenu m_journalMenu;
    [SerializeField]
    PauseMenu m_pauseMenu;

    // --CODE VARIABLES--
    InputAction m_forwardAction;
    InputAction m_yawAction;
    InputAction m_lookAction;
    InputAction m_fishAction;
    InputAction m_MinigameAction;
    InputAction m_ExitFishingAction;
    InputAction m_SellFish;
    InputAction m_ExitDock;
    InputAction JournalToggle;
    InputAction Up;
    InputAction Down;
    InputAction Left;
    InputAction Right;
    InputAction PauseToggle;

    // --UNITY METHODS--
    void Awake()
    {
        //Sailing
        m_forwardAction = m_playerInput.actions["Forward"];
        m_yawAction = m_playerInput.actions["Yaw"];
        m_lookAction = m_playerInput.actions["Look"];
        m_fishAction = m_playerInput.actions["Fish"];

        //Fishing
        m_MinigameAction = m_playerInput.actions["MinigameMover"];
        m_ExitFishingAction = m_playerInput.actions["Exit"];

        //UI
        m_SellFish = m_playerInput.actions["SellFish"];
        m_ExitDock = m_playerInput.actions["ExitDock"];

        JournalToggle = m_playerInput.actions["JournalToggle"];
        Up = m_playerInput.actions["MenuUp"];
        Down = m_playerInput.actions["MenuDown"];
        Left = m_playerInput.actions["MenuLeft"];
        Right = m_playerInput.actions["MenuRight"];

        PauseToggle = m_playerInput.actions["PauseToggle"];
    }

    void Update()
    {
        //Sailing
        if (m_forwardAction.inProgress)
        {
            m_playerController.Accelerate(m_forwardAction.ReadValue<float>());
        }

        if (m_yawAction.inProgress)
        {
            m_playerController.Turn(m_yawAction.ReadValue<float>());
        }

        if (m_lookAction.inProgress)
        {
            Vector2 lookVector = m_lookAction.ReadValue<Vector2>();
            // Swap y (up) and x (sideways) - up is "around the x axis" and sideways is "around the y axis"
            m_cameraController.RotateCamera(new Vector2(lookVector.y, lookVector.x));
        }

        if (m_fishAction.WasPressedThisFrame())
        {
            m_playerFishing.FishMinigame();
        }

        //Fishing
        if (m_MinigameAction.inProgress)
        {
            m_playerFishing.MoveMM(m_MinigameAction.ReadValue<Vector2>());
        }

        if (m_ExitFishingAction.WasPerformedThisFrame())
        {
            m_playerFishing.EndMinigame(false);
        }

        //UI
        if (m_SellFish.WasPerformedThisFrame())
        {
            m_playerManager.SellFish();
        }

        if (m_ExitDock.WasPerformedThisFrame())
        {
            m_playerManager.ExitHub();
        }

        if (JournalToggle.WasPerformedThisFrame())
        {
            m_journalMenu.ToggleJournal();
        }

        if (Up.WasPerformedThisFrame())
        {
            m_journalMenu.SectionUp();
        }

        if (Down.WasPerformedThisFrame())
        {
            m_journalMenu.SectionDown();
        }

        if (Right.WasPerformedThisFrame())
        {
            m_journalMenu.DiaryPageNext();
        }

        if (Left.WasPerformedThisFrame())
        {
            m_journalMenu.DiaryPagePrev();
        }

        if (Right.WasPerformedThisFrame())
        {
            m_journalMenu.FishPageNext();
        }

        if (Left.WasPerformedThisFrame())
        {
            m_journalMenu.FishPagePrev();
        }

        if (PauseToggle.WasPerformedThisFrame())
        {
            m_pauseMenu.PauseMenuToggle();
        }
    }

    public void ChangeActionMap(string actionMap)
    {
        m_playerInput.SwitchCurrentActionMap(actionMap);
    }
}
