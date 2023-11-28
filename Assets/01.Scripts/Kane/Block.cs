using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using UnityEngine.UI;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks.Linq;
using System;


public class Block : MonoBehaviour
{
    [FoldoutGroup("Block")] public Hero.HeroType _heroType;
    [FoldoutGroup("Block")] public bool isConnect = false;
    [FoldoutGroup("Block")] public Vector2Int _pos;
    [FoldoutGroup("Block")] public Mesh[] _meshes;
    [FoldoutGroup("Block")] public int _level;
    [FoldoutGroup("Block")] public AnimationCurve _ease;
    [FoldoutGroup("Block")] public bool isMatch = false;
    [FoldoutGroup("Block")] public bool isPromotion = false;


    // ====== Hero==============================

    bool isFirstSpawn = true;

    // ==========================================================

    [SerializeField] PuzzleManager _puzzleManager;




    public void SetType(HeroStatus _heroStat)
    {

        _heroType = _heroStat._heroType;

        _meshes = _heroStat._blockMeshes;
        GetComponent<MeshFilter>().sharedMesh = _meshes[_level];


    }

    public void Init(bool isNew = false)
    {

        if (_puzzleManager == null) _puzzleManager = Managers._puzzleManager; //PuzzleManager._instance;  //

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


        if (isFirstSpawn == false)
        {

            DOTween.Sequence()
                .Append(transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.Linear))
                .AppendCallback(() => GetComponent<MeshFilter>().sharedMesh = _meshes[_level])
                 .Append(transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce))
                 ;

            //Debug.Log("Mesh length :" + _meshes.Length);
            //Debug.Log(_meshes[_level]);

        }
        else isFirstSpawn = false;


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
            //var _effect = Managers.Pool.Pop(Resources.Load<GameObject>("Effect/MergePromote"));
            //_effect.transform.position = transform.position + Vector3.up * 0.5f;
            //this.TaskDelay(1f, () => Managers.Pool.Push(_effect));
        }
        else if (isMatch)
        { // delete block
            PuzzleManager._instance._grid[_pos.x + 2, _pos.y + 2] = null;
            //var _effect = Managers.Pool.Pop(Resources.Load<GameObject>("Effect/MergeRemove"));
            //_effect.transform.position = transform.position + Vector3.up * 0.5f;
            //this.TaskDelay(1f, () => Managers.Pool.Push(_effect));
            transform.DOScale(0, 0.25f).SetEase(Ease.Linear)
                .OnComplete(() => Managers.Pool.Push(transform.GetComponent<Poolable>()));



        }

        // add match particle or block.cs in PuzzleManager match()

    }

    // ==== Hero ================================================================================


    public void SpawnHero() //  Spawn Army
    {

        if (_level > 0)
        {
            GameObject _heroPref;
            string _typeString = _heroType.ToString();

            _heroPref = Resources.Load<GameObject>($"Hero_Prefs/{_typeString}_Pref");
            Type _heroClassType = Type.GetType(_typeString);
            Hero _newBlockHero = (Hero)Managers.Pool.Pop(_heroPref).GetComponent(_heroClassType);

            _newBlockHero.transform.position = transform.position;

            HeroStatus _heroStatus = Resources.Load<HeroStatus>($"HeroStatus/{_heroType.ToString()}");
            _newBlockHero.PushHeroList();
            _newBlockHero.InitStatus(_heroStatus, _level);

            // add merge particley
        }
        //else
        //{
        transform.DOLocalMoveY(-1f, 0.5f).SetEase(Ease.Linear);
        //}




    }




}
