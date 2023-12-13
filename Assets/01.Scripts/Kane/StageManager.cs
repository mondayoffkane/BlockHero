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
    [FoldoutGroup("BlockMachine")] public int _blockMachineCount;
    [FoldoutGroup("BlockMachine")] public BlockMachine _selectBlockMachine;
    [FoldoutGroup("BlockMachine")] public SkinnedMeshRenderer[] _skinnedBlock;



    [FoldoutGroup("HeroFactory")] public HeroFactory _heroFactory;
    [FoldoutGroup("HeroFactory")] public Transform _recipeModelGroup;
    [FoldoutGroup("HeroFactory")] public Recipe_Model[] _recipeModels;
    [FoldoutGroup("HeroFactory")] public Recipe_Model _selectModel;


    [FoldoutGroup("Battle")]
    [FoldoutGroup("Battle")] public GameObject _battleCam;
    [FoldoutGroup("Battle")] public List<Hero> _spawnHeroList = new List<Hero>();
    [FoldoutGroup("Battle")] public Transform _heroSpawnPoint;
    [FoldoutGroup("Battle")] public Enemy _bossEnemy;
    [FoldoutGroup("Battle")] public Transform _enemySpawnPoint;

    [FoldoutGroup("Stage")] public StageData _currentStage;
    [FoldoutGroup("Stage")] public double _money = 1000d;

    // =================================================




    private void OnEnable()
    {
        Managers._stageManager = this;

        int _count = _recipeModelGroup.childCount;
        _recipeModels = new Recipe_Model[_count];
        for (int i = 0; i < _count; i++)
        {
            _recipeModels[i] = _recipeModelGroup.GetChild(i).GetComponent<Recipe_Model>();
        }

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
            AddBlockFactory();
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
            AddBlockFactory();
        }

        if (Input.GetMouseButtonDown(0))
        {


            Ray ray;
            RaycastHit hit;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (EventSystem.current.IsPointerOverGameObject())// 
            {
                return;
            }

            if (Physics.Raycast(ray, out hit, 1000))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red, 1.5f);


                switch (hit.collider.tag)
                {
                    case "BlockMachine":

                        _selectBlockMachine = hit.transform.GetComponent<BlockMachine>();
                        Managers._gameUi.ChangePanel(1);
                        break;

                    case "HeroFactory":
                        Managers._gameUi.ChangePanel(2);
                        break;
                }

            }

        }





    }





    // ==================================================================

    public void SelectRecipe(int _num)
    {
        _selectModel = _recipeModels[_num];

        Managers._gameUi.ChangeRecipe(_num, _selectModel);
        Managers._gameUi.SetColorImg(_selectModel);

    }

    public void SelectModelSetColor(int _num)
    {
        _selectModel.SetColor(_num);
    }

    public void SelectModelUndoColor()
    {
        _selectModel.UndoColor();
    }

    public void SelectModelReset()
    {
        _selectModel.Reset();
    }

    public void MakeHero()
    {

        Hero _newHero = Managers.Pool.Pop(Resources.Load<GameObject>($"Hero/{_selectModel._heroType.ToString()}_Pref")).GetComponent<Hero>();
        _newHero.SetInit(_selectModel);

        _newHero.transform.position = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-8f, -15f));
        _selectModel.Reset();

        _spawnHeroList.Add(_newHero);


    }

    public void FactoryCheckButtons()
    {
        if (_selectModel != null)
        {

            Managers._gameUi.MakeButtonOnOff(
                _selectModel._currentParts_Num > _selectModel._partsCount - 1 ? true : false);


            Managers._gameUi.Recipe_Block_Count_Text.text = $"X {_selectModel._partsCount}";


        }


    }


    public void ToBattle()
    {
        Managers._gameUi.ChangePanel(3);
        _battleCam.SetActive(true);

        _bossEnemy = Managers.Pool.Pop(_currentStage._bossPref).GetComponent<Boss>();
        _bossEnemy.SetInit(_currentStage._stageLevel);
        _bossEnemy.transform.position = _enemySpawnPoint.position;
        _bossEnemy.transform.rotation = Quaternion.Euler(Vector3.up * 180f);
        _bossEnemy.Fight();



        for (int i = 0; i < _spawnHeroList.Count; i++)
        {
            _spawnHeroList[i].transform.position = _heroSpawnPoint.position + new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-8f, -15f));
            _spawnHeroList[i].Fight();
            //_spawnHeroList
        }


    }

    public void ToFactory()
    {
        Managers._gameUi.ChangePanel(0);


        _battleCam.SetActive(false);

        //if (_spawnEnemyList.Count > 0)
        //    for (int i = 0; i < _spawnEnemyList.Count; i++)
        //    {
        //        var _obj = _spawnEnemyList[0];
        //        _spawnEnemyList.Remove(_obj);
        //        Managers.Pool.Push(_obj.GetComponent<Poolable>());
        //    }
        //_spawnEnemyList.Clear();

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


    public void AddBlockFactory()
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
        }


    }

    public void CalcMoney(double _value)
    {
        _money += _value;

        Managers._gameUi.Money_Text.text = $"{_money:F0}";
    }



}
