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
        Battle_Button,
        AddBlockMachine_Button,
        ViewChange_Button,
        BlockMachine_Close_Button,
        BlockMachine_Upgrade_Button,

    }
    enum Images
    {

    }
    enum GameObjects
    {
        BlockCount_Group,
        FactoryBase_Panel,
        BlockMachine_Panel,
        BlockMachine_Color_Buttons_Group,
        MaskImg,
    }
    // ======================================================

    public GameObject BlockMachine_Panel
        , FactoryBase_Panel
        , BlockMachine_Color_Buttons_Group
        , BlockCount_Group
        , MaskImg
        ;
    //Recipe_RawImage;


    public Button
         BlockMachine_Close_Button
        , BlockMachine_Upgrade_Button
        , ViewChange_Button

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





    // =======================================================
    private void Awake()
    {
        Bind<UnityEngine.UI.Text>(typeof(Texts));
        Bind<UnityEngine.UI.Button>(typeof(Buttons));
        Bind<UnityEngine.UI.Image>(typeof(Images));
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


        // ========= Buttons


        for (int i = 0; i < BlockMachine_Color_Buttons_Group.transform.childCount; i++)
        {
            _blockMachineColorButtons[i] = BlockMachine_Color_Buttons_Group.transform.GetChild(i).GetComponent<Button>();
        }




        BlockMachine_Close_Button = GetButton(Buttons.BlockMachine_Close_Button);
        BlockMachine_Upgrade_Button = GetButton(Buttons.BlockMachine_Upgrade_Button);
        ViewChange_Button = GetButton(Buttons.ViewChange_Button);




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
            });
        }



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
