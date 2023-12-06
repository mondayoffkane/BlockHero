using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class StageManager : MonoBehaviour
{
    public List<BlockFactory> _blockfactoryList = new List<BlockFactory>();
    public HeroFactory _heroFactory;

    public Transform _recipeModelGroup;
    public Recipe_Model[] _recipeModels;
    public Recipe_Model _currentRecipe;



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
        _currentRecipe = _recipeModels[_num];

        Managers._gameUi.ChangeRecipe(_num, _currentRecipe);


    }




}
