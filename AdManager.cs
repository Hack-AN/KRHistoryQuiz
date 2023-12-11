using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    public bool istest;

    private BannerView bannerView;
    private InterstitialAd interstitial;



    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("remove_ad") == 0)
        {
            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(initStatus => { });

            this.RequestBanner();
        }



    }


    private void RequestBanner()
    {
        string adUnitId = "";
        if (istest == true)
        {
#if UNITY_ANDROID
            adUnitId = "ca-app-pub-3940256099942544/6300978111";
#else
            adUnitId = "ca-app-pub-3940256099942544/6300978111";
#endif
        }
        else
        {
#if UNITY_ANDROID
            adUnitId = "ca-app-pub-1284980277767839/2889249922";
#else
            adUnitId = "ca-app-pub-1284980277767839/2889249922";
#endif
        }



        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }


    public void RequestInterstitial()
    {
        string adUnitId = "";

        if (istest == true)
        {
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/1033173712";
#else
        adUnitId = "ca-app-pub-3940256099942544/1033173712";
#endif
        }
        else
        {
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-1284980277767839/3751454197";
#else
        adUnitId = "ca-app-pub-1284980277767839/3751454197";
#endif
        }


        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }


    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
        this.interstitial.Show();
    }

    public void remove_ad()
    {
        PlayerPrefs.SetInt("remove_ad", 1);
        bannerView.Destroy();
        interstitial.Destroy();
    }
}
