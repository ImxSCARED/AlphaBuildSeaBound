using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UpgradeData;

public class PlayerManager : MonoBehaviour
{
    public int Money = 0;

    private UpgradeManager m_UpgradeManager;
    private bool isDocked = false;
    private List<FishProperties.FishData> storedFish = new List<FishProperties.FishData>();
    
    
    public List<JournalFish> journalEntries = new List<JournalFish>();

    [Header("UI")]
    [SerializeField] private GameObject hub;
    [SerializeField] private GameObject upgradeButtonPrefab;

    [SerializeField] private GameObject journal;
    [SerializeField] private GameObject journalEntryPrefab;

    [SerializeField] private GameObject settings;

    private ZoneLevel currentZone;
    public enum ZoneLevel
    {
        Zone1, Zone2, Zone3
    }
    private void Start()
    {
        m_UpgradeManager = GetComponent<UpgradeManager>();
    }
    private void Update()
    {
        
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

        hub.SetActive(true);
        foreach(Upgrade UP in m_UpgradeManager.m_Upgrades)
        {
            UpgradeButton huh =  Instantiate(upgradeButtonPrefab, hub.transform).GetComponent<UpgradeButton>();
            huh.SetInfo(UP);
        }

        isDocked = true;
    }

    public void SellFish()
    {
        foreach(FishProperties.FishData fish in storedFish)
        {
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
        hub.SetActive(false);
        isDocked = false;
    }

    public void OpenJournal()
    {
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
    }
}