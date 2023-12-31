using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AI;

public class StageManager : MonoBehaviour
{
    [FoldoutGroup("BlockMachine")] public List<BlockMachine> _blockMachineList = new List<BlockMachine>();
    [FoldoutGroup("BlockMachine")] public float _railSpeed = 0.5f;
    [FoldoutGroup("BlockMachine")] public BlockMachine _selectBlockMachine;
    [FoldoutGroup("BlockMachine")] public SkinnedMeshRenderer[] _skinnedBlock;
    [FoldoutGroup("BlockMachine")] public int _blockMachineCount;
    [FoldoutGroup("BlockMachine")] public double[] _blockMachine_Prices = new double[8];

    [FoldoutGroup("Vehicle")] public int _vehicle_Spawn_Level = 0;
    [FoldoutGroup("Vehicle")] public int _vehicle_Capacity_Level = 0;
    [FoldoutGroup("Vehicle")] public int _vehicle_Speed_Level = 0;
    [FoldoutGroup("Vehicle")] public int _rail_Speed_Level = 0;

    [FoldoutGroup("Vehicle")] public double[] _spawnLevel_Prices = new double[50];
    [FoldoutGroup("Vehicle")] public double[] _speedLevel_Prices = new double[5];
    [FoldoutGroup("Vehicle")] public double[] _capacityLevel_Prices = new double[5];
    [FoldoutGroup("Vehicle")] public double[] _railSpeedLevel_Prices = new double[5];




    [FoldoutGroup("Vehicle")] public List<Vehicle> _vehicleList = new List<Vehicle>();


    [FoldoutGroup("Stage")] public double _money = 1000d;

    public GameObject[] _cams;
    public GameObject _machineCanvas;
    Button _machineBuyButton;
    Text _machinePriceText;

    public Transform[] _PackPosGroups;



    public int _maxSpawnCount = 10;

    public BlockStorage _blockStorage;
    public VillageManager _villageManager;



    UI_GameScene _gameUi;
    Color _redColor;
    // =================================================




    private void OnEnable()
    {

        ColorUtility.TryParseHtmlString("#FF4D00", out _redColor);




        Managers._stageManager = this;

        Managers._gameUi.ChangePanel(0);


        _vehicleList = new List<Vehicle>();


        _gameUi = Managers._gameUi;


    }

    private void Start()
    {
        LoadData();
        _machineBuyButton.AddButtonEvent(() => AddBlockMachine());

    }

#if UNITY_EDITOR
    private void FixedUpdate()
    {
        //if (_blockMachineCount < _blockMachineList.Count)
        //{
        //    if (Input.GetKeyDown(KeyCode.W))
        //    {
        //        for (int i = 0; i < 4; i++)
        //        {
        //            _blockStorage._blockCountArray[i] += 100;
        //        }
        //    }
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    AddBlockMachine(false);
        //}
        //}
    }

#endif


    public void LoadData()
    {
        _blockMachineCount = ES3.Load<int>("BlockMachineCount", 0);


        _money = ES3.Load<double>("Money", 1d);


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

                case 16:
                    _skinnedBlock[3].SetBlendShapeWeight(1, 100);
                    break;
                case 18:
                    _skinnedBlock[4].SetBlendShapeWeight(0, 100);
                    break;

            }


        }

        // Vehicl
        _vehicle_Spawn_Level = ES3.Load<int>("Vehicle_Spawn_Level", 0);
        _vehicle_Speed_Level = ES3.Load<int>("Vehicle_Speed_Level", 0);
        _vehicle_Capacity_Level = ES3.Load<int>("Vehicle_Capacity_Level", 0);
        _rail_Speed_Level = ES3.Load<int>("Rail_Level", 0);

        _railSpeed = 0.5f - (0.05f * _rail_Speed_Level);

        for (int i = 0; i < _vehicle_Spawn_Level; i++)
        {
            AddVehicle();
        }
        AllVehicleSetLevel();

    }
    public void SaveData()
    {
        ES3.Save<int>("Vehicle_Spawn_Level", _vehicle_Spawn_Level);
        ES3.Save<int>("Vehicle_Speed_Level", _vehicle_Speed_Level);
        ES3.Save<int>("Vehicle_Capacity_Level", _vehicle_Capacity_Level);
        ES3.Save<double>("Money", _money);
        ES3.Save<int>("BlockMachineCount", _blockMachineCount);
        ES3.Save<int>("Rail_Level", _rail_Speed_Level);
    }



    private void Update()
    {
#if UNITY_EDITOR


        if (Input.GetKeyDown(KeyCode.R))
        {
            CalcMoney(5000d);
        }
        //else if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    PlayerPrefs.DeleteAll();
        //    UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
        //}
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
        if (_blockMachineCount == 0) TutorialManager._instance.Tutorial_Complete();
        if (_blockMachineCount == 1)
        {
            TutorialManager._instance.Tutorial_Complete();
            TutorialManager._instance.Tutorial_Img();
        }
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
            case 17:
                _skinnedBlock[3].SetBlendShapeWeight(1, 100);
                break;
            case 19:
                _skinnedBlock[4].SetBlendShapeWeight(0, 100);
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

        CheckScrollUpgradePrice();


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

    public void ChangeCam(int _num, float _time = 0.5f)
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

    public void AddVehicle()
    {
        NavMeshAgent _vehicle = Managers.Pool.Pop(_villageManager._vehicl_Pref).GetComponent<NavMeshAgent>();
        _vehicle.Warp(_blockStorage.transform.Find("Out_Pos").position);

        Vehicle _newVehicle = _vehicle.GetComponent<Vehicle>();
        _newVehicle.SetLevel(_vehicle_Speed_Level, _vehicle_Capacity_Level);

        _vehicleList.Add(_newVehicle);

        //_vehicle_Spawn_Level++;


    }

    [Button]
    public void AllVehicleSetLevel()
    {
        for (int i = 0; i < _vehicleList.Count; i++)
        {
            _vehicleList[i].SetLevel(_vehicle_Speed_Level, _vehicle_Capacity_Level);
        }
    }


    public void VehicleUpgrade(int _num)
    {
        switch (_num)
        {
            case 0:
                CalcMoney(-_spawnLevel_Prices[_vehicle_Spawn_Level]);
                AddVehicle();
                _vehicle_Spawn_Level++;
                if (TutorialManager._instance._tutorial_Level == 2)
                {
                    TutorialManager._instance.Tutorial_Complete();
                    Managers._gameUi.Scroll_Panel.SetActive(false);
                    //TutorialManager._instance.Tutorial_Img();
                }
                break;

            case 1:
                CalcMoney(-_speedLevel_Prices[_vehicle_Speed_Level]);
                _vehicle_Speed_Level++;
                AllVehicleSetLevel();
                break;

            case 2:
                CalcMoney(-_capacityLevel_Prices[_vehicle_Capacity_Level]);
                _vehicle_Capacity_Level++;
                AllVehicleSetLevel();
                break;

            case 3:
                CalcMoney(-_railSpeedLevel_Prices[_rail_Speed_Level]);
                _rail_Speed_Level++;
                _railSpeed = 0.5f - (0.05f * _rail_Speed_Level);
                break;
        }

        SaveData();
        CheckScrollUpgradePrice();

    }

    [Button]
    public void CheckScrollUpgradePrice()
    {
        if (_vehicle_Spawn_Level < _spawnLevel_Prices.Length)
        {
            _gameUi._scrollUpgContent[0].SetActive(true);
            _gameUi._scrollUpgButtons[0].transform.Find("UpgradePrice_Text").GetComponent<Text>().text = $"{_spawnLevel_Prices[_vehicle_Spawn_Level]}";

            if (_money >= _spawnLevel_Prices[_vehicle_Spawn_Level])
            {
                _gameUi._scrollUpgButtons[0].interactable = true;
                _gameUi._scrollUpgButtons[0].transform.Find("UpgradePrice_Text").GetComponent<Text>().color
                    = Color.white;
                _gameUi._scrollUpgButtons[0].transform.Find("Coin_Img").GetComponent<Image>().color
                    = Color.white;
            }
            else
            {
                _gameUi._scrollUpgButtons[0].interactable = false;
                _gameUi._scrollUpgButtons[0].transform.Find("UpgradePrice_Text").GetComponent<Text>().color
                    = _redColor; //= _gameUi._scrollUpgButtons[0].colors.disabledColor;
                //_gameUi._scrollUpgButtons[0].transform.Find("Coin_Img").GetComponent<Image>().color
                //    = _gameUi._scrollUpgButtons[0].colors.disabledColor;
            }
        }
        else
        {
            //_gameUi._scrollUpgContent[0].SetActive(false);
            _gameUi._scrollUpgButtons[0].transform.Find("UpgradePrice_Text").GetComponent<Text>().text = $"Max";
            _gameUi._scrollUpgButtons[0].interactable = false;
            //_gameUi._scrollUpgButtons[0].transform.Find("UpgradePrice_Text").GetComponent<Text>().color
            //    = _gameUi._scrollUpgButtons[0].colors.disabledColor;
            //_gameUi._scrollUpgButtons[0].transform.Find("Coin_Img").GetComponent<Image>().color
            //    = _gameUi._scrollUpgButtons[0].colors.disabledColor;
        }
        // ============

        if (_vehicle_Speed_Level < _speedLevel_Prices.Length)
        {
            _gameUi._scrollUpgContent[1].SetActive(true);
            _gameUi._scrollUpgButtons[1].transform.Find("UpgradePrice_Text").GetComponent<Text>().text = $"{_speedLevel_Prices[_vehicle_Speed_Level]}";

            if (_money >= _speedLevel_Prices[_vehicle_Speed_Level])
            {
                _gameUi._scrollUpgButtons[1].interactable = true;
                _gameUi._scrollUpgButtons[1].transform.Find("UpgradePrice_Text").GetComponent<Text>().color
                    = Color.white;
                _gameUi._scrollUpgButtons[1].transform.Find("Coin_Img").GetComponent<Image>().color
                    = Color.white;
            }
            else
            {
                _gameUi._scrollUpgButtons[1].interactable = false;
                _gameUi._scrollUpgButtons[1].transform.Find("UpgradePrice_Text").GetComponent<Text>().color
                    = _redColor;//= _gameUi._scrollUpgButtons[1].colors.disabledColor;
                //_gameUi._scrollUpgButtons[1].transform.Find("Coin_Img").GetComponent<Image>().color
                //    = _gameUi._scrollUpgButtons[1].colors.disabledColor;
            }
        }
        else
        {
            //_gameUi._scrollUpgContent[1].SetActive(false);
            _gameUi._scrollUpgButtons[1].transform.Find("UpgradePrice_Text").GetComponent<Text>().text = $"Max";
            _gameUi._scrollUpgButtons[1].interactable = false;
            //_gameUi._scrollUpgButtons[1].transform.Find("UpgradePrice_Text").GetComponent<Text>().color
            //    = _gameUi._scrollUpgButtons[1].colors.disabledColor;
            //_gameUi._scrollUpgButtons[1].transform.Find("Coin_Img").GetComponent<Image>().color
            //    = _gameUi._scrollUpgButtons[1].colors.disabledColor;
        }
        // ==
        if (_vehicle_Capacity_Level < _capacityLevel_Prices.Length)
        {
            _gameUi._scrollUpgContent[2].SetActive(true);
            _gameUi._scrollUpgButtons[2].transform.Find("UpgradePrice_Text").GetComponent<Text>().text = $"{_capacityLevel_Prices[_vehicle_Capacity_Level]}";

            if (_money >= _capacityLevel_Prices[_vehicle_Capacity_Level])
            {
                _gameUi._scrollUpgButtons[2].interactable = true;
                _gameUi._scrollUpgButtons[2].transform.Find("UpgradePrice_Text").GetComponent<Text>().color
                    = Color.white;
                _gameUi._scrollUpgButtons[2].transform.Find("Coin_Img").GetComponent<Image>().color
                    = Color.white;
            }
            else
            {
                _gameUi._scrollUpgButtons[2].interactable = false;
                _gameUi._scrollUpgButtons[2].transform.Find("UpgradePrice_Text").GetComponent<Text>().color
                   = _redColor;// = gameUi._scrollUpgButtons[2].colors.disabledColor;
                //_gameUi._scrollUpgButtons[2].transform.Find("Coin_Img").GetComponent<Image>().color
                //    = _gameUi._scrollUpgButtons[2].colors.disabledColor;
            }
        }
        else
        {
            //_gameUi._scrollUpgContent[2].SetActive(false);
            _gameUi._scrollUpgButtons[2].transform.Find("UpgradePrice_Text").GetComponent<Text>().text = $"Max";
            _gameUi._scrollUpgButtons[2].interactable = false;
            //_gameUi._scrollUpgButtons[2].transform.Find("UpgradePrice_Text").GetComponent<Text>().color
            //    = _gameUi._scrollUpgButtons[2].colors.disabledColor;
            //_gameUi._scrollUpgButtons[2].transform.Find("Coin_Img").GetComponent<Image>().color
            //    = _gameUi._scrollUpgButtons[2].colors.disabledColor;
        }
        // ==
        if (_rail_Speed_Level < _railSpeedLevel_Prices.Length)
        {
            _gameUi._scrollUpgContent[3].SetActive(true);
            _gameUi._scrollUpgButtons[3].transform.Find("UpgradePrice_Text").GetComponent<Text>().text = $"{_railSpeedLevel_Prices[_rail_Speed_Level]}";

            if (_money >= _railSpeedLevel_Prices[_rail_Speed_Level])
            {
                _gameUi._scrollUpgButtons[3].interactable = true;
                _gameUi._scrollUpgButtons[3].transform.Find("UpgradePrice_Text").GetComponent<Text>().color
                    = Color.white;
                _gameUi._scrollUpgButtons[3].transform.Find("Coin_Img").GetComponent<Image>().color
                    = Color.white;
            }
            else
            {
                _gameUi._scrollUpgButtons[3].interactable = false;
                _gameUi._scrollUpgButtons[3].transform.Find("UpgradePrice_Text").GetComponent<Text>().color
                    = _redColor; //_gameUi._scrollUpgButtons[3].colors.disabledColor;
                //_gameUi._scrollUpgButtons[3].transform.Find("Coin_Img").GetComponent<Image>().color
                //    = _gameUi._scrollUpgButtons[3].colors.disabledColor;
            }
        }
        else
        {
            //_gameUi._scrollUpgContent[3].SetActive(false);
            _gameUi._scrollUpgButtons[3].transform.Find("UpgradePrice_Text").GetComponent<Text>().text = $"Max";
            _gameUi._scrollUpgButtons[3].interactable = false;
            //_gameUi._scrollUpgButtons[3].transform.Find("UpgradePrice_Text").GetComponent<Text>().color
            //= _gameUi._scrollUpgButtons[3].colors.disabledColor;
            //_gameUi._scrollUpgButtons[3].transform.Find("Coin_Img").GetComponent<Image>().color
            //= _gameUi._scrollUpgButtons[3].colors.disabledColor;
        }


    }



}

