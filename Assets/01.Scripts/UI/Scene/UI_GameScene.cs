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
        Start_Button,
        Clear_Button,
        Retry_Button,
        Lobby_Button,
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
        Fail_Panel,
    }
    // ========= val =====================

    public GameObject Lobby_Panel,
        Puzzle_Panel,
        Battle_Panel,
        Clear_Panel
        , Fail_Panel
        ;
    public Text MoveCountText
        ;

    public Button
        Start_Button
        , Clear_Button
        , Retry_Button
        , Lobby_Button
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
        Fail_Panel = GetObject(GameObjects.Fail_Panel);



        Start_Button = GetButton(Buttons.Start_Button);
        Clear_Button = GetButton(Buttons.Clear_Button);
        Retry_Button = GetButton(Buttons.Retry_Button);
        Lobby_Button = GetButton(Buttons.Lobby_Button);


        //ChangeCountText = GetText(Texts.ChangeCountText);

        // =====Button Event==============
        Start_Button.AddButtonEvent(() => Managers._puzzleManager.StartStage());
        Clear_Button.AddButtonEvent(() =>
        {
            ChangePanel(0);
            Managers._puzzleManager.InitStage();
            //Managers._puzzleManager.LoadStage();
        });
        Retry_Button.AddButtonEvent(() => Managers._puzzleManager.StartStage());
        Lobby_Button.AddButtonEvent(() =>
        {
            ChangePanel(0);
            Managers._puzzleManager.InitStage();
            //Managers._puzzleManager.LoadStage();
        });



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
        Clear_Panel.SetActive(false);
        Fail_Panel.SetActive(false);

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
