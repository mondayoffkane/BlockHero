using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BlockMan_2 : Hero
{

    // ============================
    public override void SetInit(HeroFactory _herofactory)
    {
        base.SetInit(_herofactory);

    }

    public override void Fight()
    {
        base.Fight();
        _heroState = HeroState.Wait;
        StartCoroutine(Cor_Update());
    }

    IEnumerator Cor_Update()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            yield return null;
            switch (_heroState)
            {
                case HeroState.Wait:
                    if (_target == null) FindTarget();

                    break;

                case HeroState.Move:
                    transform.LookAt(_target.transform);
                    transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * _speed);

                    if (_target == null) _heroState = HeroState.Wait;
                    else if (Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)
                    {
                        _heroState = HeroState.Attack;
                        _animator.SetBool("Attack", true);
                        _animator.SetBool("Walk", false);
                    }

                    yield return null;
                    break;

                case HeroState.Attack:
                    Attack();
                    float _interval = 1.1f - (0.25f * _attackInterval) < 0 ? 0.1f : 1.1f - (0.25f * _attackInterval);
                    yield return new WaitForSeconds(_interval);
                    break;

            }

        }
    }

    protected override void Attack()
    {
        if (_target._currentHP > 0)
        {

            base.Attack();
        }
        else
        {
            _target = null;
            _heroState = HeroState.Wait;
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", false);
        }
    }

    public override void OnDamage(float _Damage)
    {
    
        if (_heroState != HeroState.Dead)
        {
            base.OnDamage(_Damage);

            if (_currentHP <= 0)
            {
                _currentHP = 0;
                _heroState = HeroState.Dead;
                Dead();
              
            }
        }

    }
}
