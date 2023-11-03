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
    Fishing m_playerFish;

    // --CODE VARIABLES--
    InputAction m_forwardAction;
    InputAction m_yawAction;
    InputAction m_fishAction;
    InputAction m_MinigameAction;
    InputAction m_ExitFishingAction;

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
            m_playerFish.FishMinigame();
        }


        //Fishing
        if (m_MinigameAction.inProgress)
        {
            m_playerFish.MoveMM(m_MinigameAction.ReadValue<Vector2>());
        }

        if (m_ExitFishingAction.WasPerformedThisFrame())
        {
            m_playerFish.EndMinigame(false);
        }
    }

    public void ChangeActionMap(string actionMap)
    {
        m_playerInput.SwitchCurrentActionMap(actionMap);
    }
}
