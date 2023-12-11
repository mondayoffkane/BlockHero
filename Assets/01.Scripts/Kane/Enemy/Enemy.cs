using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _damage;
    public float _attackInterval = 1f;
    public float _maxHP = 10f;
    public float _currentHP = 10f;
    public float _attackRange = 5f;


    public enum EnemyState
    {
        Init,
        Wait,
        Attack,
        Dead,
        Victory
    }
    public EnemyState _enemyState;

    public Hero _target;


    public virtual void SetInit(int _level)
    {
        _target = null;
        _enemyState = EnemyState.Init;

        _damage = 5 + 2 * _level;
        _attackInterval = 2f - 0.1f * (float)_level < 0 ? 0.1f : 2f - 0.1f * (float)_level;
        _maxHP = 10 + 100 * _level;
        _currentHP = _maxHP;


    }

    public virtual void Fight()
    {

    }


    protected virtual void FindTarget()
    {
        if (_target == null)
        {
            if (Managers._stageManager._spawnHeroList.Count < 1)
            {
                _enemyState = EnemyState.Victory;
                Managers._stageManager.Battle_Fail();
            }

            else
            {
                _target = Managers._stageManager._spawnHeroList[0];
                for (int i = 0; i < Managers._stageManager._spawnHeroList.Count; i++)
                {
                    if (Vector3.Distance(transform.position, _target.transform.position)
                        > Vector3.Distance(transform.position, Managers._stageManager._spawnHeroList[i].transform.position))
                    {
                        _target = Managers._stageManager._spawnHeroList[i];
                    }
                }
            }
        }

    }

    public virtual void OnDamage(float _Damage)
    {
        if (_enemyState != EnemyState.Dead)
        {
            _currentHP -= _Damage;
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

        Managers._stageManager._bossEnemy = null;
        Managers.Pool.Push(transform.GetComponent<Poolable>());


        Managers._stageManager.Battle_Clear();



    }

    protected virtual void Attack()
    {

    }



}
