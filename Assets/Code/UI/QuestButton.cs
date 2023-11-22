using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestButton : MonoBehaviour
{
    public string Name;
    [SerializeField] private TextMeshProUGUI m_NameText;
    [SerializeField] private TextMeshProUGUI m_DescText;
    [SerializeField] private TextMeshProUGUI m_PriceText;
    public void SetInfo(Quest.QuestData quest)
    {
        m_NameText.text = quest.Name;
        m_DescText.text = "Unlocks " + quest.connectedUpgrade.Name;
        m_PriceText.text = "Reward: " + quest.Value.ToString();

    }

    public void QuestSelected()
    {
        if(QuestManager.instance.currentBountyFish == null)
        {
            QuestManager.instance.ChoosenQuest(Name);
        }
    }
}
