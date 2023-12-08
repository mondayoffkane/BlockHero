using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene
{
    enum Texts
    {
        Status_Text,
    }
    enum Buttons
    {
        HeroFactory_Button,
        Battle_Button,
        HeroFactory_Close_Button,
        Reset_Button,
        Make_Hero_Button,
        Undo_Button,
    }
    enum Images
    {
        BackGround,
    }
    enum GameObjects
    {
        Jerry,
        BlockFactory_Panel,
        HeroFactory_Panel,
        Recipe_Content,
        Recipe_RawImage,
        Color_Buttons_Group,
    }
    // ======================================================

    public GameObject BlockFactory_Panel,
        HeroFactory_Panel,
        //Recipe_Scroll,
        Recipe_Content,
        Color_Buttons_Group;
    //Recipe_RawImage;


    public Button HeroFactory_Button,
        Battle_Button,
        HeroFactory_Close_Button,
        Reset_Button,
        Make_Hero_Button
        , Undo_Button;



    public Button[] _recipeListBttons = new Button[3];
    public Button[] _colorButtons = new Button[3];
    public RawImage Recipe_RawImage;

    public Text Status_Text
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
        Recipe_RawImage = GetObject(GameObjects.Recipe_RawImage).GetComponent<RawImage>();

        // ========= Buttons
        HeroFactory_Button = GetButton(Buttons.HeroFactory_Button);
        Battle_Button = GetButton(Buttons.Battle_Button);
        HeroFactory_Close_Button = GetButton(Buttons.HeroFactory_Close_Button);


        for (int i = 0; i < Recipe_Content.transform.childCount; i++)
        {
            _recipeListBttons[i] = Recipe_Content.transform.GetChild(i).GetComponent<Button>();
        }

        for (int i = 0; i < Color_Buttons_Group.transform.childCount; i++)
        {
            _colorButtons[i] = Color_Buttons_Group.transform.GetChild(i).GetComponent<Button>();
        }


        Reset_Button = GetButton(Buttons.Reset_Button);
        Make_Hero_Button = GetButton(Buttons.Make_Hero_Button);
        Undo_Button = GetButton(Buttons.Undo_Button);


        // ========= Img
        Recipe_RawImage = GetObject(GameObjects.Recipe_RawImage).GetComponent<RawImage>();


        // ========= Text

        Status_Text = GetText(Texts.Status_Text);

        // ================ Add Button Listner========================================

        HeroFactory_Button.AddButtonEvent(() => HeroFactory_Panel.SetActive(true));
        Battle_Button.AddButtonEvent(() => { });
        HeroFactory_Close_Button.AddButtonEvent(() => HeroFactory_Panel.SetActive(false));

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

    }

    public void ChangeRecipe(int _num, Recipe_Model _newRecipe)
    {
        for (int i = 0; i < _recipeListBttons.Length; i++)
        {
            _recipeListBttons[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        _recipeListBttons[_num].transform.GetChild(0).gameObject.SetActive(true);

        Recipe_RawImage.texture = _newRecipe._rendTexture;

        Status_Text.text = $"ATK : 1    SPD : 1    HP : 1";

    }


    private void JerryFighting()
    {
        //Debug.Log("Test");
    }
}
