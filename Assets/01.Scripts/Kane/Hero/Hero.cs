using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;



public class Hero : MonoBehaviour
{
    public enum HeroType
    {
        Viking,
        Archer,
        Priest,
        Wizard


    }
    public HeroType _heroType;




    [FoldoutGroup("Army")] public int _level;
    [FoldoutGroup("Army")] public float _maxHP;
    [FoldoutGroup("Army")] public float _currentHP;
    [FoldoutGroup("Army")] public float _damage;
    //[FoldoutGroup("Army")] public float _targetRange;
    [FoldoutGroup("Army")] public float _attackRange;
    [FoldoutGroup("Army")] public float _attackInterval;
    [FoldoutGroup("Army")] public float _speed;
    [FoldoutGroup("Army")] public Enemy _target;
    //[FoldoutGroup("Army")] public bool isPlay = true;
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
    //protected Rigidbody _rig;
    //protected BoxCollider[] _colls;
    protected BoxCollider _boxColl;

    protected PuzzleManager _puzzleManager;
    public HeroStatus _heroStatus;

    // ===============================




    public virtual void InitStatus(HeroStatus HeroStatus, int Level)
    {
        _heroStatus = HeroStatus;

        _target = null;
        _armyState = ArmyState.Wait;

        //_heroStatus = HeroStatus;
        _level = Level;

        _maxHP = _heroStatus._maxHP[Level - 1];
        _currentHP = _maxHP;
        _damage = _heroStatus._damages[Level - 1];
        _attackRange = _heroStatus._attackRange[Level - 1];
        _attackInterval = _heroStatus._attackInterval[Level - 1];
        _speed = _heroStatus._speeds[Level - 1];


        if (_boxColl == null) _boxColl = GetComponent<BoxCollider>();

    }

    public void PushHeroList()
    {
        if (_puzzleManager == null) _puzzleManager = Managers._puzzleManager;
        _puzzleManager._heroList.Add(this);
    }


    public virtual void Fight()
    {


    }



    protected virtual void Attack()
    {
        //if (_target == null || _target._enemyState == Enemy.EnemyState.Dead)
        //{
        //    _target = null;
        //    _armyState = ArmyState.Wait;

        //}

        _target.OnDamage(_damage);
        //_armyState = ArmyState.Move;

        if (_target == null || _target._enemyState == Enemy.EnemyState.Dead || Vector3.Distance(transform.position, _target.transform.position) > _attackRange)
        {
            _target = null;
            _armyState = ArmyState.Wait;
        }



    }

    public virtual void OnDamage(float _enemyDamage)
    {
        _currentHP -= _enemyDamage;
        if (_currentHP <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        _armyState = ArmyState.Dead;
        _puzzleManager._heroList.Remove(this);
        Managers._puzzleManager.DeadArnmyNEnemy(true);
        transform.gameObject.SetActive(false);
        //Managers.Pool.Push(transform.GetComponent<Poolable>());
        //Destroy(this);

    }


    protected virtual void FindTarget()
    {

        if (_puzzleManager._enemyList.Count < 1)
        {

            //isPlay = false;
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


        }
        else if (_target._currentHP <= 0 || _target._enemyState == Enemy.EnemyState.Dead)
        {
            Debug.Log("enemy is dead");
            _target = null;
            _armyState = ArmyState.Wait;

        }
        else
        {
            _armyState = ArmyState.Move;
        }



    }

    public virtual void Skill_1()
    {

    }




    public virtual void TestFunc()
    {
        Debug.Log("Hero Parent");

    }




}
