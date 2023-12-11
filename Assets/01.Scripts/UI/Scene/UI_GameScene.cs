﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene
{
    enum Texts
    {
        Status_Text,
        Recipe_Name_Text,
        Recipe_Block_Count_Text,
    }
    enum Buttons
    {
        HeroFactory_Button,
        Battle_Button,
        HeroFactory_Close_Button,
        Reset_Button,
        Make_Hero_Button,
        Undo_Button,
        Clear_Claim_Button,
        Fail_ToFactory_Button,
    }
    enum Images
    {
        BackGround,
        BluePrint_Img,
    }
    enum GameObjects
    {
        Jerry,
        FactoryBase_Panel,
        BlockFactory_Panel,
        HeroFactory_Panel,
        Recipe_Content,
        Recipe_RawImage,
        Color_Buttons_Group,
        Battle_Panel,
        Clear_Panel,
        Fail_Panel,
    }
    // ======================================================

    public GameObject BlockFactory_Panel,
        HeroFactory_Panel,
        //Recipe_Scroll,
        Recipe_Content,
        Color_Buttons_Group
        , FactoryBase_Panel
        , Battle_Panel
        , Clear_Panel,
        Fail_Panel;
    //Recipe_RawImage;


    public Button HeroFactory_Button,
        Battle_Button,
        HeroFactory_Close_Button,
        Reset_Button,
        Make_Hero_Button
        , Undo_Button,
        Clear_Claim_Button,
        Fail_ToFactory_Button;



    public Button[] _recipeListBttons = new Button[3];
    public Button[] _colorButtons = new Button[4];
    public RawImage Recipe_RawImage;

    public Text Status_Text
        , Recipe_Name_Text
        , Recipe_Block_Count_Text
        ;

    public Image BluePrint_Img
        ;

    // =======================================================
    private void Awake()
    {
        Bind<UnityEngine.UI.Text>(typeof(Texts));
        Bind<UnityEngine.UI.Button>(typeof(Buttons));
        Bind<UnityEngine.UI.Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));

        Managers._gameUi = this;


        base.Init();

        // ========= GameObjects
        BlockFactory_Panel = GetObject(GameObjects.BlockFactory_Panel);
        HeroFactory_Panel = GetObject(GameObjects.HeroFactory_Panel);
        Recipe_Content = GetObject(GameObjects.Recipe_Content);
        Color_Buttons_Group = GetObject(GameObjects.Color_Buttons_Group);
        //Recipe_RawImage = GetObject(GameObjects.Recipe_RawImage).GetComponent<RawImage>();
        FactoryBase_Panel = GetObject(GameObjects.FactoryBase_Panel);
        Battle_Panel = GetObject(GameObjects.Battle_Panel);
        Clear_Panel = GetObject(GameObjects.Clear_Panel);
        Fail_Panel = GetObject(GameObjects.Fail_Panel);

        // ========= Buttons
        HeroFactory_Button = GetButton(Buttons.HeroFactory_Button);
        Battle_Button = GetButton(Buttons.Battle_Button);
        HeroFactory_Close_Button = GetButton(Buttons.HeroFactory_Close_Button);
        for (int i = 0; i < Recipe_Content.transform.childCount; i++)
        {
            _recipeListBttons[i] = Recipe_Content.transform.GetChild(i).GetComponent<Button>();
        }

        _colorButtons = new Button[Color_Buttons_Group.transform.childCount];
        for (int i = 0; i < Color_Buttons_Group.transform.childCount; i++)
        {
            _colorButtons[i] = Color_Buttons_Group.transform.GetChild(i).GetComponent<Button>();
        }

        Reset_Button = GetButton(Buttons.Reset_Button);
        Make_Hero_Button = GetButton(Buttons.Make_Hero_Button);
        Undo_Button = GetButton(Buttons.Undo_Button);

        Clear_Claim_Button = GetButton(Buttons.Clear_Claim_Button);
        Fail_ToFactory_Button = GetButton(Buttons.Fail_ToFactory_Button);

        // ========= Img
        Recipe_RawImage = GetObject(GameObjects.Recipe_RawImage).GetComponent<RawImage>();
        BluePrint_Img = GetImage(Images.BluePrint_Img);

        // ========= Text

        Status_Text = GetText(Texts.Status_Text);
        Recipe_Name_Text = GetText(Texts.Recipe_Block_Count_Text);
        Recipe_Block_Count_Text = GetText(Texts.Recipe_Block_Count_Text);

        // ================ Add Button Listner========================================

        HeroFactory_Button.AddButtonEvent(() => ChangePanel(2));
        HeroFactory_Close_Button.AddButtonEvent(() => ChangePanel(0));

        for (int i = 0; i < _recipeListBttons.Length; i++)
        {
            int _num = i;
            _recipeListBttons[i].AddButtonEvent(() => Managers._stageManager.SelectRecipe(_num));
        }

        for (int i = 0; i < _colorButtons.Length; i++)
        {
            int _num = i;
            _colorButtons[i].AddButtonEvent(() =>
            {
                Managers._stageManager.SelectModelSetColor(_num);
            });
        }

        Undo_Button.AddButtonEvent(() => Managers._stageManager.SelectModelUndoColor());
        Reset_Button.AddButtonEvent(() => Managers._stageManager.SelectModelReset());
        Make_Hero_Button.AddButtonEvent(() => Managers._stageManager.MakeHero());
        Battle_Button.AddButtonEvent(() => Managers._stageManager.ToBattle());

        Clear_Claim_Button.AddButtonEvent(() => Managers._stageManager.ToFactory());
        Fail_ToFactory_Button.AddButtonEvent(() => Managers._stageManager.ToFactory());
    }

    // ====================================================

    public void ChangeRecipe(int _num, Recipe_Model _newRecipe)
    {
        for (int i = 0; i < _recipeListBttons.Length; i++)
        {
            _recipeListBttons[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        _recipeListBttons[_num].transform.GetChild(0).gameObject.SetActive(true);

        //Recipe_RawImage.texture = _newRecipe._rendTexture;

        Recipe_Name_Text.text = $"{_newRecipe._recipeName}";
        Status_Text.text = $"ATK : 1   SPD : 1   HP : 1";

        Recipe_Name_Text.text = $"{_newRecipe._recipeName}";
        Recipe_Block_Count_Text.text = $"{_newRecipe._currentParts_Num} / {_newRecipe._partsCount}";
        BluePrint_Img.sprite = _newRecipe._bluePrint_Sprite;

        Managers._stageManager.FactoryCheckButtons();

    }

    public void ChangePanel(int _num)
    {
        FactoryBase_Panel.SetActive(false);
        BlockFactory_Panel.SetActive(false);
        HeroFactory_Panel.SetActive(false);
        Battle_Panel.SetActive(false);
        Clear_Panel.SetActive(false);
        Fail_Panel.SetActive(false);

        switch (_num)
        {
            case 0:
                FactoryBase_Panel.SetActive(true);
                break;

            case 1:
                BlockFactory_Panel.SetActive(true);
                break;

            case 2:
                HeroFactory_Panel.SetActive(true);
                break;

            case 3:
                Battle_Panel.SetActive(true);
                break;

            case 4:
                Clear_Panel.SetActive(true);
                break;

            case 5:
                Fail_Panel.SetActive(true);
                break;
        }

    }

    public void MakeButtonOnOff(bool isBool)
    {
        if (isBool)
        {
            Color_Buttons_Group.SetActive(false);
            Make_Hero_Button.gameObject.SetActive(true);
            Make_Hero_Button.interactable = true;
        }
        else
        {
            Color_Buttons_Group.SetActive(true);
            Make_Hero_Button.gameObject.SetActive(false);
            Make_Hero_Button.interactable = false;
        }


    }




    private void JerryFighting()
    {
        //Debug.Log("Test");
    }
}
