using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _damage;
    public float _attackInterval = 1f;
    public float _maxHP = 10f;
    public float _currentHP = 10f;
    public float _attackRange = 10f;


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

    public Hero _target;

    protected Animator _animator;

    public virtual void SetInit(int _level)
    {
        _animator = GetComponent<Animator>();

        _target = null;
        _enemyState = EnemyState.Init;

        _damage = 5 + 2 * _level;
        _attackInterval = 2f - 0.1f * (float)_level < 0.3 ? 0.3f : 2f - 0.1f * (float)_level;
        _maxHP = 100 + 50 * _level;
        _currentHP = _maxHP;

        Managers._gameUi.Boss_HP_Text.text = $"{_currentHP} / {_maxHP}";
        Managers._gameUi.Boss_HP_Guage.fillAmount = _currentHP / _maxHP;
    }

    public virtual void Fight()
    {
        _target = null;
    }


    protected virtual void FindTarget()
    {
        if (_target == null)
        {
            if (Managers._stageManager._heroBattleList.Count < 1)
            {
                _enemyState = EnemyState.Victory;
                Managers._stageManager.Battle_Fail();
            }

            else
            {
                _target = Managers._stageManager._heroBattleList[0];
                for (int i = 0; i < Managers._stageManager._heroBattleList.Count; i++)
                {
                    if (Vector3.Distance(transform.position, _target.transform.position)
                        > Vector3.Distance(transform.position, Managers._stageManager._heroBattleList[i].transform.position))
                    {
                        _target = Managers._stageManager._heroBattleList[i];
                    }
                }
                _enemyState = EnemyState.Move;
                _animator.SetBool("Attack", false);
            }
        }

    }

    public virtual void OnDamage(float _Damage)
    {
        if (_enemyState != EnemyState.Dead)
        {
            _currentHP -= _Damage;
            Managers._gameUi.Boss_HP_Text.text = $"{_currentHP} / {_maxHP}";
            Managers._gameUi.Boss_HP_Guage.fillAmount = _currentHP / _maxHP;

            //if (_currentHP <= 0)
            //{
            //    _currentHP = 0;
            //    _enemyState = EnemyState.Dead;

            //    this.TaskDelay(2f, () => Managers.Pool.Push(GetComponent<Poolable>()));
            //}
        }
    }

    protected void Dead()
    {
        _animator.SetBool("Dead", true);
        Managers._stageManager._bossEnemy = null;
        Managers.Pool.Push(transform.GetComponent<Poolable>());


        Managers._stageManager.Battle_Clear();



    }

    protected virtual void Attack()
    {
        _target.OnDamage(_damage);
    }



}
