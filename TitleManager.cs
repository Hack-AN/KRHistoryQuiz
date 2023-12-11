using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleManager : MonoBehaviour
{
    public GameObject[] btns;
    float intro_load = 1f ;

    public GameObject lobby;
    public GameObject[] lobbyobjs;

    public bool soozon;
    public Image sooz;
    public GameObject logo_board;

    public GameObject BGM;
    public GameObject SE;

    float polar = 4000f;

    public GameObject hearts;

    public float heart_term;
    float time = 0;
    public GameObject heart_pre;

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < lobbyobjs.Length; i++)
            lobbyobjs[i].SetActive(false);
        
        //hearts.SetActive(true);

        StartCoroutine("intro");
    }

    IEnumerator intro()
    {
        if(soozon == true)
        {
            logo_board.SetActive(true);
            sooz.DOFade(1, 1);
            while(sooz.color.a < 1f) { yield return null; }

            yield return new WaitForSeconds(2f);

            logo_board.SetActive(false);
        }

        intro2();

    }


    void intro2()
    {
        for (int i = 0; i < btns.Length; i++)
        {
            btns[i].SetActive(false);
            btns[i].transform.DOLocalMoveX(-polar, 0);
        }
        for (int i = 0; i < btns.Length; i++)
            btns[i].SetActive(true);



        for (int i = 0; i < btns.Length; i++)
        {
            btns[i].transform.DOLocalMoveX(0, intro_load).SetEase(Ease.InOutBack).OnComplete(() => { lobby.SetActive(false); });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(btns[0].activeSelf == true)
            {
                Application.Quit();
            }
            else
            {
                lobbyobjs[0].GetComponent<Button>().onClick.Invoke();
            }
            
        }

        if(btns[0].activeSelf == true)
        {

            time += Time.deltaTime;
            if (time >= heart_term)
            {
                
                time = 0;
                GameObject heart = Instantiate(heart_pre);
                heart.transform.parent = hearts.transform;
                int ran_x = Random.Range(-1000, 1000);
                int ran_y = Random.Range(1500, 3500);
                heart.transform.DOLocalMove(new Vector2(ran_x, 0), 0);
                heart.transform.DOLocalMove(new Vector2(ran_x, ran_y), 1f);
                heart.GetComponent<Image>().DOFade(0, 1).OnComplete(() =>
                {
                    Destroy(heart);
                });
            }
        }



    }


    public void gotolobby()
    {

        for (int i = 0; i < btns.Length; i++)
        {
            btns[i].transform.DOLocalMoveX(-polar, intro_load).SetEase(Ease.InOutBack);
        }

        for (int i = 0; i < lobbyobjs.Length; i++)
            lobbyobjs[i].SetActive(true);

        lobby.SetActive(false);
        lobby.transform.DOLocalMoveX(polar, 0);
        lobby.SetActive(true);
        lobby.transform.DOLocalMoveX(12.615f, intro_load).SetEase(Ease.InOutBack).OnComplete(() => 
        {
            for (int i = 0; i < btns.Length; i++)
                btns[i].SetActive(false);
        });
        hearts.SetActive(false);

    }

    public void gototitle()
    {
        if(lobby.activeSelf == true)
        {
            lobby.transform.DOLocalMoveX(polar, intro_load).SetEase(Ease.InOutBack).OnComplete(() => { lobby.SetActive(false); });
            
            for (int i = 0; i < lobbyobjs.Length; i++)
                lobbyobjs[i].SetActive(false);
            for (int i = 0; i < btns.Length; i++)
                btns[i].SetActive(true);

            intro2();
        }

    }


    public void bgm()
    {
        if (BGM.activeSelf == true)
        {
            BGM.SetActive(false);
            EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text = "배경음악 : OFF";
        }
        else
        {
            BGM.SetActive(true);
            EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text = "배경음악 : ON";
        }
            
    }

    public void se()
    {
        if (SE.activeSelf == true)
        {
            SE.SetActive(false);
            EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text = "효과음 : OFF";
        }
        else
        {
            SE.SetActive(true);
            EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text = "효과음 : ON";
        }

    }
}
