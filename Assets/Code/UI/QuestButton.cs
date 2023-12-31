using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestButton : MonoBehaviour
{
    public string Name;
    [SerializeField] private QuestManager m_QuestManager;
    [SerializeField] private TextMeshProUGUI m_NameText;
    [SerializeField] private TextMeshProUGUI m_DescText;
    [SerializeField] private TextMeshProUGUI m_PriceText;
    [SerializeField] public GameObject m_Blocker;

    public void SetInfo(Quest.QuestData quest)
    {
        m_NameText.text = quest.Name;
        m_DescText.text = "Unlocks " + quest.connectedUpgrade.Name;
        m_PriceText.text = "Reward: " + quest.Value.ToString();
    }

    public void QuestSelected()
    {
        if(m_QuestManager.currentBountyFish == null)
        {
            m_QuestManager.ChoosenQuest(Name);
        }
    }
}
