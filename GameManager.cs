using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{


    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }


    public Text heart;
    public Text heart_msg;
    const int full_time_minute = 15;
    const int full_heart = 10;
    string remain = full_time_minute.ToString() + ":00";
    float time = 0;


    private void Awake()
    {

        if (null == instance)
        {
            //이 클래스 인스턴스가 탄생했을 때 전역변수 instance에 게임매니저 인스턴스가 담겨있지 않다면, 자신을 넣어준다.
            instance = this;

            //씬 전환이 되더라도 파괴되지 않게 한다.
            //gameObject만으로도 이 스크립트가 컴포넌트로서 붙어있는 Hierarchy상의 게임오브젝트라는 뜻이지만, 
            //나는 헷갈림 방지를 위해 this를 붙여주기도 한다.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 GameMgr이 존재할 수도 있다.
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
            //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신(새로운 씬의 GameMgr)을 삭제해준다.
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        // 하트 개수 초기화
        if (PlayerPrefs.HasKey("heart") == false)
            PlayerPrefs.SetInt("heart", full_heart);

        if (PlayerPrefs.HasKey("remain_time") == false)
            PlayerPrefs.SetString("remain_time", remain);

        if (PlayerPrefs.HasKey("exit_time") == false)
            PlayerPrefs.SetString("exit_time", "");
        else
        {
            if (PlayerPrefs.GetInt("heart") < full_heart)
            {
                DateTime exit_time = DateTime.Parse(PlayerPrefs.GetString("exit_time"));
                TimeSpan gap = System.DateTime.Now - exit_time;

                Debug.Log("exit_time: " + exit_time);
                Debug.Log("gap: " + gap);

                if (gap.TotalMinutes < full_time_minute)
                {
                    string temp = PlayerPrefs.GetString("remain_time");
                    int remain_m = int.Parse(temp.Substring(0, 2));
                    int return_m = (int)gap.TotalMinutes;

                    Debug.Log("remain_time: " + temp);
                    Debug.Log("remain_m: " + remain_m);
                    Debug.Log("return_m: " + return_m);

                    if (remain_m > return_m)
                    {
                        if(10 > remain_m - return_m)
                            remain = "0" + (remain_m - return_m).ToString() + ":00";
                        else
                           remain = (remain_m - return_m).ToString() + ":00";

                    }
                    else
                    {
                        remain = full_time_minute.ToString() + ":00";
                        increase_heart(PlayerPrefs.GetInt("heart") + 1);
                    }
                        
                }
                else
                {
                    remain = full_time_minute.ToString() + ":00";
                    int add = PlayerPrefs.GetInt("heart") + (int)(gap.TotalMinutes / full_time_minute);
                    if (add > 10)
                        add = 10;
                    PlayerPrefs.SetInt("heart", add);

                }
            }

        }

        heart.text = PlayerPrefs.GetInt("heart").ToString();

        Debug.Log("remain: " + remain);

        // 틀린 문제 카운팅 초기화
        // 4자리로 이루어진 틀린 문제 넘버링. 읽을 때도 4자씩 끊어서 읽기.
        if (PlayerPrefs.HasKey("failed") == false)
            PlayerPrefs.SetString("failed", "");
        
        // 각 모드별로 어디까지 맞췄는지 초기화
        if (PlayerPrefs.HasKey("era") == false)
            PlayerPrefs.SetInt("era", 1);

        // 틀린 문제 카운팅
        if (PlayerPrefs.HasKey("fail_count") == false)
            PlayerPrefs.SetInt("fail_count", 0);

        // 광고 제거, 0이면 광고 있음 / 1이면 광고 제거
        if (PlayerPrefs.HasKey("remove_ad") == false)
            PlayerPrefs.SetInt("remove_ad", 0);

        // 그냥 해금 없이 할까? 그래 뭐 시대별만 해금 붙이고 나머진 해금 없이 가자
        if (PlayerPrefs.HasKey("easy") == false)
            PlayerPrefs.SetInt("easy", 0);

        if (PlayerPrefs.HasKey("mid") == false)
            PlayerPrefs.SetInt("mid", 0);

        if (PlayerPrefs.HasKey("hard") == false)
            PlayerPrefs.SetInt("hard", 0);

        if(PlayerPrefs.HasKey("gettingHeartCount") == false)
            PlayerPrefs.SetInt("gettingHeartCount", 5);

    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("remain_time", remain);
        PlayerPrefs.SetString("exit_time", System.DateTime.Now.ToString());
    }


    private void Update()
    {
        /*
        if(PlayerPrefs.GetInt("heart") >= full_heart)
        {
            heart_msg.gameObject.SetActive(false);

        }
        else
        {
            time += Time.deltaTime;
            if(time >= 1)
            {
                time = 0;
                Debug.Log("remain_real: " + remain);
                if(remain == "00:00")
                {
                    increase_heart(PlayerPrefs.GetInt("heart") + 1);
                    remain = full_time_minute.ToString() + ":00";
                }

                if (remain.Substring(3,2) == "00")
                {
                    int m = int.Parse(remain.Substring(0, 2)) - 1;
                    if (m < 10)
                        remain = "0" + m.ToString() + ":59";
                    else
                        remain = m.ToString() + ":59";
                }
                else
                {
                    int s = int.Parse(remain.Substring(3, 2)) - 1;
                    if (s < 10)
                        remain = remain.Substring(0, 2) + ":0" + s.ToString();
                    else
                        remain = remain.Substring(0, 2) + ":" + s.ToString();

                }

            }

            heart_msg.gameObject.SetActive(true);
            heart_msg.text = "복숭아 충전까지 " + remain;
        }
        */
    }

    public int increase_heart(int num)
    {
        PlayerPrefs.SetInt("heart", PlayerPrefs.GetInt("heart") + num);
        heart.text = PlayerPrefs.GetInt("heart").ToString();
        return PlayerPrefs.GetInt("heart");
    }

    public int decrease_heart(int num)
    {
        PlayerPrefs.SetInt("heart", PlayerPrefs.GetInt("heart") - num);
        heart.text = PlayerPrefs.GetInt("heart").ToString();
        return PlayerPrefs.GetInt("heart");
    }


}
