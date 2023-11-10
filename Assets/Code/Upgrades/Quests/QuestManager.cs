using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestManager : MonoBehaviour
{
    static public QuestManager instance;
    private PlayerManager m_PlayerManager;

    public bool test;

    public Quest[] m_Quests;
    public GameObject questFish;

    //Description of location as well as transform for Instantiate fish
    public struct ZoneBountySpawns
    {
        public string desctription;
        public Transform bountySpawn;
    }
    public ZoneBountySpawns[] Zone1Spawns;
    public ZoneBountySpawns[] Zone2Spawns;
    public ZoneBountySpawns[] Zone3Spawns;

    public GameObject currentBountyFish;

    
    void Start()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(this);
        }
        m_PlayerManager = GetComponent<PlayerManager>();
    }
    public void ReturnQuestFish(FishProperties.FishData questFish)
    {
        foreach(Quest quest in m_Quests)
        {
            if(quest.connectedFish.name == questFish.name)
            {
                quest.connectedUpgrade.Locked = false;
                m_PlayerManager.Money += quest.Value;
            }
        }
    }

    public void ChoosenQuest(string questName)
    {
        Quest quest = m_Quests[0];
        for(int i = 0; i < m_Quests.Length; i++)
        {
            if (m_Quests[i].Name == questName)
            {
                quest = m_Quests[i];
            }
        }
        //Spawns a fish 
        if (quest.completed)
        {
            return;
        }
        switch (quest.Zone)
        {
            case PlayerManager.ZoneLevel.Zone1:
                currentBountyFish = Instantiate(questFish, Zone1Spawns[Random.Range(0, Zone1Spawns.Length - 1)].bountySpawn);
                break;
            case PlayerManager.ZoneLevel.Zone2:
                currentBountyFish = Instantiate(questFish, Zone2Spawns[Random.Range(0, Zone2Spawns.Length - 1)].bountySpawn);
                break;
            case PlayerManager.ZoneLevel.Zone3:
                currentBountyFish = Instantiate(questFish, Zone3Spawns[Random.Range(0, Zone3Spawns.Length - 1)].bountySpawn);
                break;
        }
        //Sets the fishs tier and awaken it (Fish has OnAwake, assigns its fish data
        currentBountyFish.GetComponent<Fish>().tier = (FishProperties.FishTier)quest.Zone;
        currentBountyFish.SetActive(true);

        //Quest gets a refrence to the fish for later, and designates it as a quest fish
        currentBountyFish.GetComponent<Fish>().data.isQuestFish = true;
        quest.connectedFish = currentBountyFish.GetComponent<Fish>().data;
        quest.connectedFish.tier = (FishProperties.FishTier) quest.Zone;
        
    }
}
