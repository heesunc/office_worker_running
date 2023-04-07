using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;

public class AdmobRewardAd : MonoBehaviour
{
    private RewardedAd rewardedAd;
    string adUnitId;

    void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            //초기화 완료
        });



#if UNITY_ANDROID
        // 실제 office_Worker_running 코드
        //adUnitId = "ca-app-pub-7725655200713394~3403951947";

        // 구글 예제 코드
        adUnitId = "ca-app-pub-3940256099942544/5224354917";

#elif UNITY_IOS
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

        LoadRewardedAd();
    }

    public void LoadRewardedAd() //광고 로드 하기
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // 테스트 기기 아이디
        List<string> testDeviceIds = new List<string>() { "AB4677508C285C44E01E5FC2B460F70E" };
        RequestConfiguration configuration =
            new RequestConfiguration.Builder().SetTestDeviceIds(testDeviceIds).build();
        MobileAds.SetRequestConfiguration(configuration);

        // create our request used to load the ad.
        var adRequest = new AdRequest.Builder().Build();

        // send the request to load the ad.
        RewardedAd.Load(adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());
                rewardedAd = ad;
            });
    }

    public void ShowAd(Action rewardCallback) //광고 보기, 
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                //보상 획득하기
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));

                Debug.Log("광고 확인");
                Time.timeScale = 0.001f;
                rewardCallback();
            });
            
        }
    }

    public void RegisterReloadHandler(RewardedAd ad) //광고 재로드
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += (null);
        {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
    }
}