using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PlayingManager : MonoBehaviour
{
    QuestionDB.Question[] qtns;
    public GameObject[] fails;
    List<QuestionDB.Question> fails_q = new List<QuestionDB.Question>();

    public Text uptext;
    public GameObject Qchoice;
    public GameObject Qox;
    public GameObject Qtext;

    public GameObject menumanager;
    public GameObject discorrect;

    QuestionDB.Question current_qtn;

    public GameObject correct_btn;
    public GameObject incorrect_panel;

    public AudioSource correct;
    public AudioSource fail;
    public AudioSource touch;

    bool isplaying = false;

    GameObject board;

    public GameObject era;
    public GameObject easy;
    public GameObject mid;
    public GameObject hard;

    float polar = 4000f;

    public Sprite choice;
    public Sprite o;
    public Sprite x;
    public Sprite pressed_choice;
    public Sprite pressed_o;
    public Sprite pressed_x;

    public GameObject lobby;
    public GameObject era_obj;
    public GameObject theme_obj;
    public GameObject easy_obj;
    public GameObject mid_obj;
    public GameObject hard_obj;


    public GameObject hearts;

    public GameObject shop;

    public GameObject admanager;


    int load_index;

    public Text heartGettingText;


    private void Start()
    {
        Debug.Log(PlayerPrefs.GetString("failed"));

        int len =  QuestionDB.Instance.len;
        qtns = new QuestionDB.Question[len];
        for (int i = 0; i < len; i++)
            qtns[i] = QuestionDB.Instance.qtns[i];
        for (int i = 0; i < 50; i++)
            fails[i] = discorrect.transform.GetChild(0).transform.GetChild(0).transform.GetChild(i).gameObject;
    }

    private void Update()
    {
        if(!(Qchoice.activeSelf == true || Qox.activeSelf == true || Qtext.activeSelf == true))
        {
            correct_btn.SetActive(false);
            incorrect_panel.SetActive(false);
        }


        if(isplaying)
        {
            switch (current_qtn.type)
            {
                case 0:
                    if (Qchoice.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount == 0)
                    {
                        Qchoice.transform.GetChild(1).gameObject.SetActive(false);
                        Qchoice.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
                        fail.Play();
                        for (int i = 0; i < 4; i++)
                            Qchoice.transform.GetChild(i + 2).GetComponent<Button>().enabled = false;
                        Qchoice.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "시간 초과...";
                        StartCoroutine(Shake(Qchoice, 100f, 1f));
                    }
                    break;
                case 1:
                    if (Qox.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount == 0)
                    {
                        Qox.transform.GetChild(1).gameObject.SetActive(false);
                        Qox.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
                        fail.Play();
                        for (int i = 0; i < 2; i++)
                            Qox.transform.GetChild(i + 2).GetComponent<Button>().enabled = false;
                        Qox.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "시간 초과...";
                        StartCoroutine(Shake(Qox, 100f, 1f));
                    }
                    break;
                case 2:
                    if (Qtext.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount == 0)
                    {
                        Qtext.transform.GetChild(1).gameObject.SetActive(false);
                        Qtext.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
                        for (int i = 0; i < 6; i++)
                            Qtext.transform.GetChild(2).transform.GetChild(i).transform.GetChild(0).GetComponent<Button>().enabled = false;
                        for (int i = 0; i < 10; i++)
                            Qtext.transform.GetChild(3).transform.GetChild(i).GetComponent<Button>().enabled = false;
                        fail.Play();
                        Qtext.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "시간 초과...";
                        StartCoroutine(Shake(Qtext, 100f, 1f));
                    }
                    break;
            }
        }


    }

    void load_question(QuestionDB.Question qtn)
    {

        if (PlayerPrefs.GetInt("heart") <= 0)
            return;


        current_qtn = qtn;
        string title = "";

        Qchoice.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
        Qox.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
        Qtext.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 1;

        switch (qtn.in_div)
        {
            case 0:
                title += "선사";
                break;
            case 1:
                title += "고조선";
                break;
            case 2:
                title += "삼국";
                break;
            case 3:
                title += "남북극";
                break;
            case 4:
                title += "고려";
                break;
            case 5:
                title += "조선";
                break;
            case 6:
                title += "근대";
                break;
            case 7:
                title += "현대";
                break;
            case 8:
                title += "삼일절";
                break;
            case 9:
                title += "6.25전쟁";
                break;
            case 10:
                title += "독립운동";
                break;
            case 11:
                title += "한국예술";
                break;
            case 12:
                title += "스포츠";
                break;

        }

        title += " ";

        switch (qtn.in_div)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
                title += qtn.index.ToString() + "-" + qtn.in_index;
                break;
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
                title += qtn.index.ToString();
                break;
        }

        uptext.text = title;

        Qchoice.SetActive(false);
        Qox.SetActive(false);
        Qtext.SetActive(false);

        float load_time = 0.5f;
        Ease motion = Ease.OutBack;

        float timelimit = 20f;

        isplaying = true;
        switch (qtn.type)
        {
            case 0:
                board = Qchoice;
                for (int i = 0; i < 6; i++)
                {
                    Qchoice.transform.GetChild(i).gameObject.SetActive(true);
                }

                Qchoice.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = qtn.question_desc;
                for (int i = 0; i < 4; i++)
                {
                    Qchoice.transform.GetChild(i + 2).transform.GetChild(0).GetComponent<Text>().text = qtn.choices[i];
                    Qchoice.transform.GetChild(i + 2).GetComponent<Image>().DOColor(Color.white, 0);
                    Qchoice.transform.GetChild(i + 2).GetComponent<Image>().sprite = choice;
                    Qchoice.transform.GetChild(i + 2).GetComponent<Button>().enabled = true;
                    Qchoice.transform.GetChild(i + 2).transform.GetChild(0).GetComponent<Text>().DOColor(Color.white, 0);
                }

                Qchoice.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "문제";
                Qchoice.SetActive(true);
                Qchoice.transform.DOLocalMoveX(polar, 0);
                Qchoice.transform.DOLocalMoveX(0, load_time).SetEase(motion);

                Qchoice.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().DOFillAmount(1, 0);
                Qchoice.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().DOFillAmount(0, timelimit);
                

                break;
            case 1:
                board = Qox;
                for (int i = 0; i < 4; i++)
                {
                    Qox.transform.GetChild(i).gameObject.SetActive(true);
                }
                for (int i = 0; i < 2; i++)
                {
                    Qox.transform.GetChild(i + 2).GetComponent<Image>().DOColor(Color.white, 0);
                    Qox.transform.GetChild(i + 2).GetComponent<Button>().enabled = true;
                }

                Qox.transform.GetChild(2).GetComponent<Image>().sprite = o;
                Qox.transform.GetChild(3).GetComponent<Image>().sprite = x;

                Qox.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "문제";
                Qox.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = qtn.question_desc;
                Qox.SetActive(true);
                Qox.transform.DOLocalMoveX(polar, 0);
                Qox.transform.DOLocalMoveX(0, load_time).SetEase(motion);
                Qox.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().DOFillAmount(1, 0);
                Qox.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().DOFillAmount(0, timelimit);

                break;
            case 2:
                board = Qtext;
                for (int i = 0; i < 4; i++)
                {
                    Qtext.transform.GetChild(i).gameObject.SetActive(true);
                }

                for(int i = 0; i < 6; i++)
                {
                    Qtext.transform.GetChild(2).transform.GetChild(i).gameObject.SetActive(false);
                    Qtext.transform.GetChild(2).transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
                    Qtext.transform.GetChild(2).transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,1);
                }
                    

                for (int i = 0; i < current_qtn.answer.Length; i++)
                {
                    Qtext.transform.GetChild(2).transform.GetChild(i).transform.GetChild(0).GetComponent<Button>().enabled = true;
                    
                }
                for (int i = 0; i < 10; i++)
                {
                    Qtext.transform.GetChild(3).gameObject.transform.GetChild(i).GetComponent<Button>().enabled = true;
                }

                Qtext.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "문제";
                Qtext.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = qtn.question_desc;
                for (int i = 0; i < qtn.answer.Length; i++)
                    Qtext.transform.GetChild(2).transform.GetChild(i).gameObject.SetActive(true);
                for (int i = 0; i < 10; i++)
                    Qtext.transform.GetChild(3).transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = qtn.choices[i];
                Qtext.SetActive(true);
                Qtext.transform.DOLocalMoveX(polar, 0);
                Qtext.transform.DOLocalMoveX(0, load_time).SetEase(motion);
                Qtext.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().DOFillAmount(1, 0);
                Qtext.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().DOFillAmount(0, 30);

                break;
        }



    }

    public void stop_timer()
    {
        isplaying = false;
        Qchoice.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().DOFillAmount(0, 0).Kill();
        Qox.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().DOFillAmount(0, 0).Kill();
        Qtext.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().DOFillAmount(0, 0).Kill();
    }

    public void speed_mode()
    {
        if (PlayerPrefs.GetInt("heart") <= 0)
        {
            shop.SetActive(true);
            return;

        }


        int len = qtns.Length;
        int ran = Random.Range(0,len);
        load_question(qtns[ran]);
        menumanager.GetComponent<MenuManager>().write_beforestage(lobby);

    }

    public void open_era()
    {
        for(int i = 1; i < PlayerPrefs.GetInt("era"); i++)
        {
            era.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            era.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
        }
        
    }

    public void open_easy()
    {
        /*
        for (int i = 1; i < PlayerPrefs.GetInt("easy"); i++)
        {
            easy.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            easy.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
        }
        */
    }

    public void open_mid()
    {
        /*
        for (int i = 1; i < PlayerPrefs.GetInt("mid"); i++)
        {
            mid.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            mid.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
        }
        */
    }

    public void open_hard()
    {
        /*
        for (int i = 1; i < PlayerPrefs.GetInt("hard"); i++)
        {
            hard.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            hard.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
        }
        */
    }

    // index 처음 값은 1이다. in_index의 처음 값이 1이기 때문.
    public void era_mode(int index)
    {

        if (PlayerPrefs.GetInt("heart") <= 0)
        {
            shop.SetActive(true);
            return;

        }

        load_index = index;

        // era playerpref와 비교하고 index가 그보다 작으면 오픈, 아니면 오픈 못 함
        // DB가 순서대로 문제가 저장되어있다는 전제 하에 짜여짐
        if (index <= PlayerPrefs.GetInt("era"))
        {
            int counter = 0;

            for (int i = 0; i < qtns.Length; i++)
            {
                if (qtns[i].div == 0 && qtns[i].in_index == 1)
                {
                    counter++;

                    if (counter == index)
                    {
                        load_question(qtns[i]);
                        menumanager.GetComponent<MenuManager>().write_beforestage(era_obj);
                        return;
                    }

                }
            }
        }

    }


    public void theme_mode(int index)
    {

        if (PlayerPrefs.GetInt("heart") <= 0)
        {
            shop.SetActive(true);
            return;

        }

        for (int i = 0; i < qtns.Length; i++)
        {
            if (qtns[i].div == 1 && qtns[i].in_div - 7 == index && qtns[i].index == 1)
            {
                load_question(qtns[i]);
                menumanager.GetComponent<MenuManager>().write_beforestage(theme_obj);
                return;
            }
        }
        
    }


    public void easy_mode(int index)
    {

        if (PlayerPrefs.GetInt("heart") <= 0)
        {
            shop.SetActive(true);
            return;

        }

        //if (index <= PlayerPrefs.GetInt("easy"))
        {
            int counter = 0;

            for (int i = 0; i < qtns.Length; i++)
            {
                if(qtns[i].level == 0)
                {
                    counter++;
                    if (counter == (index -1) * 10 + 1)
                    {
                        load_question(qtns[i]);
                        menumanager.GetComponent<MenuManager>().write_beforestage(easy_obj);
                        return;
                    }
                }
            }
        }
    }

    public void mid_mode(int index)
    {

        if (PlayerPrefs.GetInt("heart") <= 0)
        {
            shop.SetActive(true);
            return;

        }

        //if (index <= PlayerPrefs.GetInt("mid"))
        {
            int counter = 0;

            for (int i = 0; i < qtns.Length; i++)
            {
                if (qtns[i].level == 1)
                {
                    counter++;
                    if (counter == (index - 1) * 10 + 1)
                    {
                        load_question(qtns[i]);
                        menumanager.GetComponent<MenuManager>().write_beforestage(mid_obj);
                        return;
                    }
                }
            }
        }
    }


    public void hard_mode(int index)
    {

        if (PlayerPrefs.GetInt("heart") <= 0)
        {
            shop.SetActive(true);
            return;

        }

        //if (index <= PlayerPrefs.GetInt("hard"))
        {
            int counter = 0;

            for (int i = 0; i < qtns.Length; i++)
            {
                if (qtns[i].level == 2)
                {
                    counter++;
                    if (counter == (index - 1) * 10 + 1)
                    {
                        load_question(qtns[i]);
                        menumanager.GetComponent<MenuManager>().write_beforestage(hard_obj);
                        return;
                    }
                }
            }
        }
    }

    public void next_step()
    {
        // 현재 스테이지에서 다음 문제가 있으면 다음 문제 로드
        // 현재 스테이지에서 마지막 문제를 풀었다면 다음 스테이지로드. 그리고 playerpref 수정.
    }

    public void press_btn(int index)
    {
        switch(current_qtn.type)
        {
            case 0:
                board = Qchoice;
                break;
            case 1:
                board = Qox;
                break;
        }

        board.transform.GetChild(1).gameObject.SetActive(false);
        isplaying = false;
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        if (index.ToString() == current_qtn.answer)
        {
            switch (current_qtn.type)
            {
                case 0:
                    for (int i = 0; i < 4; i++)
                    {
                        Qchoice.transform.GetChild(i + 2).GetComponent<Button>().enabled = false;
                    }
                    Qchoice.transform.GetChild(2 + index).GetComponent<Image>().sprite = pressed_choice;
                    Qchoice.transform.GetChild(2 + index).transform.GetChild(0).GetComponent<Text>().DOColor(new Color(1, 0.5254902f, 0.4901961f, 1), 0);
                    break;
                case 1:
                    for (int i = 0; i < 2; i++)
                    {
                        Qox.transform.GetChild(i + 2).GetComponent<Button>().enabled = false;
                        
                    }
                    
                    if(index == 0)
                        Qox.transform.GetChild(2).GetComponent<Image>().sprite = pressed_o;
                    else if(index == 1)
                        Qox.transform.GetChild(3).GetComponent<Image>().sprite = pressed_x;
                    
                    
                    break;
            }
            correct.Play();
            stop_timer();
            //obj.GetComponent<Image>().DOColor(new Color(0.8537736f, 1, 0.8537736f),0.3f);
            board.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "정답!";
            heart_up();
            board.transform.GetChild(0).transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 1).SetEase(Ease.InOutElastic).OnComplete(() =>
            {
                board.transform.GetChild(0).transform.DOScale(new Vector3(1f, 1f, 1f), 1f).SetEase(Ease.OutQuad).OnComplete(() => {
                    board.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "해설";
                    board.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "";
                    board.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().color = new Color(1, 0.5254902f, 0.4901961f, 0);
                    board.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = current_qtn.explain;
                    board.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().DOFade(1, 1);
                    switch (current_qtn.type)
                    {
                        case 0:
                            for (int i = 0; i < 4; i++)
                                board.transform.GetChild(2 + i).gameObject.SetActive(false);
                            correct_btn.SetActive(true);
                            break;
                        case 1:
                            for (int i = 0; i < 2; i++)
                                board.transform.GetChild(2 + i).gameObject.SetActive(false);
                            correct_btn.SetActive(true);

                            break;
                    }
                    hearts.SetActive(false);
                    heartGettingText.gameObject.SetActive(true); // 이거 언제 끄지?
                    PlayerPrefs.SetInt("gettingHeartCount", PlayerPrefs.GetInt("gettingHeartCount") - 1);
                    Debug.Log("count: " + PlayerPrefs.GetInt("gettingHeartCount"));
                    if (PlayerPrefs.GetInt("gettingHeartCount") == 0)
                    {
                        heartGettingText.text = "하트 얻기 성공!";
                        PlayerPrefs.SetInt("gettingHeartCount", 5);
                        GameManager.Instance.increase_heart(1);
                    }                     
                    else
                        heartGettingText.text = "하트 얻기까지 남은 문제\n" + PlayerPrefs.GetInt("gettingHeartCount") ;
                });

            });
            

        }
        else 
        {
            switch (current_qtn.type)
            {
                case 0:
                    for (int i = 0; i < 4; i++)
                    {
                        Qchoice.transform.GetChild(i + 2).GetComponent<Button>().enabled = false;
                    }
                    Qchoice.transform.GetChild(2 + index).transform.GetChild(0).GetComponent<Text>().DOColor(new Color(1, 0.5254902f, 0.4901961f, 1), 0);
                    Qchoice.transform.GetChild(index + 2).GetComponent<Image>().sprite = pressed_choice;
                    break;
                case 1:
                    for (int i = 0; i < 2; i++)
                    {
                        Qox.transform.GetChild(i + 2).GetComponent<Button>().enabled = false;
                    }
                    if (index == 0)
                        Qox.transform.GetChild(2).GetComponent<Image>().sprite = pressed_o;
                    else if (index == 1)
                        Qox.transform.GetChild(3).GetComponent<Image>().sprite = pressed_x;
                    break;
            }


            fail.Play();
            stop_timer();

            PlayerPrefs.SetInt("fail_count", PlayerPrefs.GetInt("fail_count") + 1);
            if (PlayerPrefs.GetInt("remove_ad") == 0 && PlayerPrefs.GetInt("fail_count") >= 10)
            {
                admanager.GetComponent<AdManager>().RequestInterstitial();

                PlayerPrefs.SetInt("fail_count", 0);
            }

            // 문제 틀렸을 때 fail 입력
            string failed = PlayerPrefs.GetString("failed");
            string add = current_qtn.div.ToString() + current_qtn.in_div.ToString() + current_qtn.index.ToString() + current_qtn.in_index.ToString();
            int length = failed.Length / 4;
            for (int i = 0; i < length; i++)
            {
                if (add == failed.Substring(i * 4, 4))
                {
                    GameManager.Instance.decrease_heart(1);
                    board.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "틀렸어요...";
                    StartCoroutine(Shake(board, 100f, 1f));
                    return;
                }
            }


               
            GameManager.Instance.decrease_heart(1);
            PlayerPrefs.SetString("failed", failed + add);
            board.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "틀렸어요...";
            StartCoroutine(Shake(board, 100f, 1f));
            
        }

        
    }

   
    public IEnumerator Shake(GameObject board, float _amount, float _duration)
    {
        Vector3 originPos = board.transform.localPosition;

        float timer = 0;
        while (timer <= _duration)
        {
            board.transform.localPosition = (Vector3)Random.insideUnitCircle * _amount + originPos;

            timer += Time.deltaTime;
            yield return null;
        }
        board.transform.localPosition = originPos;
        yield return new WaitForSeconds(1f);
        incorrect_panel.SetActive(true);

    }
    public void press_text_btn(int index)
    {
        board = Qtext;
        GameObject blks = board.transform.GetChild(2).gameObject;

        int len = 0;
        for(int i = 0; i < current_qtn.answer.Length; i++)
        {
            len++;
            if (board.transform.GetChild(2).transform.GetChild(i).transform.GetChild(0).gameObject.activeSelf == false)
            {
                touch.Play();
                blks.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
                blks.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = board.transform.GetChild(3).transform.GetChild(index).transform.GetChild(0).GetComponent<Text>().text;
                break;
            }
            
        }

        if(len == current_qtn.answer.Length)
        {
            string temp = "";
            string answer = "";
            for (int i = 0; i < current_qtn.answer.Length; i++)
            {
                temp += blks.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
                answer += board.transform.GetChild(3).transform.GetChild(current_qtn.answer[i] - '0').transform.GetChild(0).GetComponent<Text>().text;
            }

            board.transform.GetChild(1).gameObject.SetActive(false);

            if (answer == temp)
            {
                for(int i = 0; i < 10; i++)
                {
                    board.transform.GetChild(3).gameObject.transform.GetChild(i).GetComponent<Button>().enabled = false;
                }
                for (int i = 0; i < len; i++)
                {
                    board.transform.GetChild(2).gameObject.transform.GetChild(i).transform.GetChild(0).GetComponent<Button>().enabled = false;
                    //board.transform.GetChild(2).gameObject.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().DOColor(new Color(0.831450f, 1, 0.7311321f),1);
                }
                correct.Play();
                stop_timer();
                board.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "정답!";
                heart_up();
                board.transform.GetChild(0).transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 1).SetEase(Ease.InOutElastic).OnComplete(() =>
                {
                    board.transform.GetChild(0).transform.DOScale(new Vector3(1f, 1f, 1f), 1f).SetEase(Ease.OutQuad).OnComplete(() => 
                    {
                        board.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "해설";
                        board.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "";
                        board.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().color = new Color(1, 0.5254902f, 0.4901961f, 0);
                        board.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = current_qtn.explain;
                        board.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().DOFade(1, 1);
                        board.transform.GetChild(3).gameObject.SetActive(false);
                        correct_btn.SetActive(true);
                        hearts.SetActive(false);
                        heartGettingText.gameObject.SetActive(true); // 이거 언제 끄지?
                        PlayerPrefs.SetInt("gettingHeartCount", PlayerPrefs.GetInt("gettingHeartCount") - 1);
                        Debug.Log("count: " + PlayerPrefs.GetInt("gettingHeartCount"));
                        if (PlayerPrefs.GetInt("gettingHeartCount") == 0)
                        {
                            heartGettingText.text = "하트 얻기 성공!";
                            PlayerPrefs.SetInt("gettingHeartCount", 5);
                            GameManager.Instance.increase_heart(1);
                        }
                        else
                            heartGettingText.text = "하트 얻기까지 남은 문제\n" + PlayerPrefs.GetInt("gettingHeartCount");
                    });

                });
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    board.transform.GetChild(3).gameObject.transform.GetChild(i).GetComponent<Button>().enabled = false;
                }
                for (int i = 0; i < current_qtn.answer.Length; i++)
                {
                    blks.transform.GetChild(i).transform.GetChild(0).GetComponent<Button>().enabled = false;
                    //blks.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().DOColor(new Color(1, 0.5337858f, 0.504717f), 1);
                }

                fail.Play();
                stop_timer();


                // 문제 틀렸을 때 fail 입력

                string failed = PlayerPrefs.GetString("failed");
                string add = current_qtn.div.ToString() + current_qtn.in_div.ToString() + current_qtn.index.ToString() + current_qtn.in_index.ToString();
                int length = failed.Length / 4;

                PlayerPrefs.SetInt("fail_count", PlayerPrefs.GetInt("fail_count") + 1);
                if (PlayerPrefs.GetInt("remove_ad") == 0 && PlayerPrefs.GetInt("fail_count") >= 10)
                {
                    admanager.GetComponent<AdManager>().RequestInterstitial();

                    PlayerPrefs.SetInt("fail_count", 0);
                }

                for (int i = 0; i < length; i++)
                {
                    if(add == failed.Substring(i*4, 4))
                    {
                        GameManager.Instance.decrease_heart(1);
                        board.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "틀렸어요...";
                        StartCoroutine(Shake(board, 100f, 1f));
                        return;
                    }
                }

                GameManager.Instance.decrease_heart(1);
                PlayerPrefs.SetString("failed", failed + add);
                board.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "틀렸어요...";
                StartCoroutine(Shake(board, 100f, 1f));
            }
        }
        
    }

    void heart_up()
    {
        hearts.SetActive(true);
        
        for (int i = 0; i < hearts.transform.childCount; i++)
        {
            hearts.transform.GetChild(i).GetComponent<Image>().DOFade(1, 0);
            hearts.transform.GetChild(i).transform.DOLocalMoveY(0, 0);
            hearts.transform.GetChild(i).transform.DOLocalMoveX(Random.Range(-1000, 1000), 0);
            hearts.transform.GetChild(i).transform.DOLocalMoveY(Random.Range(1000, 2000),1f).SetEase(Ease.OutQuint);
            hearts.transform.GetChild(i).GetComponent<Image>().DOFade(0,1f);
        }
    }

    public void out_text_btn(int index)
    {
        board = Qtext;
        board.transform.GetChild(2).transform.GetChild(index).transform.GetChild(0).gameObject.SetActive(false);
        board.transform.GetChild(2).transform.GetChild(index).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "";
        touch.Play();
    }

    public void next_question()
    {
        float load_time = 1f;
        Ease motion = Ease.InOutBack;
        correct_btn.SetActive(false);
        heartGettingText.gameObject.SetActive(false);
        switch (current_qtn.type)
        {
            case 0:
                board = Qchoice;
                break;
            case 1:
                board = Qox;

                break;
            case 2:
                board = Qtext;
                break;
        }
        board.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
        board.transform.DOLocalMoveX(-polar, load_time).SetEase(motion).OnComplete(() => {
            switch (menumanager.GetComponent<MenuManager>().beforequiz.name)
            {
                case "Lobby":
                    speed_mode();
                    break;
                case "Era_quiz":
                    if(current_qtn.in_index == 6)
                    {
                        //다음 스테이지 해금
                        if(load_index == PlayerPrefs.GetInt("era"))
                        {
                            PlayerPrefs.SetInt("era", PlayerPrefs.GetInt("era") + 1);
                            open_era();
                        }

                        Debug.Log("exit");
                        // 나가기
                        stop_timer();
                        menumanager.GetComponent<MenuManager>().back_chg_text(board);
                    }
                    else
                    {
                        for (int i = 0; i < qtns.Length; i++)
                        {
                            if (qtns[i].div == 0 && qtns[i].in_div == current_qtn.in_div && qtns[i].index == current_qtn.index && qtns[i].in_index == current_qtn.in_index + 1)
                            {
                                load_question(qtns[i]);
                                return;
                            }
                        }

                        if (load_index == PlayerPrefs.GetInt("era"))
                        {
                            PlayerPrefs.SetInt("era", PlayerPrefs.GetInt("era") + 1);
                            open_era();
                        }

                        stop_timer();
                        menumanager.GetComponent<MenuManager>().back_chg_text(board);
                    }
                    break;
                case "Theme_quiz":
                    if(current_qtn.index == 15)
                    {
                        stop_timer();
                        menumanager.GetComponent<MenuManager>().back_chg_text(board);
                    }
                    else
                    {
                        for (int i = 0; i < qtns.Length; i++)
                        {
                            if (qtns[i].div == 1 && qtns[i].in_div == current_qtn.in_div && qtns[i].index == current_qtn.index + 1)
                            {
                                load_question(qtns[i]);
                                return;
                            }
                        }
                        stop_timer();
                        menumanager.GetComponent<MenuManager>().back_chg_text(board);
                    }
                    break;
                case "Level_easy":

                    bool found = false;

                    for (int i = 0; i < qtns.Length; i++)
                    {
                        if (found == true && qtns[i].level == 0)
                        {
                            load_question(qtns[i]);
                            return;
                        }

                        if (found == false && qtns[i].level == 0 && qtns[i].div == current_qtn.div && qtns[i].in_div == current_qtn.in_div && qtns[i].index == current_qtn.index && qtns[i].in_index == current_qtn.in_index)
                        {
                            found = true;
                        }
                    }

                    stop_timer();
                    menumanager.GetComponent<MenuManager>().back_chg_text(board);

                    break;
                case "Level_mid":

                    found = false;

                    for (int i = 0; i < qtns.Length; i++)
                    {
                        if (found == true && qtns[i].level == 1)
                        {
                            load_question(qtns[i]);
                            return;
                        }

                        if (found == false && qtns[i].level == 1 && qtns[i].div == current_qtn.div && qtns[i].in_div == current_qtn.in_div && qtns[i].index == current_qtn.index && qtns[i].in_index == current_qtn.in_index)
                        {
                            found = true;
                        }
                    }

                    stop_timer();
                    menumanager.GetComponent<MenuManager>().back_chg_text(board);

                    break;
                case "Level_hard":
                    found = false;

                    for (int i = 0; i < qtns.Length; i++)
                    {
                        if (found == true && qtns[i].level == 2)
                        {
                            load_question(qtns[i]);
                            return;
                        }

                        if (found == false && qtns[i].level == 2 && qtns[i].div == current_qtn.div && qtns[i].in_div == current_qtn.in_div && qtns[i].index == current_qtn.index && qtns[i].in_index == current_qtn.in_index)
                        {
                            found = true;
                        }
                    }

                    stop_timer();
                    menumanager.GetComponent<MenuManager>().back_chg_text(board);

                    break;
                case "Discorrect":
                    stop_timer();
                    menumanager.GetComponent<MenuManager>().back_chg_text(board);

                    break;
            }
        });


    }

    public void retry()
    {
        if(PlayerPrefs.GetInt("heart") > 0)
        {
            incorrect_panel.SetActive(false);
            load_question(current_qtn);
        }
        else
        {
            shop.SetActive(true);   
        }

    }

    public void open_fail()
    {

        // 다시 짜기
        for (int i = 0; i < fails.Length; i++)
            fails[i].SetActive(false);

        int length = PlayerPrefs.GetString("failed").Length / 4;
        int end = 0;
        if (length > 50)
            end = length - 50;

        for(int i = length-1; i >= end; i--)
        {
            string title = "";
            string desc = "";
            string temp = PlayerPrefs.GetString("failed").Substring(i * 4, 4);

            switch (temp[1] - '0')
            {
                case 0:
                    title += "선사";
                    break;
                case 1:
                    title += "고조선";
                    break;
                case 2:
                    title += "삼국";
                    break;
                case 3:
                    title += "남북극";
                    break;
                case 4:
                    title += "고려";
                    break;
                case 5:
                    title += "조선";
                    break;
                case 6:
                    title += "근대";
                    break;
                case 7:
                    title += "현대";
                    break;
                case 8:
                    title += "삼일절";
                    break;
                case 9:
                    title += "6.25전쟁";
                    break;
                case 10:
                    title += "독립운동";
                    break;
                case 11:
                    title += "한국 예술";
                    break;
                case 12:
                    title += "스포츠";
                    break;
            }

            title += " ";

            switch (temp[1] - '0')
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    title += temp[2] + "-" + temp[3];
                    break;
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    title += temp[2];
                    break;
            }

            for(int j = 0; j < qtns.Length; j++)
                if(qtns[j].div == temp[0]- '0' && qtns[j].in_div == temp[1] - '0' && qtns[j].index == temp[2] - '0' && qtns[j].in_index == temp[3] - '0')
                {
                    desc = qtns[j].question_desc;
                    break;
                }
                    

            fails[length - 1 - i].SetActive(true);
            fails[length - 1 - i].transform.GetChild(0).GetComponent<Text>().text = title;
            fails[length - 1 - i].transform.GetChild(1).GetComponent<Text>().text = desc;

        }  
                
    }

    // 미리 틀렸던 문제 배열 생성. 그 배열에 index대로 문제 불러오게 하기.
    public void fail_mode()
    {
        if (PlayerPrefs.GetInt("heart") <= 0)
        {
            shop.SetActive(true);
            return;

        }

        menumanager.GetComponent<MenuManager>().write_beforestage(discorrect);
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        int index = obj.name[5] - '0';
        string failed = PlayerPrefs.GetString("failed");
        int length = failed.Length - (index + 1) * 4;
        string temp = failed.Substring(length, 4);
        for(int i =0; i < qtns.Length; i++)
        {
            if(qtns[i].div == temp[0] - '0' && qtns[i].in_div == temp[1] - '0' && qtns[i].index == temp[2] - '0' && qtns[i].in_index == temp[3] - '0')
            {
                load_question(qtns[i]);
                return;
            }
        }

        

    }

    public void freeHeart()
    {
        admanager.GetComponent<AdManager>().RequestInterstitial();
        GameManager.Instance.increase_heart(1);
    }


}
