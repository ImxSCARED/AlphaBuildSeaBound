using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{

    [SerializeField] private int fadeInTime = 1;

    // Fish tutorial variables
    public bool fishTutorialCompleted = false;
    public CanvasGroup startFishTutorial;
    public bool startFishTutorialFadeIn = false;
    public bool startFishTutorialFadeOut = false;

    // Fishing minigame tutorials variables
    //public bool fishingMinigameTutorialCompleted = false;
    public CanvasGroup fishingMinigameTutorial;
    public bool fishingMinigameTutorialFadeIn = false;
    public bool fishingMinigameTutorialFadeOut = false;

    // Move tutorial variables
    public bool movementTutorialCompleted;
    public CanvasGroup MovementTutorial;
    public bool MovementTutorialFadeIn = false;
    public bool MovementTutorialFadeOut = false;

    // Journal tutorial variables
    public bool journalTutorialCompleted;
    public CanvasGroup journalTutorial;
    public bool journaltTutorialFadeIn = false;
    public bool journalTutorialFadeOut = false;

    // Update is called once per frame
    void Update()
    {
        //if (FishTutorialCompleted == false)
        //{
            if (startFishTutorialFadeIn)
            {
                if (startFishTutorial.alpha < fadeInTime)
                {
                    startFishTutorial.alpha += Time.deltaTime;
                }
                else
                {
                    startFishTutorialFadeIn = false;
                }
            }

            if (startFishTutorialFadeOut)
            {
                startFishTutorial.alpha -= Time.deltaTime;
                if (startFishTutorial.alpha <= 0)
                {
                    startFishTutorialFadeOut = false;
                }
            }

            if (fishingMinigameTutorialFadeIn)
            {
                if (fishingMinigameTutorial.alpha < fadeInTime)
                {
                    fishingMinigameTutorial.alpha += Time.deltaTime;
                }
                else
                {
                fishingMinigameTutorialFadeIn = false;
                }
            }
            
            if (fishingMinigameTutorialFadeOut)
            {
                fishingMinigameTutorial.alpha -= Time.deltaTime;
                if (fishingMinigameTutorial.alpha <= 0)
                {
                    fishingMinigameTutorialFadeOut = false;
                }
            }
        //}
        
    }

    public void StartFishTutorial()
    {
        if (fishTutorialCompleted == false)
        {
            startFishTutorialFadeIn = true;
        }
    }

    public void StopFishTutorial()
    {
        if (fishTutorialCompleted == false)
        {
            startFishTutorialFadeIn = false;
            startFishTutorialFadeOut = true;
        }
    }

    public void StartFishingMinigameTutorial()
    {
        if (fishTutorialCompleted == false)
        {
            fishingMinigameTutorialFadeIn = true;
        }
    }

    public void StopFishingMinigameTutorial()
    {
        if (fishTutorialCompleted == false)
        {
            fishingMinigameTutorialFadeIn = false;
            fishingMinigameTutorialFadeOut = true;
        }
    }

    public void StartMoveTutorial()
    {

    }

    public void StopMoveTutorial()
    {

    }

    public void StartJournalTutorial()
    {
        
    }

    public void StopJournalTutorial()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Move Tutorial Trigger")
        {
            if (movementTutorialCompleted == false)
            {
                StartMoveTutorial();
            }
        }

        if (other.name == "Journal Tutorial Trigger")
        {
            StartJournalTutorial();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Tutorial Trigger")
        {
            StopJournalTutorial();
            StopMoveTutorial();
        }
    }
}
