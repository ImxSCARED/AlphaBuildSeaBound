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

    // --CODE VARIABLES--
    InputAction m_forwardAction;
    InputAction m_yawAction;
    InputAction m_lookAction;
    InputAction m_fishAction;
    InputAction m_dock;
    InputAction m_SailingJournalToggle;
    InputAction m_SailingPauseToggle;
    InputAction m_ExpandQuest;

    InputAction m_MinigameAction;
    InputAction m_ExitFishingAction;

    InputAction m_SellFish;
    InputAction m_ExitDock;
    InputAction m_UIJournalToggle;
    InputAction m_UIPauseToggle;
    InputAction BumperNav;
    InputAction DpadNav;
    

    // --UNITY METHODS--
    void Awake()
    {
        //Sailing
        m_forwardAction = m_playerInput.actions["Forward"];
        m_yawAction = m_playerInput.actions["Yaw"];
        m_lookAction = m_playerInput.actions["Look"];
        m_fishAction = m_playerInput.actions["Fish"];
        m_dock = m_playerInput.actions["Dock"];
        m_SailingJournalToggle = m_playerInput.actions["SailingJournalToggle"];
        m_SailingPauseToggle = m_playerInput.actions["SailingPauseToggle"];
        m_ExpandQuest = m_playerInput.actions["ExpandQuest"];

        //Fishing
        m_MinigameAction = m_playerInput.actions["MinigameMover"];
        m_ExitFishingAction = m_playerInput.actions["Exit"];

        //UI
        m_SellFish = m_playerInput.actions["SellFish"];
        m_ExitDock = m_playerInput.actions["ExitDock"];


        m_UIJournalToggle = m_playerInput.actions["UIJournalToggle"];
        m_UIPauseToggle = m_playerInput.actions["UIPauseToggle"];
        BumperNav = m_playerInput.actions["BumperNav"];
        DpadNav = m_playerInput.actions["DpadNav"];
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

        if (m_dock.WasPerformedThisFrame())
        {
            m_playerManager.Dock();
        }

        if (m_SailingJournalToggle.WasPerformedThisFrame())
        {
            m_playerManager.JournalToggle();
        }

        if (m_SailingPauseToggle.WasPerformedThisFrame())
        {
            m_playerManager.PauseToggle();
        }

        if (m_ExpandQuest.WasPerformedThisFrame())
        {
            m_playerManager.PlayAnim();
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

        if (m_UIJournalToggle.WasPerformedThisFrame())
        {
            m_playerManager.JournalToggle();
        }

        if (m_UIPauseToggle.WasPerformedThisFrame())
        {
            m_playerManager.PauseToggle();
        }

        if (BumperNav.WasPerformedThisFrame())
        {
            m_playerManager.BumperNavigate(BumperNav.ReadValue<float>());
        }

        if (DpadNav.WasPerformedThisFrame())
        {
            m_playerManager.DpadNavigate(DpadNav.ReadValue<float>());
        }
    }

    public void ChangeActionMap(string actionMap)
    {
        m_playerInput.SwitchCurrentActionMap(actionMap);
    }
}
