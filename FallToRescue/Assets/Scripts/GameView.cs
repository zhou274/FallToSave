using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TTSDK.UNBridgeLib.LitJson;
using TTSDK;
using StarkSDKSpace;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class GameView : MonoBehaviour
{
	[SerializeField]
	private GameObject Info;

	[SerializeField]
	private Text txtCurrentLevel;

	[SerializeField]
	private Text txtNextLevel;

	[SerializeField]
	private Text txtCoins;

	[SerializeField]
	private Text txtScore;

	[SerializeField]
	private Image imgProgress;

	[SerializeField]
	private Transform spawnPoint;

	[Header("----------------------------"), SerializeField]
	private GameObject over;

	[SerializeField]
	private Button btnVideoLife;

	[SerializeField]
	private Button btnVideoLifeStatic;

	[SerializeField]
	private Button btnReplay;

	[SerializeField]
	private Image imgTimer;

	[Header("----------------------------"), SerializeField]
	private GameObject Skin;

	[SerializeField]
	private Button btnSkin;

	[SerializeField]
	private TextMeshProUGUI txtBestScore;

	[SerializeField]
	private GameObject text;

	[Header("----------------------------"), SerializeField]
	private GameObject ExitTanChuang;

	[SerializeField]
	private Button btnExit;

	[SerializeField]
	private Button btnCloseExit;

	[SerializeField]
	private Button btnOpenExit;

	[Header("----------------------------"), SerializeField]
	private GameObject RestartTanChuang;

	[SerializeField]
	private Button btnRestart;

	[SerializeField]
	private Button btnCloseRestart;

	[SerializeField]
	private Button btnOpenRestart;

	[Header("----------------------------")]
	public GameObject m_TryOn;

	public GameObject m_Sign;

	private bool showOver;

	private float timer = 2f;

	[Header("是否在游戏中隐藏UI")]
	public bool hideUIInGame = true;

	private bool hidebtnSkin;

    public string clickid;
    private StarkAdManager starkAdManager;
    private void Start()
	{
		this.over.SetActive(false);
		this.Skin.SetActive(false);
		this.btnSkin.onClick.AddListener(new UnityAction(this.OnbtnSkinClick));
		this.btnVideoLife.onClick.AddListener(new UnityAction(this.OnbtnVideoLifeClick));
		this.btnVideoLifeStatic.onClick.AddListener(new UnityAction(this.OnbtnVideoLifeClick));
		this.btnReplay.onClick.AddListener(new UnityAction(this.OnbtnReplayClick));
		this.btnExit.onClick.AddListener(new UnityAction(this.OnbtnExitClick));
		//this.btnCloseExit.onClick.AddListener(new UnityAction(this.OnbtnCloseExit));
		this.btnOpenExit.onClick.AddListener(new UnityAction(this.OnbtnOpenExitClick));
		//this.btnRestart.onClick.AddListener(new UnityAction(this.OnbtnRestartClick));
		this.btnCloseRestart.onClick.AddListener(new UnityAction(this.OnbtnCloseRestart));
		this.btnOpenRestart.onClick.AddListener(new UnityAction(this.OnbtnOpenRestartClick));
		this.btnOpenRestart.gameObject.SetActive(false);
		TouchRotate.Instance.fristAction = new Action(this.HideBtnSkin);
		this.txtBestScore.text = "最高分: " + PlayerManager.Instance.HighScore;
		if (Global.canShowSign)
		{
			this.m_Sign.gameObject.SetActive(true);
		}
	}

	private void OnbtnOpenRestartClick()
	{
		this.RestartTanChuang.gameObject.SetActive(true);
		Time.timeScale = 0f;
	}

	private void OnbtnOpenExitClick()
	{
		this.ExitTanChuang.gameObject.SetActive(true);
		Time.timeScale = 0f;
	}

	private void OnbtnExitClick()
	{
        Global.score = 0;
        SceneManager.LoadScene("Level");
        Time.timeScale = 1f;
    }

    public void OnbtnRestartClick()
	{
		Global.score = 0;
		SceneManager.LoadScene("Level");
		Time.timeScale = 1f;
	}

	private void OnbtnCloseRestart()
	{
		this.RestartTanChuang.gameObject.SetActive(false);
		Time.timeScale = 1f;
	}

	public void OnbtnCloseExit()
	{
		this.ExitTanChuang.gameObject.SetActive(false);
		Time.timeScale = 1f;
	}

	private void OnbtnSkinClick()
	{
        ShowVideoAd("192if3b93qo6991ed0",
            (bol) => {
                if (bol)
                {

                    this.Skin.GetComponent<SkinView>().Show();


                    clickid = "";
                    getClickid();
                    apiSend("game_addiction", clickid);
                    apiSend("lt_roi", clickid);


                }
                else
                {
                    StarkSDKSpace.AndroidUIManager.ShowToast("观看完整视频才能获取奖励哦！");
                }
            },
            (it, str) => {
                Debug.LogError("Error->" + str);
                //AndroidUIManager.ShowToast("广告加载异常，请重新看广告！");
            });
        
	}

	public void OnbtnVideoLifeClick()
	{
		//Ads.Instance.WatchVideo(new Action(GamePlay.Instance.GetLife));
		GamePlay.Instance.GetLife();

    }

	private void OnbtnReplayClick()
	{
		Global.canShowSign = true;
		Global.score = 0;
		CarManager.Instance.tryOnCar = null;
		SceneManager.LoadScene("Level");
		EventRecord.Instance.EventSet("replay", (PlayerManager.Instance.CurrentLevel + 1).ToString());
	}

	public void Init(int coins, int score, int currentLevel, int nextLevel, bool show = true)
	{
		this.txtCoins.text = coins.ToString();
		this.txtScore.text = score.ToString();
		this.imgProgress.fillAmount = 0f;
		this.txtCurrentLevel.text = (currentLevel + 1).ToString();
		this.txtNextLevel.text = (nextLevel + 1).ToString();
		this.Info.SetActive(show);
	}

	public void SetProgress(float f)
	{
		this.imgProgress.fillAmount = 1f - f;
	}

	public void SetTest(int ceng, int total)
	{
	}

	public void UpdateCoins(int coinsNum)
	{
		this.txtCoins.text = coinsNum.ToString();
	}

	public void UpdateScore(int score)
	{
		this.txtScore.text = score.ToString();
		this.txtScore.transform.DOScale(1.2f, 0.2f).OnComplete(delegate
		{
			this.txtScore.transform.DOScale(1f, 0.2f);
		});
	}

	public void ShowMessage(string str)
	{
		if (this.hideUIInGame)
		{
			return;
		}
		GameObject @object = PoolManager.Instance.GetObject("Message");
		@object.transform.SetParent(base.transform);
		@object.transform.localScale = Vector3.one;
		@object.SetActive(true);
		HintAni component = @object.GetComponent<HintAni>();
		component.Init(str);
	}

	public void ShowScore()
	{
		if (this.hideUIInGame)
		{
			return;
		}
		GameObject @object = PoolManager.Instance.GetObject("Score");
		@object.transform.SetParent(base.transform);
		@object.transform.position = this.spawnPoint.position;
		ScoreAni component = @object.GetComponent<ScoreAni>();
		component.Init(this.txtScore.transform);
	}

	public void ShowOver()
	{
		this.timer = 6f;
		this.showOver = true;
		this.over.SetActive(true);
		this.imgTimer.fillAmount = 1f;
		int iD;
		if (CarManager.Instance.tryOnCar != null)
		{
			iD = CarManager.Instance.tryOnCar.ID;
		}
		else
		{
			iD = CarManager.Instance.currentCar.ID;
		}
		for (int i = 0; i < CarManager.Instance.cars.Count; i++)
		{
			this.over.transform.Find("Move/Ball" + i).gameObject.SetActive(i == iD);
		}
		//this.btnVideoLife.gameObject.SetActive(true);
		this.btnReplay.gameObject.SetActive(false);
        ShowInterstitialAd("1lcaf5895d5l1293dc",
            () => {
                Debug.LogError("--插屏广告完成--");

            },
            (it, str) => {
                Debug.LogError("Error->" + str);
            });
    }
    /// <summary>
    /// 播放插屏广告
    /// </summary>
    /// <param name="adId"></param>
    /// <param name="errorCallBack"></param>
    /// <param name="closeCallBack"></param>
    public void ShowInterstitialAd(string adId, System.Action closeCallBack, System.Action<int, string> errorCallBack)
    {
        starkAdManager = StarkSDK.API.GetStarkAdManager();
        if (starkAdManager != null)
        {
            var mInterstitialAd = starkAdManager.CreateInterstitialAd(adId, errorCallBack, closeCallBack);
            mInterstitialAd.Load();
            mInterstitialAd.Show();
        }
    }
    public void HideOver()
	{
		this.over.SetActive(false);
	}

	private void Update()
	{
		if (this.showOver)
		{
			this.timer -= Time.deltaTime;
			this.imgTimer.fillAmount = this.timer / 6f;
			if (this.timer <= 0f)
			{
				this.showOver = false;
				this.btnVideoLife.gameObject.SetActive(false);
			}
			else if ((double)this.timer <= 4.5)
			{
				this.btnReplay.gameObject.SetActive(true);
			}
		}
	}

	public void ShowSkin()
	{
		this.Skin.SetActive(true);
	}

	public void HideBtnSkin()
	{
		if (this.hidebtnSkin)
		{
			return;
		}
		if (PlayerManager.Instance.OpenApplicationNum != 1)
		{
			this.m_TryOn.gameObject.SetActive(true);
		}
		this.hidebtnSkin = true;
		Vector3 localPosition = this.btnSkin.transform.localPosition;
		localPosition.x += 300f;
		this.btnSkin.transform.DOLocalMove(localPosition, 0.3f, false);
		localPosition = this.txtBestScore.transform.localPosition;
		localPosition.x -= 1080f;
		this.txtBestScore.transform.DOLocalMove(localPosition, 0.5f, false);
		this.btnOpenRestart.gameObject.SetActive(true);
		UnityEngine.Object.Destroy(this.text);
		GamePlay.Instance.StartGame();
		if (this.hideUIInGame)
		{
			this.btnOpenRestart.gameObject.SetActive(false);
			this.Info.gameObject.SetActive(false);
			this.txtCoins.gameObject.SetActive(false);
			this.txtScore.gameObject.SetActive(false);
		}
	}
    public void getClickid()
    {
        var launchOpt = StarkSDK.API.GetLaunchOptionsSync();
        if (launchOpt.Query != null)
        {
            foreach (KeyValuePair<string, string> kv in launchOpt.Query)
                if (kv.Value != null)
                {
                    Debug.Log(kv.Key + "<-参数-> " + kv.Value);
                    if (kv.Key.ToString() == "clickid")
                    {
                        clickid = kv.Value.ToString();
                    }
                }
                else
                {
                    Debug.Log(kv.Key + "<-参数-> " + "null ");
                }
        }
    }

    public void apiSend(string eventname, string clickid)
    {
        TTRequest.InnerOptions options = new TTRequest.InnerOptions();
        options.Header["content-type"] = "application/json";
        options.Method = "POST";

        JsonData data1 = new JsonData();

        data1["event_type"] = eventname;
        data1["context"] = new JsonData();
        data1["context"]["ad"] = new JsonData();
        data1["context"]["ad"]["callback"] = clickid;

        Debug.Log("<-data1-> " + data1.ToJson());

        options.Data = data1.ToJson();

        TT.Request("https://analytics.oceanengine.com/api/v2/conversion", options,
           response => { Debug.Log(response); },
           response => { Debug.Log(response); });
    }


    /// <summary>
    /// </summary>
    /// <param name="adId"></param>
    /// <param name="closeCallBack"></param>
    /// <param name="errorCallBack"></param>
    public void ShowVideoAd(string adId, System.Action<bool> closeCallBack, System.Action<int, string> errorCallBack)
    {
        starkAdManager = StarkSDK.API.GetStarkAdManager();
        if (starkAdManager != null)
        {
            starkAdManager.ShowVideoAdWithId(adId, closeCallBack, errorCallBack);
        }
    }
}
