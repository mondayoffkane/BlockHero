using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Mushroom : Enemy
{

    public override void InitStatus(EnemyStatus EnemyStatus, int Level)
    {

        base.InitStatus(EnemyStatus, Level);
        Fight();
    }



    public override void Fight()
    {
        transform.localScale = Vector3.zero;

        DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.Linear))
            .AppendInterval(1.5f)
            .OnComplete(() =>
            {
                _enemyState = EnemyState.Wait;
                StartCoroutine(Cor_Fight());
            });


    }


    IEnumerator Cor_Fight()
    {
        yield return null;


        while (_enemyState != EnemyState.Dead && _enemyState != EnemyState.Victory)
        {

            switch (_enemyState)
            {
                case EnemyState.Wait:
                    if (_target == null) FindTarget();
                    else
                    {
                        _enemyState = EnemyState.Move;
                        _animator.SetBool("Run", true);
                    }
                    _animator.SetBool("Attack", false);
                    //yield return null;
                    break;

                case EnemyState.Move:
                    if (_target != null || _target._currentHP > 0)
                    {
                        transform.LookAt(_target.transform);

                        if (Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)
                        {
                            _enemyState = EnemyState.Attack;
                        }
                        else
                        {
                            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
                        }

                    }
                    else
                    {
                        _target = null;
                        _enemyState = EnemyState.Wait;
                        _animator.SetBool("Attack", false);
                    }
                    yield return null;
                    break;

                case EnemyState.Attack:
                    _animator.SetBool("Run", false);
                    _animator.SetBool("Attack", true);
                    Attack();
                    yield return new WaitForSeconds(_attackInterval);
                    break;

                case EnemyState.Dead:


                    //yield return null;
                    break;
            }



            //yield return null;
        }



    }





}
