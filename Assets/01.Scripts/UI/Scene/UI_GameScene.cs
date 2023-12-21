using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
//using System.Drawing;

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
        Guage_Text,
        Boss_HP_Text,
    }
    enum Buttons
    {
        HeroFactory_Button,
        Battle_Button,
        AddBlockMachine_Button,
        AddHeroFactory_Button,
        ViewChange_Button,
        BlockMachine_Close_Button,
        BlockMachine_Upgrade_Button,
        HeroFactory_Close_Button,
        Reset_Button,
        Make_Hero_Button,
        Stop_Hero_Button,
        Undo_Button,
        Clear_Claim_Button,
        Fail_ToFactory_Button,
    }
    enum Images
    {
        Guage_Fill,
        Boss_HP_Guage,
    }
    enum GameObjects
    {
        FactoryBase_Panel,
        BlockCount_Group,
        BlockMachine_Panel,
        BlockMachine_Color_Buttons_Group,
        HeroFactory_Panel,
        Recipe_Content,
        BluePrint_Img_Group,
        BlockImg_Group,
        Recipe_RawImage,
        Color_Buttons_Group,
        Guage_Empty,
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
        //, BlockImg_Group
        , BlockMachine_Color_Buttons_Group
        , Guage_Empty
        , BluePrint_Img_Group
        , BlockCount_Group
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
        BlockMachine_Upgrade_Button
        , ViewChange_Button
        , AddBlockMachine_Button,
        AddHeroFactory_Button
        , Stop_Hero_Button

        ;



    public Button[] _recipeListBttons = new Button[3];
    public Button[] _heroFactoryColorButtons = new Button[4];
    public RawImage Recipe_RawImage;
    //public Sprite[] _colorImgs = new Sprite[4];
    public Button[] _blockMachineColorButtons = new Button[4];



    public Text Recipe_Status_Text
        , Recipe_Name_Text
        , Recipe_Block_Count_Text
        , Money_Text
        , Boss_HP_Text
        , BlockMachine_Upgrade_Price_Text,
        BlockMachine_Status_Text,
        BlockMachine_UpgradeValue_Text
        , Guage_Text
        ;

    public Text[] _blockCountTexts = new Text[4];


    public Image //BluePrint_Img
         Boss_HP_Guage
        , Guage_Fill
        ;
    //public Image[] _colorButtonImgs;
    public Image[] _bluePrint_Imgs;

    public Color _color;





    // =======================================================
    private void Awake()
    {
        ColorUtility.TryParseHtmlString("#4F9BCD", out _color);

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
        //BlockImg_Group = GetObject(GameObjects.BlockImg_Group);
        BlockMachine_Color_Buttons_Group = GetObject(GameObjects.BlockMachine_Color_Buttons_Group);
        Guage_Empty = GetObject(GameObjects.Guage_Empty);
        BluePrint_Img_Group = GetObject(GameObjects.BluePrint_Img_Group);
        BlockCount_Group = GetObject(GameObjects.BlockCount_Group);



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
        ViewChange_Button = GetButton(Buttons.ViewChange_Button);

        AddBlockMachine_Button = GetButton(Buttons.AddBlockMachine_Button);
        AddHeroFactory_Button = GetButton(Buttons.AddHeroFactory_Button);
        Stop_Hero_Button = GetButton(Buttons.Stop_Hero_Button);

        // ========= Img
        Recipe_RawImage = GetObject(GameObjects.Recipe_RawImage).GetComponent<RawImage>();
        //BluePrint_Img = GetImage(Images.BluePrint_Img);


        //for (int i = 0; i < _colorImgs.Length; i++)
        //{
        //    //Debug.Log($"BlockImg_Group/Block_{i}");
        //    _colorImgs[i] = Resources.Load<Sprite>($"BlockImg_Group/Block_{i}");
        //}

        //int _count = BlockImg_Group.transform.childCount;
        //_colorButtonImgs = new Image[_count];
        //for (int i = 0; i < _count; i++)
        //{
        //    _colorButtonImgs[i] = BlockImg_Group.transform.GetChild(i).GetComponent<Image>();
        //    _colorButtonImgs[i].gameObject.SetActive(false);
        //}

        for (int i = 0; i < 5; i++)
        {
            _bluePrint_Imgs[i] = BluePrint_Img_Group.transform.GetChild(i).GetComponent<Image>();
        }

        for (int i = 0; i < 5; i++)
        {
            _bluePrint_Imgs[i].transform.SetSiblingIndex(0);
        }



        Boss_HP_Guage = GetImage(Images.Boss_HP_Guage);
        Guage_Fill = GetImage(Images.Guage_Fill);
        // ========= Text

        Recipe_Status_Text = GetText(Texts.Recipe_Status_Text);
        Recipe_Name_Text = GetText(Texts.Recipe_Name_Text);
        Recipe_Block_Count_Text = GetText(Texts.Recipe_Block_Count_Text);
        Money_Text = GetText(Texts.Money_Text);
        Boss_HP_Text = GetText(Texts.Boss_HP_Text);
        BlockMachine_Upgrade_Price_Text = GetText(Texts.BlockMachine_Upgrade_Price_Text);
        BlockMachine_Status_Text = GetText(Texts.BlockMachine_Status_Text);
        BlockMachine_UpgradeValue_Text = GetText(Texts.BlockMachine_UpgradeValue_Text);
        Guage_Text = GetText(Texts.Guage_Text);

        for (int i = 0; i < 4; i++)
        {
            _blockCountTexts[i] = BlockCount_Group.transform.GetChild(i).GetChild(0).GetComponent<Text>();
        }


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
        Make_Hero_Button.AddButtonEvent(() =>
        {
            Managers._stageManager.MakeHero(true);
            MakeButtonOnOff();
        });
        Battle_Button.AddButtonEvent(() => Managers._stageManager.ToBattle());

        Clear_Claim_Button.AddButtonEvent(() =>
        {
            DOTween.Sequence()
            .AppendCallback(() =>
            {
                Clear_Claim_Button.interactable = false;
                Clear_Claim_Button.transform.Find("UIAttractor").gameObject.SetActive(true);

            }).AppendInterval(1f)
            .AppendCallback(() =>
            {
                Managers._stageManager.ToFactory();
                Clear_Claim_Button.interactable = true;
                Clear_Claim_Button.transform.Find("UIAttractor").gameObject.SetActive(false);
            });
        });
        Fail_ToFactory_Button.AddButtonEvent(() => Managers._stageManager.ToFactory());

        BlockMachine_Close_Button.AddButtonEvent(() => ChangePanel(0));

        BlockMachine_Upgrade_Button.AddButtonEvent(() =>
        {
            Managers._stageManager.SelectBlockMachine_Upgrade();
        });

        ViewChange_Button.AddButtonEvent(() =>
        {
            if (Managers._stageManager._cams[0].activeSelf) Managers._stageManager.ChangeCam(1, 1f);
            else Managers._stageManager.ChangeCam(0, 1f);
        });


        AddBlockMachine_Button.AddButtonEvent(() => Managers._stageManager.AddBlockMachine());
        AddHeroFactory_Button.AddButtonEvent(() => Managers._stageManager.AddHeroFactory());
        Stop_Hero_Button.AddButtonEvent(() =>
        {
            Managers._stageManager.MakeHero(false);
            MakeButtonOnOff();
        });

    }

    // ====================================================

    //public HeroFactory _currentHeroFactory;
    StageManager _stageManager;

    private void Start()
    {
        StartCoroutine(Cor_Update());
        _stageManager = Managers._stageManager;
    }

    IEnumerator Cor_Update()
    {
        while (true)
        {
            yield return null;


            if (_stageManager._selectHeroFactory != null)
            {

                Guage_Text.text =
                    $"{(int)((_stageManager._selectHeroFactory._currentRecipe._makingTime - _stageManager._selectHeroFactory._currentTime) / 60)} : {((int)(_stageManager._selectHeroFactory._currentRecipe._makingTime - _stageManager._selectHeroFactory._currentTime) % 60)}";
                Guage_Fill.fillAmount = (_stageManager._selectHeroFactory._currentTime / _stageManager._selectHeroFactory._maxTime);
            }

        }
    }



    public void ChangeRecipe(int _num)
    {
        if (!_stageManager._selectHeroFactory.isProduction)
        {
            _stageManager._selectHeroFactory._currentParts_Num = 0;
            _stageManager._selectHeroFactory._tempBlockList.Clear();
        }

        //_selectHeroFactory._selectMeshes
        for (int i = 0; i < _recipeListBttons.Length; i++)
        {
            _recipeListBttons[i].transform.GetChild(0).gameObject.SetActive(false);
            _recipeListBttons[i].interactable = true;
        }
        _recipeListBttons[_num].transform.GetChild(0).gameObject.SetActive(true);
        _recipeListBttons[_num].interactable = false;
        //PartsAlpha(0);



        Recipe_Name_Text.text = $"{_stageManager._selectHeroFactory._currentRecipe._recipeName}";

        Recipe_Status_Text.text = $"ATK : {_stageManager._selectHeroFactory._damage} SPD : {_stageManager._selectHeroFactory._speed} HP : {_stageManager._selectHeroFactory._maxHP} DEF : {_stageManager._selectHeroFactory._defense}";



        Recipe_Block_Count_Text.text = $"{_stageManager._selectHeroFactory._currentParts_Num}/{_stageManager._selectHeroFactory._partsCount}";

        for (int i = 0; i < 5; i++)
        {
            _bluePrint_Imgs[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < _stageManager._selectHeroFactory._partsCount; i++)
        {

            _bluePrint_Imgs[i].gameObject.SetActive(true);
            _bluePrint_Imgs[i].sprite = _stageManager._selectHeroFactory._currentRecipe._bluePrint_Sprites[i];

            _bluePrint_Imgs[i].color = _color;

        }

        if (_stageManager._selectHeroFactory.isProduction)
        {

            for (int i = 0; i < _stageManager._selectHeroFactory._currentParts_Num; i++)
            {

                _bluePrint_Imgs[i].sprite = _stageManager._selectHeroFactory._2arraySprites[i, (int)_stageManager._selectHeroFactory._tempBlockList[i]];

                _bluePrint_Imgs[i].color = Color.white;

            }
        }

        PartsAlpha(0);

        //Managers._stageManager.FactoryCheckButtons();
        MakeButtonOnOff();

        //SetColorImg(_currentHeroFactory);

        // set blueprint Img;
        //for (int i = 0; i < 5; i++)
        //{
        //    if (i < _stageManager._selectHeroFactory._partsCount)
        //    {
        //        //_bluePrint_Imgs[i].sprite = _stageManager._selectHeroFactory._currentRecipe._bluePrint_Sprites[i];

        //    }
        //    else
        //    {
        //        _bluePrint_Imgs[i].gameObject.SetActive(false);
        //    }
        //}
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
                //BlockMachine_Upgrade_Button.interactable =
                Managers._stageManager._selectBlockMachine.CheckPrice();

                break;

            case 2:
                PanelOnOff(HeroFactory_Panel, true);
                //Managers._stageManager.FactoryCheckButtons();
                MakeButtonOnOff();
                ChangeRecipe(_stageManager._selectHeroFactory._recipeNum);
                //if (_stageManager._selectHeroFactory._currentParts_Num == _stageManager._selectHeroFactory._partsCount)
                //{

                //}


                //if (_stageManager._selectHeroFactory.isProduction)
                //{
                //    MakeButtonOnOff(true);
                //}
                //else
                //{
                //    MakeButtonOnOff(false);
                //}



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
            //Debug.Log("On");
            _obj.SetActive(true);
            _obj.transform.localScale = Vector3.zero;
            _obj.transform.DOScale(Vector3.one, _time).SetEase(Ease.OutBack);

        }
        else
        {
            //Debug.Log("Off");
            //_obj.transform.DOScale(Vector3.zero, _time).SetEase(Ease.InBack).OnComplete(() => _obj.SetActive(false));
            _obj.SetActive(false);
        }


    }



    public void MakeButtonOnOff()
    {
        if (_stageManager._selectHeroFactory.isProduction)
        {
            Color_Buttons_Group.SetActive(false);
            Make_Hero_Button.gameObject.SetActive(false);
            Make_Hero_Button.interactable = false;
            Stop_Hero_Button.gameObject.SetActive(true);
            Undo_Button.gameObject.SetActive(false);
            Guage_Empty.SetActive(true);

            for (int i = 0; i < _recipeListBttons.Length; i++)
            {
                _recipeListBttons[i].interactable = false;
            }
        }
        else
        {
            if (_stageManager._selectHeroFactory._currentParts_Num > _stageManager._selectHeroFactory._partsCount - 1)
            {
                Color_Buttons_Group.SetActive(false);
                Make_Hero_Button.gameObject.SetActive(true);
                Make_Hero_Button.interactable = true;
                Stop_Hero_Button.gameObject.SetActive(false);
                Undo_Button.gameObject.SetActive(true);
                Guage_Empty.SetActive(true);

                //for (int i = 0; i < _recipeListBttons.Length; i++)
                //{
                //    _recipeListBttons[i].interactable = true;
                //}
            }
            else
            {
                Color_Buttons_Group.SetActive(true);
                Make_Hero_Button.gameObject.SetActive(false);
                Make_Hero_Button.interactable = false;
                Stop_Hero_Button.gameObject.SetActive(false);
                Undo_Button.gameObject.SetActive(true);
                Guage_Empty.SetActive(false);

                //for (int i = 0; i < _recipeListBttons.Length; i++)
                //{
                //    _recipeListBttons[i].interactable = true;
                //}
            }
            for (int i = 0; i < _recipeListBttons.Length; i++)
            {
                _recipeListBttons[i].interactable = true;
            }
            _recipeListBttons[_stageManager._selectHeroFactory._recipeNum].interactable = false;
        }


    }






    public void SetColorImg(bool isBool = true)
    {
        //float[] _values = new float[4];

        if (isBool)
        {

            _bluePrint_Imgs[_stageManager._selectHeroFactory._currentParts_Num].gameObject.SetActive(true);
            _bluePrint_Imgs[_stageManager._selectHeroFactory._currentParts_Num].transform.localScale = Vector3.zero;
            _bluePrint_Imgs[_stageManager._selectHeroFactory._currentParts_Num].transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
            _bluePrint_Imgs[_stageManager._selectHeroFactory._currentParts_Num].sprite = _stageManager._selectHeroFactory._2arraySprites[_stageManager._selectHeroFactory._currentParts_Num, (int)_stageManager._selectHeroFactory._tempBlockList[_stageManager._selectHeroFactory._currentParts_Num]];
            _bluePrint_Imgs[_stageManager._selectHeroFactory._currentParts_Num].color = Color.white;

            PartsAlpha(_stageManager._selectHeroFactory._currentParts_Num + 1);
        }
        else
        {

            for (int i = _stageManager._selectHeroFactory._partsCount - 1; i >= _stageManager._selectHeroFactory._currentParts_Num; i--)
            {

                _bluePrint_Imgs[i].sprite = _stageManager._selectHeroFactory._currentRecipe._bluePrint_Sprites[i];

                _bluePrint_Imgs[i].color = _color;

            }

            PartsAlpha(_stageManager._selectHeroFactory._currentParts_Num);
        }




        //for (int i = 0; i < _currentHeroFactory._partsCount; i++)
        //{
        //    if (i < _currentHeroFactory._currentParts_Num)
        //    {
        //        _bluePrint_Imgs[i].gameObject.SetActive(true);
        //        _bluePrint_Imgs[i].transform.localScale = Vector3.zero;
        //        _bluePrint_Imgs[i].transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        //        _bluePrint_Imgs[i].sprite = _currentHeroFactory._2arraySprites[i, (int)_currentHeroFactory._tempBlockList[i]];

        //    }
        //    else
        //    {
        //        //_bluePrint_Imgs[i].gameObject.SetActive(false);
        //        _bluePrint_Imgs[i].sprite = _currentHeroFactory._currentRecipe._bluePrint_Sprites[i];
        //    }
        //}
    }








    public void BlockMachine_SetColor(int _num)
    {
        for (int i = 0; i < 4; i++)
        {
            BlockMachine_Color_Buttons_Group.transform
                .GetChild(i).GetChild(0).gameObject.SetActive(false);
            BlockMachine_Color_Buttons_Group.transform
            .GetChild(i).GetComponent<Button>().interactable = true;
        }
        BlockMachine_Color_Buttons_Group.transform
            .GetChild(_num).GetChild(0).gameObject.SetActive(true);
        BlockMachine_Color_Buttons_Group.transform
            .GetChild(_num).GetComponent<Button>().interactable = false;
    }

    public void PartsAlpha(int _num)
    {
        //Debug.Log("Alpha Func : " + _num);

        for (int i = 0; i < 5; i++)
        {
            //DOTween.kill
            //Debug.Log(DOTween.Kill(_bluePrint_Imgs[i]));
            DOTween.Kill(_bluePrint_Imgs[i]);
        }

        if (_num < 5)
        {

            //_bluePrint_Imgs[_num].sprite = _stageManager._selectHeroFactory._currentRecipe._bluePrintFillSprites[_num];
            Color _color2 = Color.white;

            _bluePrint_Imgs[_num].color = _color2;

            //_bluePrint_Imgs[_num].DOColor(Color.white, 0.4f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }


    }






}
