using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    static public UpgradeManager instance;
    private Fishing m_fishing;
    private MovementController m_movementController;
    private PlayerManager m_playerManager;

    public Upgrade[] m_Upgrades;
    [SerializeField] private UpgradeData m_UpgradeData;

    void Start()
    {
        instance = this;
        m_fishing = GetComponent<Fishing>();
        m_movementController = GetComponent<MovementController>();
    }

    private void ImplementUpgrade(Upgrade UpgradeToAdd)
    {

        switch (UpgradeToAdd.Type)
        {
            case UpgradeData.UpgradeType.AMMO:
                m_fishing.ammoUpgrade = m_UpgradeData.AmmoIncreaseAmount * UpgradeToAdd.Level;
                break;

            case UpgradeData.UpgradeType.SPEED:

                break;

            case UpgradeData.UpgradeType.TURN:

                break;

            case UpgradeData.UpgradeType.WRANGLE:
                m_fishing.wrangleUpgrade = m_UpgradeData.WrangleIncreaseAmount * UpgradeToAdd.Level;
                break;

            case UpgradeData.UpgradeType.SIZE:
                m_fishing.wrangleUpgrade = m_UpgradeData.WrangleIncreaseAmount * UpgradeToAdd.Level;
                break;
            case UpgradeData.UpgradeType.RANGE:
                m_fishing.rangeUpgrade = m_UpgradeData.RangeIncreaseAmount * UpgradeToAdd.Level;
                m_fishing.ChangeFishingRangeSize();
                break;
        }
    }
    public void BuyUpgrade(UpgradeData.UpgradeType type)
    {
        foreach(Upgrade UP in m_Upgrades)
        {
            if(UP.Type == type)
            {
                if(UP.Level == UP.MaxLevel)
                {
                    return;
                }
                if (m_playerManager.Money >= UP.Price)
                {
                    m_playerManager.Money -= UP.Price;
                    ImplementUpgrade(UP);

                    UP.Level++;
                    UP.Price = Mathf.RoundToInt(UP.Price * UP.PriceIncrease);
                    return;
                }
                return;
            }
        }
    }
    public void GameReloaded(Upgrade[] upgrades)
    {
        foreach(Upgrade UP in upgrades)
        {
            for(int i = 0; i < m_Upgrades.Length; i++)
            {
                if(m_Upgrades[i].Type == UP.Type)
                {
                    m_Upgrades[i] = UP;
                }
            }
        }
        foreach(Upgrade UP in m_Upgrades)
        {
            ImplementUpgrade(UP);
        }
    }
}
