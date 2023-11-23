using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "JournalFish")]
public class JournalFish : ScriptableObject
{
    public Sprite fishSilhouette;
    public Sprite fishCutiePatootie;
    public bool hasBeenCaught;
    public string fishName;
    public string fishSName;

    //If we add levels of amount catched
    public int amountCaught;
    public int amountBeforeEntry1;
    [TextArea(15, 20)]
    public string journalEntry1;
    public int amountBeforeEntry2;
    [TextArea(15, 20)]
    public string journalEntry2;
    public int amountBeforeEntry3;
    [TextArea(15, 20)]
    public string journalEntry3;
}
