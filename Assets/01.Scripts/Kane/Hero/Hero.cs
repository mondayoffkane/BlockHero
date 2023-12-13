using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;


public class Hero : MonoBehaviour
{
    public enum HeroType
    {
        Human,
        Tank


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

    public Transform _partsGroup;

    public MeshFilter[] _meshfilters;

    public float _damage = 10f;
    public float _attackInterval = 1f;
    public float _speed = 5f;
    public float _maxHP = 11f;
    public float _currentHP = 11f;
    public float _defense = 1f;

    public float _attackRange = 3f;

    public Enemy _target;

    public virtual void SetInit(Recipe_Model _recipe)
    {
        _heroType = _recipe._heroType;
        _heroState = HeroState.Init;

        if (_heroType != HeroType.Human)
        {
            if (_partsGroup == null) _partsGroup = transform.Find("PartsGroup");
            int _count = _partsGroup.childCount;
            _meshfilters = new MeshFilter[_count];
            for (int i = 0; i < _count; i++)
            {
                _meshfilters[i] = _partsGroup.GetChild(i).GetComponent<MeshFilter>();
                _meshfilters[i].sharedMesh = _recipe._selectMeshes[i];
            }
        }
        else
        {
            _meshfilters[0].sharedMesh = _recipe._selectMeshes[0];
        }

        _damage = _recipe._damage;
        _speed = _recipe._speed;
        _maxHP = _recipe._maxHP;
        _currentHP = _maxHP;
        _defense = _recipe._defense;



        _currentHP = _maxHP;

        //_heroState = HeroState.Wait;

    }

    public virtual void Fight()
    {

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
        Managers._stageManager._spawnHeroList.Remove(this);
        Managers.Pool.Push(GetComponent<Poolable>());

        if (Managers._stageManager._spawnHeroList.Count < 1)
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
        }

    }



}
