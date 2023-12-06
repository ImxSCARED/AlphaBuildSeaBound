using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(menuName = "Diary_Mylestone")]
public class Diary_Mylestone : ScriptableObject
{
    public int milestoneScore;
    [TextArea(15,20)]
    public string noteWords;
    public bool milestoneReached;
}
