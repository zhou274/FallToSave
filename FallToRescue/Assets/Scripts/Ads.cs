using System;
using UnityEngine;

public class Ads : MonoBehaviour
{
	public static Ads Instance;

	public static string uniqueUserId = "demoUserUnity";

	public static string appKey = "97715215";

	public int replayInterstitialRecord = -1;

	private void Awake()
	{
		if (Ads.Instance == null)
		{
            Ads.Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
        /*
		UnityEngine.Debug.Log("unity-script: MyAppStart Start called");
	IronSourceConfig.Instance.setClientSideCallbacks(true);
	string advertiserId = IronSource.Agent.getAdvertiserId();
	UnityEngine.Debug.Log("unity-script: IronSource.Agent.getAdvertiserId : " + advertiserId);
		UnityEngine.Debug.Log("unity-script: IronSource.Agent.validateIntegration");
	IronSource.Agent.validateIntegration();
		UnityEngine.Debug.Log("unity-script: unity version" + IronSource.unityVersion());
		UnityEngine.Debug.Log("unity-script: IronSource.Agent.init");
		IronSource.Agent.setUserId("uniqueUserId");
		IronSource.Agent.init(AdsControl.appKey);
		*/      
		this.OpenBanner();
        PlayerPrefs.DeleteAll();
	}

	private void OpenBanner()
	{
        //base.gameObject.GetComponent<AdBannerScript>().ShowBanner();
        AdsControl.Instance.ShowBanner();
	}

	public bool HasVideo()
	{
		//return base.gameObject.GetComponent<RewardedVideoControl>().HasRewardedVideo();
        return AdsControl.Instance.GetRewardAvailable();
	}

	public void WatchVideo(Action action)
	{
        //base.gameObject.GetComponent<RewardedVideoControl>().ShowRewardedVideo(action);
       
       
        if (AdsControl.Instance.GetRewardAvailable())
        {
            AdsControl.Instance.PlayDelegateRewardVideo(delegate
            {
                FindObjectOfType<GamePlay>().GetLife();
            });
           
        }

    }

    public void ShowInterstitial()
	{
        //base.gameObject.GetComponent<InterstitialControl>().ShowInterstitial();
        AdsControl.Instance.showAds();
	}

	private void OnApplicationPause(bool isPaused)
	{
		UnityEngine.Debug.Log("unity-script: OnApplicationPause = " + isPaused);
		//IronSource.Agent.onApplicationPause(isPaused);
	}

	private void OnInitComplete()
	{
		//UnityEngine.Debug.Log("FaceBookOnInitComplete");
	}

	private void OnHideUnity(bool isGameShown)
	{
		//UnityEngine.Debug.Log("FaceBookOnHideUnity");
	}
}
