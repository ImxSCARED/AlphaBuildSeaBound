using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UpgradeButton : MonoBehaviour
{
    public UpgradeData.UpgradeType m_UpgradeType;
    [SerializeField] private TextMeshProUGUI m_NameText;
    [SerializeField] private TextMeshProUGUI m_DescText;
    [SerializeField] private TextMeshProUGUI m_PriceText;
    public void SetInfo(Upgrade upgrade)
    {
        m_NameText.text = upgrade.Name;
        m_DescText.text = upgrade.Description;
        if (upgrade.Level == upgrade.MaxLevel)
        {
            m_PriceText.text = "Max";
        }
        else
        {
            m_PriceText.text = "$" + upgrade.Price.ToString();
        }

    }

    public void UpgradeSelected()
    {
        Upgrade upgrade = UpgradeManager.instance.BuyUpgrade(m_UpgradeType);
        if (upgrade)
        {
            SetInfo(upgrade);
        }
    }
}
