//Author: Jamie Wright
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public TutorialManager instance;
    private PlayerManager m_PlayerManager;

    [SerializeField] private int fadeInTime = 1;


    [Header("Movement Tutorial")]
    private bool movementTutorialCompleted = false;
    [SerializeField] private CanvasGroup movementTutorial;
    private Vector3 playerStartingPosition;

    [Header("Fish Tutorial")]
    private bool fishTutorialCompleted = false;
    [SerializeField] private CanvasGroup fishTutorial;
    [SerializeField] private GameObject informativeArrowPrefab;
    private List<GameObject> currentArrows = new List<GameObject>();


    [Header("Fishing Tutorial")]
    private bool fishingMinigameTutorialCompleted = false;
    [SerializeField] private CanvasGroup fishingMinigameTutorial;

    [Header("Dock Tutorial")]
    private bool uptoDock = false;
    public bool gotoDockTutorialCompleted = false;
    [SerializeField] private CanvasGroup gotoDockTutorial;
    public bool dockTutorialCompleted = false;
    [SerializeField] private CanvasGroup dockTutorial;
    float timer = 0;
    bool ranOutOfHarpoons = false;

    [Header("Journal Tutorial")]
    
    private bool openJournalTutorialCompleted = false;
    [SerializeField] private CanvasGroup openJournalTutorial;
    private bool journalTutorialCompleted = false;
    [SerializeField] private CanvasGroup journalTutorial;

    private int pathsComplete = 0; //If 2, removes the tutorial

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        m_PlayerManager = GetComponent<PlayerManager>();
        playerStartingPosition = transform.position;
        RunMovementTutorial(true);
    }

    private void Update()
    {
        //Checks if this tutorial has been completed, if not runs a check relative to that tutorial, if completed start the next tutorial and do its check

        //Completes movement tutorial after moving 50 meters from spawn point
        if (!movementTutorialCompleted)
        {
            if(Vector3.Distance(playerStartingPosition, transform.position) > 50)
            {
                RunMovementTutorial(false);
                RunFishTutorial(true);
                movementTutorialCompleted = true;
            }
        }
        //Completes fishing tutorial after being next to a fish
        else if(!fishTutorialCompleted)
        {
            foreach(var fish in m_PlayerManager.fishOnMap)
            {
                //Second Check so if player is close to two fish doesnt run it twice
                if (!fishTutorialCompleted)
                {
                    if (Vector3.Distance(fish.transform.position, transform.position) < 30)
                    {
                        RunFishTutorial(false);
                        RunFishingTutorial(true);
                        fishTutorialCompleted = true;
                        break;
                    }
                }
                
            }
        }
        //Completes minigame after catching a fish, but starts dock tutorial after losing all harpoons to replenish them
        else if (!fishingMinigameTutorialCompleted)
        {
            if(m_PlayerManager.AmountOfFish > 0)
            {
                RunFishingTutorial(false);
                RunOpenJournalTutorial(true);
                fishingMinigameTutorialCompleted = true;
                uptoDock = true;
                return;
            }
            else if(GetComponent<Fishing>().currentHarpoons == 0)
            {
                if (!ranOutOfHarpoons)
                {
                    StartCoroutine(FadeOut(fishingMinigameTutorial));
                    uptoDock = true;
                    RunGotoDockTutorial(true);
                }
                ranOutOfHarpoons = true;
            }
        }

        //if finished minigame tut, lets you perform journal tutorials

        //Completes open journal tut after opening journal
        if (fishingMinigameTutorialCompleted && !openJournalTutorialCompleted)
        {
            if (m_PlayerManager.journalOpen)
            {
                //Set alpha instad of coroutineFade because open journal pause timescale, halting all coroutines
                openJournalTutorial.alpha = 0;
                journalTutorial.alpha = 1;
                openJournalTutorialCompleted = true;
            }
        }
        //Completes journal tut after closing it
        else if (fishingMinigameTutorialCompleted && !journalTutorialCompleted)
        {
            if (m_PlayerManager.journalOpen == false)
            {
                journalTutorial.alpha = 0;
                if (!gotoDockTutorialCompleted)
                {
                    RunGotoDockTutorial(true);
                }
                    
                journalTutorialCompleted = true;
                pathsComplete++;
            }
        }
        //if they completed one of the paths in minigame tutorial, lets them complete docking tutorials

        //completes after opening quest info (It tells them to go to dock)
        else if (uptoDock && !gotoDockTutorialCompleted)
        {
            if (m_PlayerManager.questTextUp)
            {
                currentArrows.Add(Instantiate(informativeArrowPrefab, FindObjectOfType<Dock>().transform));
                currentArrows[0].transform.rotation = Quaternion.identity;
                currentArrows[0].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                currentArrows[0].transform.position += Vector3.up;
                RunGotoDockTutorial(false);
                dockTutorial.alpha = 1;
                gotoDockTutorialCompleted = true;
            }
        }
        //Completes after closing the shop after having it open for longer than 2 seconds  
        else if (uptoDock && gotoDockTutorialCompleted && !dockTutorialCompleted)
        {
            if (m_PlayerManager.isDocked)
            {
                if(currentArrows.Count > 0)
                {
                    Destroy(currentArrows[0]);
                    currentArrows.Clear();
                }
                timer += Time.deltaTime;
            }
            if (!m_PlayerManager.isDocked)
            {
                if(timer > 2)
                {
                    dockTutorial.alpha = 0;
                    dockTutorialCompleted = true;
                    pathsComplete++;
                }
                timer = 0;
            }
        }

        if (pathsComplete > 1)
            RemoveTutorial();
    }

    private void RunMovementTutorial(bool fadeIn)
    {
        if (fadeIn)
            StartCoroutine(FadeIn(movementTutorial));
        else
            StartCoroutine(FadeOut(movementTutorial));
    }

    private void RunFishTutorial(bool fadeIn)
    {
        if (fadeIn)
        {
            StartCoroutine(FadeIn(fishTutorial));
            foreach(var fish in m_PlayerManager.fishOnMap)
            {
                currentArrows.Add(Instantiate(informativeArrowPrefab, fish.transform));
            }
        }
        else
        {
            StartCoroutine(FadeOut(fishTutorial));
            foreach (var arrow in currentArrows)
            {
                Destroy(arrow);
            }
            currentArrows.Clear();
        }
    }

    private void RunFishingTutorial(bool fadeIn)
    {
        if (fadeIn)
            StartCoroutine(FadeIn(fishingMinigameTutorial));
        else
            StartCoroutine(FadeOut(fishingMinigameTutorial));
    }

    private void RunOpenJournalTutorial(bool fadeIn)
    {
        if (fadeIn)
            StartCoroutine(FadeIn(openJournalTutorial));
        else
            StartCoroutine(FadeOut(openJournalTutorial));
    }

    private void RunGotoDockTutorial(bool fadeIn)
    {
        if (fadeIn)
            StartCoroutine(FadeIn(gotoDockTutorial));
        else
            StartCoroutine(FadeOut(gotoDockTutorial));
    }

    private IEnumerator FadeIn(CanvasGroup objectToFade)
    {
        yield return new WaitForSeconds(0.5f);
        while (objectToFade.alpha < 1)
        {
            if (instance == null)
                yield break;
            objectToFade.alpha += Time.deltaTime * 2f;
            yield return null;
        }
        yield break;
    }
    private IEnumerator FadeOut(CanvasGroup objectToFade)
    {
        while (objectToFade.alpha > 0)
        {
            if (instance == null)
                yield break;
            objectToFade.alpha -= Time.deltaTime * 2f;
            yield return null;
        }
        yield break;
    }

    public void RemoveTutorial()
    {
        movementTutorial.alpha = 0;
        fishTutorial.alpha = 0;
        fishingMinigameTutorial.alpha = 0;
        gotoDockTutorial.alpha = 0;
        dockTutorial.alpha = 0;
        openJournalTutorial.alpha = 0;
        journalTutorial.alpha = 0;
        foreach (var arrow in currentArrows)
        {
            Destroy(arrow);
        }
        currentArrows.Clear();
        instance = null;
        Destroy(this);
    }
}
