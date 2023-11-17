using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using UnityEngine.UI;
using Sirenix.OdinInspector;


public class Block : MonoBehaviour
{
    [FoldoutGroup("Block")] public bool isConnect = false;
    [FoldoutGroup("Block")] public Vector2Int _pos;
    [FoldoutGroup("Block")] public Material[] _mats;
    [FoldoutGroup("Block")] public Mesh[] _meshes;
    public enum BlockType
    {
        Red,
        Blue,
        Green,
        Yellow
    }
    [FoldoutGroup("Block")] public BlockType _blockType;
    [FoldoutGroup("Block")] public int _level;
    [FoldoutGroup("Block")] public AnimationCurve _ease;
    [FoldoutGroup("Block")] public bool isMatch = false;
    [FoldoutGroup("Block")] public bool isPromotion = false;

    // ====== Hero==============================


    [FoldoutGroup("Army")] public float _maxHP;
    [FoldoutGroup("Army")] public float _currentHP;
    [FoldoutGroup("Army")] public float _damage;
    [FoldoutGroup("Army")] public float _targetRange;
    [FoldoutGroup("Army")] public float _attackRange;
    [FoldoutGroup("Army")] public float _attackInterval;
    [FoldoutGroup("Army")] public float _speed;
    [FoldoutGroup("Army")] public Enemy _target;



    // ==========================================================

    PuzzleManager _puzzleManager;
    public void SetType(bool isMorote = false)
    {
        if (_puzzleManager == null) _puzzleManager = Managers._puzzleManager;

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
        //GetComponent<MeshFilter>().sharedMesh = _meshes[_level];

        //transform.localScale = Vector3.zero;
        DOTween.Sequence()
            .Append(transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.Linear))
            .AppendCallback(() => GetComponent<MeshFilter>().sharedMesh = _meshes[_level])
             .Append(transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce));


    }



    public void SetPos(int _x, int _y)
    {
        //transform.name = $"({_x},{_y})"; // delete


        _pos = new Vector2Int(_x - 2, _y - 2);


        DOTween.Sequence() //.AppendInterval(1f)
        .Append(transform.DOLocalMove(new Vector3(_pos.x + _puzzleManager._posInterval.x, 0, _pos.y + _puzzleManager._posInterval.y), _puzzleManager._blockMoveSpeed).SetEase(_ease));

    }

    public void SetOrigin()
    {
        DOTween.Sequence() //.AppendInterval(1f)
      .Append(transform.DOLocalMove(new Vector3(_pos.x + _puzzleManager._posInterval.x, 0, _pos.y + _puzzleManager._posInterval.y), _puzzleManager._blockMoveSpeed).SetEase(_ease));
    }




    public void RemoveBlock()
    {
        Managers.Pool.Push(transform.GetComponent<Poolable>());

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

            transform.DOScale(0, 0.25f).SetEase(Ease.Linear)
                .OnComplete(() => Managers.Pool.Push(transform.GetComponent<Poolable>()));



        }

        // add match particle or block.cs in PuzzleManager match()

    }




    public void SetFightMode()
    {
        if (_level > 0)
        {


            DOTween.Sequence().Append(transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.Linear))
                .AppendInterval(1f)
                .AppendCallback(() => StartCoroutine(Cor_Fight()));
        }
    }


    IEnumerator Cor_Fight()
    {
        yield return null;



        while (true)
        {
            yield return null;
        }

    }
    // ==== Hero ================================================================================

    protected virtual void Attack()
    {

    }


    protected virtual void FindTarget()
    {


    }



}
