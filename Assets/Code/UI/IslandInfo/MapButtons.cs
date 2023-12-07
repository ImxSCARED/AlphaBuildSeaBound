using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapButtons : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI islandTitleTxt;
    [SerializeField] TextMeshProUGUI islandDescriptionTxt;
    [SerializeField] IslandInfo islandInfo;

    public void ChangeMapText()
    {
        if (islandInfo.discovered)
        {
            islandTitleTxt.text = islandInfo.islandName;
            islandDescriptionTxt.text = islandInfo.islandDesc;
        }
        else
        {
            islandTitleTxt.text = "???";
            islandDescriptionTxt.text = "???";
        }
    }
}
