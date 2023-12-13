using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_GameScene : UI_Scene
{
    enum Texts
    {
        Money_Text,
        BlockMachine_Upgrade_Price_Text,
        BlockMachine_Status_Text,
        BlockMachine_UpgradeValue_Text,
        Recipe_Status_Text,
        Recipe_Name_Text,
        Recipe_Block_Count_Text,
        Boss_HP_Text,
    }
    enum Buttons
    {
        HeroFactory_Button,
        Battle_Button,
        BlockMachine_Close_Button,
        BlockMachine_Upgrade_Button,
        HeroFactory_Close_Button,
        Reset_Button,
        Make_Hero_Button,
        Undo_Button,
        Clear_Claim_Button,
        Fail_ToFactory_Button,
    }
    enum Images
    {
        BluePrint_Img,
        Boss_HP_Guage,
    }
    enum GameObjects
    {
        FactoryBase_Panel,
        BlockMachine_Panel,
        BlockMachine_Color_Buttons_Group,
        HeroFactory_Panel,
        Recipe_Content,
        BlockImg_Group,
        Recipe_RawImage,
        Color_Buttons_Group,
        Battle_Panel,
        Clear_Panel,
        Fail_Panel,
    }
    // ======================================================

    public GameObject BlockMachine_Panel,
        HeroFactory_Panel,
        //Recipe_Scroll,
        Recipe_Content,
        Color_Buttons_Group
        , FactoryBase_Panel
        , Battle_Panel
        , Clear_Panel,
        Fail_Panel
        , BlockImg_Group
        , BlockMachine_Color_Buttons_Group
        ;
    //Recipe_RawImage;


    public Button HeroFactory_Button,
        Battle_Button,
        HeroFactory_Close_Button,
        Reset_Button,
        Make_Hero_Button
        , Undo_Button,
        Clear_Claim_Button,
        Fail_ToFactory_Button
        , BlockMachine_Close_Button,
        BlockMachine_Upgrade_Button;



    public Button[] _recipeListBttons = new Button[3];
    public Button[] _heroFactoryColorButtons = new Button[4];
    public RawImage Recipe_RawImage;
    public Sprite[] _colorImgs = new Sprite[4];
    public Button[] _blockMachineColorButtons = new Button[4];



    public Text Recipe_Status_Text
        , Recipe_Name_Text
        , Recipe_Block_Count_Text
        , Money_Text
        , Boss_HP_Text
        , BlockMachine_Upgrade_Price_Text,
        BlockMachine_Status_Text,
        BlockMachine_UpgradeValue_Text
        ;

    public Image BluePrint_Img
        , Boss_HP_Guage
        ;
    public Image[] _colorButtonImgs;

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
        BlockMachine_Panel = GetObject(GameObjects.BlockMachine_Panel);
        HeroFactory_Panel = GetObject(GameObjects.HeroFactory_Panel);
        Recipe_Content = GetObject(GameObjects.Recipe_Content);
        Color_Buttons_Group = GetObject(GameObjects.Color_Buttons_Group);
        //Recipe_RawImage = GetObject(GameObjects.Recipe_RawImage).GetComponent<RawImage>();
        FactoryBase_Panel = GetObject(GameObjects.FactoryBase_Panel);
        Battle_Panel = GetObject(GameObjects.Battle_Panel);
        Clear_Panel = GetObject(GameObjects.Clear_Panel);
        Fail_Panel = GetObject(GameObjects.Fail_Panel);
        BlockImg_Group = GetObject(GameObjects.BlockImg_Group);
        BlockMachine_Color_Buttons_Group = GetObject(GameObjects.BlockMachine_Color_Buttons_Group);

        // ========= Buttons
        HeroFactory_Button = GetButton(Buttons.HeroFactory_Button);
        Battle_Button = GetButton(Buttons.Battle_Button);
        HeroFactory_Close_Button = GetButton(Buttons.HeroFactory_Close_Button);
        for (int i = 0; i < Recipe_Content.transform.childCount; i++)
        {
            _recipeListBttons[i] = Recipe_Content.transform.GetChild(i).GetComponent<Button>();
        }

        _heroFactoryColorButtons = new Button[Color_Buttons_Group.transform.childCount];
        for (int i = 0; i < Color_Buttons_Group.transform.childCount; i++)
        {
            _heroFactoryColorButtons[i] = Color_Buttons_Group.transform.GetChild(i).GetComponent<Button>();
        }

        for (int i = 0; i < BlockMachine_Color_Buttons_Group.transform.childCount; i++)
        {
            _blockMachineColorButtons[i] = BlockMachine_Color_Buttons_Group.transform.GetChild(i).GetComponent<Button>();
        }


        Reset_Button = GetButton(Buttons.Reset_Button);
        Make_Hero_Button = GetButton(Buttons.Make_Hero_Button);
        Undo_Button = GetButton(Buttons.Undo_Button);

        Clear_Claim_Button = GetButton(Buttons.Clear_Claim_Button);
        Fail_ToFactory_Button = GetButton(Buttons.Fail_ToFactory_Button);

        BlockMachine_Close_Button = GetButton(Buttons.BlockMachine_Close_Button);
        BlockMachine_Upgrade_Button = GetButton(Buttons.BlockMachine_Upgrade_Button);


        // ========= Img
        Recipe_RawImage = GetObject(GameObjects.Recipe_RawImage).GetComponent<RawImage>();
        BluePrint_Img = GetImage(Images.BluePrint_Img);

        for (int i = 0; i < _colorImgs.Length; i++)
        {
            //Debug.Log($"BlockImg_Group/Block_{i}");
            _colorImgs[i] = Resources.Load<Sprite>($"BlockImg_Group/Block_{i}");
        }

        int _count = BlockImg_Group.transform.childCount;
        _colorButtonImgs = new Image[_count];
        for (int i = 0; i < _count; i++)
        {
            _colorButtonImgs[i] = BlockImg_Group.transform.GetChild(i).GetComponent<Image>();
            _colorButtonImgs[i].gameObject.SetActive(false);
        }

        Boss_HP_Guage = GetImage(Images.Boss_HP_Guage);

        // ========= Text

        Recipe_Status_Text = GetText(Texts.Recipe_Status_Text);
        Recipe_Name_Text = GetText(Texts.Recipe_Name_Text);
        Recipe_Block_Count_Text = GetText(Texts.Recipe_Block_Count_Text);
        Money_Text = GetText(Texts.Money_Text);
        Boss_HP_Text = GetText(Texts.Boss_HP_Text);


        // ================ Add Button Listner========================================

        HeroFactory_Button.AddButtonEvent(() => ChangePanel(2));
        HeroFactory_Close_Button.AddButtonEvent(() => ChangePanel(0));

        for (int i = 0; i < _recipeListBttons.Length; i++)
        {
            int _num = i;
            _recipeListBttons[i].AddButtonEvent(() => Managers._stageManager.SelectRecipe(_num));
        }

        for (int i = 0; i < _heroFactoryColorButtons.Length; i++)
        {
            int _num = i;
            _heroFactoryColorButtons[i].AddButtonEvent(() =>
            {
                Managers._stageManager.SelectModelSetColor(_num);
            });
        }
        for (int i = 0; i < _blockMachineColorButtons.Length; i++)
        {
            int _num = i;
            _blockMachineColorButtons[i].AddButtonEvent(() =>
            {
                //Managers._stageManager.SelectModelSetColor(_num);
                Managers._stageManager._selectBlockMachine.SetBlockType(_num);
                BlockMachine_SetColor(_num);
            });
        }




        Undo_Button.AddButtonEvent(() => Managers._stageManager.SelectModelUndoColor());
        Reset_Button.AddButtonEvent(() => Managers._stageManager.SelectModelReset());
        Make_Hero_Button.AddButtonEvent(() => Managers._stageManager.MakeHero());
        Battle_Button.AddButtonEvent(() => Managers._stageManager.ToBattle());

        Clear_Claim_Button.AddButtonEvent(() => Managers._stageManager.ToFactory());
        Fail_ToFactory_Button.AddButtonEvent(() => Managers._stageManager.ToFactory());

        BlockMachine_Close_Button.AddButtonEvent(() => ChangePanel(0));


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
        //Recipe_Status_Text.text = $"ATK : 1   SPD : 1   HP : 1";
        Recipe_Status_Text.text = $"ATK : {_newRecipe._damage} Speed : {_newRecipe._speed} HP : {_newRecipe._maxHP} DEF : {_newRecipe._defense}";


        //Recipe_Block_Count_Text.text = $"{_newRecipe._currentParts_Num} / {_newRecipe._partsCount}";
        Recipe_Block_Count_Text.text = $"X {_newRecipe._partsCount}";
        BluePrint_Img.sprite = _newRecipe._bluePrint_Sprite;

        Managers._stageManager.FactoryCheckButtons();

    }

    public void ChangePanel(int _num)
    {

        PanelOnOff(FactoryBase_Panel, false);
        PanelOnOff(BlockMachine_Panel, false);
        PanelOnOff(HeroFactory_Panel, false);
        PanelOnOff(Battle_Panel, false);
        PanelOnOff(Clear_Panel, false);
        PanelOnOff(Fail_Panel, false);


        switch (_num)
        {
            case 0:

                PanelOnOff(FactoryBase_Panel, true, 0);
                break;

            case 1:
                PanelOnOff(BlockMachine_Panel, true);
                break;

            case 2:
                PanelOnOff(HeroFactory_Panel, true);
                break;

            case 3:
                PanelOnOff(Battle_Panel, true);
                break;

            case 4:
                PanelOnOff(Clear_Panel, true);
                break;

            case 5:
                PanelOnOff(Fail_Panel, true);
                break;
        }

    }

    public void PanelOnOff(GameObject _obj, bool isOn, float _time = 0.3f)
    {
        if (isOn)
        {
            Debug.Log("On");
            _obj.SetActive(true);
            _obj.transform.localScale = Vector3.zero;
            _obj.transform.DOScale(Vector3.one, _time).SetEase(Ease.OutBack);

        }
        else
        {
            Debug.Log("Off");
            //_obj.transform.DOScale(Vector3.zero, _time).SetEase(Ease.InBack).OnComplete(() => _obj.SetActive(false));
            _obj.SetActive(false);
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



    public void SetColorImg(Recipe_Model _newRecipe)
    {
        float[] _values = new float[4];

        for (int i = 0; i < BlockImg_Group.transform.childCount; i++)
        {
            if (i < _newRecipe._currentParts_Num)
            {
                _colorButtonImgs[i].gameObject.SetActive(true);
                _colorButtonImgs[i].sprite = _colorImgs[(int)_newRecipe._tempBlockList[i]];
                _values[(int)_newRecipe._tempBlockList[i]]++;
            }
            else
            {
                _colorButtonImgs[i].gameObject.SetActive(false);
            }
        }
    }

    public void BlockMachine_SetColor(int _num)
    {
        for (int i = 0; i < 4; i++)
        {
            BlockMachine_Color_Buttons_Group.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
        BlockMachine_Color_Buttons_Group.transform.GetChild(_num).GetChild(0).gameObject.SetActive(true);
    }





    private void JerryFighting()
    {



        //Debug.Log("Test");
    }
}
