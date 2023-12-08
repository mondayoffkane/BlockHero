using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class Recipe_Model : MonoBehaviour
{


    public Hero.HeroType _heroType;

    public Mesh[] _partsMeshes;
    public Mesh[,] _2arrayMeshes;



    public Transform _partsGroup;
    public Renderer[] _renderers;
    public Mesh[] _selectMeshes;

    public int _partsCount = 0;

    public int _currentParts_Num = 0;

    public Texture _rendTexture;

    public Material[] _colorMats = new Material[4];

    public List<Block.BlockType> _tempBlockList = new List<Block.BlockType>();

    // ======================================================

    private void Start()
    {
        if (_heroType != Hero.HeroType.Human)
        {
            int _count = _partsMeshes.Length / 4;
            _2arrayMeshes = ConvertTo2DArray(_partsMeshes, _count, 4);

            _partsCount = _partsGroup.childCount;

            _renderers = new Renderer[_partsCount];
            _selectMeshes = new Mesh[_partsCount];

            for (int i = 0; i < _partsCount; i++)
            {
                _renderers[i] = _partsGroup.GetChild(i).GetComponent<Renderer>();
                _renderers[i].material = Instantiate(_renderers[i].material);
            }
        }
        else
        {
            //_renderers = new Renderer[1];

            _renderers[0].material = Instantiate(_renderers[0].material);

            int _count = _partsMeshes.Length / 4;
            _2arrayMeshes = ConvertTo2DArray(_partsMeshes, _count, 4);

            _selectMeshes = new Mesh[1];
        }

    }



    public void SetColor(int _num)
    {
        if (_currentParts_Num < _renderers.Length)
        {
            //_renderers[_currentParts_Num].material.color = _colorMats[_num].color;
            //_renderers[_currentParts_Num].GetComponent<MeshFilter>().sharedMesh = _2arrayMeshes[_currentParts_Num, _num];
            _selectMeshes[_currentParts_Num] = _2arrayMeshes[_currentParts_Num, _num];

            _currentParts_Num++;
            Managers._stageManager._heroFactory._blockCountArray[_num]--;
            _tempBlockList.Add((Block.BlockType)_num);
            Managers._stageManager.FactoryCheckButtons();
        }

    }



    public void UndoColor()
    {
        if (_currentParts_Num > 0)
        {
            _currentParts_Num--;

            Color _color = Color.gray;
            _color.a = 0.6f;

            _renderers[_currentParts_Num].material.color = _color;

            int _tempblocknum = (int)_tempBlockList[_currentParts_Num];
            _tempBlockList.RemoveAt(_currentParts_Num);
            Managers._stageManager._heroFactory._blockCountArray[_tempblocknum]++;
            Managers._stageManager.FactoryCheckButtons();

        }
    }

    public void Reset()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            Color _color = Color.white;
            _color.a = 0.6f;
            _renderers[i].material.color = _color;
        }
        _currentParts_Num = 0;


        for (int i = _tempBlockList.Count - 1; i >= 0; i--)
        {
            int _tempblocknum = (int)_tempBlockList[i];
            _tempBlockList.RemoveAt(i);
            Managers._stageManager._heroFactory._blockCountArray[_tempblocknum]++;
        }
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
