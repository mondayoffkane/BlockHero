using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class Recipe_Model : MonoBehaviour
{
    public enum HeroType
    {
        Human,
        Tank1,
        Tank2

    }
    public HeroType _heroType;



    public Transform _partsGroup;
    public Renderer[] _renderers;

    public int _partsCount = 0;

    public int _currentParts_Num = 0;

    public Texture _rendTexture;

    public Material[] _colorMats = new Material[3];

    public List<Block.BlockType> _tempBlockList = new List<Block.BlockType>();

    // ======================================================

    private void Start()
    {

        _partsCount = _partsGroup.childCount;

        _renderers = new Renderer[_partsCount];
        for (int i = 0; i < _partsCount; i++)
        {
            _renderers[i] = _partsGroup.GetChild(i).GetComponent<Renderer>();
            _renderers[i].material = Instantiate(_renderers[i].material);
        }

    }



    public void SetColor(int _num)
    {
        if (_currentParts_Num < 3)
        {
            _renderers[_currentParts_Num].material.color = _colorMats[_num].color;
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
            Color _color = Color.gray;
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


}
