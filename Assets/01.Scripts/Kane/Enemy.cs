using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public enum EnemyType
    {
        Mushroom,
        EnemyCastle,
        BossDragon


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

    public GameObject[] _deadEffects;

    //protected Animator _animator;

    protected BoxCollider _boxcol;

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

    public Image _hpGuage;



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
        if (_boxcol == null) _boxcol = GetComponent<BoxCollider>();
        //if (_animator == null) _animator = GetComponent<Animator>();
        if (_puzzleManager == null) _puzzleManager = Managers._puzzleManager;
        if (_hpGuage == null) _hpGuage = transform.Find("HP_Canvas").Find("HP_Guage").GetComponent<Image>();


        _enemyStatus = EnemyStatus;
        _level = Level;

        _maxHP = _enemyStatus._maxHP[_level];
        _currentHP = _maxHP;
        _damage = _enemyStatus._damages[_level];
        _attackRange = _enemyStatus._attackRange[_level];
        _attackInterval = _enemyStatus._attackInterval[_level];
        _speed = _enemyStatus._speeds[_level];
        _hpGuage.fillAmount = (_currentHP / _maxHP);
        _enemyState = EnemyState.Wait;

        if (_meshfilter == null) _meshfilter = GetComponent<MeshFilter>();
        _meshfilter.sharedMesh = _enemyStatus._meshes[_level];


        foreach (GameObject _obj in _deadEffects)
        {
            _obj.SetActive(false);
        }

        _hpGuage.transform.parent.gameObject.SetActive(false);
        //_animator.SetBool("Dead", false);
        //_animator.SetBool("Run", false);
        //_animator.SetBool("Attack", false);

        _boxcol.size = _meshfilter.sharedMesh.bounds.size;
        _boxcol.center = _meshfilter.sharedMesh.bounds.center;
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
            _hpGuage.fillAmount = (_currentHP / _maxHP);

            if (_hpGuage.fillAmount >= 0.95f)
            {
                _hpGuage.transform.parent.gameObject.SetActive(false);
            }
            else if (_hpGuage.transform.parent.gameObject.activeSelf == false)
            {
                _hpGuage.transform.parent.gameObject.SetActive(true);
            }

            if (_currentHP <= 0)
            {
                Dead();
            }
            else if (_currentHP >= _maxHP)
            {
                _currentHP = _maxHP;
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
        //_animator.SetBool("Dead", true);
        //_animator.SetBool("Attack", false);
        //_animator.SetBool("Run", false);
        foreach (GameObject _effect in _deadEffects)
        {
            DOTween.Sequence().AppendCallback(() => _effect.SetActive(true))
           .AppendInterval(3f).
           AppendCallback(() =>
           {
               _effect.SetActive(false);
               //Managers.Pool.Push(transform.GetComponent<Poolable>());
           });
        }

    }



    protected virtual void FindTarget()
    {
        if (_puzzleManager._heroList.Count < 1)
        {
            _enemyState = EnemyState.Victory;
            //_animator.SetBool("Run", false);
            //_animator.SetBool("Attack", false);
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
            //_animator.SetBool("Run", false);
            //_animator.SetBool("Attack", false);

        }
        else if (_target._currentHP <= 0 || _target._armyState == Hero.ArmyState.Dead)
        {
            _target = null;
            _enemyState = EnemyState.Wait;

        }
        else
        {
            _enemyState = EnemyState.Move;
            //_animator.SetBool("Run", true);
        }




    }






}
