using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
//using GoogleMobileAds;


public class AdManager : MonoBehaviour
{
    private BannerView bannerView;
    [HideInInspector] public InterstitialAd interstitialAd;
    //[SerializeField] private string adUnitIdBanner = "ca-app-pub-1754302936861474~7705229110";
    //[SerializeField] private string adUnitIdInterstitial = "ca-app-pub-3940256099942544/8691691433";
    public AdPosition _pos;
    public int sizeWidth;
    public int sizeHeight;

    [HideInInspector]public bool interstitialClosed;
    
    // Start is called before the first frame update
    void Start()
    {
        RequestConfiguration reklamKonfigurasyonu = new RequestConfiguration.Builder()
    .SetTestDeviceIds(new System.Collections.Generic.List<string>() { "1d3ec0a0-422d-46ea-a47d-767c9e328fe4" })
    .build();
        MobileAds.SetRequestConfiguration(reklamKonfigurasyonu);

        MobileAds.Initialize(initStatus => { });        
        //this.requestInterstitial();
    }

    // Update is called once per frame
    void Update()
    {
        if (interstitialClosed)
        {
            // Perform actions here.
            
        }
    }
    public void requestBanner()
    {
#if UNITY_ANDROID
        string adId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
        string adId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adId = "unexpected_platform";
#endif
        if (bannerView != null)
            bannerView.Destroy();
        AdSize adsize = new AdSize(sizeWidth, sizeHeight);
        this.bannerView = new BannerView(adId, AdSize.SmartBanner, _pos);
        //this.bannerView = new BannerView(adId, adsize, AdPosition.Top);
        AdRequest request = new AdRequest.Builder().Build();
        this.bannerView.LoadAd(request);
    }
    public void OnDestroy()
    {
        print("s");
        if (bannerView != null)
            bannerView.Destroy();
        if (interstitialAd != null)
            interstitialAd.Destroy();
    }
    public void requestInterstitial()
    {
#if UNITY_ANDROID
        string adId = "ca-app-pub-3940256099942544/8691691433";
#elif UNITY_IPHONE
        string adId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adId = "unexpected_platform";
#endif
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }            
        //AdSize adsize = new AdSize(_sizeWidth, _sizeHeight);
        //this.bannerView = new BannerView(adId, adsize, _pos);
        this.interstitialAd = new InterstitialAd (adId);
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitialAd.LoadAd(request);
        StartCoroutine(ShowAd());
        interstitialAd.OnAdClosed += HandleInterstitialClosed;
        interstitialClosed = false;
    }
    IEnumerator ShowAd()
    {
        while (!interstitialAd.IsLoaded())
            yield return null;
        interstitialAd.Show();
        
        
    }
    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        interstitialClosed = true;
    }
    
}
