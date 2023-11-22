using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UpgradeData;

public class PlayerManager : MonoBehaviour
{
    public int Money = 0;

    
    private UpgradeManager m_UpgradeManager;
    private QuestManager m_QuestManager;
    private MovementController m_MovementController;
    private bool isDocked = false;
    [HideInInspector] public bool isAtDock = false;
    private List<FishProperties.FishData> storedFish = new List<FishProperties.FishData>();

    //UI
    [Header("Menu Button Identifiers")]
    public GameObject questsFirstButton;
    public GameObject upgradesFirstButton;
    public GameObject pauseFirstButton;
    public GameObject settingsFirstButton;

    [Header("UI")]
    [SerializeField] private Canvas hub;

    [SerializeField] private QuestButton[] QuestUI;
    [SerializeField] private GameObject questsHolder;

    [SerializeField] private UpgradeButton[] UpradgeUI;
    [SerializeField] private GameObject upgradesHolder;

    public JournalFish[] journalFishEntryies;
    [SerializeField] private Canvas journal;
    private bool journalOpen = false;
    private string[] diaryPages = new string[6];
    public GameObject[] journalTabs;
    private int entryCounter = 0;
    private int currentPage = 0;
    private int currentTab = 0;

    [SerializeField] private Image silhoutte;
    [SerializeField] private Image cutiePatootie;
    [SerializeField] private TextMeshProUGUI fishName;
    [SerializeField] private TextMeshProUGUI fishSName;
    [SerializeField] private TextMeshProUGUI[] journalEntries;

    [SerializeField] private TextMeshProUGUI diaryTxtBoxP1;
    [SerializeField] private TextMeshProUGUI diaryTxtBoxP2;

    [SerializeField] private TextMeshProUGUI[] fishCountTxt;
    private int[] fishCounters = new int[10];

    [SerializeField] private Canvas pause;
    private bool pauseOpen = false;
    [SerializeField] private Canvas settings;
    private bool settingsOpen = false;



    [HideInInspector] public ZoneLevel currentZone = ZoneLevel.Zone1;

    private float timer;
    public List<GameObject> fishOnMap = new List<GameObject>();
    public GameObject smallFish;
    public Transform[] zone1FishSpawn;
    public GameObject mediumFish;
    public Transform[] zone2FishSpawn;
    public GameObject largeFish;
    public Transform[] zone3FishSpawn;

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
        timer += Time.deltaTime;
        if(timer > 30)
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
        timer = 0;
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
                journalFishEntryies[i].hasBeenCaught = true;
            }
        }
        switch (caughtFish.name)
        {
            case "Noodles":
                fishCounters[0]++;
                fishCountTxt[0].text = fishCounters[0].ToString();
                break;
            case "Bass":
                fishCounters[1]++;
                fishCountTxt[1].text = fishCounters[1].ToString();
                break;
            case "Duckie":
                fishCounters[2]++;
                fishCountTxt[2].text = fishCounters[2].ToString();
                break;
            case "Swordfish":
                fishCounters[3]++;
                fishCountTxt[3].text = fishCounters[3].ToString();
                break;
            case "Siren":
                fishCounters[4]++;
                fishCountTxt[4].text = fishCounters[4].ToString();
                break;
            case "Shark":
                fishCounters[5]++;
                fishCountTxt[5].text = fishCounters[5].ToString();
                break;
            case "Leviathan":
                fishCounters[6]++;
                fishCountTxt[6].text = fishCounters[6].ToString();
                break;
            case "Hippocampus":
                fishCounters[7]++;
                fishCountTxt[7].text = fishCounters[7].ToString();
                break;
            case "Kraken":
                fishCounters[8]++;
                fishCountTxt[8].text = fishCounters[8].ToString();
                break;
            case "Cthylla":
                fishCounters[9]++;
                fishCountTxt[9].text = fishCounters[9].ToString();
                break;

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
        for (int i = 0; i < fishCounters.Length; i++)
        {
            fishCountTxt[i].text = "0";
            fishCounters[i] = 0;
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
            if (currentTab == 1)
            {
                currentPage = Math.Clamp(currentPage + ((int)value * 2), 0, journalFishEntryies.Length - 1);
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

    public void DisplayDiary()
    {
        currentPage = 0;
        diaryTxtBoxP1.gameObject.SetActive(!diaryTxtBoxP1.gameObject.activeSelf);
        diaryTxtBoxP2.gameObject.SetActive(!diaryTxtBoxP2.gameObject.activeSelf);
        DisplayDiaryPages();
    }

    private void DisplayFishPage()
    {
        silhoutte.sprite = journalFishEntryies[currentPage].fishSilhouette;
        if (journalFishEntryies[currentPage].hasBeenCaught)
        {
            cutiePatootie.SetEnabled(true);
            cutiePatootie.sprite = journalFishEntryies[currentPage].fishCutiePatootie;
            fishName.text = journalFishEntryies[currentPage].fishName;
            fishSName.text = journalFishEntryies[currentPage].fishSName;
        }
        else
        {
            cutiePatootie.SetEnabled(false);
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
}
