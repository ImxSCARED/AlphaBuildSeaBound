using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public bool isAtDock = false;
    private List<FishProperties.FishData> storedFish = new List<FishProperties.FishData>();

    //UI
    [Header("Menu Button Identifiers")]
    public GameObject questsFirstButton;
    public GameObject upgradesFirstButton;
    public GameObject journalFirstButton;
    public GameObject pauseFirstButton;

    [Header("UI")]
    [SerializeField] private Canvas hub;

    [SerializeField] private QuestButton[] QuestUI;
    [SerializeField] private GameObject questsHolder;

    [SerializeField] private UpgradeButton[] UpradgeUI;
    [SerializeField] private GameObject upgradesHolder;

    public List<JournalFish> journalEntries = new List<JournalFish>();
    [SerializeField] private Canvas journal;
    private string[] diaryPages = new string[6];
    private int entryCounter = 0;
    private int currentPage = 0;
    private bool diaryOpen = true;
    [SerializeField] private TextMeshProUGUI diaryTxtBoxP1;
    [SerializeField] private TextMeshProUGUI diaryTxtBoxP2;

    [SerializeField] private Canvas settings;

    public bool test;
    private ZoneLevel currentZone;
    public enum ZoneLevel
    {
        Zone1, Zone2, Zone3
    }
    private void Start()
    {
        m_UpgradeManager = GetComponent<UpgradeManager>();
        m_QuestManager = GetComponent<QuestManager>();
        m_MovementController = GetComponent<MovementController>();
        diaryPages[0] = "1";
        diaryPages[1] = "2";
        diaryPages[2] = "3";
        diaryPages[3] = "4";
        diaryPages[4] = "5";
        diaryPages[5] = "6";

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
        if (isAtDock)
        {
            GetComponent<InputManager>().ChangeActionMap("UI");
            m_MovementController.StopMovement();

            questsHolder.SetActive(true);
            upgradesHolder.SetActive(false);
            hub.enabled = true;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(questsFirstButton);

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

    public void NavigateHub(float value)
    {
        if (isDocked)
        {
            questsHolder.SetActive(!questsHolder.activeSelf);
            upgradesHolder.SetActive(!upgradesHolder.activeSelf);

            if (questsHolder.activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(questsFirstButton);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(upgradesFirstButton);
            }
        }
        else if (diaryOpen)
        {
            //increases page by two, cause two pages diary, and clamps it 2 before .length because the display works of the first page, so 4 would display page 4 and 5.
            currentPage = Math.Clamp(currentPage + ((int)value * 2), 0, diaryPages.Length - 2);
            DisplayDiaryPages();
        }
    }

    public void AddDiaryEntry(string entry)
    {
        switch (entryCounter)
        {
            case > 0 and <= 4:
                diaryPages[0] += "\n" + entry;
                break;
            case > 4 and <= 7:
                diaryPages[1] += "\n" + entry;
                break;
        }
        entryCounter++;
        
    }

    private void DisplayDiaryPages()
    {
        diaryTxtBoxP1.text = diaryPages[currentPage];
        diaryTxtBoxP2.text = diaryPages[currentPage + 1];
    }
}
