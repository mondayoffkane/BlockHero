using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class StageManager : MonoBehaviour
{
    public GameObject _battleCam;


    public List<BlockFactory> _blockfactoryList = new List<BlockFactory>();
    public HeroFactory _heroFactory;

    public Transform _recipeModelGroup;
    public Recipe_Model[] _recipeModels;
    public Recipe_Model _selectModel;


    public List<Hero> _spawnHeroList = new List<Hero>();
    public Transform _heroSpawnPoint;

    public List<Enemy> _spawnEnemyList = new List<Enemy>();
    public Transform _enemySpawnPoint;

    public StageData _currentStage;


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

    }



    private void Update()
    {




    }





    // ==================================================================

    public void SelectRecipe(int _num)
    {
        _selectModel = _recipeModels[_num];

        Managers._gameUi.ChangeRecipe(_num, _selectModel);


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

        _newHero.transform.position = new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-8f, -15f));
        _selectModel.Reset();

        _spawnHeroList.Add(_newHero);


    }

    public void FactoryCheckButtons()
    {

        Managers._gameUi.Make_Hero_Button.interactable = _selectModel._currentParts_Num > 2 ? true : false;


        for (int i = 0; i < Managers._gameUi._colorButtons.Length; i++)
        {
            Managers._gameUi._colorButtons[i].interactable = _heroFactory._blockCountArray[i] > 0 ? true : false;
        }



    }


    public void ToBattle()
    {
        _battleCam.SetActive(true);



        for (int i = 0; i < 1; i++)
        {
            Enemy _newBoss = Managers.Pool.Pop(_currentStage._bossPref).GetComponent<Boss>();
            _newBoss.SetInit(_currentStage._stageLevel);
        }


        for (int i = 0; i < _spawnHeroList.Count; i++)
        {
            _spawnHeroList[i].transform.position = _heroSpawnPoint.position + new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-8f, -15f));

            //_spawnHeroList
        }


    }

    public void ToFactory()
    {
        _battleCam.SetActive(false);

        if (_spawnEnemyList.Count > 0)
            for (int i = 0; i < _spawnEnemyList.Count; i++)
            {
                var _obj = _spawnEnemyList[0];
                _spawnEnemyList.Remove(_obj);
                Managers.Pool.Push(_obj.GetComponent<Poolable>());
            }
        _spawnEnemyList.Clear();

        if (_spawnHeroList.Count > 0)
            for (int i = 0; i < _spawnHeroList.Count; i++)
            {
                var _obj = _spawnHeroList[0];
                _spawnHeroList.Remove(_obj);
                Managers.Pool.Push(_obj.GetComponent<Poolable>());
            }
        _spawnHeroList.Clear();



    }



}
