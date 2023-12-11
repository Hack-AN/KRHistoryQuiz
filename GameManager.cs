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
            //�� Ŭ���� �ν��Ͻ��� ź������ �� �������� instance�� ���ӸŴ��� �ν��Ͻ��� ������� �ʴٸ�, �ڽ��� �־��ش�.
            instance = this;

            //�� ��ȯ�� �Ǵ��� �ı����� �ʰ� �Ѵ�.
            //gameObject�����ε� �� ��ũ��Ʈ�� ������Ʈ�μ� �پ��ִ� Hierarchy���� ���ӿ�����Ʈ��� ��������, 
            //���� �򰥸� ������ ���� this�� �ٿ��ֱ⵵ �Ѵ�.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //���� �� �̵��� �Ǿ��µ� �� ������ Hierarchy�� GameMgr�� ������ ���� �ִ�.
            //�׷� ��쿣 ���� ������ ����ϴ� �ν��Ͻ��� ��� ������ִ� ��찡 ���� �� ����.
            //�׷��� �̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ�(���ο� ���� GameMgr)�� �������ش�.
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        // ��Ʈ ���� �ʱ�ȭ
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

        // Ʋ�� ���� ī���� �ʱ�ȭ
        // 4�ڸ��� �̷���� Ʋ�� ���� �ѹ���. ���� ���� 4�ھ� ��� �б�.
        if (PlayerPrefs.HasKey("failed") == false)
            PlayerPrefs.SetString("failed", "");
        
        // �� ��庰�� ������ ������� �ʱ�ȭ
        if (PlayerPrefs.HasKey("era") == false)
            PlayerPrefs.SetInt("era", 1);

        // Ʋ�� ���� ī����
        if (PlayerPrefs.HasKey("fail_count") == false)
            PlayerPrefs.SetInt("fail_count", 0);

        // ���� ����, 0�̸� ���� ���� / 1�̸� ���� ����
        if (PlayerPrefs.HasKey("remove_ad") == false)
            PlayerPrefs.SetInt("remove_ad", 0);

        // �׳� �ر� ���� �ұ�? �׷� �� �ô뺰�� �ر� ���̰� ������ �ر� ���� ����
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
            heart_msg.text = "������ �������� " + remain;
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
