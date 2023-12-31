//Author: Jamie Wright
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int Money = 0;

    public Fishing m_Fishing;
    private UpgradeManager m_UpgradeManager;
    private QuestManager m_QuestManager;
    private MovementController m_MovementController;
    public bool isDocked = false;
    public bool isAtDock = false;
    private List<FishProperties.FishData> storedFish = new List<FishProperties.FishData>();

    [Header("FishSpawns")]
    [HideInInspector] public ZoneLevel currentZone = ZoneLevel.Zone1;
    [HideInInspector] public List<GameObject> fishOnMap = new List<GameObject>();

    public int AmountOfFish;
    public TextMeshProUGUI moneyTxt;

    [Serializable]
    public class ZonicusSpawnicus
    {
        public Transform[] fishSpawns;
    }
    public GameObject smallFish;
    public ZonicusSpawnicus[] zone1FishSpawns;
    public GameObject mediumFish;
    public ZonicusSpawnicus[] zone2FishSpawns;
    public GameObject largeFish;
    public ZonicusSpawnicus[] zone3FishSpawns;

    //UI
    [Header("Menu Button Identifiers")]
    public GameObject questsFirstButton;
    public GameObject upgradesFirstButton;
    public GameObject pauseFirstButton;
    public GameObject settingsFirstButton;
    public GameObject controlsFirstButton;

    [Header("HUB")]
    [SerializeField] private Canvas hub;

    [SerializeField] public QuestButton[] QuestUI;
    [SerializeField] private GameObject questsHolder;

    [SerializeField] public UpgradeButton[] UpradgeUI;
    [SerializeField] private GameObject upgradesHolder;

    [Header("Journal")]
    public JournalFish[] journalFishEntryies;
    [SerializeField] private Canvas journal;
    public bool journalOpen = false;
    private string[] diaryPages = new string[6];
    public GameObject[] journalTabs;
    private int entryCounter = 1;
    private int currentPage = 0;
    private int currentTab = 0;

    //Map Page
    [SerializeField] private GameObject firstMapButton;
    //Fish Page
    [SerializeField] private Image silhoutte;
    [SerializeField] private Image cutiePatootie;
    [SerializeField] private TextMeshProUGUI fishName;
    [SerializeField] private TextMeshProUGUI fishSName;
    [SerializeField] private TextMeshProUGUI[] fishJournalTxts;
    //Diary Page
    [SerializeField] private TextMeshProUGUI diaryTxtBoxP1;
    [SerializeField] private TextMeshProUGUI diaryTxtBoxP2;
    //Inventory
    [SerializeField] private Transform inventoryParent;
    [SerializeField] private GameObject fishIconPrefab;
    private int[] fishCounters = new int[10];

    [Header("HUD")]
    [SerializeField] private Animation newEntryAnim;
    [SerializeField] private Animation islandNearAnim;
    [SerializeField] private TextMeshProUGUI islandNearTxt;
    [SerializeField] private Animator questTextAnimator;
    public TextMeshProUGUI harpoonCount;
    public bool questTextUp = false;
    
    [Header("Pause")]
    [SerializeField] private GameObject pause;
    private bool pauseOpen = false;
    [SerializeField] private GameObject settings;
    private bool settingsOpen = false;
    [SerializeField] private GameObject controls;
    private bool controlsOpen = false;

    //Tutorial
    [SerializeField] private TutorialManager journalTutorial;

    public enum ZoneLevel
    {
        Zone1, Zone2, Zone3
    }
    private void Start()
    {
        m_UpgradeManager = GetComponent<UpgradeManager>();
        m_QuestManager = GetComponent<QuestManager>();
        m_MovementController = GetComponent<MovementController>();
        SpawnFish();
        

    }
    /// <summary>
    /// Every 5 minutes (or upon this being called when entering a new zone), despawns all fish unless its currently being fish, 
    /// then spawns them all at the current zones fishSpawnpoints currentZone is given when entering a trigger with the zoneDetection script on it
    /// </summary>
    public void SpawnFish()
    {
        foreach (GameObject fish in fishOnMap)
        {
            if (fish != null)
            {
                if(fish.GetComponent<Fish>().dontDestory == false)
                    Destroy(fish);
            }
        }
        fishOnMap.Clear();

        switch (currentZone)
        {
            case ZoneLevel.Zone1:
                foreach (Transform spot in zone1FishSpawns[UnityEngine.Random.Range(0,zone1FishSpawns.Length)].fishSpawns)
                {
                    fishOnMap.Add(Instantiate(smallFish, spot.position, smallFish.transform.rotation));
                }
                break;

            case ZoneLevel.Zone2:
                foreach (Transform spot in zone2FishSpawns[UnityEngine.Random.Range(0, zone1FishSpawns.Length)].fishSpawns)
                {
                    fishOnMap.Add(Instantiate(mediumFish, spot.position, mediumFish.transform.rotation));
                }
                break;

            case ZoneLevel.Zone3:
                foreach (Transform spot in zone3FishSpawns[UnityEngine.Random.Range(0, zone1FishSpawns.Length)].fishSpawns)
                {
                    fishOnMap.Add(Instantiate(largeFish, spot.position, largeFish.transform.rotation));
                }
                break;
        }
    }

    /// <summary>
    /// Adds fish to inventory to be sold later, also adds onto the inventory counter on the journal
    /// </summary>
    /// <param name="caughtFish"></param>
    public void AddFish(FishProperties.FishData caughtFish)
    {
        if (caughtFish.isQuestFish)
        {
            m_QuestManager.currentQuestTitle.text = "No current quest";
            m_QuestManager.currentQuestDesc.text = "Select a quest at the dock.";
        }
        storedFish.Add(caughtFish);
        AmountOfFish++;
        for (int i = 0; i < journalFishEntryies.Length; i++)
        {
            if (caughtFish.name == journalFishEntryies[i].fishName)
            {
                journalFishEntryies[i].amountCaught++;
                if (!journalFishEntryies[i].hasBeenCaught)
                {
                    journalFishEntryies[i].hasBeenCaught = true;
                    newEntryAnim.Play();
                }
                    
            }
        }
        //Spawns image in inventory and sets its sprite to the caught fishs icon
        Instantiate(fishIconPrefab, inventoryParent).GetComponent<Image>().sprite = caughtFish.fishImage;
        
    }

    public void SellFish()
    {
        foreach (FishProperties.FishData fish in storedFish)
        {
            if (fish.isQuestFish)
            {
                m_QuestManager.ReturnQuestFish(fish);
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
        AmountOfFish = 0;
        //Clears all of inventories children
        foreach (Transform child in inventoryParent)
        {
            Destroy(child.gameObject);
        }
        moneyTxt.text = Money.ToString();
    }
    

    public void RemoveFishFromTracked(GameObject deadFish)
    {
        fishOnMap.Remove(deadFish);
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
            m_Fishing.ReplenishHarpoons();

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
            moneyTxt.text = Money.ToString();
        }
    }


    
    

    public void ExitHub()
    {
        GetComponent<InputManager>().ChangeActionMap("Sailing");
        hub.enabled = false;
        isDocked = false;
    }

    public void JournalToggle()
    {
        if (pauseOpen == false)
        {
            if (journalOpen)
            {
                GetComponent<InputManager>().ChangeActionMap("Sailing");
                Time.timeScale = 1;
                journal.enabled = false;
                journalOpen = false;

            }
            else
            {
                GetComponent<InputManager>().ChangeActionMap("UI");
                Time.timeScale = 0;
                journal.enabled = true;
                journalOpen = true;
                currentTab = 0;
                journalTabs[0].SetActive(false);
                journalTabs[1].SetActive(false);
                journalTabs[2].SetActive(false);
                DpadNavigate(0);
            }
        }
    }

    public void PauseToggle()
    {
        if (settingsOpen == true)
        {
            SettingsToggle();
            return;
        }
        if (pauseOpen)
        {
            GetComponent<InputManager>().ChangeActionMap("Sailing");
            Time.timeScale = 1;
            pause.SetActive(false);
            pauseOpen = false;
        }
        else
        {
            if (journalOpen)
            {
                JournalToggle();
            }
            GetComponent<InputManager>().ChangeActionMap("UI");
            Time.timeScale = 0;
            pause.SetActive(true);
            pauseOpen = true;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        }
    }
    public void SettingsToggle()
    {
        
        if (settingsOpen == true)
        {
            pause.SetActive(true);
            settings.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
            settingsOpen = false;
        }
        else if (settingsOpen == false)
        {
            pause.SetActive(false);
            settings.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(settingsFirstButton);
            settingsOpen = true;
        }
    }
    public void ControlsToggle()
    {

        if (controlsOpen == true)
        {
            settings.SetActive(true);
            controls.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(settingsFirstButton);
            controlsOpen = false;
        }
        else if (controlsOpen == false)
        {
            settings.SetActive(false);
            controls.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(controlsFirstButton);
            controlsOpen = true;
        }
    }




    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GoldMainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void BumperNavigate(float value)
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
        if (journalOpen)
        {
            if (currentTab == 0)
            {

            }
            else if (currentTab == 1)
            {
                currentPage = Math.Clamp(currentPage + ((int)value), 0, journalFishEntryies.Length - 1);
                DisplayFishPage();
            }
            else if (currentTab == 2)
            {
                //increases page by two, cause two pages diary, and clamps it 2 before .length because the display works of the first page, so 4 would display page 4 and 5.
                currentPage = Math.Clamp(currentPage + ((int)value * 2), 0, 4);
                DisplayDiaryPages();
            }
        }
        
    }
    /// <summary>
    /// Navigates between journal tabs
    /// </summary>
    /// <param name="value"></param>
    public void DpadNavigate(float value)
    {
        if (journalOpen) //Only works when journal open so it cant be called by accident
        {

            //Turns off the current tab, then goes to the next tab and turn it on
            journalTabs[currentTab].SetActive(false);
            currentTab = Math.Clamp(currentTab + (int)value, 0, journalTabs.Length - 1);
            journalTabs[currentTab].SetActive(true);

            //makes the current page 0 then starts the current tab off at 0 (display functions)
            currentPage = 0;
            if(currentTab == 0)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstMapButton);
            }
            if(currentTab == 1)
            {
                DisplayFishPage();
            }
            if(currentTab == 2)
            {
                DisplayDiaryPages();
            }
        }
        
    }


    //Diary
    public void DisplayDiary()
    {
        currentPage = 0;
        diaryTxtBoxP1.gameObject.SetActive(!diaryTxtBoxP1.gameObject.activeSelf);
        diaryTxtBoxP2.gameObject.SetActive(!diaryTxtBoxP2.gameObject.activeSelf);
        DisplayDiaryPages();
    }

    private void DisplayDiaryPages()
    {
        diaryTxtBoxP1.text = diaryPages[currentPage];
        diaryTxtBoxP2.text = diaryPages[currentPage + 1];
    }

    public void AddDiaryEntry(string entry)
    {
        switch (entryCounter)
        {
            case > 0 and <= 4:
                diaryPages[0] += entry;
                break;
            case > 4 and <= 8:
                diaryPages[1] += entry;
                break;
            case > 8 and <= 12:
                diaryPages[2] += entry;
                break;
            case > 12 and <= 15:
                diaryPages[3] += entry;
                break;
        }
        entryCounter++;
        newEntryAnim.Play();
    }
    //public void Milestone1()
    //{
    //    m_PlayerManager.addDiaryEntry(Milestone1Text);
    //}

    //FishInfo
    private void DisplayFishPage()
    {
        silhoutte.sprite = journalFishEntryies[currentPage].fishSilhouette;
        if (journalFishEntryies[currentPage].hasBeenCaught)
        {
            cutiePatootie.enabled = true;
            cutiePatootie.sprite = journalFishEntryies[currentPage].fishCutiePatootie;
            fishName.text = journalFishEntryies[currentPage].fishName;
            fishSName.text = journalFishEntryies[currentPage].fishSName;
        }
        else
        {
            cutiePatootie.enabled = false;
            fishName.text = "???";
            fishSName.text = "???";
        }
        if(journalFishEntryies[currentPage].amountCaught >= journalFishEntryies[currentPage].amountBeforeEntry1)
        {
            fishJournalTxts[0].text = journalFishEntryies[currentPage].journalEntry1;
        }
        else
            fishJournalTxts[0].text = "???";
        if (journalFishEntryies[currentPage].amountCaught >= journalFishEntryies[currentPage].amountBeforeEntry2)
        {
            fishJournalTxts[1].text = journalFishEntryies[currentPage].journalEntry2;
        }
        else
            fishJournalTxts[1].text = "???";
        if (journalFishEntryies[currentPage].amountCaught >= journalFishEntryies[currentPage].amountBeforeEntry3)
        {
            fishJournalTxts[2].text = journalFishEntryies[currentPage].journalEntry3;
        }
        else
            fishJournalTxts[2].text = "???";
    }

    public void IslandNamePopup(string islandName)
    {
        islandNearTxt.text = islandName;
        islandNearAnim.Play();
    }

    public void ExpandQuest()
    {
        if (questTextUp)
        {
            questTextAnimator.SetFloat("Speed", -1f);
            questTextAnimator.Play("BaseLayer.QuestText", 0, 1);
            questTextUp = false;
        }
        else
        {
            questTextAnimator.SetFloat("Speed", 1f);
            questTextAnimator.Play("BaseLayer.QuestText", 0, 0);
            questTextUp = true;
        }
    }
}
