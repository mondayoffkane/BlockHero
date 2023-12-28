using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [FoldoutGroup("BlockMachine")] public List<BlockMachine> _blockMachineList = new List<BlockMachine>();
    [FoldoutGroup("BlockMachine")] public float _railSpeed = 0.5f;
    [FoldoutGroup("BlockMachine")] public BlockMachine _selectBlockMachine;
    [FoldoutGroup("BlockMachine")] public SkinnedMeshRenderer[] _skinnedBlock;
    [FoldoutGroup("BlockMachine")] public int _blockMachineCount;
    [FoldoutGroup("BlockMachine")] public double[] _blockMachine_Prices = new double[8];



    [FoldoutGroup("Stage")] public double _money = 1000d;

    public GameObject[] _cams;
    public GameObject _machineCanvas;
    Button _machineBuyButton;
    Text _machinePriceText;

    public Transform[] _PackPosGroups;



    public int _maxSpawnCount = 10;

    public BlockStorage _blockStorage;
    public VillageManager _villageManager;

    // =================================================




    private void OnEnable()
    {
        Managers._stageManager = this;



        Managers._gameUi.ChangePanel(0);

        LoadData();

        _machineBuyButton.AddButtonEvent(() => AddBlockMachine());



    }


    public void LoadData()
    {
        _blockMachineCount = ES3.Load<int>("BlockMachineCount", 0);


        _money = ES3.Load<double>("Money", 100d);


        CalcMoney(0);

        for (int i = 0; i < _blockMachineCount; i++)
        {

            _blockMachineList[i].gameObject.SetActive(true);

            switch (i)
            {
                case 2:
                    _skinnedBlock[0].SetBlendShapeWeight(0, 100);
                    break;

                case 4:
                    _skinnedBlock[0].SetBlendShapeWeight(1, 100);
                    break;

                case 6:
                    _skinnedBlock[1].SetBlendShapeWeight(0, 100);
                    break;

                case 8:
                    _skinnedBlock[1].SetBlendShapeWeight(1, 100);
                    break;
                case 10:
                    _skinnedBlock[2].SetBlendShapeWeight(0, 100);
                    break;

                case 12:
                    _skinnedBlock[2].SetBlendShapeWeight(1, 100);
                    break;
                case 14:
                    _skinnedBlock[3].SetBlendShapeWeight(0, 100);
                    break;


            }


        }



    }


    private void Update()
    {
#if UNITY_EDITOR

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    AddBlockMachine();
        //}

        if (Input.GetKeyDown(KeyCode.R))
        {
            CalcMoney(1000d);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerPrefs.DeleteAll();
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
        }
        // ================= Mouse ====================
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Mouse Down");
            if (!EventSystem.current.IsPointerOverGameObject())// 
            {
                //Managers._gameUi.ChangePanel(0);

                Ray ray;
                RaycastHit hit;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red, 1.5f);
                    switch (hit.collider.tag)
                    {
                        case "BlockMachine":
                            _selectBlockMachine = hit.transform.GetComponent<BlockMachine>();
                            Managers._gameUi.ChangePanel(1);
                            Managers._gameUi.BlockMachine_SetColor((int)_selectBlockMachine._spawnBlockType);
                            break;

                    }
                }

            }
        }
        //else if (Input.GetMouseButtonUp(0))
        //{
        //Debug.Log("Mouse Up");
        //}

        //#if UNITY_EDITOR
#elif !UNITY_EDITOR

        if (Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began)
            {
                if (!EventSystem.current.IsPointerOverGameObject(_touch.fingerId))
                {
                    Managers._gameUi.ChangePanel(0);
                    Ray ray;
                    RaycastHit hit;
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        Debug.DrawLine(ray.origin, hit.point, Color.red, 1.5f);
                        switch (hit.collider.tag)
                        {
                            case "BlockMachine":
                                _selectBlockMachine = hit.transform.GetComponent<BlockMachine>();
                                Managers._gameUi.ChangePanel(1);
                                Managers._gameUi.BlockMachine_SetColor((int)_selectBlockMachine._spawnBlockType);
                                break;
                            case "HeroFactory":
                                _selectHeroFactory = hit.transform.GetComponent<HeroFactory>();
                                if (_selectHeroFactory._currentRecipe == null) _selectHeroFactory.SetRecipe(0);
                                Managers._gameUi.ChangePanel(2);
                                break;
                        }
                    }
                }

            }
        }
#endif
    }





    // ==================================================================







    public void AddBlockMachine(bool isPay = true)
    {
        if (isPay)
        {
            CalcMoney(-_blockMachine_Prices[_blockMachineCount]);
        }
        _blockMachineList[_blockMachineCount].gameObject.SetActive(true);

        _blockMachineCount++;
        ES3.Save<int>("BlockMachineCount", _blockMachineCount);
        //Debug.Log("Save BlockMachine Count :" + _blockMachineCount);

        switch (_blockMachineCount)
        {
            case 3:
                _skinnedBlock[0].SetBlendShapeWeight(0, 100);
                break;

            case 5:
                _skinnedBlock[0].SetBlendShapeWeight(1, 100);
                break;

            case 7:
                _skinnedBlock[1].SetBlendShapeWeight(0, 100);
                break;

            case 9:
                _skinnedBlock[1].SetBlendShapeWeight(1, 100);
                break;
            case 11:
                _skinnedBlock[2].SetBlendShapeWeight(0, 100);
                break;

            case 13:
                _skinnedBlock[2].SetBlendShapeWeight(1, 100);
                break;
            case 15:
                _skinnedBlock[3].SetBlendShapeWeight(0, 100);
                break;


        }
        CheckMoney();
    }



    public void CalcMoney(double _value)
    {
        _money += _value;

        Managers._gameUi.Money_Text.text = $"{_money:F0}";

        ES3.Save<double>("Money", _money);
        CheckMoney();
    }

    public void CheckMoney()
    {
        // ==== Machine Check ============== 
        if (_machinePriceText == null)
        {
            _machineBuyButton = _machineCanvas.transform.GetChild(1).GetComponent<Button>();
            _machinePriceText = _machineBuyButton.transform.Find("MachinePriceText").GetComponent<Text>();
        }

        if (_blockMachineCount < _blockMachineList.Count)
        {
            _machineCanvas.SetActive(true);
            _machineCanvas.transform.position = _blockMachineList[_blockMachineCount].transform.position + Vector3.up * 0.05f;
            if (_money >= _blockMachine_Prices[_blockMachineCount])
            {
                _machineBuyButton.interactable = true;
                _machinePriceText.text = $"{_blockMachine_Prices[_blockMachineCount]}";
                _machinePriceText.color = Color.white;
            }
            else
            {
                _machineBuyButton.interactable = false;
                _machinePriceText.text = $"{_blockMachine_Prices[_blockMachineCount]}";
                _machinePriceText.color = _machineBuyButton.colors.disabledColor;
            }
        }
        else
        {
            _machineCanvas.SetActive(false);
        }

        // ==== Factory Check ============== 






    }


    public void SelectBlockMachine_Upgrade()
    {
        _selectBlockMachine.UpgradeMachine();



    }

    public void ChangeCam(int _num, float _time = 1f)
    {
        for (int i = 0; i < _cams.Length; i++)
        {
            if (_num == i) _cams[i].SetActive(true);
            else _cams[i].SetActive(false);

            Camera.main.transform.GetComponent<Cinemachine.CinemachineBrain>()
                .m_DefaultBlend.m_Time = _time;

            //_cams[i].SetActive(false);
        }



    }


}

