﻿using System.Collections;
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
        Home_Button,
        Retry_Button,
    }
    enum Texts
    {
        MoveCountText,
        Clear_Stage_Text,
        Fail_Stage_Text,
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
        Defeat_Panel,
    }
    // ========= val =====================

    public GameObject Lobby_Panel,
        Puzzle_Panel,
        Battle_Panel,
        Clear_Panel
        , Defeat_Panel
        ;
    public Text MoveCountText
        , Clear_Stage_Text
        , Fail_Stage_Text
        ;

    public Button
        Start_Button
        , Clear_Button
        , Retry_Button
        , Home_Button
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
        Clear_Stage_Text = GetText(Texts.Clear_Stage_Text);
        Fail_Stage_Text = GetText(Texts.Fail_Stage_Text);


        Lobby_Panel = GetObject(GameObjects.Lobby_Panel);
        Puzzle_Panel = GetObject(GameObjects.Puzzle_Panel);
        Battle_Panel = GetObject(GameObjects.Battle_Panel);
        Clear_Panel = GetObject(GameObjects.Clear_Panel);
        Defeat_Panel = GetObject(GameObjects.Defeat_Panel);



        Start_Button = GetButton(Buttons.Start_Button);
        Clear_Button = GetButton(Buttons.Clear_Button);
        Retry_Button = GetButton(Buttons.Retry_Button);
        Home_Button = GetButton(Buttons.Home_Button);


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
        Home_Button.AddButtonEvent(() =>
        {
            ChangePanel(0);
            Managers._puzzleManager.CamChange(0);
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
        Defeat_Panel.SetActive(false);

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
