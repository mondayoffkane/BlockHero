using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;


public class Hero : MonoBehaviour
{
    public enum HeroType
    {
        BlockMan_0,
        BlockMan_1,
        BlockMan_2



    }
    public HeroType _heroType;


    public enum HeroState
    {
        Init,
        Wait,
        Idle,
        Move,
        Attack,
        Dead,
        Victory
    }
    public HeroState _heroState;

    //public Transform _partsGroup;



    public SkinnedMeshRenderer[] _meshfilters;

    public float _damage = 10f;
    public float _attackInterval = 1f;
    public float _speed = 5f;
    public float _maxHP = 11f;
    public float _currentHP = 11f;
    public float _defense = 1f;

    public float _attackRange = 3f;

    public Enemy _target;

    protected Animator _animator;
    /// ===========================
    public virtual void SetInit(HeroFactory _herofactory)
    {
        _animator = GetComponent<Animator>();

        _heroState = HeroState.Init;


        for (int i = 0; i < _meshfilters.Length; i++)
        {
            _meshfilters[i].sharedMesh = _herofactory._selectMeshes[i];
        }


        _damage = _herofactory._damage;
        _speed = _herofactory._speed;
        _maxHP = _herofactory._maxHP;
        _currentHP = _maxHP;
        _defense = _herofactory._defense;



        _currentHP = _maxHP;


        _animator.SetBool("Pack", true);


    }

    public virtual void Fight()
    {
        _animator.SetBool("Pack", false);
    }

    protected virtual void Attack()
    {
        _target.OnDamage(_damage);
    }

    public virtual void OnDamage(float _Damage)
    {
        if (_heroState != HeroState.Dead)
        {
            _currentHP -= _Damage;
            if (_currentHP <= 0)
            {
                _currentHP = 0;
                _heroState = HeroState.Dead;

                this.TaskDelay(2f, () => Managers.Pool.Push(GetComponent<Poolable>()));
            }
        }
    }

    protected virtual void Dead()
    {
        _animator.SetBool("Dead", true);
        Managers._stageManager._heroBattleList.Remove(this);
        Managers.Pool.Push(GetComponent<Poolable>());

        if (Managers._stageManager._heroBattleList.Count < 1)
        {
            Managers._stageManager.Battle_Fail();
        }

    }

    protected void FindTarget()
    {
        if (Managers._stageManager._bossEnemy == null)
        {
            _heroState = HeroState.Victory;
        }
        else
        {
            _target = Managers._stageManager._bossEnemy;
            _heroState = HeroState.Move;
            _animator.SetBool("Walk", true);
            _animator.SetBool("Attack", false);

        }

    }



}
