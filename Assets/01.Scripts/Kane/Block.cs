using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Block : MonoBehaviour
{

    public bool isConnect = false;


    public Vector2Int _pos;
    public Material[] _mats;
    //public int _typeNum = 0;
    public enum BlockType
    {
        Red,
        Blue,
        Green,
        Yellow
    }
    public BlockType _blockType;
    public int _level;
    public AnimationCurve _ease;

    public void SetPos(int _x, int _y)
    {

        GetComponent<Renderer>().material = _mats[(int)_blockType];

        _pos = new Vector2Int(_x - 2, _y - 2);

        //if (UnityEditor.EditorApplication.isPlaying)
        //{
        DOTween.Sequence() //.AppendInterval(1f)
        .Append(transform.DOLocalMove(new Vector3(_pos.x + PuzzleManager._instance._posInterval.x, 0, _pos.y + PuzzleManager._instance._posInterval.y), PuzzleManager._instance._blockMoveSpeed).SetEase(_ease));
        //}
        //else
        //{
        //    transform.localPosition = new Vector3(_pos.x, 0, _pos.y);
        //}


    }




    public void RemoveBlock()
    {
        Managers.Pool.Push(transform.GetComponent<Poolable>());
        //Destroy(gameObject);
    }


}
