using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[CreateAssetMenu(menuName = "Quest")]
public class Quest : ScriptableObject
{
    [System.Serializable]
    public struct QuestData
    {
        public string Name; //Name shown on quest UI
        public int Value; //Money given to player when completed
        public PlayerManager.ZoneLevel Zone; //Zone the fish will spawn in

        public Upgrade connectedUpgrade; //Upgrade it will unlock 

        public FishProperties.FishData connectedFish; //
        public bool completed; //If completed or not
    }
    public QuestData[] quests; //Stores list of questData, holding multiple quests per QuestButton that itll go through
    public int currentQuest = 0; //Used to refer to current quest, quests[currentQuest]
    public string Name; //Name of whole quest chain, used for identification when questManager loops through its list of Quest's
}
