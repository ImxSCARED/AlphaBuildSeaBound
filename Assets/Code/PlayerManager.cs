using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UpgradeData;

public class PlayerManager : MonoBehaviour
{
    public int Money = 0;

    private UpgradeManager m_UpgradeManager;
    private QuestManager m_QuestManager;
    private MovementController m_MovementController;
    private bool isDocked = false;
    private List<FishProperties.FishData> storedFish = new List<FishProperties.FishData>();


    public List<JournalFish> journalEntries = new List<JournalFish>();

    //UI
    public GameObject hubFirstButton, journalFirstButton, pauseFirstButton;

    [Header("UI")]
    [SerializeField] private Canvas hub;
    [SerializeField] private UpgradeButton[] UpradgeUI;

    [SerializeField] private Canvas journal;
    [SerializeField] private QuestButton[] QuestUI;

    [SerializeField] private Canvas settings;

    public bool test;
    public ZoneLevel currentZone;
    public enum ZoneLevel
    {
        Zone1, Zone2, Zone3
    }
    private void Start()
    {
        m_UpgradeManager = GetComponent<UpgradeManager>();
        m_QuestManager = GetComponent<QuestManager>();
        m_MovementController = GetComponent<MovementController>();
    }
    private void Update()
    {
        if (test)
        {
            SellFish();
        }
    }
    public void AddFish(FishProperties.FishData caughtFish)
    {
        storedFish.Add(caughtFish);
        for (int i = 0; i < journalEntries.Count; i++)
        {
            if (caughtFish.name == journalEntries[i].name)
            {
                journalEntries[i].amountCaught++;
                journalEntries[i].hasBeenCaught = true;
            }
        }
    }
    public void Dock()
    {
        GetComponent<InputManager>().ChangeActionMap("UI");
        m_MovementController.StopMovement();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(hubFirstButton);

        hub.enabled = true;
        for (int i = 0; i < UpradgeUI.Length; i++)
        {
            for (int j = 0; j < m_UpgradeManager.m_Upgrades.Length; j++)
            {
                if (m_UpgradeManager.m_Upgrades[j].Type == UpradgeUI[i].m_UpgradeType)
                {
                    UpradgeUI[i].SetInfo(m_UpgradeManager.m_Upgrades[j]);
                    break;
                }
            }
        }
        for (int i = 0; i < QuestUI.Length; i++)
        {
            for (int j = 0; j < m_QuestManager.m_Quests.Length; j++)
            {
                if (m_QuestManager.m_Quests[j].Name == QuestUI[i].Name)
                {
                    QuestUI[i].SetInfo(m_QuestManager.m_Quests[j].quests[m_QuestManager.m_Quests[j].currentQuest]);
                    break;
                }
            }
        }
        isDocked = true;
        
    }

    public void SellFish()
    {
        foreach (FishProperties.FishData fish in storedFish)
        {
            if (fish.isQuestFish)
            {
                QuestManager.instance.ReturnQuestFish(fish);
            }
            switch (fish.tier)
            {
                case FishProperties.FishTier.SMALL:
                    Money += fish.value;
                    break;
                case FishProperties.FishTier.MEDIUM:
                    Money += fish.value;
                    break;
                case FishProperties.FishTier.LARGE:
                    Money += fish.value;
                    break;
            }
        }
        storedFish.Clear();
    }

    public void ExitHub()
    {
        GetComponent<InputManager>().ChangeActionMap("Sailing");
        hub.enabled = false;
        isDocked = false;
    }
}
