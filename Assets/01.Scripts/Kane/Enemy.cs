using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public enum EnemyType
    {
        Mushroom


    }
    public EnemyType _enemyType;

    public int _level;
    public float _maxHP;
    public float _currentHP;
    public float _damage;
    public float _attackRange;
    public float _attackInterval;
    public float _speed;
    public Hero _target;


    protected PuzzleManager _puzzleManager;
    public EnemyStatus _enemyStatus;

    MeshFilter _meshfilter;

    // =========================================



    public enum EnemyState
    {
        Init,
        Wait,
        Move,
        Attack,
        Dead,
        Victory
    }
    public EnemyState _enemyState;


    //public virtual void SetStatus(EnemyStatus _EnemyStatus, int Level)
    //{
    //    if (_puzzleManager == null) _puzzleManager = Managers._puzzleManager;
    //    _enemyStatus = _EnemyStatus;
    //    _level = Level;

    //}



    public virtual void Fight()
    {

    }




    public virtual void InitStatus(EnemyStatus EnemyStatus, int Level)
    {
        if (_puzzleManager == null) _puzzleManager = Managers._puzzleManager;

        _enemyStatus = EnemyStatus;
        _level = Level;

        _maxHP = _enemyStatus._maxHP[_level];
        _currentHP = _maxHP;
        _damage = _enemyStatus._damages[_level];
        _attackRange = _enemyStatus._attackRange[_level];
        _attackInterval = _enemyStatus._attackInterval[_level];
        _speed = _enemyStatus._speeds[_level];

        if (_meshfilter == null) _meshfilter = GetComponent<MeshFilter>();
        _meshfilter.sharedMesh = _enemyStatus._meshes[_level];
    }



    protected virtual void Attack()
    {
        if (_target == null || _target._armyState == Hero.ArmyState.Dead)
        {
            _target = null;
            _enemyState = EnemyState.Wait;

        }
        else
        {
            _target.OnDamage(_damage);

        }


    }


    public virtual void OnDamage(float _tempDamage)
    {
        if (_currentHP > 0 && _enemyState != EnemyState.Dead)
        {
            _currentHP -= _tempDamage;
            if (_currentHP <= 0)
            {
                Dead();
            }
            //Destroy(this.gameObject);
        }

    }

    public void Dead()
    {
        _enemyState = EnemyState.Dead;
        Managers._puzzleManager._enemyList.Remove(this);
        Managers._puzzleManager.DeadArnmyNEnemy(false);
        Managers.Pool.Push(transform.GetComponent<Poolable>());
    }



    protected virtual void FindTarget()
    {
        if (_puzzleManager._heroList.Count < 1)
        {
            _enemyState = EnemyState.Victory;
        }
        else
        {
            _target = _puzzleManager._heroList[0];

            if (_puzzleManager._heroList.Count > 2)
            {
                for (int i = 1; i < _puzzleManager._heroList.Count; i++)
                {
                    _target = Vector3.Distance(transform.position, _puzzleManager._heroList[i].transform.position)
                    < Vector3.Distance(transform.position, _target.transform.position)
                    ? _puzzleManager._heroList[i] : _target;
                }
            }

        }
        if (_target == null)
        {
            //Debug.Log("enemy victory");
            _enemyState = EnemyState.Victory;

        }
        else if (_target._currentHP <= 0 || _target._armyState == Hero.ArmyState.Dead)
        {
            _target = null;
            _enemyState = EnemyState.Wait;

        }
        else
        {
            _enemyState = EnemyState.Move;
        }




    }






}
