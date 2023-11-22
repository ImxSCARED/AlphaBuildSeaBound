using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalTutorialUI : MonoBehaviour
{

    [SerializeField] private CanvasGroup journalTutorial;

    private bool fadeIn = false;
    private bool fadeOut = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            fadeIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            fadeOut = true;
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }


    private void Update()
    {
        if (fadeIn)
        {
            journalTutorial.alpha += Time.deltaTime;
            if (journalTutorial.alpha >= 1)
            {
                fadeIn = false;
            }
        }

        if (fadeOut)
        {
            journalTutorial.alpha -= Time.deltaTime;
            if (journalTutorial.alpha <= 0)
            {
                fadeOut = false;
            }
        }
    }
}
