using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Boss : Enemy
{
    public GameObject _bullet_Pref;

    public override void SetInit(int _level)
    {
        base.SetInit(_level);
    }


    public override void Fight()
    {
        base.Fight();
        _enemyState = EnemyState.Wait;
        StartCoroutine(Cor_Update());
    }


    IEnumerator Cor_Update()
    {
        while (true)
        {
            yield return null;
            switch (_enemyState)
            {
                case EnemyState.Wait:
                    if (_target == null) FindTarget();
                    break;

                case EnemyState.Move:
                    transform.LookAt(_target.transform);
                    if (_target == null) _enemyState = EnemyState.Wait;
                    else if (Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)
                    {
                        _enemyState = EnemyState.Attack;
                        _animator.SetBool("Attack", true);
                    }

                    break;

                case EnemyState.Attack:

                    Attack();

                    yield return new WaitForSeconds(_attackInterval);

                    break;


            }


        }

    }

    protected override void Attack()
    {

        if (_target._currentHP > 0)
        {

            Transform _newBullet = Managers.Pool.Pop(_bullet_Pref).transform;
            _newBullet.transform.position = transform.position;

            _newBullet.DOMove(_target.transform.position, 1f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    base.Attack();
                    Managers.Pool.Push(_newBullet.GetComponent<Poolable>());
                });


            //base.Attack();
        }
        else
        {
            _target = null;
            _enemyState = EnemyState.Wait;
            _animator.SetBool("Attack", false);
        }
    }


    public override void OnDamage(float _Damage)
    {
        //Debug.Log("Boss OnDamaged");
        if (_enemyState != EnemyState.Dead)
        {
            base.OnDamage(_Damage);

            if (_currentHP <= 0)
            {
                _currentHP = 0;
                _enemyState = EnemyState.Dead;
                Dead();
                //this.TaskDelay(2f, () => Managers.Pool.Push(GetComponent<Poolable>()));
            }
        }

    }





}
