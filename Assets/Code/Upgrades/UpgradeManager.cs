using UnityEngine;

//Author: JamieWright
public class UpgradeManager : MonoBehaviour
{
    static public UpgradeManager instance;
    private Fishing m_Fishing;
    [SerializeField] private FishingHitbox m_FishingHitbox;
    private MovementController m_MovementController;
    private PlayerManager m_PlayerManager;
    [SerializeField] private CaptureCircle m_CaptureCircle;
    public Upgrade[] m_Upgrades;
    [SerializeField] private UpgradeData m_UpgradeData;
    [SerializeField] private GameObject[] speedUpgradeVisuals;
    void Start()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this);
        }
        m_Fishing = GetComponent<Fishing>();
        m_MovementController = GetComponent<MovementController>();
        m_PlayerManager = GetComponent<PlayerManager>();
    }

    /// <summary>
    /// Depeding on which upgrade is passed through, sets the Upgrade variable in the related script to an amount.
    /// </summary>
    /// <param name="UpgradeToAdd"></param>
    private void ImplementUpgrade(Upgrade UpgradeToAdd)
    {
        
        switch (UpgradeToAdd.Type)
        {
            case UpgradeData.UpgradeType.AMMO:
                m_Fishing.ammoUpgrade = m_UpgradeData.AmmoIncreaseAmount * UpgradeToAdd.Level;
                break;

            case UpgradeData.UpgradeType.SPEED:
                m_MovementController.m_upgradeAmount = (m_UpgradeData.SpeedIncreaseAmount + 1) * UpgradeToAdd.Level;
                break;

            case UpgradeData.UpgradeType.WRANGLE:
                m_Fishing.wrangleUpgrade = (m_UpgradeData.WrangleIncreaseAmount + 1) * UpgradeToAdd.Level;
                break;

            case UpgradeData.UpgradeType.SIZE:
                m_CaptureCircle.sizeUpgrade = (m_UpgradeData.CatchingSizeIncreaseAmount + 1) * UpgradeToAdd.Level;
                m_CaptureCircle.ChangeCatcherSize();
                break;
            case UpgradeData.UpgradeType.RANGE:
                m_FishingHitbox.RedrawOval(m_FishingHitbox.fishingLineRenderer, m_FishingHitbox.fishingRadius * (m_UpgradeData.RangeIncreaseAmount + 1), m_FishingHitbox.fishingWidthRatio, m_FishingHitbox.fishingHeightRatio);
                break;
        }
        AudioManager.instance.PlaySound("UpgradedShip");
        ParticleManager.instance.PlayUpgradeParticle(transform.position - new Vector3(0,0,0));
    }

    /// <summary>
    /// Cheacks all requirements to get an upgrade is met, then implements upgrade, increases price and locks it again for next quest
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Upgrade BuyUpgrade(UpgradeData.UpgradeType type)
    {
        foreach (Upgrade UP in m_Upgrades)
        {
            //Checks if this is the correct upgrade in the list, then checks if it can be bought
            if (UP.Type == type)
            {
                if (!UP.Locked)
                {
                    if (UP.Level != UP.MaxLevel)
                    {
                        if (m_PlayerManager.Money >= UP.Price)
                        {
                            UpdateModel(UP);
                            m_PlayerManager.Money -= UP.Price;
                            UP.Level++;
                            UP.Locked = true;
                            ImplementUpgrade(UP);


                            UP.Price = Mathf.RoundToInt(UP.Price * UP.PriceIncrease);
                            UP.assignedQuest.currentQuest++;
                            return UP;
                        }
                    }

                }
                return null;
            }
        }
        return null;
    }

    private void UpdateModel(Upgrade curUpgrade)
    {
       if(curUpgrade.Type == UpgradeData.UpgradeType.SPEED)
       {
            speedUpgradeVisuals[curUpgrade.Level].SetActive(true);
       }
    }

    /// <summary>
    /// Used for saving and loading game, might not be implemented, takes in a array of upgrades
    /// </summary>
    /// <param name="upgrades"></param>
    public void GameReloaded(Upgrade[] upgrades)
    {
        foreach (Upgrade UP in upgrades)
        {
            for (int i = 0; i < m_Upgrades.Length; i++)
            {
                if (m_Upgrades[i].Type == UP.Type)
                {
                    m_Upgrades[i] = UP;
                }
            }
        }
        foreach (Upgrade UP in m_Upgrades)
        {
            ImplementUpgrade(UP);
        }
    }
}
