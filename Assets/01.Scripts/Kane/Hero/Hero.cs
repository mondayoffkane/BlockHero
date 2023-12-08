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

    public Renderer[] _renderers;

    public float _damage = 10f;
    public float _attackInterval = 2f;
    public float _maxHP = 11f;
    public float _currentHP = 11f;

    public float _attackRange = 3f;

    public Enemy _target;

    public virtual void SetInit(Recipe_Model _recipe)
    {
        _heroType = _recipe._heroType;

        if (_heroType != HeroType.Human)
        {
            if (_partsGroup == null) _partsGroup = transform.Find("PartsGroup");
            int _count = _partsGroup.childCount;
            _renderers = new Renderer[_count];
            for (int i = 0; i < _count; i++)
            {
                _renderers[i] = _partsGroup.GetChild(i).GetComponent<Renderer>();
            }
        }
        else
        {
            //_renderers = new Renderer[0];
            //_renderers[0] = _partsGroup.GetChild(0).GetComponent<Renderer>();
        }

        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material = Instantiate(_recipe._renderers[i].material);
        }

        for (int i = 0; i < _recipe._tempBlockList.Count; i++)
        {
            switch (_recipe._tempBlockList[i])
            {
                case Block.BlockType.Red:
                    _damage += 5f;
                    break;

                case Block.BlockType.Green:
                    _attackInterval += 2f;
                    break;

                case Block.BlockType.Blue:
                    _maxHP += 10f;
                    break;

                default:

                    break;
            }
        }

        _currentHP = _maxHP;

        _heroState = HeroState.Wait;

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
