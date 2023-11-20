using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class UI_GameScene : UI_Scene
{
    enum Buttons
    {
        Clear_Button,
    }
    enum Texts
    {
        MoveCountText,
    }
    enum Images
    {
        BackGround,
    }
    enum GameObjects
    {
        Jerry,
        Lobby_Panel,
        Puzzle_Panel,
        Battle_Panel,
        Clear_Panel,
    }
    // ========= val =====================

    public GameObject Lobby_Panel,
        Puzzle_Panel,
        Battle_Panel,
        Clear_Panel
        ;
    public Text MoveCountText
        ;

    public Button Clear_Button
        ;

    // ============================


    private void Awake()
    {
        Bind<UnityEngine.UI.Text>(typeof(Texts));
        Bind<UnityEngine.UI.Button>(typeof(Buttons));
        Bind<UnityEngine.UI.Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
        Managers._gameUI = this;

        // =====  Bind  ===================================

        MoveCountText = GetText(Texts.MoveCountText);

        Lobby_Panel = GetObject(GameObjects.Lobby_Panel);
        Puzzle_Panel = GetObject(GameObjects.Puzzle_Panel);
        Battle_Panel = GetObject(GameObjects.Battle_Panel);
        Clear_Panel = GetObject(GameObjects.Clear_Panel);


        Clear_Button = GetButton(Buttons.Clear_Button);



        //ChangeCountText = GetText(Texts.ChangeCountText);

        // =====Button Event==============

        Clear_Button.AddButtonEvent(() => UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex));


        // ========================

        base.Init();




    }
    private void JerryFighting()
    {
        Debug.Log("Test");
    }

    public void ChangePanel(int _num)
    {
        Lobby_Panel.SetActive(false);
        Puzzle_Panel.SetActive(false);
        Battle_Panel.SetActive(false);

        switch (_num)
        {
            case 0:
                Lobby_Panel.SetActive(true);
                break;

            case 1:
                Puzzle_Panel.SetActive(true);

                break;

            case 2:
                Battle_Panel.SetActive(true);
                break;

            default:
                break;
        }

    }
}
