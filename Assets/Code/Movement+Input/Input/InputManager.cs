using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
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
    Fishing m_playerFishing;
    [SerializeField]
    PlayerManager m_playerManager;
<<<<<<< HEAD
    [SerializeField]
    JournalMenu m_journalMenu;

=======
>>>>>>> Jamies_Branch
    // --CODE VARIABLES--
    InputAction m_forwardAction;
    InputAction m_yawAction;
    InputAction m_fishAction;
    InputAction m_MinigameAction;
    InputAction m_ExitFishingAction;
    InputAction m_SellFish;
    InputAction m_ExitDock;
<<<<<<< HEAD
    InputAction JournalToggle;
    InputAction Up;
    InputAction Down;
    InputAction Left;
    InputAction Right;
=======
>>>>>>> Jamies_Branch

    // --UNITY METHODS--
    void Awake()
    {
        //Sailing
        m_forwardAction = m_playerInput.actions["Forward"];
        m_yawAction = m_playerInput.actions["Yaw"];
        m_fishAction = m_playerInput.actions["Fish"];

        //Fishing
        m_MinigameAction = m_playerInput.actions["MinigameMover"];
        m_ExitFishingAction = m_playerInput.actions["Exit"];

        //UI
        m_SellFish = m_playerInput.actions["SellFish"];
        m_ExitDock = m_playerInput.actions["ExitDock"];
<<<<<<< HEAD

        JournalToggle = m_playerInput.actions["JournalToggle"];
        Up = m_playerInput.actions["MenuUp"];
        Down = m_playerInput.actions["MenuDown"];
        Left = m_playerInput.actions["MenuLeft"];
        Right = m_playerInput.actions["MenuRight"];
=======
>>>>>>> Jamies_Branch
    }

    void Update()
    {
        //Sailing
        if (m_forwardAction.inProgress)
        {
<<<<<<< HEAD
            m_playerController.Accelerate(m_forwardAction.ReadValue<float>());
=======
            m_playerController.AddVelocity(m_forwardAction.ReadValue<float>());
>>>>>>> Jamies_Branch
        }

        if (m_yawAction.inProgress)
        {
            m_playerController.Turn(m_yawAction.ReadValue<float>());
        }

        if (m_fishAction.WasPressedThisFrame())
        {
            m_playerFishing.FishMinigame();
        }

<<<<<<< HEAD
=======

>>>>>>> Jamies_Branch
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
<<<<<<< HEAD

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
            m_journalMenu.PageNext();
        }

        if (Left.WasPerformedThisFrame())
        {
            m_journalMenu.PagePrev();
        }
=======
>>>>>>> Jamies_Branch
    }

    public void ChangeActionMap(string actionMap)
    {
        m_playerInput.SwitchCurrentActionMap(actionMap);
    }
}
