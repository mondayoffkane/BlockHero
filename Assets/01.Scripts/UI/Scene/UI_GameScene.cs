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
        ChangeCountText,
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


    public TextMeshProUGUI ChangeCountText;





    private void Awake()
    {
        Managers._gameUI = this;

        Bind<UnityEngine.UI.Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));


        // =====  Bind  ===================================
        //ChangeCountText = GetText(Texts.ChangeCountText);

        ChangeCountText = GetTextMesh(Texts.ChangeCountText);


        // ========================

        base.Init();

        ChangeCountText.text = "Count : 11";


    }
    private void JerryFighting()
    {
        Debug.Log("Test");
    }
}
