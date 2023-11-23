//Author: Jamie Wright
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int Money = 0;

    
    private UpgradeManager m_UpgradeManager;
    private QuestManager m_QuestManager;
    private MovementController m_MovementController;
    private bool isDocked = false;
    [HideInInspector] public bool isAtDock = false;
    private List<FishProperties.FishData> storedFish = new List<FishProperties.FishData>();

    [Header("FishSpawns")]
    [HideInInspector] public ZoneLevel currentZone = ZoneLevel.Zone1;

    private float fishSpawnTimer;
    public List<GameObject> fishOnMap = new List<GameObject>();
    public GameObject smallFish;
    public Transform[] zone1FishSpawn;
    public GameObject mediumFish;
    public Transform[] zone2FishSpawn;
    public GameObject largeFish;
    public Transform[] zone3FishSpawn;

    //UI
    [Header("Menu Button Identifiers")]
    public GameObject questsFirstButton;
    public GameObject upgradesFirstButton;
    public GameObject pauseFirstButton;
    public GameObject settingsFirstButton;

    [Header("HUB")]
    [SerializeField] private Canvas hub;

    [SerializeField] private QuestButton[] QuestUI;
    [SerializeField] private GameObject questsHolder;

    [SerializeField] private UpgradeButton[] UpradgeUI;
    [SerializeField] private GameObject upgradesHolder;

    [Header("Journal")]
    public JournalFish[] journalFishEntryies;
    [SerializeField] private Canvas journal;
    private bool journalOpen = false;
    private string[] diaryPages = new string[6];
    public GameObject[] journalTabs;
    private int entryCounter = 0;
    private int currentPage = 0;
    private int currentTab = 0;

    //Map Page
    [SerializeField] private TextMeshProUGUI IslandNameTxt;
    [SerializeField] private TextMeshProUGUI IslandDescTxt;
    [SerializeField] private IslandInfo[] islandInfos;
    [SerializeField] private GameObject[] islandButtons;
    //Fish Page
    [SerializeField] private Image silhoutte;
    [SerializeField] private Image cutiePatootie;
    [SerializeField] private TextMeshProUGUI fishName;
    [SerializeField] private TextMeshProUGUI fishSName;
    [SerializeField] private TextMeshProUGUI[] journalEntries;
    //Diary Page
    [SerializeField] private TextMeshProUGUI diaryTxtBoxP1;
    [SerializeField] private TextMeshProUGUI diaryTxtBoxP2;
    //Inventory
    [SerializeField] private Transform inventoryParent;
    [SerializeField] private GameObject fishIconPrefab;
    private int[] fishCounters = new int[10];

    [Header("Pause")]
    [SerializeField] private Canvas pause;
    private bool pauseOpen = false;
    [SerializeField] private Canvas settings;
    private bool settingsOpen = false;


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
        SpawnFish();

    }
    private void Update()
    {
        fishSpawnTimer += Time.deltaTime;
        if(fishSpawnTimer > 30)
        {
            SpawnFish();
        }
    }
    /// <summary>
    /// Every 5 minutes (or upon this being called when entering a new zone), despawns all fish unless its currently being fish, 
    /// then spawns them all at the current zones fishSpawnpoints currentZone is given when entering a trigger with the zoneDetection script on it
    /// </summary>
    public void SpawnFish()
    {
        fishSpawnTimer = 0;
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
                foreach (Transform spot in zone1FishSpawn)
                {
                    fishOnMap.Add(Instantiate(smallFish, spot.position, smallFish.transform.rotation));
                }
                break;

            case ZoneLevel.Zone2:
                foreach (Transform spot in zone2FishSpawn)
                {
                    fishOnMap.Add(Instantiate(mediumFish, spot.position, mediumFish.transform.rotation));
                }
                break;

            case ZoneLevel.Zone3:
                foreach (Transform spot in zone3FishSpawn)
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
        storedFish.Add(caughtFish);
        for (int i = 0; i < journalFishEntryies.Length; i++)
        {
            if (caughtFish.name == journalFishEntryies[i].fishName)
            {
                journalFishEntryies[i].amountCaught++;
                if(!journalFishEntryies[i].hasBeenCaught)
                    journalFishEntryies[i].hasBeenCaught = true;
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
        //Clears all of inventories children
        foreach (Transform child in inventoryParent)
        {
            Destroy(child.gameObject);
        }
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
                DpadNavigate(0);
            }
        }
    }

    public void PauseToggle()
    {
        if (settingsOpen == true)
        {
            toggleSettings();
            return;
        }
        if (pauseOpen)
        {
            GetComponent<InputManager>().ChangeActionMap("Sailing");
            Time.timeScale = 1;
            pause.enabled = false;
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
            pause.enabled = true;
            pauseOpen = true;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        }
    }
    public void toggleSettings()
    {
        pause.enabled = !pause.enabled;
        settings.enabled = !settings.enabled;
        settingsOpen = settings.enabled;
        if (settingsOpen)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(settingsFirstButton);
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene");
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
                currentPage = Math.Clamp(currentPage, 0, islandButtons.Length - 1);
                DisplaySelectedIsland();
            }
            else if (currentTab == 1)
            {
                currentPage = Math.Clamp(currentPage + ((int)value), 0, journalFishEntryies.Length - 1);
                DisplayFishPage();
            }
            else if (currentTab == 2)
            {
                //increases page by two, cause two pages diary, and clamps it 2 before .length because the display works of the first page, so 4 would display page 4 and 5.
                currentPage = Math.Clamp(currentPage + ((int)value * 2), 0, diaryPages.Length - 2);
                DisplayDiaryPages();
            }
        }
        
    }

    public void DpadNavigate(float value)
    {
        if (journalOpen)
        {
            journalTabs[currentTab].SetActive(false);
            currentTab = Math.Clamp(currentTab + (int)value, 0, journalTabs.Length - 1);
            journalTabs[currentTab].SetActive(true);
            currentPage = 0;
            if(currentTab == 0)
            {
                DisplaySelectedIsland();
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

    //Map
    private void DisplaySelectedIsland()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(islandButtons[currentPage]);
        if (islandInfos[currentPage].discovered)
        {
            IslandNameTxt.text = islandInfos[currentPage].islandName;
            IslandDescTxt.text = islandInfos[currentPage].islandDesc;
        }
        else
        {
            IslandNameTxt.text = "???";
            IslandDescTxt.text = "???";
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
                diaryPages[0] += "\n" + entry;
                break;
            case > 4 and <= 7:
                diaryPages[1] += "\n" + entry;
                break;
        }
        entryCounter++;

    }

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
            journalEntries[0].text = journalFishEntryies[currentPage].journalEntry1;
        }
        if (journalFishEntryies[currentPage].amountCaught >= journalFishEntryies[currentPage].amountBeforeEntry2)
        {
            journalEntries[1].text = journalFishEntryies[currentPage].journalEntry2;
        }
        if (journalFishEntryies[currentPage].amountCaught >= journalFishEntryies[currentPage].amountBeforeEntry3)
        {
            journalEntries[2].text = journalFishEntryies[currentPage].journalEntry3;
        }
    }

    public void IslandNamePopup(string islandName)
    {
        
    }
}
