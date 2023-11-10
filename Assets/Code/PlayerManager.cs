using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
=======
using UnityEngine.EventSystems;
>>>>>>> Jamies_Branch
using static UpgradeData;

public class PlayerManager : MonoBehaviour
{
    public int Money = 0;

    private UpgradeManager m_UpgradeManager;
<<<<<<< HEAD
=======
    private QuestManager m_QuestManager;
>>>>>>> Jamies_Branch
    private bool isDocked = false;
    private List<FishProperties.FishData> storedFish = new List<FishProperties.FishData>();
    
    
    public List<JournalFish> journalEntries = new List<JournalFish>();

<<<<<<< HEAD
    [Header("UI")]
    [SerializeField] private GameObject hub;
    [SerializeField] private GameObject upgradeButtonPrefab;

    [SerializeField] private GameObject journal;
    [SerializeField] private GameObject journalEntryPrefab;

    [SerializeField] private GameObject settings;

=======
    //UI
    public GameObject hubFirstButton, journalFirstButton, pauseFirstButton;

    [Header("UI")]
    [SerializeField] private Canvas hub;
    [SerializeField] private UpgradeButton[] UpradgeUI;
    
    [SerializeField] private Canvas journal;
    [SerializeField] private QuestButton[] QuestUI;

    [SerializeField] private Canvas settings;

    public bool test;
>>>>>>> Jamies_Branch
    private ZoneLevel currentZone;
    public enum ZoneLevel
    {
        Zone1, Zone2, Zone3
    }
    private void Start()
    {
        m_UpgradeManager = GetComponent<UpgradeManager>();
<<<<<<< HEAD
    }
    private void Update()
    {
        
=======
        m_QuestManager = GetComponent<QuestManager>();
    }
    private void Update()
    {
        if (test)
        {
            SellFish();
        }
>>>>>>> Jamies_Branch
    }
    public void AddFish(FishProperties.FishData caughtFish)
    {
        storedFish.Add(caughtFish);
        for(int i = 0; i < journalEntries.Count; i++)
        {
            if(caughtFish.name == journalEntries[i].name)
            {
                journalEntries[i].amountCaught++;
                journalEntries[i].hasBeenCaught = true;
            }
        }
    }
    public void Dock()
    {
        GetComponent<InputManager>().ChangeActionMap("UI");
<<<<<<< HEAD

        hub.SetActive(true);
        foreach(Upgrade UP in m_UpgradeManager.m_Upgrades)
        {
            UpgradeButton huh =  Instantiate(upgradeButtonPrefab, hub.transform).GetComponent<UpgradeButton>();
            huh.SetInfo(UP);
        }

=======
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(hubFirstButton);

        hub.enabled = true;
        for(int i = 0; i < UpradgeUI.Length; i++)
        {
            for(int j = 0; j < m_UpgradeManager.m_Upgrades.Length; j++)
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
                    QuestUI[i].SetInfo(m_QuestManager.m_Quests[j]);
                    break;
                }
            }
        }
>>>>>>> Jamies_Branch
        isDocked = true;
    }

    public void SellFish()
    {
        foreach(FishProperties.FishData fish in storedFish)
        {
<<<<<<< HEAD
=======
            if (fish.isQuestFish)
            {
                QuestManager.instance.ReturnQuestFish(fish);
            }
>>>>>>> Jamies_Branch
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
<<<<<<< HEAD
        hub.SetActive(false);
=======
        hub.enabled = false;
>>>>>>> Jamies_Branch
        isDocked = false;
    }

    public void OpenJournal()
    {
<<<<<<< HEAD
        foreach(JournalFish JF in journalEntries)
        {
            Instantiate(journalEntryPrefab, journal.transform).GetComponent<JournalFish>();
            //public void SetInfo(Upgrade upgrade)
            //{
            //    m_UpgradeType = upgrade.Type;
            //    m_NameText.text = upgrade.Name;
            //    m_DescText.text = upgrade.Description;
            //    m_PriceText.text = "$" + upgrade.Price.ToString();
            //}
            //put this in journal fish
        }
=======
        GetComponent<InputManager>().ChangeActionMap("UI");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(journalFirstButton);

        
>>>>>>> Jamies_Branch
    }
}
