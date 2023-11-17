using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class UI_GameScene : UI_Scene
{
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
    }


    public Text MoveCountText;





    private void Awake()
    {
        Bind<UnityEngine.UI.Text>(typeof(Texts));
        Bind<UnityEngine.UI.Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
        Managers._gameUI = this;

        MoveCountText = GetText(Texts.MoveCountText);


        // =====  Bind  ===================================



        //ChangeCountText = GetText(Texts.ChangeCountText);


        // ========================

        base.Init();




    }
    private void JerryFighting()
    {
        Debug.Log("Test");
    }
}
