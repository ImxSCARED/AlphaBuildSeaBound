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
    // --CODE VARIABLES--
    InputAction m_forwardAction;
    InputAction m_yawAction;
    InputAction m_fishAction;
    InputAction m_MinigameAction;
    InputAction m_ExitFishingAction;
    InputAction m_SellFish;
    InputAction m_ExitDock;

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
    }

    void Update()
    {
        //Sailing
        if (m_forwardAction.inProgress)
        {
            m_playerController.AddVelocity(m_forwardAction.ReadValue<float>());
        }

        if (m_yawAction.inProgress)
        {
            m_playerController.Turn(m_yawAction.ReadValue<float>());
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
    }

    public void ChangeActionMap(string actionMap)
    {
        m_playerInput.SwitchCurrentActionMap(actionMap);
    }
}
