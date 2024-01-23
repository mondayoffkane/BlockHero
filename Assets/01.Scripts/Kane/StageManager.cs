using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AI;
using MondayOFF;

public class StageManager : MonoBehaviour
{
    public int _stageLevel = 0;
    public enum VehicleType
    {
        Car,
        Train,
        Boat
    }
    public VehicleType vehicleType;

    [FoldoutGroup("BlockMachine")] public List<BlockMachine> _blockMachineList = new List<BlockMachine>();
    [FoldoutGroup("BlockMachine")] public float _railSpeed = 0.5f;
    [FoldoutGroup("BlockMachine")] public BlockMachine _selectBlockMachine;
    [FoldoutGroup("BlockMachine")] public SkinnedMeshRenderer[] _skinnedBlock;
    [FoldoutGroup("BlockMachine")] public int _blockMachineCount;
    [FoldoutGroup("BlockMachine")] public double[] _blockMachine_Prices = new double[8];

    [FoldoutGroup("Vehicle")] public GameObject _vehicle_Head_Pref;
    [FoldoutGroup("Vehicle")] public GameObject _vehicle_Pref;
    [FoldoutGroup("Vehicle")] public int _vehicle_Spawn_Level = 0;
    [FoldoutGroup("Vehicle")] public int _vehicle_Capacity_Level = 0;
    [FoldoutGroup("Vehicle")] public int _vehicle_Speed_Level = 0;
    [FoldoutGroup("Vehicle")] public int _rail_Speed_Level = 0;

    [FoldoutGroup("Vehicle")] public double[] _spawnLevel_Prices = new double[50];
    [FoldoutGroup("Vehicle")] public double[] _speedLevel_Prices = new double[5];
    [FoldoutGroup("Vehicle")] public double[] _capacityLevel_Prices = new double[5];
    [FoldoutGroup("Vehicle")] public double[] _railSpeedLevel_Prices = new double[5];
    [FoldoutGroup("Vehicle")] public List<Vehicle> _vehicleList = new List<Vehicle>();
    [FoldoutGroup("Vehicle")][ShowInInspector] public Queue<Vehicle> _vehicleQueue = new Queue<Vehicle>();

    //[FoldoutGroup("Village")] public double _money = 1000d;
    [FoldoutGroup("Village")] public List<Building> buildingList = new List<Building>();
    [FoldoutGroup("Village")] public int buldingCompleteCount = 0;
    [FoldoutGroup("Village")] public bool _villageComplete = false;
    [FoldoutGroup("Village")] public Transform[] AreaGroups;

    [FoldoutGroup("UI")] public Sprite[] scrollUpImg;
    [FoldoutGroup("UI")] public string[] scrollUpName;
    [FoldoutGroup("UI")] public string[] scrollUpExplain;

    [FoldoutGroup("Order")] public Sprite[] peopleSprites;
    [FoldoutGroup("Order")] public Sprite[] blockSprites;


    public struct OrderStruct
    {
        public Sprite personSprite;
        public Sprite blockSprite;
        public int orderCount;
        public int[] blockCount;
        public int rewardCount;
    }
    public OrderStruct[] orderStructs;



    public GameObject[] _cams;
    public GameObject _machineCanvas;
    Button _machineBuyButton;
    Text _machinePriceText;


    public BlockStorage _blockStorage;
    Transform _vehicleGroup;

    UI_GameScene _gameUi;
    Color _redColor;

    public float _vehicleTerm = 1f;

    //public bool isSet = false;
    public int _playTime = 0;
    //[SerializeField] bool isHeadReady = true;
    [SerializeField] int queueCount;
    // =================================================




    private void Awake()
    {
        ColorUtility.TryParseHtmlString("#FF4D00", out _redColor);
        //Managers._stageManager = this;
        //Managers._gameUi.ChangePanel(0);
        _vehicleList = new List<Vehicle>();
        _gameUi = Managers._gameUi;
        _vehicleGroup = transform.Find("5.Vehicle_Group");
        int _count = transform.Find("6.MapAreaGroup").childCount;
        AreaGroups = new Transform[_count];
        for (int i = 0; i < _count; i++)
        {
            AreaGroups[i] = transform.Find("6.MapAreaGroup").GetChild(i);
        }


    }

    private void OnEnable()
    {
        LoadData();
        CheckMoney();




        _machineBuyButton.AddButtonEvent(() => AddBlockMachine());
        //SetStage();

        switch (vehicleType)
        {
            case VehicleType.Car:
                StartCoroutine(Cor_Update());
                break;

            case VehicleType.Train:
                StartCoroutine(Cor_Upgrade_Train());
                break;
        }

    }

    IEnumerator Cor_Update()
    {
        while (true)
        {
            yield return new WaitForSeconds(_vehicleTerm);

            if (_vehicleQueue.Count > 0 && (_blockStorage._blockCountArray[0] > 0 || _blockStorage._blockCountArray[1] > 0 || _blockStorage._blockCountArray[2] > 0 || _blockStorage._blockCountArray[3] > 0))
            {

                Vehicle _vehicle = _vehicleQueue.Dequeue();
                _vehicle._state = Vehicle.State.Move;
                _vehicle._target = _blockStorage.transform.Find("Out_Pos");
                _vehicle.GetComponent<NavMeshAgent>()
                    .Warp(_blockStorage.transform.Find("Out_Pos").position);

            }
        }
    }


    IEnumerator Cor_Upgrade_Train()
    {
        Debug.Log("Cor Update Train");

        yield return new WaitForSeconds(_vehicleTerm);
        queueCount = _vehicleList.Count;
        for (int i = 0; i < _vehicleList.Count; i++)
        {
            Vehicle _vehicle = _vehicleList[i];
            if (_vehicle._state == Vehicle.State.Wait || _vehicle._state == Vehicle.State.Sleep)
                _vehicle._state = Vehicle.State.Move;
            _vehicle._target = _blockStorage.transform.Find("Out_Pos");
            _vehicle.GetComponent<NavMeshAgent>()
                .Warp(_blockStorage.transform.Find("Out_Pos").position);
            yield return new WaitForSeconds(_vehicleTerm);  // new WaitForSeconds(1f);
        }

    }

    private void Start()
    {
        for (int i = 0; i < buildingList.Count; i++)
        {
            if (buildingList[i].isBuildComplete == false)
            {
                buildingList[i].SetCanvas();
                break;
            }
        }
    }


    public void LoadData()
    {
        _blockMachineCount = ES3.Load<int>($"Stage_{_stageLevel}_BlockMachineCount", 0);

        buldingCompleteCount = ES3.Load<int>($"Stage_{_stageLevel}_buildingCompleteCount", 0);




        _playTime = ES3.Load<int>("PlayTime", _playTime);


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

        // Vehicle
        _vehicle_Spawn_Level = ES3.Load<int>($"Stage_{_stageLevel}_Vehicle_Spawn_Level", 0);
        _vehicle_Speed_Level = ES3.Load<int>($"Stage_{_stageLevel}_Vehicle_Speed_Level", 0);
        _vehicle_Capacity_Level = ES3.Load<int>($"Stage_{_stageLevel}_Vehicle_Capacity_Level", 0);
        _rail_Speed_Level = ES3.Load<int>($"Stage_{_stageLevel}_Rail_Level", 0);

        _railSpeed = 0.5f - (0.05f * _rail_Speed_Level);


        switch (vehicleType)
        {
            case VehicleType.Train:
                NavMeshAgent _vehicle = Managers.Pool.Pop(_vehicle_Head_Pref, _vehicleGroup).GetComponent<NavMeshAgent>();
                _vehicle.GetComponent<Vehicle>()._state = Vehicle.State.Wait;
                _vehicle.Warp(_blockStorage.transform.Find("Out_Pos").position);
                _vehicle.destination = _blockStorage.transform.Find("Out_Pos").position;

                Vehicle _newVehicle = _vehicle.GetComponent<Vehicle>();
                _newVehicle.SetLevel(_vehicle_Speed_Level, _vehicle_Capacity_Level);
                _vehicleList.Add(_newVehicle);

                break;
        }


        for (int i = 0; i < _vehicle_Spawn_Level; i++)
        {
            AddVehicle();
        }
        AllVehicleSetLevel();

    }
    public void SaveData()
    {
        ES3.Save<int>($"Stage_{_stageLevel}_Vehicle_Spawn_Level", _vehicle_Spawn_Level);
        ES3.Save<int>($"Stage_{_stageLevel}_Vehicle_Speed_Level", _vehicle_Speed_Level);
        ES3.Save<int>($"Stage_{_stageLevel}_Vehicle_Capacity_Level", _vehicle_Capacity_Level);

        ES3.Save<int>($"Stage_{_stageLevel}_BlockMachineCount", _blockMachineCount);
        ES3.Save<int>($"Stage_{_stageLevel}_Rail_Level", _rail_Speed_Level);
    }

    // ====== Block Machine ============================================================

    public void AddBlockMachine(bool isPay = true)
    {
        if (isPay)
        {
            Managers.Game.CalcMoney(-_blockMachine_Prices[_blockMachineCount]);
        }
        _blockMachineList[_blockMachineCount].gameObject.SetActive(true);
        if (_stageLevel == 0 && _blockMachineCount == 0) TutorialManager._instance.Tutorial_Complete();
        if (_stageLevel == 0 && _blockMachineCount == 1)
        {
            TutorialManager._instance.Tutorial_Complete();
            TutorialManager._instance.Tutorial_Img();
        }
        _blockMachineCount++;
        ES3.Save<int>($"Stage_{_stageLevel}_BlockMachineCount", _blockMachineCount);
        EventTracker.LogCustomEvent("BlockMachine"
, new Dictionary<string, string> { { "BlockMachine", $"StageNum-{_stageLevel}_AddMachine-{_blockMachineCount}" } });


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
    public void SelectBlockMachine_Upgrade()
    {
        _selectBlockMachine.UpgradeMachine();

    }

    // ====== Vehicle =====================

    public void AddVehicle()
    {
        NavMeshAgent _vehicle = Managers.Pool.Pop(_vehicle_Pref, _vehicleGroup).GetComponent<NavMeshAgent>();
        _vehicle.GetComponent<Vehicle>()._state = Vehicle.State.Wait;
        _vehicle.Warp(_blockStorage.transform.Find("Out_Pos").position);
        _vehicle.destination = _blockStorage.transform.Find("Out_Pos").position;

        Vehicle _newVehicle = _vehicle.GetComponent<Vehicle>();
        _newVehicle.SetLevel(_vehicle_Speed_Level, _vehicle_Capacity_Level);
        _vehicleList.Add(_newVehicle);


        switch (vehicleType)
        {
            case VehicleType.Car:
                _vehicleQueue.Enqueue(_newVehicle);
                //_vehicleQueue.Enqueue(_newVehicle);

                break;

            case VehicleType.Train:
                //_newVehicle = _vehicleList[_vehicleList.Count - 1];
                //_newVehicle.GetComponent<NavMeshAgent>().Warp(_vehicleList[_vehicleList.Count - 1].transform.position);
                //_newVehicle.GetComponent<NavMeshAgent>().destination = _vehicleList[_vehicleList.Count - 1].GetComponent<NavMeshAgent>().destination;
                //_newVehicle.SetDest(_vehicleList[_vehicleList.Count - 1]._target);
                break;
        }



    }
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
                Managers.Game.CalcMoney(-_spawnLevel_Prices[_vehicle_Spawn_Level]);
                AddVehicle();
                _vehicle_Spawn_Level++;
                if (TutorialManager._instance._tutorial_Level == 2)
                {
                    TutorialManager._instance.Tutorial_Complete();
                    Managers._gameUi.Scroll_Panel.SetActive(false);
                    //TutorialManager._instance.Tutorial_Img();
                }

                EventTracker.LogCustomEvent("Vehicle"
, new Dictionary<string, string> { { "Vehicle", $"AddVehicle-{_vehicle_Spawn_Level}" } });

                break;

            case 1:
                Managers.Game.CalcMoney(-_speedLevel_Prices[_vehicle_Speed_Level]);
                _vehicle_Speed_Level++;
                AllVehicleSetLevel();

                EventTracker.LogCustomEvent("Vehicle"
, new Dictionary<string, string> { { "Vehicle", $"SpeedUp-{_vehicle_Speed_Level}" } });

                break;

            case 2:
                Managers.Game.CalcMoney(-_capacityLevel_Prices[_vehicle_Capacity_Level]);
                _vehicle_Capacity_Level++;
                AllVehicleSetLevel();

                EventTracker.LogCustomEvent("Vehicle"
, new Dictionary<string, string> { { "Vehicle", $"CapacityUp-{_vehicle_Capacity_Level}" } });

                break;

            case 3:
                Managers.Game.CalcMoney(-_railSpeedLevel_Prices[_rail_Speed_Level]);
                _rail_Speed_Level++;
                _railSpeed = 0.5f - (0.025f * _rail_Speed_Level);

                EventTracker.LogCustomEvent("BlockMachine"
 , new Dictionary<string, string> { { "BlockMachine", $"RailUpgrade-{_rail_Speed_Level}" } });
                break;
        }

        SaveData();
        CheckScrollUpgradePrice();

    }

    // ======  Village =====================

    public void BuildComplete()
    {
        buldingCompleteCount++;
        ES3.Save<int>($"Stage_{_stageLevel}_buildingCompleteCount", buldingCompleteCount);
        if (buldingCompleteCount < buildingList.Count)
            buildingList[buldingCompleteCount].SetCanvas();

        if (buldingCompleteCount >= buildingList.Count)
        {
            _villageComplete = true;
            ES3.Save<bool>($"VillageComplete_{_stageLevel}", _villageComplete);

            Managers.Game.ClearStage();

        }

    }

    public Transform FindBuilding()
    {
        for (int i = 0; i < buildingList.Count; i++)
        {
            if (buildingList[i]._currentCount < buildingList[i]._maxCount)
            {
                return buildingList[i].transform;
            }
        }
        return buildingList[Random.Range(0, buildingList.Count)].transform;

    }

    public Transform ReFindBuilding()
    {
        for (int i = 0; i < buildingList.Count; i++)
        {
            if (Managers.Game.currentStageManager._blockStorage._blockCountArray[(int)buildingList[i]._blockType] > 0)
            {
                return buildingList[i].transform;
            }
        }

        return null;
    }



    // ======== UI =========================

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
            if (Managers.Game.money >= _blockMachine_Prices[_blockMachineCount])
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







    }


    public void ChangeUI()
    {
        CheckScrollUpgradePrice();
        CheckOrderPanel();
    }

    public void CheckOrderPanel()
    {

    }


    [Button]
    public void CheckScrollUpgradePrice()
    {

        for (int i = 0; i < 4; i++)
        {
            _gameUi._scrollUpgContent[i].transform.Find("Image").GetComponent<Image>().sprite =
                    scrollUpImg[i];
            _gameUi._scrollUpgContent[i].transform.Find("Name_Text").GetComponent<Text>().text = scrollUpName[i];
            _gameUi._scrollUpgContent[i].transform.Find("Explain_Text").GetComponent<Text>().text = scrollUpExplain[i];

        }



        if (_vehicle_Spawn_Level < _spawnLevel_Prices.Length)
        {
            _gameUi._scrollUpgContent[0].SetActive(true);
            _gameUi._scrollUpgButtons[0].transform.Find("UpgradePrice_Text").GetComponent<Text>().text = $"{_spawnLevel_Prices[_vehicle_Spawn_Level]}";

            if (Managers.Game.money >= _spawnLevel_Prices[_vehicle_Spawn_Level])
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

            }
        }
        else
        {

            _gameUi._scrollUpgButtons[0].transform.Find("UpgradePrice_Text").GetComponent<Text>().text = $"Max";
            _gameUi._scrollUpgButtons[0].transform.Find("UpgradePrice_Text").GetComponent<Text>().color
                    = Color.white;
            _gameUi._scrollUpgButtons[0].interactable = false;

        }
        // ============

        if (_vehicle_Speed_Level < _speedLevel_Prices.Length)
        {
            _gameUi._scrollUpgContent[1].SetActive(true);
            _gameUi._scrollUpgButtons[1].transform.Find("UpgradePrice_Text").GetComponent<Text>().text = $"{_speedLevel_Prices[_vehicle_Speed_Level]}";

            if (Managers.Game.money >= _speedLevel_Prices[_vehicle_Speed_Level])
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

            }
        }
        else
        {
            //_gameUi._scrollUpgContent[1].SetActive(false);
            _gameUi._scrollUpgButtons[1].transform.Find("UpgradePrice_Text").GetComponent<Text>().text = $"Max";
            _gameUi._scrollUpgButtons[1].transform.Find("UpgradePrice_Text").GetComponent<Text>().color
                   = Color.white;
            _gameUi._scrollUpgButtons[1].interactable = false;

        }
        // ==
        if (_vehicle_Capacity_Level < _capacityLevel_Prices.Length)
        {
            _gameUi._scrollUpgContent[2].SetActive(true);
            _gameUi._scrollUpgButtons[2].transform.Find("UpgradePrice_Text").GetComponent<Text>().text = $"{_capacityLevel_Prices[_vehicle_Capacity_Level]}";

            if (Managers.Game.money >= _capacityLevel_Prices[_vehicle_Capacity_Level])
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

            }
        }
        else
        {

            _gameUi._scrollUpgButtons[2].transform.Find("UpgradePrice_Text").GetComponent<Text>().text = $"Max";
            _gameUi._scrollUpgButtons[2].transform.Find("UpgradePrice_Text").GetComponent<Text>().color
                   = Color.white;
            _gameUi._scrollUpgButtons[2].interactable = false;

        }
        // ==
        if (_rail_Speed_Level < _railSpeedLevel_Prices.Length)
        {
            _gameUi._scrollUpgContent[3].SetActive(true);
            _gameUi._scrollUpgButtons[3].transform.Find("UpgradePrice_Text").GetComponent<Text>().text = $"{_railSpeedLevel_Prices[_rail_Speed_Level]}";

            if (Managers.Game.money >= _railSpeedLevel_Prices[_rail_Speed_Level])
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

            }
        }
        else
        {

            _gameUi._scrollUpgButtons[3].transform.Find("UpgradePrice_Text").GetComponent<Text>().text = $"Max";
            _gameUi._scrollUpgButtons[3].transform.Find("UpgradePrice_Text").GetComponent<Text>().color
                   = Color.white;
            _gameUi._scrollUpgButtons[3].interactable = false;

        }


    }

    [Button]
    public void SetStage()
    {

        _vehicleQueue.Clear();


        for (int i = 0; i < _vehicleList.Count; i++)
        {
            _vehicleList[i].SetReturn();

            _vehicleList[i]._target = _blockStorage.transform.Find("Out_Pos");
            _vehicleList[i].GetComponent<NavMeshAgent>()
                .Warp(_blockStorage.transform.Find("Out_Pos").position);
            //_vehicleList[i].SetDest(_blockStorage.transform.Find("Out_Pos"));
            _vehicleList[i].GetComponent<NavMeshAgent>().destination = _blockStorage.transform.Find("Out_Pos").position;

            switch (vehicleType)
            {
                case VehicleType.Car:
                    _vehicleQueue.Enqueue(_vehicleList[i]);

                    break;
            }


        }


        // add UI Change 
        _blockStorage.UpdateBlockCount();




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

    public void VehicleEnqueue(Vehicle _vehicle)
    {
        _vehicleQueue.Enqueue(_vehicle);
        switch (vehicleType)
        {
            case VehicleType.Train:
                if (_vehicleQueue.Count >= queueCount)
                {
                    StartCoroutine(Cor_Upgrade_Train());
                    _vehicleQueue.Clear();
                }

                break;
        }
    }

    public void Order()
    {
        for (int i = 0; i < 3; i++)
        {
            int num = i;
            orderStructs[num] = CreateOrder();
            Managers._gameUi.SetOrderPanel(num, orderStructs[num]);
        }
    }

    public OrderStruct CreateOrder()
    {
        OrderStruct newOrder = new OrderStruct();
        newOrder.personSprite = peopleSprites[Random.Range(0, peopleSprites.Length)];
        newOrder.blockSprite = blockSprites[Random.Range(0, blockSprites.Length)];
        newOrder.orderCount = Random.Range(1, 3);
        for (int i = 0; i < newOrder.orderCount; i++)
        {
            newOrder.blockCount[i] = Random.Range(0, 11) * 10;
        }
        newOrder.rewardCount = 100;

        return newOrder;
    }



}

