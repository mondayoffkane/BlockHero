using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

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
    [FoldoutGroup("Battle")] public Transform _heroSpawnPoint;
    [FoldoutGroup("Battle")] public Enemy _bossEnemy;
    [FoldoutGroup("Battle")] public Transform _enemySpawnPoint;
    [FoldoutGroup("Battle")] public int _bossLevel;

    //[FoldoutGroup("Stage")] public StageData _currentStage;
    [FoldoutGroup("Stage")] public GameObject[] _bossList;
    [FoldoutGroup("Stage")] public double _money = 1000d;

    public GameObject[] _cams;

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



    }


    public void LoadData()
    {
        //_blockFactoryCount = ES3.Load<int>("BlockFactoryCount", 0);
        //_money = ES3.Load<double>("Money", 1000d);

        CalcMoney(0);

        for (int i = 0; i < _blockMachineCount; i++)
        {
            AddBlockMachine();
        }

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToBattle();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ToFactory();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            AddBlockMachine();
        }

        if (Input.GetMouseButtonDown(0))
        {


            Ray ray;
            RaycastHit hit;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (EventSystem.current.IsPointerOverGameObject())// 
            {
                //Managers._gameUi.ChangePanel(0);
                return;
            }

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
                        Managers._gameUi.ChangePanel(2);
                        break;
                }

            }

        }

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

        Managers._gameUi.Make_Hero_Button.gameObject.SetActive(isBool);
        Managers._gameUi.Stop_Hero_Button.gameObject.SetActive(!isBool);


    }

    public void FactoryCheckButtons()
    {
        if (_selectHeroFactory != null)
        {

            Managers._gameUi.MakeButtonOnOff(
                _selectHeroFactory._currentParts_Num > _selectHeroFactory._partsCount - 1 ? true : false);



            Managers._gameUi.Recipe_Block_Count_Text.text = $"X {_selectHeroFactory._partsCount}";


        }


    }


    public void ToBattle()
    {
        Managers._gameUi.ChangePanel(3);
        //_battleCam.SetActive(true);
        ChangeCam(2, 0);
        _bossEnemy = Managers.Pool.Pop(_bossList[_bossLevel % 3]).GetComponent<Boss>();
        //_bossEnemy.SetInit(_currentStage._stageLevel);
        _bossEnemy.SetInit(_bossLevel);
        _bossEnemy.transform.position = _enemySpawnPoint.position;
        _bossEnemy.transform.rotation = Quaternion.Euler(Vector3.up * 180f);
        _bossEnemy.Fight();



        for (int i = 0; i < _spawnHeroList.Count; i++)
        {
            _spawnHeroList[i].transform.position = _heroSpawnPoint.position + new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-2f, -5f));
            _spawnHeroList[i].Fight();
            //_spawnHeroList
        }


    }

    public void ToFactory()
    {
        Managers._gameUi.ChangePanel(0);


        //_battleCam.SetActive(false);
        ChangeCam(0, 0);

        if (_bossEnemy != null) Managers.Pool.Push(_bossEnemy.GetComponent<Poolable>());


        if (_spawnHeroList.Count > 0)
        {

            int _count = _spawnHeroList.Count;
            for (int i = 0; i < _count; i++)
            {
                var _obj = _spawnHeroList[0];
                _spawnHeroList.Remove(_obj);
                Managers.Pool.Push(_obj.GetComponent<Poolable>());
            }
            //_spawnHeroList.Clear();
        }


    }


    public void Battle_Clear()
    {


        this.TaskDelay(2f, () => Managers._gameUi.ChangePanel(4));

    }
    public void Battle_Fail()
    {

        this.TaskDelay(2f, () => Managers._gameUi.ChangePanel(5));
    }


    public void AddBlockMachine()
    {
        CalcMoney(-100);

        _blockMachineList[_blockMachineCount].gameObject.SetActive(true);
        _blockMachineCount++;
        //ES3.Save<int>("BlockFactoryCount", _blockFactoryCount);

        switch (_blockMachineCount)
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

    public void AddHeroFactory()
    {
        CalcMoney(0);

        _heroFactoryList[_heroFactoryCount].gameObject.SetActive(true);
        _heroFactoryCount++;
    }


    public void CalcMoney(double _value)
    {
        _money += _value;

        Managers._gameUi.Money_Text.text = $"{_money:F0}";
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
