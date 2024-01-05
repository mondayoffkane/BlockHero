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
    }
    enum Buttons
    {
        Scroll_Button,
        Speed_RV_Button,
        View_Button,
        Cpi_Rail_Button,
        ViewChange_Button,
        AddCar_Button,
        BlockMachine_Close_Button,
        BlockMachine_Upgrade_Button,
        Scroll_Close_Button,
    }
    enum GameObjects
    {
        BlockCount_Group,
        FactoryBase_Panel,
        BlockMachine_Panel,
        BlockMachine_Color_Buttons_Group,
        MaskImg,
        Scroll_Panel,
        Scroll_Content,
    }

    DOTween _camTween;
    // ======================================================

    public GameObject BlockMachine_Panel
        , FactoryBase_Panel
        , BlockMachine_Color_Buttons_Group
        , BlockCount_Group
        , MaskImg
        , Scroll_Panel
        , Scroll_Content
        ;
    //Recipe_RawImage;


    public Button
         BlockMachine_Close_Button
        , BlockMachine_Upgrade_Button
        , ViewChange_Button
        , AddCar_Button
        , Scroll_Close_Button
        , Scroll_Button
        , Speed_RV_Button
        , View_Button
        , Cpi_Rail_Button
        ;

    public Button[] _blockMachineColorButtons = new Button[4];



    public Text
        Money_Text
        , BlockMachine_Upgrade_Price_Text,
        BlockMachine_Status_Text,
        BlockMachine_UpgradeValue_Text
                ;

    public Text[] _blockCountTexts = new Text[4];


    //public Image //BluePrint_Img


    //public Image[] _colorButtonImgs;


    public Color _color;

    public GameObject[] _scrollUpgContent = new GameObject[4];
    public Button[] _scrollUpgButtons = new Button[4];




    // =======================================================
    private void Awake()
    {
        Bind<UnityEngine.UI.Text>(typeof(Texts));
        Bind<UnityEngine.UI.Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        ColorUtility.TryParseHtmlString("#4F9BCD", out _color);


        Managers._gameUi = this;


        base.Init();

        // ========= GameObjects
        BlockMachine_Panel = GetObject(GameObjects.BlockMachine_Panel);

        FactoryBase_Panel = GetObject(GameObjects.FactoryBase_Panel);

        BlockMachine_Color_Buttons_Group = GetObject(GameObjects.BlockMachine_Color_Buttons_Group);

        BlockCount_Group = GetObject(GameObjects.BlockCount_Group);
        MaskImg = GetObject(GameObjects.MaskImg);
        Scroll_Panel = GetObject(GameObjects.Scroll_Panel);
        Scroll_Content = GetObject(GameObjects.Scroll_Content);

        for (int i = 0; i < Scroll_Content.transform.childCount; i++)
        {
            _scrollUpgContent[i] = Scroll_Content.transform.GetChild(i).gameObject;
        }

        // ========= Buttons


        for (int i = 0; i < BlockMachine_Color_Buttons_Group.transform.childCount; i++)
        {
            _blockMachineColorButtons[i] = BlockMachine_Color_Buttons_Group.transform.GetChild(i).GetComponent<Button>();
        }



        BlockMachine_Close_Button = GetButton(Buttons.BlockMachine_Close_Button);
        BlockMachine_Upgrade_Button = GetButton(Buttons.BlockMachine_Upgrade_Button);
        ViewChange_Button = GetButton(Buttons.ViewChange_Button);
        AddCar_Button = GetButton(Buttons.AddCar_Button);

        Scroll_Close_Button = GetButton(Buttons.Scroll_Close_Button);
        Scroll_Button = GetButton(Buttons.Scroll_Button);
        Speed_RV_Button = GetButton(Buttons.Speed_RV_Button);
        Cpi_Rail_Button = GetButton(Buttons.Cpi_Rail_Button);
        for (int i = 0; i < Scroll_Content.transform.childCount; i++)
        {
            _scrollUpgButtons[i] = _scrollUpgContent[i].transform.Find("Upgrade_Button").GetComponent<Button>();
        }

        View_Button = GetButton(Buttons.View_Button);


        // ========= Img






        // ========= Text

        //Recipe_Status_Text = GetText(Texts.Recipe_Status_Text);

        Money_Text = GetText(Texts.Money_Text);

        BlockMachine_Upgrade_Price_Text = GetText(Texts.BlockMachine_Upgrade_Price_Text);
        BlockMachine_Status_Text = GetText(Texts.BlockMachine_Status_Text);
        BlockMachine_UpgradeValue_Text = GetText(Texts.BlockMachine_UpgradeValue_Text);



        for (int i = 0; i < 4; i++)
        {
            _blockCountTexts[i] = BlockCount_Group.transform.GetChild(i).GetChild(0).GetComponent<Text>();
        }


        // ================ Add Button Listner========================================


        for (int i = 0; i < _blockMachineColorButtons.Length; i++)
        {
            int _num = i;
            _blockMachineColorButtons[i].AddButtonEvent(() =>
            {
                //Managers._stageManager.SelectModelSetColor(_num);
                Managers._stageManager._selectBlockMachine.SetBlockType(_num);
                BlockMachine_SetColor(_num);

                if (TutorialManager._instance._tutorial_Level == 6)
                {
                    TutorialManager._instance.Tutorial_Complete();
                    BlockMachine_Panel.SetActive(false);
                    _stageManager._cams[0].transform.position = TutorialManager._instance._cams[6].transform.position;
                }

            });
        }



        BlockMachine_Close_Button.AddButtonEvent(() => ChangePanel(0));

        BlockMachine_Upgrade_Button.AddButtonEvent(() =>
        {
            Managers._stageManager.SelectBlockMachine_Upgrade();
        });

        ViewChange_Button.AddButtonEvent(() =>
        {
            if (Managers._stageManager._cams[0].activeSelf) Managers._stageManager.ChangeCam(1);
            else Managers._stageManager.ChangeCam(0);
        });

        AddCar_Button.AddButtonEvent(() =>
        {
            Managers._stageManager.AddVehicle();
        });

        Scroll_Close_Button.AddButtonEvent(() => Scroll_Panel.SetActive(false));


        Scroll_Button.AddButtonEvent(() =>
        {
            if ((Scroll_Panel.activeSelf == false)) ChangePanel(2);
            else PanelOnOff(Scroll_Panel, false);

            if (TutorialManager._instance._tutorial_Level == 1)
            {
                TutorialManager._instance.Tutorial_Complete();

                _stageManager._cams[0].transform.position = TutorialManager._instance._cams[2].transform.position;
                TutorialManager._instance.Tutorial_Img();

            }
        });
        Speed_RV_Button.AddButtonEvent(() => { /* add func */});


        for (int i = 0; i < _scrollUpgButtons.Length; i++)
        {
            int _num = i;
            _scrollUpgButtons[i].AddButtonEvent(() => Managers._stageManager.VehicleUpgrade(_num));
        }

        Cpi_Rail_Button.AddButtonEvent(() =>
        {
            if (_stageManager._rail_Speed_Level < 10)
            {
                _stageManager._rail_Speed_Level++;
                _stageManager._railSpeed = 0.5f - (0.05f * _stageManager._rail_Speed_Level);
            }
        });

        View_Button.AddButtonEvent(() =>
        {
            Vector3 _pos = _stageManager._cams[0].transform.position;
            //_camTween = DOTween.Sequence()

            if (_stageManager._cams[0].transform.position.z < -22)
            {
                //_pos.z = -7f;
                //_stageManager._cams[0].transform.position = _pos;

                _stageManager._cams[0].transform.DOMoveZ(-7f, 0.5f).SetEase(Ease.Linear);
            }
            else
            {
                //_pos.z = -40f;
                //_stageManager._cams[0].transform.position = _pos;
                _stageManager._cams[0].transform.DOMoveZ(-43f, 0.5f).SetEase(Ease.Linear);
            }

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




        }
    }







    public void ChangePanel(int _num)
    {
        //Debug.Log("Panel OnOff");
        PanelOnOff(FactoryBase_Panel, false);
        PanelOnOff(BlockMachine_Panel, false);



        switch (_num)
        {
            case 0:

                PanelOnOff(FactoryBase_Panel, true, 0);
                break;

            case 1:
                PanelOnOff(BlockMachine_Panel, true);
                //BlockMachine_Upgrade_Button.interactable =
                Managers._stageManager._selectBlockMachine.CheckPrice();

                if (TutorialManager._instance._tutorial_Level == 5)
                {
                    TutorialManager._instance.Tutorial_Complete();
                    TutorialManager._instance.Tutorial_Img();
                }

                break;

            case 2:
                PanelOnOff(Scroll_Panel, true);
                _stageManager.CheckScrollUpgradePrice();
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









}
