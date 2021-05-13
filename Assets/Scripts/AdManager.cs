using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    private InterstitialAd interstitial;
    private BannerView bannerView;

    [SerializeField] private Button removeAdButton;

    public static AdManager Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeOnLoad()
    {
        GameObject gameObject = new GameObject("AD Manager");
        Instance = gameObject.AddComponent<AdManager>();

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }

    public void Start()
    {
        if (PlayerPrefs.GetInt("RemoveAD", 0) == 0)
        {
            RequestBanner();
        }

        removeAdButton.gameObject.SetActive(PlayerPrefs.GetInt("RemoveAD", 0) == 0);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        this.bannerView?.Destroy();
    }

   
    public void ShowInterstitial()
    {
        if (PlayerPrefs.GetInt("RemoveAD", 0) == 0)
        {
            if (this.interstitial.IsLoaded())
            {
                this.interstitial.Show();
            }
        }
    }

    public void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-6474563713628422/8916939402";
#elif UNITY_IPHONE
        string adUnitId = "unexpected_platform";
#else
        string adUnitId = "unexpected_platform";
#endif

        this.interstitial = new InterstitialAd(adUnitId);

        AdRequest request = new AdRequest.Builder().AddTestDevice("7B12044D2265F6456FCECD97E6126E15").Build();

        this.interstitial.LoadAd(request);        
    }
    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-6474563713628422/5065038895";
#elif UNITY_IPHONE
        string adUnitId = "unexpected_platform";
#else
        string adUnitId = "unexpected_platform";
#endif

        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().AddTestDevice("7B12044D2265F6456FCECD97E6126E15").Build();

        this.bannerView.LoadAd(request);
    }

    public void RemoveAD()
    {
        PlayerPrefs.SetInt("RemoveAD", 1);
        removeAdButton.gameObject.SetActive(false);
    }

}