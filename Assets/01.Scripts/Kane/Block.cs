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
    //[FoldoutGroup("Block")] public Material[] _mats;

    [FoldoutGroup("Block")] public Mesh[] _meshes;
    public enum BlockType
    {
        Red, // viking
        Yellow, // archer
        Green, // priest
        Blue // wizard

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
    //Rigidbody _rig;
    //BoxCollider[] _colls;

    // ==========================================================

    [SerializeField] PuzzleManager _puzzleManager;




    public void SetType(HeroStatus _heroStat, int _num)
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



        _meshes = _heroStat._meshes;
        GetComponent<MeshFilter>().sharedMesh = _meshes[_level];
        _blockHero._heroStatus = _heroStat;
        _blockHero.Setting();

    }

    public void Init(bool isNew = false)
    {

        if (_puzzleManager == null) _puzzleManager = Managers._puzzleManager; //PuzzleManager._instance;  //



        if (isNew == false)
        {
            _level++;
            //Debug.Log("Level :" + _level);

        }
        else
        {
            _level = 0;
        }
        isMatch = false;
        isPromotion = false;


        if (isFirst == false)
        {

            DOTween.Sequence()
                .Append(transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.Linear))
                .AppendCallback(() => GetComponent<MeshFilter>().sharedMesh = _meshes[_level])
                 .Append(transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce))
                 ;

            //Debug.Log("Mesh length :" + _meshes.Length);
            //Debug.Log(_meshes[_level]);

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
            _blockHero.PushHeroList();
            _blockHero.InitStatus(_level);
            //_blockHero.Fight(_level);

            DOTween.Sequence()
                .AppendInterval(0.5f)
                .Append(transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.Linear))
                .AppendInterval(0.5f)
                .AppendCallback(() => _blockHero.Fight());
        }
        else
        {
            transform.DOLocalMoveY(-1f, 0.5f).SetEase(Ease.Linear);
        }




    }




}
