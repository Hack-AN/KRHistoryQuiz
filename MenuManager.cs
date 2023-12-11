using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    float load = 0.5f;
    public Text upper_text;

    public GameObject playingmanager;

    public GameObject[] boards;
    /* boards[]
     * 0 : Lobby
     * 1 : Era_quiz
     * 
     * 
     */


    float polar = 4000f;

    public GameObject beforequiz;

    public void out_left(GameObject board)
    {
        board.transform.DOLocalMoveX(-polar, load).SetEase(Ease.InOutBack).OnComplete(() => {
            board.SetActive(false);
        }); 
    }
    public void in_left(GameObject board)
    {
        board.transform.DOLocalMoveX(polar, 0);
        board.SetActive(true);
        board.transform.DOLocalMoveX(0, load).SetEase(Ease.InOutBack);
    }
    public void out_right(GameObject board)
    {
        board.transform.DOLocalMoveX(polar, load).SetEase(Ease.InOutBack).OnComplete(() => {
            board.SetActive(false);
        });
    }
    public void in_right(GameObject board)
    {
        board.transform.DOLocalMoveX(-polar, 0);
        board.SetActive(true);
        board.transform.DOLocalMoveX(0, load).SetEase(Ease.InOutBack);
    }

    public void back_chg_text(GameObject board)
    {
        if(board.activeSelf == true)
        {
            out_right(board);
            switch(board.name)
            {
                case "Era_quiz":
                case "Theme_quiz":
                case "Level_quiz":
                case "Discorrect":
                    upper_text.text = "로비";
                    in_right(boards[0]);
                    break;
                case "Level_easy":
                case "Level_mid":
                case "Level_hard":
                    upper_text.text = "난이도 선택";
                    in_right(boards[3]);
                    break;
                case "Qchoice":
                case "QOX":
                case "QText":
                    switch(beforequiz.name)
                    {
                        case "Lobby":
                            upper_text.text = "로비";
                            break;

                        case "Era_quiz":
                            upper_text.text = "시대별 퀴즈";
                            break;
                        case "Theme_quiz":
                            upper_text.text = "테마 퀴즈";
                            break;
                        case "Level_easy":
                            upper_text.text = "쉬운 퀴즈";
                            break;
                        case "Level_mid":
                            upper_text.text = "보통 퀴즈";
                            break;
                        case "Level_hard":
                            upper_text.text = "어려운 퀴즈";
                            break;
                        case "Discorrect":
                            upper_text.text = "틀린 퀴즈";
                            break;
                    }
                    playingmanager.GetComponent<PlayingManager>().stop_timer();
                    playingmanager.GetComponent<PlayingManager>().correct_btn.SetActive(false);
                    in_right(beforequiz);
                    break;
            }
        }
    }

    public void write_beforestage(GameObject board)
    {

        beforequiz = board;
        out_left(board);
    }

    public void increase_heart(int num)
    {
        GameManager.Instance.increase_heart(num);
    }

    public void decrease_heart(int num)
    {
        GameManager.Instance.decrease_heart(num);
    }

}
