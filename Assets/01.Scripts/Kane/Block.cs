using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    public enum BlockType
    {
        Red,
        Green,
        Blue,
        Yellow,
        Black,
        White,
        //Purple,
        //Orange
    }
    public BlockType _blockType;

    public Material[] _colorMats;

    Renderer _renderer;


    public void SetInit(BlockType _type)
    {
        _blockType = _type;
        if (_renderer == null) _renderer = GetComponent<Renderer>();
        _renderer.sharedMaterial = _colorMats[(int)_blockType];
    }



}
