using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    
    [SerializeField] private PlayerManager m_PlayerManager;

    [SerializeField] public TextMeshProUGUI currentQuestTitle;
    [SerializeField] public TextMeshProUGUI currentQuestDesc;

    public Quest[] m_Quests;
    public GameObject questFish;

    //Description of location as well as transform for Instantiate questFish
    [System.Serializable]
    public struct ZoneBountySpawns
    {
        public string desctription;
        public Transform bountySpawn;
    }
    public ZoneBountySpawns[] Zone1Spawns;
    public ZoneBountySpawns[] Zone2Spawns;
    public ZoneBountySpawns[] Zone3Spawns;

    //refrence to current bounty fish
    public GameObject currentBountyFish;

    public void ReturnQuestFish(FishProperties.FishData questFish)
    {
        foreach(Quest quest in m_Quests)
        {
            if (quest.quests[quest.currentQuest].completed == false)
            {
                if (quest.quests[quest.currentQuest].connectedFish.name == questFish.name)
                {
                    quest.quests[quest.currentQuest].connectedUpgrade.Locked = false;
                    for (int i = 0; i < m_PlayerManager.UpradgeUI.Length; i++)
                    {
                        if (quest.quests[quest.currentQuest].connectedUpgrade.Type == m_PlayerManager.UpradgeUI[i].m_UpgradeType)
                        {
                            m_PlayerManager.UpradgeUI[i].SetInfo(quest.quests[quest.currentQuest].connectedUpgrade);
                            break;
                        }
                    }
                    quest.quests[quest.currentQuest].completed = true;
                    m_PlayerManager.Money += quest.quests[quest.currentQuest].Value;
                    currentBountyFish = null;
                    

                    for (int i = 0; i < m_PlayerManager.QuestUI.Length - 1; i++)
                    {
                        if (quest.Name == m_PlayerManager.QuestUI[i].Name)
                        {
                            m_PlayerManager.QuestUI[i].m_Blocker.GetComponentInChildren<TextMeshProUGUI>().text = "Locked Behind Upgrade";
                        }
                    }
                    AudioManager.instance.PlaySound("QuestDone");
                }
            }
        }

        for (int i = 0; i < m_PlayerManager.QuestUI.Length; i++)
        {
            for (int j = 0; j < m_Quests.Length; j++)
            {
                if (m_Quests[j].Name == m_PlayerManager.QuestUI[i].Name)
                {
                    //Checks if it has Max Quests, or if its locked behind an upgrade, else removes blocker
                    if (m_Quests[j].currentQuest == m_Quests[j].quests.Length)
                        break;
                    else if (m_PlayerManager.QuestUI[i].m_Blocker.GetComponentInChildren<TextMeshProUGUI>().text == "Locked Behind Upgrade")
                        break;
                    else
                    {
                        m_PlayerManager.QuestUI[i].m_Blocker.SetActive(false);
                        break;
                    }
                }
                
            }
        }
    }

    public void ChoosenQuest(string questName)
    {
        Quest quest = m_Quests[0];
        //Assigns the Quest from the list of Quests to local variable quest for furtheruse, using the string passed in the function as a search
        for(int i = 0; i < m_Quests.Length; i++)
        {
            if (m_Quests[i].Name == questName)
            {
                quest = m_Quests[i];
            }
        }

        //Spawns a fish if the quest isnt already completed
        if (!quest.quests[quest.currentQuest].completed)
        {
            if(quest.currentQuest != quest.quests.Length)
            {
                switch (quest.quests[quest.currentQuest].Zone)
                {
                    case PlayerManager.ZoneLevel.Zone1:
                        int i = Random.Range(0, Zone1Spawns.Length);
                        currentBountyFish = Instantiate(questFish, Zone1Spawns[i].bountySpawn);
                        currentQuestDesc.text = Zone1Spawns[i].desctription;
                        break;
                    case PlayerManager.ZoneLevel.Zone2:
                        int j = Random.Range(0, Zone2Spawns.Length);
                        currentBountyFish = Instantiate(questFish, Zone2Spawns[j].bountySpawn);
                        currentQuestDesc.text = Zone2Spawns[j].desctription;
                        break;
                    case PlayerManager.ZoneLevel.Zone3:
                        int k = Random.Range(0, Zone3Spawns.Length);
                        currentBountyFish = Instantiate(questFish, Zone3Spawns[k].bountySpawn);
                        currentQuestDesc.text = Zone3Spawns[k].desctription;
                        break;
                }
                currentQuestTitle.text = quest.Name;
                //Sets the fishs tier and awaken it (Fish has OnAwake, assigns its fish data
                currentBountyFish.GetComponent<Fish>().tier = (FishProperties.FishTier)quest.quests[quest.currentQuest].Zone;
                currentBountyFish.SetActive(true);

                //Quest gets a refrence to the fish for later, and designates it as a quest fish
                currentBountyFish.GetComponent<Fish>().data.isQuestFish = true;
                quest.quests[quest.currentQuest].connectedFish = currentBountyFish.GetComponent<Fish>().data;
                quest.quests[quest.currentQuest].connectedFish.tier = (FishProperties.FishTier)quest.quests[quest.currentQuest].Zone;

                for (int i = 0; i < m_PlayerManager.QuestUI.Length; i++)
                {
                    m_PlayerManager.QuestUI[i].m_Blocker.SetActive(true);
                    if (questName == m_PlayerManager.QuestUI[i].Name)
                    {
                        m_PlayerManager.QuestUI[i].m_Blocker.GetComponentInChildren<TextMeshProUGUI>().text = "Current Quest \nselected";

                    }
                }
            }
        }
    }


}
