using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    static public ParticleManager instance;

    // Always play on boat
    [SerializeField] private ParticleSystem upgradePrefab;
    [SerializeField] private ParticleSystem fishCaughtPrefab;
    [SerializeField] private ParticleSystem coinsPrefab;

    // Play on something else
    [SerializeField] private ParticleSystem waterSplashPrefab;
    [SerializeField] private ParticleSystem fishSplashPrefab;

    public bool TestUpgrade = false;
    public bool TestFishCaught = false;
    public bool TestCoins = false;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Update()
    {
        if (TestUpgrade)
        {
            TestUpgrade = false;
            PlayUpgradeParticle(transform.position);
        }

        if (TestFishCaught)
        {
            TestFishCaught = false;
            PlayFishCaughtParticle(transform.position);
        }

        if (TestCoins)
        {
            TestCoins = false;
            PlayCoinsParticle(transform.position);
        }
    }

    public void PlayUpgradeParticle(Vector3 position)
    {
        ParticleSystem upgradePart = Instantiate(upgradePrefab, position, upgradePrefab.transform.rotation);
        Destroy(upgradePart.gameObject, upgradePart.main.duration);
    }
    public void PlayFishCaughtParticle(Vector3 position)
    {
        ParticleSystem fishCaughtPart = Instantiate(fishCaughtPrefab, position, fishCaughtPrefab.transform.rotation);
        Destroy(fishCaughtPart.gameObject, fishCaughtPart.main.duration);
    }
    public void PlayWaterSplashParticle(Vector3 position)
    {
        ParticleSystem waterSplashPart = Instantiate(waterSplashPrefab, position, waterSplashPrefab.transform.rotation);
        Destroy(waterSplashPart.gameObject, waterSplashPart.main.duration);
    }
    public void PlayCoinsParticle(Vector3 position)
    {
        ParticleSystem coinsPart = Instantiate(coinsPrefab, position, coinsPrefab.transform.rotation);
        Destroy(coinsPart.gameObject, coinsPart.main.duration);
    }
    public void PlayFishSplashParticle(Transform fish)
    {
        ParticleSystem fishSplashPart = Instantiate(fishSplashPrefab, fish);

    }
}
