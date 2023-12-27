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


    [FoldoutGroup("HeroFactory")] public BlockStorage _blockStorage;
    [FoldoutGroup("HeroFactory")] public List<HeroFactory> _heroFactoryList = new List<HeroFactory>();
    [FoldoutGroup("HeroFactory")] public HeroFactory _selectHeroFactory;
    [FoldoutGroup("HeroFactory")] public Transform _recipeModelGroup;
    //[FoldoutGroup("HeroFactory")] public Recipe_Model[] _recipeModels;
    //[FoldoutGroup("HeroFactory")] public Recipe_Model _selectModel;
    [FoldoutGroup("HeroFactory")] public int _heroFactoryCount;
    [FoldoutGroup("HeroFactory")] public double[] _heroFactory_Prices = new double[9];



    //[FoldoutGroup("Battle")] public GameObject _battleCam;
    [FoldoutGroup("Battle")] public List<Hero> _spawnHeroList = new List<Hero>();
    [FoldoutGroup("Battle")] public List<Hero> _heroBattleList = new List<Hero>();
    [FoldoutGroup("Battle")] public Transform _heroSpawnPoint;
    [FoldoutGroup("Battle")] public Enemy _bossEnemy;
    [FoldoutGroup("Battle")] public Transform _enemySpawnPoint;
    [FoldoutGroup("Battle")] public int _bossLevel;

    //[FoldoutGroup("Stage")] public StageData _currentStage;
    [FoldoutGroup("Stage")] public GameObject[] _bossList;
    [FoldoutGroup("Stage")] public double _money = 1000d;

    public GameObject[] _cams;
    public GameObject _machineCanvas;
    Button _machineBuyButton;
    Text _machinePriceText;

    public GameObject _factoryCanvas;
    Button _factoryBuyButton;
    Text _factoryPriceText;

    public Transform[] _PackPosGroups;

    public Text _spawnCount_Text;

    public int _maxSpawnCount = 10;
    // =================================================




    private void OnEnable()
    {
        Managers._stageManager = this;

        int _count = _recipeModelGroup.childCount;
        //_recipeModels = new Recipe_Model[_count];
        //for (int i = 0; i < _count; i++)
        //{
        //    _recipeModels[i] = _recipeModelGroup.GetChild(i).GetComponent<Recipe_Model>();
        //}

        Managers._gameUi.ChangePanel(0);

        LoadData();

        _machineBuyButton.AddButtonEvent(() => AddBlockMachine());
        _factoryBuyButton.AddButtonEvent(() => AddHeroFactory());

        _spawnCount_Text.text = $"{_spawnHeroList.Count}/{_maxSpawnCount}";
    }


    public void LoadData()
    {
        _blockMachineCount = ES3.Load<int>("BlockMachineCount", 0);
        //Debug.Log(_blockMachineCount);
        _heroFactoryCount = ES3.Load<int>("HeroFactoryCount", 0);
        _money = ES3.Load<double>("Money", 100d);
        _bossLevel = ES3.Load<int>("BossLevel", 0);

        CalcMoney(0);

        for (int i = 0; i < _blockMachineCount; i++)
        {
            //AddBlockMachine(false);
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

        for (int i = 0; i < _heroFactoryCount; i++)
        {
            //AddHeroFactory(false);

            _heroFactoryList[i].gameObject.SetActive(true);

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

    public void SelectRecipe(int _num)
    {
        //_selectModel = _recipeModels[_num];

        //Managers._gameUi.ChangeRecipe(_num, _selectModel);
        //Managers._gameUi.SetColorImg(_selectModel);
        _selectHeroFactory.SetRecipe(_num);

    }

    public void SelectModelSetColor(int _num)
    {
        //_selectModel.SetColor(_num);
        _selectHeroFactory.SetColor(_num);
    }

    public void SelectModelUndoColor()
    {
        //_selectModel.UndoColor();
        _selectHeroFactory.UndoColor();
    }

    public void SelectModelReset()
    {
        //_selectModel.Reset();
        //_selectHeroFactory.SetRecipe(_selectModel);
    }

    public void MakeHero(bool isBool)
    {
        _selectHeroFactory.MakeHeroOnOff(isBool);

        //Managers._gameUi.Make_Hero_Button.gameObject.SetActive(isBool);
        //Managers._gameUi.Stop_Hero_Button.gameObject.SetActive(!isBool);


    }

    //public void FactoryCheckButtons()
    //{
    //    if (_selectHeroFactory != null)
    //    {

    //        Managers._gameUi.MakeButtonOnOff(
    //            _selectHeroFactory._currentParts_Num > _selectHeroFactory._partsCount - 1 ? true : false);



    //        Managers._gameUi.Recipe_Block_Count_Text.text = $"X {_selectHeroFactory._partsCount}";


    //    }


    //}


    public void ToBattle()
    {
        Managers._gameUi.ChangePanel(3);
        //_battleCam.SetActive(true);
        ChangeCam(2, 0);
        _bossEnemy = Managers.Pool.Pop(_bossList[_bossLevel % 2]).GetComponent<Boss>();
        //_bossEnemy.SetInit(_currentStage._stageLevel);
        _bossEnemy.SetInit(_bossLevel);
        _bossEnemy.transform.position = _enemySpawnPoint.position;
        _bossEnemy.transform.rotation = Quaternion.Euler(Vector3.up * 180f);
        _bossEnemy.Fight();

        _heroBattleList = new List<Hero>(_spawnHeroList);
        _spawnHeroList.Clear();

        for (int i = 0; i < _heroBattleList.Count; i++)
        {
            _heroBattleList[i].transform.position = _heroSpawnPoint.position + new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-2f, -5f));
            _heroBattleList[i].Fight();
            _heroBattleList[i].transform.SetParent(null);
            //_spawnHeroList
        }


    }

    public void ToFactory()
    {
        Managers._gameUi.ChangePanel(0);


        //_battleCam.SetActive(false);
        ChangeCam(0, 0);

        if (_bossEnemy != null) Managers.Pool.Push(_bossEnemy.GetComponent<Poolable>());


        if (_heroBattleList.Count > 0)
        {

            int _count = _heroBattleList.Count;
            for (int i = 0; i < _count; i++)
            {
                var _obj = _heroBattleList[0];
                _heroBattleList.Remove(_obj);
                Managers.Pool.Push(_obj.GetComponent<Poolable>());
            }
            //_spawnHeroList.Clear();
        }


    }


    public void Battle_Clear()
    {
        _bossLevel++;
        ES3.Save<int>("BossLevel", _bossLevel);

        this.TaskDelay(2f, () => Managers._gameUi.ChangePanel(4));

    }
    public void Battle_Fail()
    {

        this.TaskDelay(2f, () => Managers._gameUi.ChangePanel(5));
    }


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

    public void AddHeroFactory(bool isPay = true)
    {

        if (isPay)
        {
            CalcMoney(-_heroFactory_Prices[_heroFactoryCount]);
        }
        _heroFactoryList[_heroFactoryCount].gameObject.SetActive(true);

        _heroFactoryCount++;
        CheckMoney();
        ES3.Save<int>("HeroFactoryCount", _heroFactoryCount);

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
        if (_factoryPriceText == null)
        {
            _factoryBuyButton = _factoryCanvas.transform.GetChild(1).GetComponent<Button>();
            _factoryPriceText = _factoryBuyButton.transform.Find("FactoryPriceText").GetComponent<Text>();
        }

        if (_heroFactoryCount < _heroFactoryList.Count)
        {
            _factoryCanvas.SetActive(true);
            _factoryCanvas.transform.position = _heroFactoryList[_heroFactoryCount].transform.position + Vector3.up * 0.05f;
            if (_money >= _heroFactory_Prices[_heroFactoryCount])
            {
                _factoryBuyButton.interactable = true;
                _factoryPriceText.text = $"{_heroFactory_Prices[_heroFactoryCount]}";
                _factoryPriceText.color = Color.white;
            }
            else
            {
                _factoryBuyButton.interactable = false;
                _factoryPriceText.text = $"{_heroFactory_Prices[_heroFactoryCount]}";
                _factoryPriceText.color = _factoryBuyButton.colors.disabledColor;
            }
        }
        else
        {
            _factoryCanvas.SetActive(false);
        }



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

    public void SetPackPos(Hero _heroPack)
    {
        int _num = (int)_heroPack._heroType;

        _heroPack.transform.SetParent(_PackPosGroups[_num]);
        _heroPack.transform.localPosition = new Vector3(0f, 1f + (_PackPosGroups[_num].childCount - 1) * 2f, 0f);

        //Debug.Log(_heroPack.transform.position);

        _spawnCount_Text.text = $"{_spawnHeroList.Count}/{_maxSpawnCount}";


    }


}

