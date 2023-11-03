using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    public int Money = 0;

    private UpgradeManager m_UpgradeManager;
    private bool isDocked = false;
    private List<FishProperties.FishData> storedFish = new List<FishProperties.FishData>();

    [Header("UI")]
    [SerializeField] private GameObject hub;
    [SerializeField] private GameObject upgradeButtonPrefab;

    [SerializeField] private GameObject journal;

    [SerializeField] private GameObject settings;

    private void Update()
    {
        Debug.Log(isDocked);
    }
    public void AddFish(FishProperties.FishData caughtFish)
    {
        storedFish.Add(caughtFish);
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
}
