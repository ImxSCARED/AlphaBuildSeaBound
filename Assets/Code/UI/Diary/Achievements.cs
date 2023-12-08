using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    private int Score = 0;
    [SerializeField] private Diary_Mylestone[] m_MyleStones;
    [SerializeField] private PlayerManager playerManager;

    public void AddScore()
    {
        Score++;
        playerManager.AddDiaryEntry(m_MyleStones[Score].noteWords);
    }

    public void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AddScore();
            Destroy(gameObject);
        }
    }

    /*Achievements to add post Submission
     * Discover 4 Islands
     * Discover 8 Islands
     * Discover 12 Islands
     * Discover 16(All) Islands
     * Discover 3 Fish
     * Discover 5 Fish
     * Discover 10(All) Fish
     * Catch 5 Fish
     * Catch 10 Fish
     * Catch 20 Fish
     * Upgrade Once
     * Upgrade 3 Times
     * Upgrade 5 Times
     * Find EE 1
     * Find EE 2
    */



}
