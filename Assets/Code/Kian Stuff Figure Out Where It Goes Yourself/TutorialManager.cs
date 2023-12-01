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
    private bool dockTutorialCompleted = false;
    [SerializeField] private CanvasGroup dockTutorial;

    [Header("Journal Tutorial")]
    private bool journalTutorialCompleted = false;
    [SerializeField] private CanvasGroup journalTutorial;


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

    private void FixedUpdate()
    {
        if (!movementTutorialCompleted)
        {
            if(Vector3.Distance(playerStartingPosition, transform.position) > 50)
            {
                RunMovementTutorial(false);
                StartCoroutine(RunFishTutorial(true));
                movementTutorialCompleted = true;
            }
        }
        else if(!fishTutorialCompleted)
        {
            foreach(var fish in m_PlayerManager.fishOnMap)
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
        else if (!fishingMinigameTutorial)
        {
            if(m_PlayerManager.AmountOfFish > 0)
            {
                RunFishingTutorial(false);
                StartCoroutine(RunJournalTutorial(true));
                fishingMinigameTutorialCompleted = true;
            }
            else if(GetComponent<Fishing>().currentHarpoons == 0)
            {
                RunFishingTutorial(false);
                StartCoroutine(RunDockTutorial(true));
                fishingMinigameTutorialCompleted = true;
            }
        }
    }

    private void RunMovementTutorial(bool fadeIn)
    {
        if (fadeIn)
            StartCoroutine(FadeIn(movementTutorial));
        else
            StartCoroutine(FadeOut(movementTutorial));
    }

    private IEnumerator RunFishTutorial(bool fadeIn)
    {
        
        if (fadeIn)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(FadeIn(fishTutorial));
            foreach(var fish in m_PlayerManager.fishOnMap)
            {
                currentArrows.Add(Instantiate(informativeArrowPrefab, fish.transform));
            }
        }
        else
        {
            StartCoroutine(FadeOut(fishTutorial));
            foreach(var arrow in currentArrows)
            {
                Destroy(arrow);
                currentArrows.Remove(arrow);
            }
        }
            
        yield break;
    }

    private IEnumerator RunFishingTutorial(bool fadeIn)
    {
        if (fadeIn)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(FadeIn(fishingMinigameTutorial));
        }
        else
            StartCoroutine(FadeOut(fishingMinigameTutorial));

        yield break;
    }

    private IEnumerator RunJournalTutorial(bool fadeIn)
    {
        if (fadeIn)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(FadeIn(journalTutorial));
        }
        else
            StartCoroutine(FadeOut(journalTutorial));

        yield break;
    }

    private IEnumerator RunDockTutorial(bool fadeIn)
    {
        if (fadeIn)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(FadeIn(dockTutorial));
        }
        else
            StartCoroutine(FadeOut(journalTutorial));
    }
    private IEnumerator FadeIn(CanvasGroup objectToFade)
    {
        while(objectToFade.alpha < 1)
        {
            objectToFade.alpha += Time.deltaTime * 2f;
            yield return null;
        }
        yield break;
    }
    private IEnumerator FadeOut(CanvasGroup objectToFade)
    {
        while (objectToFade.alpha > 0)
        {
            objectToFade.alpha -= Time.deltaTime * 2f;
            yield return null;
        }
        yield break;
    }
}
