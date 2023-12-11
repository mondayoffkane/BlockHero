using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class Recipe_Model : MonoBehaviour
{

    public string _recipeName;
    public Hero.HeroType _heroType;
    public Sprite _bluePrint_Sprite;

    public Mesh[] _partsMeshes;
    public Mesh[,] _2arrayMeshes;



    //public Transform _partsGroup;
    //public Renderer[] _renderers;
    public Mesh[] _selectMeshes;

    public int _partsCount = 0;

    public int _currentParts_Num = 0;

    //public Texture _rendTexture;

    //public Material[] _colorMats = new Material[4];

    public List<Block.BlockType> _tempBlockList = new List<Block.BlockType>();

    // ======================================================

    private void Start()
    {

        int _count = _partsMeshes.Length / 4;
        _2arrayMeshes = ConvertTo2DArray(_partsMeshes, _count, 4);
        _selectMeshes = new Mesh[_partsCount];

        //if (_heroType != Hero.HeroType.Human)
        //{
        //    int _count = _partsMeshes.Length / 4;
        //    _2arrayMeshes = ConvertTo2DArray(_partsMeshes, _count, 4);
        //    _selectMeshes = new Mesh[_partsCount];

        //_partsCount = _partsGroup.childCount;

        //_renderers = new Renderer[_partsCount];

        //for (int i = 0; i < _partsCount; i++)
        //{
        //    _renderers[i] = _partsGroup.GetChild(i).GetComponent<Renderer>();
        //    //_renderers[i].material = Instantiate(_renderers[i].material);
        //    _renderers[i].gameObject.SetActive(false);
        //    Vector3 _pos = _renderers[i].transform.position;
        //    _pos.y = i + 4f;
        //    _renderers[i].transform.position = _pos;
        //}
        //}
        //else
        //{
        //_renderers = new Renderer[1];
        //_partsCount = 1;
        //_renderers[0].material = Instantiate(_renderers[0].material);
        //_renderers[0].gameObject.SetActive(false);
        //Vector3 _pos = _renderers[0].transform.position;
        //_pos.y = 4f;
        //_renderers[0].transform.position = _pos;

        //int _count = _partsMeshes.Length / 4;
        //_2arrayMeshes = ConvertTo2DArray(_partsMeshes, _count, 4);

        //_selectMeshes = new Mesh[1];
        //}

    }



    public void SetColor(int _num)
    {
        if (_currentParts_Num < _partsCount)
        {
            //_renderers[_currentParts_Num].material.color = _colorMats[_num].color;
            //_renderers[_currentParts_Num].GetComponent<MeshFilter>().sharedMesh = _2arrayMeshes[_currentParts_Num, _num];
            _selectMeshes[_currentParts_Num] = _2arrayMeshes[_currentParts_Num, _num];

            //_renderers[_currentParts_Num].gameObject.SetActive(true);
            //_renderers[_currentParts_Num].transform.DOMoveY(_currentParts_Num, 0.25f).SetEase(Ease.Linear);
            //Managers._gameUi.SetColorImg(_currentParts_Num, _num);

            _currentParts_Num++;

            Managers._stageManager._heroFactory._blockCountArray[_num]--;
            _tempBlockList.Add((Block.BlockType)_num);
            Managers._stageManager.FactoryCheckButtons();

            //Managers._gameUi.Recipe_Block_Count_Text.text = $"{_currentParts_Num} / {_partsCount}";
            Managers._gameUi.Recipe_Block_Count_Text.text = $"X {_partsCount}";

            Managers._gameUi.SetColorImg(this);



        }

    }



    public void UndoColor()
    {
        if (_currentParts_Num > 0)
        {
            _currentParts_Num--;
            //_renderers[_currentParts_Num].gameObject.SetActive(false);
            //Color _color = Color.white;
            //_color.a = 0.6f;

            //_renderers[_currentParts_Num].material.color = _color;
            //_renderers[_currentParts_Num].transform.DOMoveY(_currentParts_Num + 4f, 0.25f).SetEase(Ease.Linear);
            //Managers._gameUi.SetColorImg(_currentParts_Num, 0, false);
            Managers._gameUi.SetColorImg(this);
            int _tempblocknum = (int)_tempBlockList[_currentParts_Num];
            _tempBlockList.RemoveAt(_currentParts_Num);
            Managers._stageManager._heroFactory._blockCountArray[_tempblocknum]++;
            Managers._stageManager.FactoryCheckButtons();



        }
    }

    public void Reset()
    {
        //for (int i = 0; i < _renderers.Length; i++)
        //{
        //    Color _color = Color.white;
        //    _color.a = 0.6f;
        //    _renderers[i].material.color = _color;
        //    _renderers[i].gameObject.SetActive(false);
        //    Vector3 _pos = _renderers[i].transform.position;
        //    _pos.y = i + 4f;
        //    _renderers[i].transform.position = _pos;

        //}
        _currentParts_Num = 0;


        for (int i = _tempBlockList.Count - 1; i >= 0; i--)
        {
            int _tempblocknum = (int)_tempBlockList[i];
            _tempBlockList.RemoveAt(i);
            Managers._stageManager._heroFactory._blockCountArray[_tempblocknum]++;
            //Managers._gameUi.SetColorImg(i, 0, false);
        }
        Managers._gameUi.SetColorImg(this);
        Managers._stageManager.FactoryCheckButtons();

    }




    public Mesh[,] ConvertTo2DArray(Mesh[] oneDArray, int rows, int cols)
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

    //public int[] ConvertTo1DArray(int[,] twoDArray)
    //{
    //    int rows = twoDArray.GetLength(0);
    //    int cols = twoDArray.GetLength(1);
    //    int[] oneDArray = new int[rows * cols];
    //    int index = 0;
    //    for (int i = 0; i < rows; i++)
    //    {
    //        for (int j = 0; j < cols; j++)
    //        {
    //            oneDArray[index] = twoDArray[i, j];
    //            index++;
    //        }
    //    }
    //    return oneDArray;
    //}


}
