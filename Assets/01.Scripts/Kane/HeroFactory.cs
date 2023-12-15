using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class HeroFactory : MonoBehaviour
{


    public SkinnedMeshRenderer _guage_Obj;
    public Image _bluePrint_Img;

    [SerializeField] RecipeData[] _recipeDatas = new RecipeData[3];
    public RecipeData _currentRecipe;

    public Sprite[,] _2arraySprites;
    public Mesh[,] _2arrayMeshes;
    public Mesh[] _selectMeshes;
    public int _partsCount = 0;
    public int _currentParts_Num = 0;
    public List<Block.BlockType> _tempBlockList = new List<Block.BlockType>();

    [FoldoutGroup("Status")] public float _damage = 10f;
    [FoldoutGroup("Status")] public float _attackInterval = 1f;
    [FoldoutGroup("Status")] public float _speed = 5f;
    [FoldoutGroup("Status")] public float _maxHP = 10f;
    [FoldoutGroup("Status")] public float _defense = 5f;


    // ==============================

    public float _currentTime = 0f;
    public float _maxTime = 1f;

    public bool isProduction = false;

    // ===============================================


    private void Awake()
    {
        for (int i = 0; i < _recipeDatas.Length; i++)
        {
            _recipeDatas[i] = Resources.Load<RecipeData>($"RecipeDatas/Recipe_Data_{i}");
        }




    }

    private void OnEnable()
    {
        if (_guage_Obj == null) _guage_Obj = transform.Find("Guage_Obj").GetComponent<SkinnedMeshRenderer>();
        if (_bluePrint_Img == null) _bluePrint_Img = transform.Find("Canvas").GetChild(0).GetComponent<Image>();

        StartCoroutine(Cor_Update());
    }


    IEnumerator Cor_Update()
    {
        while (true)
        {
            yield return null;

            if (isProduction)
            {
                _currentTime += Time.deltaTime;
                _guage_Obj.SetBlendShapeWeight(0, _currentTime / _maxTime);

                if (_currentTime >= _maxTime)
                {
                    _currentTime = 0f;

                    SpawnHero();

                }
            }

        }
    }


    public void MakeHeroOnOff(bool isOn)
    {
        isProduction = isOn;

        if (isProduction)
        {



        }
        else
        {

        }
    }

    public void SetRecipe(int _num)
    {
        //_bluePrint_Img.sprite = _newRecipe._bluePrint_Sprite;

        _currentRecipe = Instantiate(_recipeDatas[_num]);

        _partsCount = _currentRecipe._partsCount;

        int _count = _currentRecipe._partsMeshes.Length / 4;
        _2arrayMeshes = ConvertTo2DArrayMesh(_currentRecipe._partsMeshes, _count, 4);
        _selectMeshes = new Mesh[_partsCount];

        _2arraySprites = ConvertTo2DArraySprite(_currentRecipe._partsSprites, _count, 4);

        _damage = _currentRecipe._baseDamage;
        _attackInterval = _currentRecipe._baseAttackInterval;
        _speed = _currentRecipe._baseSpeed;
        _maxHP = _currentRecipe._baseHP;
        _defense = _currentRecipe._baseDefense;


        Managers._gameUi.ChangeRecipe(_num, this);


    }

    public void SetColor(int _num)
    {
        if (_currentParts_Num < _partsCount)
        {
            _selectMeshes[_currentParts_Num] = _2arrayMeshes[_currentParts_Num, _num];

            switch (_num)
            {
                case 0:
                    _damage += _currentRecipe._damageValue;
                    break;
                case 1:
                    _speed += _currentRecipe._speedValue;
                    break;

                case 2:
                    _maxHP += _currentRecipe._HPValue;
                    break;

                case 3:
                    _defense += _currentRecipe._defenseValue;
                    break;
            }


            _currentParts_Num++;
            Managers._stageManager._blockStorage._blockCountArray[_num]--;
            _tempBlockList.Add((Block.BlockType)_num);
            Managers._stageManager.FactoryCheckButtons();
            Managers._gameUi.Recipe_Block_Count_Text.text = $"X {_partsCount}";
            Managers._gameUi.SetColorImg(this);


            Managers._gameUi.Recipe_Status_Text.text = $"ATK : {_damage} Speed : {_speed} HP : {_maxHP} DEF : {_defense}";
        }
    }

    public void UndoColor()
    {
        if (_currentParts_Num > 0)
        {
            _currentParts_Num--;

            Managers._gameUi.SetColorImg(this);
            int _tempblocknum = (int)_tempBlockList[_currentParts_Num];
            _tempBlockList.RemoveAt(_currentParts_Num);
            Managers._stageManager._blockStorage._blockCountArray[_tempblocknum]++;

            switch (_tempblocknum)
            {
                case 0:
                    _damage -= _currentRecipe._damageValue;
                    break;
                case 1:
                    _speed -= _currentRecipe._speedValue;
                    break;

                case 2:
                    _maxHP -= _currentRecipe._HPValue;
                    break;

                case 3:
                    _defense -= _currentRecipe._defenseValue;
                    break;
            }


            Managers._gameUi.Recipe_Status_Text.text = $"ATK : {_damage} Speed : {_speed} HP : {_maxHP} DEF : {_defense}";
            Managers._stageManager.FactoryCheckButtons();



        }
    }


    public void SpawnHero()
    {

    }



    public Mesh[,] ConvertTo2DArrayMesh(Mesh[] oneDArray, int rows, int cols)
    {
        Mesh[,] twoDArray = new Mesh[rows, cols];
        int index = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                twoDArray[i, j] = oneDArray[index];
                index++;
            }
        }
        return twoDArray;
    }

    public Sprite[,] ConvertTo2DArraySprite(Sprite[] oneDArray, int rows, int cols)
    {
        Sprite[,] twoDArray = new Sprite[rows, cols];
        int index = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                twoDArray[i, j] = oneDArray[index];
                index++;
            }
        }
        return twoDArray;
    }

}
