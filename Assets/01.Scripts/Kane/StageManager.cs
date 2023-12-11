using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class StageManager : MonoBehaviour
{
    [FoldoutGroup("BlockFactory")] public List<BlockFactory> _blockfactoryList = new List<BlockFactory>();


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
    [FoldoutGroup("Stage")] public float _money;

    // =================================================
    private void Awake()
    {
        Managers._stageManager = this;

        int _count = _recipeModelGroup.childCount;
        _recipeModels = new Recipe_Model[_count];
        for (int i = 0; i < _count; i++)
        {
            _recipeModels[i] = _recipeModelGroup.GetChild(i).GetComponent<Recipe_Model>();
        }

        Managers._gameUi.ChangePanel(0);
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

            //Managers._gameUi.Recipe_Block_Count_Text.text = $"{_selectModel._currentParts_Num} / {_selectModel._partsCount}";
            Managers._gameUi.Recipe_Block_Count_Text.text = $"X {_selectModel._partsCount}";

            //Managers._gameUi.Make_Hero_Button.interactable = _selectModel._currentParts_Num > _selectModel._renderers.Length - 1 ? true : false;

            //for (int i = 0; i < Managers._gameUi._colorButtons.Length; i++)
            //{
            //    Managers._gameUi._colorButtons[i].interactable = _heroFactory._blockCountArray[i] > 0 ? true : false;
            //}
        }


    }


    public void ToBattle()
    {
        Managers._gameUi.ChangePanel(3);
        _battleCam.SetActive(true);

        _bossEnemy = Managers.Pool.Pop(_currentStage._bossPref).GetComponent<Boss>();
        _bossEnemy.SetInit(_currentStage._stageLevel);
        _bossEnemy.transform.position = _enemySpawnPoint.position;

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
        Debug.Log("Clear");

        this.TaskDelay(2f, () => Managers._gameUi.ChangePanel(4));

    }
    public void Battle_Fail()
    {
        Debug.Log("Fail");
        this.TaskDelay(2f, () => Managers._gameUi.ChangePanel(5));
    }





}
