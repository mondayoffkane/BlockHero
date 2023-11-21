using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;



public class Hero : MonoBehaviour
{

    [FoldoutGroup("Army")] public float _maxHP;
    [FoldoutGroup("Army")] public float _currentHP;
    [FoldoutGroup("Army")] public float _damage;
    //[FoldoutGroup("Army")] public float _targetRange;
    [FoldoutGroup("Army")] public float _attackRange;
    [FoldoutGroup("Army")] public float _attackInterval;
    [FoldoutGroup("Army")] public float _speed;
    [FoldoutGroup("Army")] public Enemy _target;
    [FoldoutGroup("Army")] public bool isPlay = true;
    //bool isFirst = true;




    public enum ArmyState
    {
        Wait,
        Move,
        Attack,
        Dead,
        Victory
    }
    public ArmyState _armyState;

    protected bool isFirst = true;
    protected Rigidbody _rig;
    protected BoxCollider[] _colls;

    protected PuzzleManager _puzzleManager;
    public HeroStatus _heroStatus;

    // ===============================

    private void OnEnable()
    {
        if (_puzzleManager == null) _puzzleManager = Managers._puzzleManager;
    }

    public void InitStatus( int _level)
    {
        Debug.Log("Set");
        //_heroStatus = _status;
        _maxHP = _heroStatus._maxHP[_level];
        _currentHP = _maxHP;
        _damage = _heroStatus._damages[_level];
        _attackRange = _heroStatus._attackRange[_level];
        _attackInterval = _heroStatus._attackInterval[_level];
        _speed = _heroStatus._speeds[_level];


        if (_rig == null) _rig = GetComponent<Rigidbody>();
        if (_colls == null) _colls = GetComponents<BoxCollider>();

        _rig.isKinematic = true;
        _colls[1].enabled = false;


    }

    public virtual void Fight(int _level)
    {
        Debug.Log("Parent");
    }



    protected virtual void Attack()
    {
        if (_target == null || _target._currentHP <= 0)
        {
            _target = null;
            _armyState = ArmyState.Wait;

        }
        else
        {
            _target.OnDamage(_damage);

        }


    }

    protected virtual void OnDamage(float _enemyDamage)
    {
        _currentHP -= _enemyDamage;
        if (_currentHP <= 0) _armyState = ArmyState.Dead;
    }



    protected virtual void FindTarget()
    {
        if (_puzzleManager._enemyList.Count < 1)
        {
            isPlay = false;
            _armyState = ArmyState.Victory;

        }
        else
        {
            _target = _puzzleManager._enemyList[0];



            if (_puzzleManager._enemyList.Count > 2)
            {

                for (int i = 1; i < _puzzleManager._enemyList.Count; i++)
                {
                    _target = Vector3.Distance(transform.position, _puzzleManager._enemyList[i].transform.position)
                    < Vector3.Distance(transform.position, _target.transform.position)
                    ? _puzzleManager._enemyList[i] : _target;
                }
            }
        }

        if (_target == null)
        {
            _armyState = ArmyState.Victory;
            Debug.Log("Test2");

        }
        else if (_target._currentHP <= 0)
        {
            _target = null;
            _armyState = ArmyState.Wait;

        }
        else
        {
            _armyState = ArmyState.Move;
        }



    }








}
