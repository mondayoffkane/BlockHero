using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class Block : MonoBehaviour
{
    public bool isConnect = false;


    public Vector2Int _pos;
    public Material[] _mats;
    public Mesh[] _meshes;

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
    public bool isMatch = false;
    public bool isPromotion = false;

    public void SetType(bool isMorote = false)
    {
        if (isMorote)
        {
            _level++;

        }
        else
        {
            _level = 0;
        }
        isMatch = false;
        isPromotion = false;


        GetComponent<Renderer>().material = _mats[(int)_blockType];
        GetComponent<MeshFilter>().sharedMesh = _meshes[_level];



    }



    public void SetPos(int _x, int _y)
    {
        transform.name = $"({_x},{_y})"; // delete
        transform.Find("Canvas").Find("Text").GetComponent<Text>().text = $"{_x},{_y}";

        _pos = new Vector2Int(_x - 2, _y - 2);


        DOTween.Sequence() //.AppendInterval(1f)
        .Append(transform.DOLocalMove(new Vector3(_pos.x + PuzzleManager._instance._posInterval.x, 0, _pos.y + PuzzleManager._instance._posInterval.y), PuzzleManager._instance._blockMoveSpeed).SetEase(_ease));

    }

    public void SetOrigin()
    {
        DOTween.Sequence() //.AppendInterval(1f)
      .Append(transform.DOLocalMove(new Vector3(_pos.x + PuzzleManager._instance._posInterval.x, 0, _pos.y + PuzzleManager._instance._posInterval.y), PuzzleManager._instance._blockMoveSpeed).SetEase(_ease));
    }




    public void RemoveBlock()
    {
        Managers.Pool.Push(transform.GetComponent<Poolable>());
        //Destroy(gameObject);
    }


    public void OnMatchBlock()
    {
        if (isPromotion)
        { // upgrade block

            SetType(true);

        }
        else if (isMatch)
        { // delete block
            PuzzleManager._instance._grid[_pos.x + 2, _pos.y + 2] = null;
            Managers.Pool.Push(transform.GetComponent<Poolable>());

        }

    }


}
