using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    private int Score = 0;
    [SerializeField] private Diary_Mylestone[] m_MyleStones;

    public void AddScore()
    {
        Score++;
        FindObjectOfType<PlayerManager>().AddDiaryEntry(m_MyleStones[Score].noteWords);
    }
}
