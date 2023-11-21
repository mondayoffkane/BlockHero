using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using UnityEngine.UI;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks.Linq;


public class Block : MonoBehaviour
{
    [FoldoutGroup("Block")] public bool isConnect = false;
    [FoldoutGroup("Block")] public Vector2Int _pos;
    [FoldoutGroup("Block")] public Material[] _mats;
    [FoldoutGroup("Block")] public Mesh[] _meshes;
    public enum BlockType
    {
        Red, // viking
        Blue, // wizard
        Green, // priest
        Yellow // archer
    }
    [FoldoutGroup("Block")] public BlockType _blockType;
    [FoldoutGroup("Block")] public int _level;
    [FoldoutGroup("Block")] public AnimationCurve _ease;
    [FoldoutGroup("Block")] public bool isMatch = false;
    [FoldoutGroup("Block")] public bool isPromotion = false;

    public Hero _blockHero;



    // ====== Hero==============================


    //[FoldoutGroup("Army")] public float _maxHP;
    //[FoldoutGroup("Army")] public float _currentHP;
    //[FoldoutGroup("Army")] public float _damage;
    //[FoldoutGroup("Army")] public float _targetRange;
    //[FoldoutGroup("Army")] public float _attackRange;
    //[FoldoutGroup("Army")] public float _attackInterval;
    //[FoldoutGroup("Army")] public float _speed;
    //[FoldoutGroup("Army")] public Enemy _target;
    //[FoldoutGroup("Army")] public bool isPlay = true;


    bool isFirst = true;
    Rigidbody _rig;
    BoxCollider[] _colls;

    // ==========================================================

    [SerializeField] PuzzleManager _puzzleManager;

    public void SetType(Mesh[] meshes, int _num)
    {
        if (_blockHero != null) Destroy(_blockHero);

        _blockType = (BlockType)_num;

        switch (_blockType)
        {
            case BlockType.Red:
                _blockHero = this.gameObject.AddComponent<Viking>();
                break;

            case BlockType.Blue:
                _blockHero = this.gameObject.AddComponent<Wizard>();
                break;

            case BlockType.Green:
                _blockHero = this.gameObject.AddComponent<Priest>();
                break;

            case BlockType.Yellow:
                _blockHero = this.gameObject.AddComponent<Archer>();
                break;
        }

        _meshes = meshes;
    }

    public void Init(bool isNew = false)
    {
        //Debug.Log("SetType");

        if (_puzzleManager == null) _puzzleManager = Managers._puzzleManager; //PuzzleManager._instance;  //
        if (_rig == null) _rig = GetComponent<Rigidbody>();
        if (_colls == null) _colls = GetComponents<BoxCollider>();

        _rig.isKinematic = true;
        _colls[1].enabled = false;
        //isPlay = false;
        //_armyState = ArmyState.Wait;


        if (isNew == false)
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
        if (isFirst == false)
        {

            DOTween.Sequence()
                .Append(transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.Linear))
                .AppendCallback(() => GetComponent<MeshFilter>().sharedMesh = _meshes[_level])
                 .Append(transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce))
                 ;
        }
        else isFirst = false;


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

            Init();

        }
        else if (isMatch)
        { // delete block
            PuzzleManager._instance._grid[_pos.x + 2, _pos.y + 2] = null;

            transform.DOScale(0, 0.25f).SetEase(Ease.Linear)
                .OnComplete(() => Managers.Pool.Push(transform.GetComponent<Poolable>()));



        }

        // add match particle or block.cs in PuzzleManager match()

    }

    // ==== Hero ================================================================================


    public void SetFightMode()
    {
        if (_level > 0)
        {

            DOTween.Sequence()
                .AppendInterval(0.5f)
                .Append(transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.Linear))
                .AppendInterval(1f)
                .AppendCallback(() => _blockHero.Fight());
        }
    }


    //public enum ArmyState
    //{
    //    Wait,
    //    Move,
    //    Attack,
    //    Dead,
    //    Victory
    //}
    //public ArmyState _armyState;


    //IEnumerator Cor_Fight()
    //{
    //    yield return null;

    //    _rig.isKinematic = false;
    //    isPlay = true;
    //    _colls[1].enabled = true;

    //    _colls[1].size = GetComponent<MeshFilter>().sharedMesh.bounds.size;
    //    _colls[1].center = GetComponent<MeshFilter>().sharedMesh.bounds.center;

    //    while (isPlay)
    //    {

    //        switch (_armyState)
    //        {
    //            case ArmyState.Wait:
    //                if (_target == null) FindTarget();
    //                else _armyState = ArmyState.Move;

    //                yield return null;
    //                break;

    //            case ArmyState.Move:
    //                if (_target != null)
    //                {

    //                    transform.LookAt(_target.transform);
    //                    transform.Translate(Vector3.forward * _speed * Time.deltaTime);

    //                    if (Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)
    //                    {
    //                        _armyState = ArmyState.Attack;
    //                    }

    //                }
    //                else
    //                {
    //                    _armyState = ArmyState.Wait;
    //                }
    //                yield return null;
    //                break;

    //            case ArmyState.Attack:
    //                Attack();
    //                yield return new WaitForSeconds(_attackInterval);
    //                break;

    //            case ArmyState.Dead:

    //                isPlay = false;
    //                yield return null;
    //                break;
    //        }



    //        yield return null;
    //    }

    //    // add Victory Animation

    //}


    //protected virtual void Attack()
    //{
    //    if (_target == null || _target._currentHP <= 0)
    //    {
    //        _target = null;
    //        _armyState = ArmyState.Wait;

    //    }
    //    else
    //    {
    //        _target.OnDamage(_damage);

    //    }


    //}

    //protected virtual void OnDamage(float _enemyDamage )
    //{
    //    _currentHP -= _enemyDamage;
    //    if (_currentHP <= 0) _armyState = ArmyState.Dead;
    //}



    //protected virtual void FindTarget()
    //{
    //    if (_puzzleManager._enemyList.Count < 1)
    //    {
    //        isPlay = false;
    //        _armyState = ArmyState.Victory;

    //    }
    //    else
    //    {
    //        _target = _puzzleManager._enemyList[0];



    //        if (_puzzleManager._enemyList.Count > 2)
    //        {

    //            for (int i = 1; i < _puzzleManager._enemyList.Count; i++)
    //            {
    //                _target = Vector3.Distance(transform.position, _puzzleManager._enemyList[i].transform.position)
    //                < Vector3.Distance(transform.position, _target.transform.position)
    //                ? _puzzleManager._enemyList[i] : _target;
    //            }
    //        }
    //    }

    //    if (_target == null)
    //    {
    //        _armyState = ArmyState.Victory;
    //        Debug.Log("Test2");

    //    }
    //    else if (_target._currentHP <= 0)
    //    {
    //        _target = null;
    //        _armyState = ArmyState.Wait;

    //    }
    //    else
    //    {
    //        _armyState = ArmyState.Move;
    //    }



    //}



}
