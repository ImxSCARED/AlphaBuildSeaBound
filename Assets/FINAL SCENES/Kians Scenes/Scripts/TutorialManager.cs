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
    public CanvasGroup movementTutorial;
    public bool movementTutorialFadeIn = false;
    public bool movementTutorialFadeOut = false;

    // Journal tutorial variables
    public bool journalTutorialCompleted;
    public CanvasGroup journalTutorial;
    public bool journalTutorialFadeIn = false;
    public bool journalTutorialFadeOut = false;

    // Update is called once per frame
    void Update()
    {
            
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

        if (journalTutorialFadeIn)
        {
            if (journalTutorial.alpha < fadeInTime)
            {
                journalTutorial.alpha += Time.deltaTime;
            }
            else
            {
                journalTutorialFadeIn = false;
            }
        }

        if (journalTutorialFadeOut)
        {
            journalTutorial.alpha -= Time.deltaTime;
            if (journalTutorial.alpha <= 0)
            {
                journalTutorialFadeOut = false;
            }
        }

        if (movementTutorialFadeIn)
        {
            if (movementTutorial.alpha < fadeInTime)
            {
                movementTutorial.alpha += Time.deltaTime;
            }
            else
            {
                movementTutorialFadeIn = false;
            }
        }

        if (movementTutorialFadeOut)
        {
            movementTutorial.alpha -= Time.deltaTime;
            if (movementTutorial.alpha <= 0)
            {
                movementTutorialFadeOut = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            Debug.Log("Backslash works");
            StopJournalTutorial();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("Backspace works");
            StartMovementTutorial();
        }

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

    public void StartMovementTutorial()
    {
        movementTutorialFadeIn = true;
    }

    public void StopMoveTutorial()
    {
        movementTutorialFadeIn = false;
        movementTutorialFadeOut = true;
        movementTutorialCompleted = true;
    }

    public void StartJournalTutorial()
    {
        journalTutorialFadeIn = true;
    }

    public void StopJournalTutorial()
    {
        journalTutorialFadeOut = true;
        journalTutorialFadeIn = false;
        journalTutorialCompleted = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Tutorial Trigger")
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
            Destroy(other.gameObject);
        }
    }
}
