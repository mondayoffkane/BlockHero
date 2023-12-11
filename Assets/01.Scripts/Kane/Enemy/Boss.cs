using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{


    public override void SetInit(int _level)
    {
        base.SetInit(_level);
    }


    public override void Fight()
    {
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

                case EnemyState.Attack:
                    if (_target == null)
                    {
                        _enemyState = EnemyState.Wait;
                        yield return null;
                    }
                    else if (Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)
                    {
                        Attack();

                        yield return new WaitForSeconds(_attackInterval);
                    }
                    else
                    {
                        _target = null;
                        _enemyState = EnemyState.Wait;
                        yield return null;
                    }
                    break;



                    //default:
                    //    yield return null;
                    //    break;

            }


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
